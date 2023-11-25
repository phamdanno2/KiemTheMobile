using GameDBServer.Core;
using GameDBServer.Core.GameEvent;
using GameDBServer.Core.GameEvent.EventObjectImpl;
using GameDBServer.DB;
using GameDBServer.DB.DBController;
using GameDBServer.Logic;
using GameDBServer.Logic.GuildLogic;
using GameDBServer.Logic.KT_ItemManager;
using GameDBServer.Logic.KT_Recore;
using GameDBServer.Logic.Name;
using GameDBServer.Logic.Rank;
using GameDBServer.Logic.SystemParameters;
using GameDBServer.Logic.TeamBattle;
using GameDBServer.Logic.Ten;
using GameDBServer.Server.Network;
using GameDBServer.Tools;
using Server.Data;
using Server.Protocol;
using Server.Tools;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameDBServer.Server
{
    /// <summary>
    /// 用户命令
    /// </summary>
    internal enum TCPGameServerCmds
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

        CMD_PREREMOVE_ROLE = 98, // 预删除角色
        CMD_UNREMOVE_ROLE = 99, // 恢复预删除的角色
        CMD_LOGIN_ON = 100,
        CMD_ROLE_LIST, CMD_CREATE_ROLE, CMD_REMOVE_ROLE,
        CMD_INIT_GAME, CMD_SYNC_TIME, CMD_PLAY_GAME, CMD_SPR_MOVE, CMD_SPR_MOVEEND, CMD_SPR_MOVE2,
        CMD_OTHER_ROLE, CMD_OTHER_ROLE_DATA, CMD_SPR_POSITION, CMD_SPR_PETPOS, CMD_SPR_ACTTION, CMD_SPR_ACTTION2,
        CMD_SPR_MAGICCODE, CMD_SPR_ATTACK, CMD_SPR_INJURE,
        CMD_SPR_REALIVE, CMD_SPR_RELIFE, CMD_SPR_CLICKON, CMD_SYSTEM_MONSTER,
        CMD_SPR_MAPCHANGE, CMD_LOG_OUT_Unused_Dont_Del, CMD_SPR_NEWTASK, CMD_SPR_GETATTRIB2,
        CMD_SPR_LEAVE, CMD_SPR_NPC_BUY, CMD_SPR_NPC_SALEOUT, CMD_SPR_ADD_GOODS, CMD_SPR_MOD_GOODS,
        CMD_SPR_MERGE_GOODS, CMD_SPR_SPLIT_GOODS, CMD_SPR_GET_MERGETYPES, CMD_SPR_GET_MERGEITEMS, CMD_SPR_GET_MERGENEWGOODS,
        CMD_SPR_CHGCODE, CMD_SPR_MONEYCHANGE, CMD_SPR_MODTASK, CMD_SPR_COMPTASK, CMD_SPR_EXPCHANGE,
        CMD_SPR_GETFRIENDS, CMD_SPR_ADDFRIEND, CMD_SPR_REMOVEFRIEND, CMD_SPR_REJECTFRIEND, CMD_SPR_ASKFRIEND,
        CMD_SPR_NEWGOODSPACK, CMD_SPR_DELGOODSPACK, CMD_SPR_CLICKONGOODSPACK,
        CMD_SPR_GETTHING, CMD_SPR_CHGPKMODE, CMD_SPR_CHGPKVAL, CMD_SPR_UPDATENPCSTATE, CMD_SPR_NPCSTATELIST, CMD_SPR_GETNEWTASKDATA,
        CMD_SPR_ABANDONTASK, CMD_SPR_HITED, CMD_SPR_MODKEYS, CMD_SPR_CHAT,
        CMD_SPR_USEGOODS, CMD_SPR_CHANGEPOS, CMD_SPR_NOTIFYCHGMAP, CMD_SPR_FORGE, CMD_SPR_ENCHANCE,
        CMD_SPR_GETOTHERATTRIB, CMD_SPR_UPDATE_ROLEDATA, CMD_SPR_REMOVE_COOLDOWN,
        CMD_SPR_MALL_BUY, CMD_SPR_YINLIANG_BUY, CMD_SPR_USERMONEYCHANGE, CMD_SPR_USERYINLIANGCHANGE, CMD_SPR_GOODSEXCHANGE, CMD_SPR_EXCHANGEDATA,
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
        CMD_SPR_GETFUBENHISTLISTDATA, CMD_SPR_CHGHEROINDEX, CMD_GETOTHERHORSEDATA, CMD_UPDATEALLTHINGINDEXS, CMD_SPR_CHGHALFYINLIANGPERIOD,
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
        CMD_SPR_COMPLETETINYCLIENT, CMD_SPR_USERGOLDCHANGE, CMD_SPR_NOTIFYSHENGXIAOGUESSSTAT, CMD_SPR_NOTIFYSHENGXIAOGUESSRESULT, CMD_SPR_ADDSHENGXIAOMORTGAGE,
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
        CMD_SPR_GOLDCOPYSCENEPREPAREFIGHT,          // 金币副本准备战斗 -- 客户端显示战斗倒计时 [6/11/2014 LiaoWei]
        CMD_SPR_GOLDCOPYSCENEMONSTERWAVE,           // 金币副本刷怪波数 [6/11/2014 LiaoWei]
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
        CMD_SPR_GET_STORE_YINLIANG = 761,                 // 取得仓库金币
        CMD_SPR_GET_STORE_MONEY = 762,                    // 取得仓库绑定金币
        CMD_SPR_STORE_YINLIANG_CHANGE = 763,              // 通知客户端仓库金币改变
        CMD_SPR_STORE_MONEY_CHANGE = 764,                 // 通知客户端仓库绑定金币改变

        CMD_SPR_JIERIACT_STATE = 770,                     // 通知客户端节日活动开启或结束

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

        CMD_SPR_RETURN_RECELL_INFO = 900,                   // (当前推荐人信息——获取信息)
        CMD_SPR_RETURN_RECELL_SET = 901,                    // (当前推荐人信息——设置)
        CMD_SPR_RETURN_AWARD_INFO = 902,                    // (回归礼包——获取信息)
        CMD_SPR_RETURN_AWARD_SET = 903,                     // (回归礼包——领取)
        CMD_SPR_RETURN_CHECK_INFO = 904,                    // (签到信息——获取信息)
        CMD_SPR_RETURN_CHECK_SET = 905,                     // (签到信息——签到)
        CMD_SPR_RETURN_RECELL_AWARD_INFO = 906,             // (召回奖励——获取信息)
        CMD_SPR_RETURN_RECELL_AWARD_SET = 907,              // (召回奖励——设置)
        CMD_SPR_RETURN_RECELL_EXTRA_AWARD = 908,           // (召回奖励——额外奖励领取)

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
        CMD_SPR_TALENT_GET_DATA = 1000,//获取天赋数据
        CMD_SPR_TALENT_ADD_EXP = 1001,//注入经验
        CMD_SPR_TALENT_WASH = 1002,//洗点
        CMD_SPR_TALENT_ADD_EFFECT = 1003,//效果升级

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

        CMD_SPR_KUAFU_MAP_INFO = 1140, //跨服主线地图线路状态信息
        CMD_SPR_KUAFU_MAP_ENTER = 1141, //跨服主线地图进入

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

        CMD_ID_PLACE_HOLDER_BY_CHENJG_END = 1399, // 1300---1399 被我预定了 chenjingui

        CMD_SPR_QUERY_OTHER_SALE_PRICE = 1400, //交易所,根据goodsId查询其他人卖出物品的价格

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

        CMD_SPR_VIDEO_OPEN = 1700,                      //亲加视频 打开视频请求

        CMD_DB_START_CMD = 10000,//数据库命令
        CMD_DB_UPDATE_POS, CMD_DB_UPDATE_EXPLEVEL, CMD_DB_UPDATE_ROLE_AVARTA,
        CMD_DB_UPDATEMONEY1_CMD, CMD_DB_ADDGOODS_CMD, CMD_DB_UPDATEGOODS_CMD,
        CMD_DB_UPDATETASK_CMD, CMD_DB_UPDATEPKMODE_CMD, CMD_DB_UPDATEPKVAL_CMD, CMD_DB_UPDATEKEYS,
        CMD_DB_UPDATEUSERMONEY_CMD, CMD_DB_UPDATEUSERYINLIANG_CMD, CMD_DB_MOVEGOODS_CMD, CMD_DB_UPDATE_LEFTFIGHTSECS,
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
        CMD_DB_ADDYINLIANGBUYITEM, CMD_DB_ADDBANGGONGBUYITEM, CMD_DB_SENDUSERMAIL, CMD_DB_GETUSERMAILDATA, CMD_DB_FINDROLEID_BYROLENAME, CMD_DB_QUERYLIMITGOODSUSEDNUM,
        CMD_DB_UPDATELIMITGOODSUSEDNUM, CMD_DB_UPDATEDAILYVIPDATA, CMD_DB_UPDATEDAILYYANGGONGBKJIFENDATA, CMD_DB_UPDATESINGLETIMEAWARDFLAG, CMD_DB_ADDSHENGXIAOGUESSHIST,
        CMD_DB_UPDATEUSERGOLD_CMD, CMD_DB_ADDGOLDBUYITEM, CMD_DB_UPDATEROLEBAGNUM, CMD_DB_SETLINGDIWARREQUEST, CMD_DB_UPDATEGOODSLIMIT, CMD_DB_UPDATEROLEPARAM,
        CMD_DB_ADDQIANGGOUBUYITEM, CMD_DB_ADDQIANGGOUITEM, CMD_DB_QUERYCURRENTQIANGGOUITEM, CMD_DB_QUERYQIANGGOUBUYITEMS, CMD_DB_UPDATEQIANGGOUTIMEOVER,
        CMD_DB_GETBANGHUIMINIDATA, CMD_DB_ADDBUYITEMFROMNPC, CMD_DB_ADDZAJINDANHISTORY, CMD_DB_QUERYQIANGGOUBUYITEMINFO, CMD_DB_QUERYFIRSTCHONGZHIBYUSERID,
        CMD_DB_QUERYKAIFUONLINEAWARDROLEID, CMD_DB_ADDKAIFUONLINEAWARD, CMD_DB_ADDGIVEUSERMONEYITEM, CMD_DB_QUERYKAIFUONLINEAWARDLIST,
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
        CMD_DB_ADD_STORE_YINLIANG,                  // GS-DB 更新玩家仓库金币信息
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

        CMD_DB_QUERY_ROLEMINIINFO = 10220, //根据角色ID查询角色区号/帐号等不易变信息
        CMD_DB_QUERY_USERACTIVITYINFO = 10221, //查询帐号_活动_活动时间相对应的活动状态和奖励信息
        CMD_DB_UPDATE_USERACTIVITYINFO = 10222, //保存帐号_活动_活动时间相对应的活动状态和奖励信息

        CMD_DB_GET_SERVERLIST = 11000,
        CMD_DB_ONLINE_SERVERHEART,
        CMD_DB_GET_SERVERID,
        CMD_NAME_REGISTERNAME = 12000,             //注册名字到名字服务器
        CMD_SPR_GM_SET_MAIN_TASK = 13000,          //GM命令设置任务

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

        CMD_LOGDB_ADD_ITEM_LOG = 20000,             // 向日志数据库服务器添加物品操作日志
        CMD_LOGDB_ADD_TRADEMONEY_FREQ_LOG = 20001,             // 向日志数据库服务器添加物品操作日志
        CMD_LOGDB_ADD_TRADEMONEY_NUM_LOG = 20002,             // 向日志数据库服务器添加物品操作日志
        CMD_LOGDB_UPDATE_ROLE_KUAFU_DAY_LOG = 20003,             // 向日志数据库服务器添加物品操作日志

        CMD_DB_UPDATA_TAROT = 20100,                //塔罗牌 向DB服更新数据

        CMD_DB_DEL_SKILL = 20101,

        CMD_SPR_TASKLIST_DATA = 29900,
        CMD_SPR_TASKLIST_KEY = 30000,
        CMD_SPR_TASKLIST_NOTIFY = 30001,
        CMD_KT_QUERY_PLAYERRANKING = 50076,
        CMD_DB_ERR_RETURN = 30767, //MAX 消息定义不可超过此值

        CMD_KT_FAMILY_CREATE = 50082,
        CMD_KT_FAMILY_REQUESTJOIN = 50083,
        CMD_KT_FAMILY_KICKMEMBER = 50084,
        CMD_KT_FAMILY_GETLISTFAMILY = 50085,
        CMD_KT_FAMILY_DESTROY = 50086,
        CMD_KT_FAMILY_CHANGENOTIFY = 50087,
        CMD_KT_FAMILY_CHANGE_REQUESTJOIN_NOTIFY = 50088,
        CMD_KT_FAMILY_CHANGE_RANK = 50089,
        CMD_KT_FAMILY_RESPONSE_REQUEST = 50090,

        CMD_KT_FAMILY_QUIT = 50091,

        CMD_KT_FAMILY_OPEN = 50092,

        /// <summary>
        /// Số lượt đã mở phụ bản Vượt ải gia tộc
        /// </summary>
        CMD_KT_FAMILY_WEEKLYFUBENCOUNT = 50093,

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
        /// Thoát tộc nếu thằng này là tộc trưởng
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
        /// Thực hiện rút cổ tức
        /// </summary>
        CMD_KT_GUILD_DOWTIHDRAW = 50120,

        /// <summary>
        /// Gia tộc rời khỏi bang hội
        /// </summary>
        CMD_KT_GUILD_FAMILYQUIT = 50121,

        CMD_KT_GETTERRORY_DATA = 50123,

        CMD_KT_G2C_RECHAGE = 50132,

        #region GuildUpdateMoney

        CMD_KT_UPDATE_ROLEGUILDMONEY = 50141,

        /// <summary>
        /// Lấy ra ranking của người chơi
        /// </summary>
        CMD_KT_RANKING_CHECKING = 50142,

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


        CMD_KT_GMCHANGERANK = 50148,


        CMD_KT_GUILD_ALLTERRITORY = 50149,

        //Update thông tin lãnh thổ sau khi tranh đoạt lãnh thổ
        CMD_KT_GUILD_UPDATE_TERRITORY = 50151,


        // Lấy thông tin mini của bang hội
        CMD_KT_GUILD_GETMINIGUILDINFO = 50152,

        #endregion GuildUpdateMoney

        #endregion Bang hội
    };

    public enum TCPProcessCmdResults { RESULT_OK = 0, RESULT_FAILED = 1, RESULT_DATA = 2, RESULT_UNREGISTERED = 3, };

    /// <summary>
    /// 处理收到的TCP协议命令
    /// </summary>
    internal class TCPCmdHandler
    {
        /// <summary>
        /// 处理网络协议命令
        /// </summary>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static TCPProcessCmdResults ProcessCmd(GameServerClient client, DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            TCPProcessCmdResults result = TCPProcessCmdResults.RESULT_FAILED;
            tcpOutPacket = null;

            var enumDisplayStatus = (TCPGameServerCmds)nID;
            string stringValue = enumDisplayStatus.ToString();

            // Console.WriteLine("PACKET FROM GS: " + stringValue);
            switch (nID)
            {
                case (int)TCPGameServerCmds.CMD_DB_REGUSERID:
                    {
                        result = ProcessRegUserIDCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_ROLE_LIST:
                    {
                        result = ProcessGetRoleListCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_CREATE_ROLE:
                    {
                        result = ProcessCreateRoleCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_INIT_GAME:
                    {
                        result = ProcessInitGameCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_QUERY_ROLEMINIINFO:
                    {
                        result = ProcessQueryRoleMiniInfoCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_ROLE_ONLINE:
                    {
                        result = ProcessRoleOnLineGameCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_UPDATE_ACCOUNT_ACTIVE:
                    {
                        result = ProcessUpdateAccountActiveCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_ROLE_HEART:
                    {
                        result = ProcessRoleHeartGameCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_ROLE_OFFLINE:
                    {
                        result = ProcessRoleOffLineGameCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_GET_SERVERLIST:
                    {
                        result = ProcessGetServerListCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_ONLINE_SERVERHEART:
                    {
                        result = ProcessOnlineServerHeartCmd(client, dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_GET_SERVERID:
                    {
                        result = ProcessGetServerIdCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_SPR_NEWTASK:
                    {
                        result = ProcessNewTaskCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_UPDATE_POS:
                    {
                        result = ProcessUpdatePosCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_UPDATE_EXPLEVEL:
                    {
                        result = ProcessUpdateExpLevelCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_UPDATE_ROLE_AVARTA:
                    {
                        result = ProcessUpdateRoleAvartaCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_UPDATEMONEY1_CMD:
                    {
                        result = ProcessUpdateMoney1Cmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_ADDGOODS_CMD:
                    {
                        result = ProcessAddGoodsCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_UPDATEGOODS_CMD:
                    {
                        result = ProcessUpdateGoodsCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_UPDATETASK_CMD:
                    {
                        result = ProcessUpdateTaskCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_SPR_COMPTASK:
                    {
                        result = ProcessCompleteTaskCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_SPR_GM_SET_MAIN_TASK:
                    {
                        result = ProcessGMSetTaskCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_SPR_GETFRIENDS:
                    {
                        result = ProcessGetFriendsCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_SPR_ADDFRIEND:
                    {
                        result = ProcessAddFriendCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_SPR_REMOVEFRIEND:
                    {
                        result = ProcessRemoveFriendCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_UPDATEPKMODE_CMD:
                    {
                        result = ProcessUpdatePKModeCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_UPDATEPKVAL_CMD:
                    {
                        result = ProcessUpdatePKValCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_SPR_ABANDONTASK:
                    {
                        result = ProcessAbandonTaskCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_UPDATEKEYS:
                    {
                        result = ProcessModKeysCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_UPDATEUSERMONEY_CMD:
                    {
                        result = ProcessUpdateUserMoneyCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_UPDATEUSERYINLIANG_CMD:
                    {
                        result = ProcessUpdateUserYinLiangCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_UPDATEUSERGOLD_CMD:
                    {
                        result = ProcessUpdateUserGoldCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_ROLE_BUY_YUE_KA_BUT_OFFLINE:
                    {
                        result = ProcessRoleBuyYueKaButOffline(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_MOVEGOODS_CMD:
                    {
                        result = ProcessMoveGoodsCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_UPDATE_LEFTFIGHTSECS:
                    {
                        result = ProcessUpdateLeftFightSecsCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_SPR_QUERYIDBYNAME:
                    {
                        result = ProcessQueryNameByIDCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_SPR_QUERYUMBYNAME:
                    {
                        result = ProcessQueryUserMoneyByNameCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_SPR_CHAT: //转发聊天消息
                    {
                        result = ProcessSpriteChatCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_GET_CHATMSGLIST:
                    {
                        result = ProcessGetChatMsgListCmd(client, dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_ADDHORSE:
                    {
                        //  result = ProcessAddHorseCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_ADDPET:
                    {
                        // result = ProcessAddPetCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_GETHORSELIST:
                    {
                        // result = ProcessGetHorseListCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_GETOTHERHORSELIST:
                    {
                        //  result = ProcessGetOtherHorseListCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_GETPETLIST:
                    {
                        //  result = ProcessGetPetListCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_MODHORSE:
                    {
                        // result = ProcessModHorseCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_MODPET:
                    {
                        //  result = ProcessModPetCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_HORSEON:
                    {
                        // result = ProcessHorseOnCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_HORSEOFF:
                    {
                        // result = ProcessHorseOffCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_PETOUT:
                    {
                        // result = ProcessPetOutCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_PETIN:
                    {
                        //  result = ProcessPetInCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_GETGOODSLISTBYSITE:
                    {
                        result = ProcessGetGoodsListBySiteCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_ADDDJPOINT:
                    {
                        //  result = ProcessGetAddDJPointCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_SPR_GETDJPOINTS:
                    {
                        // result = ProcessGetDJPointsCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_UPJINGMAI_LEVEL:
                    {
                        // result = ProcessUpDianJiangLevelCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_BANROLENAME:
                    {
                        result = ProcessBanRoleNameCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_BANROLECHAT:
                    {
                        result = ProcessBanRoleChatCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_GETBANROLECATDICT:
                    {
                        result = ProcessGetBanRoleChatDictCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_UPDATEONLINETIME:
                    {
                        result = ProcessUpdateOnlineTimeCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_GAMECONFIGDICT:
                    {
                        result = ProcessGameConfigDictCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_GAMECONIFGITEM:
                    {
                        result = ProcessGameConfigItemCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_RESETBIGUAN:
                    {
                        // result = ProcessResetBigGuanCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_ADDSKILL:
                    {
                        result = ProcessAddSkillCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_UPSKILLINFO:
                    {
                        result = ProcessUpSkillInfoCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_DEL_SKILL:
                    {
                        result = ProcessDelSkillCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_UPDATEJINGMAIEXP:
                    {
                        // result = ProcessUpdateJingMaiExpCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_UPDATEDEFSKILLID:
                    {
                        // result = ProcessUpdateSkillIDCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_UPDATEJIEBIAOINFO:
                    {
                        //   result = ProcessUpdateJieBiaoInfoCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_UPDATEAUTODRINK:
                    {
                        //   result = ProcessUpdateAutoDrinkCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_UPDATEBUFFERITEM:
                    {
                        result = ProcessUpdateBufferItemCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_UNDELROLENAME:
                    {
                        result = ProcessUnDelRoleNameCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_DELROLENAME:
                    {
                        result = ProcessDelRoleNameCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_UPDATEDAILYTASKDATA:
                    {
                        result = ProcessUpdateDailyTaskDataCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_UPDATEDAILYJINGMAI:
                    {
                        //  result = ProcessUpdateDailyJingMaiCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_UPDATENUMSKILLID:
                    {
                        //  result = ProcessUpdateNumSkillIDCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_UPDATEPBINFO:
                    {
                        result = ProcessUpdatePBInfoCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_UPDATEROLEBAGNUM:
                    {
                        result = ProcessUpdateRoleBagNumCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_UPDATHUODONGINFO:
                    {
                        result = ProcessUpdateHuoDongInfoCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_UPDATE_SPECACT:
                    {
                        result = ProcessUpdateSpecActCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_GET_SPECACTINFO:
                    {
                        result = ProcessGetSpecActInfoCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_DELETE_SPECACT:
                    {
                        result = ProcessDeleteSpecActCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_USELIPINMA:
                    {
                        result = ProcessUseLiPinMaCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_UPDATEFUBENDATA:
                    {
                        // result = ProcessUpdateFuBenDataCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_GETFUBENSEQID:
                    {
                        //  result = ProcessGetFuBenSeqIDCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_SPR_GETFUBENHISTDATA:
                    {
                        //  result = ProcessGetFuBenHistDataCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_ADDFUBENHISTDATA:
                    {
                        //  result = ProcessAddFuBenHistDataCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_UPDATELIANZHAN:
                    {
                        // result = ProcessUpdateLianZhanCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_UPDATEROLEDAILYDATA:
                    {
                        result = ProseccUpdateRanking(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_UPDATEKILLBOSS:
                    {
                        result = ProcessUpdateKillBossCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_UPDATEROLESTAT:
                    {
                        // result = ProcessUpdateRoleStatCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_KT_QUERY_PLAYERRANKING:
                    {
                        result = ProcessGetPaiHangListCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_UPDATEYABIAODATA:
                    {
                        // result = ProcessUpdateYaBiaoDataCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_UPDATEYABIAODATASTATE:
                    {
                        //  result = ProcessUpdateYaBiaoDataStateCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_SPR_GETOTHERATTRIB:
                case (int)TCPGameServerCmds.CMD_SPR_GETOTHERATTRIB2:
                    {
                        result = ProcessGetOtherAttrib2DataCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_UPDATEBATTLENAME:
                    {
                        // result = ProcessUpdateBattleNameCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_ADDMALLBUYITEM:
                    {
                        result = ProcessAddMallBuyItemCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_ADDQIZHENGEBUYITEM:
                    {
                        result = ProcessAddQiZhenGeBuyItemCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_GETLIPINMAINFO:
                    {
                        result = ProcessGetLiPinMaInfoCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_UPDATECZTASKID:
                    {
                        result = ProcessUpdateCZTaskIDCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_GETTOTALONLINENUM:
                    {
                        result = ProcessGetTotalOnlineNumCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_SPR_GETFUBENHISTLISTDATA:
                    {
                        // result = ProcessGetFuBenHistListDataCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_UPDATEBATTLENUM:
                    {
                        // result = ProcessUpdateBattleNumCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_UPDATEHEROINDEX:
                    {
                        //  result = ProcessUpdateHeroIndexCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_FORCERELOADPAIHANG:
                    {
                        result = ProcessForceReloadPaiHangCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_GETOTHERHORSEDATA:
                    {
                        // result = ProcessGetOtherHorseDataCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_ADDYINPIAOBUYITEM:
                    {
                        // result = ProcessAddYinPiaoBuyItemCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_GETBANGHUILIST:
                    {
                        // result = ProcessGetBangHuiListCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_CREATEBANGHUI:
                    {
                        // result = ProcessCreateBangHuiCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_QUERYBANGHUIDETAIL:
                    {
                        // result = ProcessQueryBangHuiDetailCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_UPDATEBANGHUIBULLETIN:
                    {
                        //  result = ProcessUpdateBangHuiBulletinCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_GETBHMEMBERDATALIST:
                    {
                        // result = ProcessGetBHMemberDataListCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_UPDATEBHVERIFY:
                    {
                        //  result = ProcessUpdateBHVerifyCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_QUERYBHMGRLIST:
                    {
                        //result = ProcessQueryBHMGRListCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_BANGHUIVERIFY:
                    {
                        // result = ProcessBangHuiVerifyCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_ADDBHMEMBER:
                    {
                        //result = ProcessAddBHMemberCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_REMOVEBHMEMBER:
                    {
                        //  result = ProcessRemoveBHMemberCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_QUITFROMBANGHUI:
                    {
                        //result = ProcessQuitFromBangHuiCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_DESTROYBANGHUI:
                    {
                        // result = ProcessDestroyBangHuiCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_CHGBHMEMBERZHIWU:
                    {
                        //result = ProcessChgBHMemberZhiWuCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_CHGBHMEMBERCHENGHAO:
                    {
                        // result = ProcessChgBHMemberChengHaoCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_SEARCHROLESFROMDB:
                    {
                        result = ProcessSearchRolesFromDBCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_GETBANGGONGHIST:
                    {
                        // result = ProcessGetBangGongHistCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_DONATEBGMONEY:
                    {
                        // result = ProcessDonateBGMoneyCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_DONATEBGGOODS:
                    {
                        // result = ProcessDonateBGGoodsCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_UPDATEBANGGONG_CMD:
                    {
                        result = ProseccUpdateGuildMoney(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_UPDATEBHTONGQIAN_CMD:
                    {
                        // result = ProcessUpdateBHTongQianCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_ADDBHTONGQIAN_CMD:
                    {
                        // result = ProcessAddBHTongQianCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_GETBANGQIINFO:
                    {
                        //result = ProcessGetBangQiInfoCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_RENAMEBANGQI:
                    {
                        // result = ProcessRenameBangQiCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_UPLEVELBANGQI:
                    {
                        //  result = ProcessUpLevelBangQiCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_GETBHJUNQILIST:
                    {
                        // result = ProcessGetBHJunQiListCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_GETBHLINGDIDICT:
                    {
                        // result = ProcessGetBHLingDiDictCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_UPDATELINGDIFORBH:
                    {
                        // result = ProcessUpdateLingDiForBHCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_GETLEADERROLEIDBYBHID:
                    {
                        //  result = ProcessGetLeaderRoleIDByBHIDCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_GETBHLINGDIINFODICTBYBHID:
                    {
                        //  result = ProcessGetBHLingDiInfoDictByBHIDCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_SETLINGDITAX:
                    {
                        // result = ProcessSetLingDiTaxCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_TAKELINGDITAXMONEY:
                    {
                        // result = ProcessTakeLingDiTaxMoneyCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_ADDLINGDITAXMONEY:
                    {
                        //  result = ProcessAddLingDiTaxMoneyCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_GETHUANGDIBHINFO:
                    {
                        //  result = ProcessGetHuangDiBHInfoCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_QUERYQIZHEGEBUYHIST:
                    {
                        //  result = ProcessQueryQiZhenGeBuyHistCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_GETHUANGDIROLEDATA:
                    {
                        result = ProcessGetHuangDiRoleDataCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_ADDHUANGFEI:
                    {
                        // result = ProcessAddHuangFeiCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_REMOVEHUANGFEI:
                    {
                        // result = ProcessRemoveHuangFeiCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_GETHUANGFEIDATA:
                    {
                        // result = ProcessGetHuangFeiDataCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_SENDTOLAOFANG:
                    {
                        //result = ProcessSendToLaoFangCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_TAKEOUTLAOFANG:
                    {
                        // result = ProcessSendToLaoFangCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_BANCHAT:
                    {
                        // result = ProcessSendToLaoFangCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_ADDREFRESHQIZHENREC:
                    {
                        result = ProcessAddRefreshQiZhenRecCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_CLEARCACHINGROLEIDATA:
                    {
                        result = ProcessClrCachingRoleDataCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_CLEARALLCACHINGROLEDATA:
                    {
                        result = ProcessClrAllCachingRoleDataCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_ADDMONEYWARNING:
                    {
                        result = ProcessAddMoneyWarningCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_GETGOODSBYDBID:
                    {
                        result = ProcessGetGoodsByDbIDCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_QUERYCHONGZHIMONEY:
                    {
                        result = ProcessQueryChongZhiMoneyCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_QUERYTODAYCHONGZHIMONEY:
                    {
                        result = ProcessQueryDayChongZhiMoneyCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_ADDYINLIANGBUYITEM:
                    {
                        result = ProcessAddYinLiangBuyItemCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_ADDBUYITEMFROMNPC:
                    {
                        result = ProcessAddBuyItemFromNpcCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_ADDGOLDBUYITEM:
                    {
                        result = ProcessAddGoldBuyItemCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_ADDBANGGONGBUYITEM:
                    {
                        // result = ProcessAddBangGongBuyItemCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_GETUSERMAILLIST:
                    {
                        result = ProcessGetUserMailListCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_GETUSERMAILCOUNT:
                    {
                        result = ProcessGetUserMailCountCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_GETUSERMAILDATA:
                    {
                        result = ProcessGetUserMailDataCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_GETUSERMAILDATA:
                    {
                        result = ProcessGetUserMailDataCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_FINDROLEID_BYROLENAME:
                    {
                        result = ProcessGetRoleIDByRoleNameCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_SENDUSERMAIL:
                    {
                        result = ProcessSendUserMailCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_FETCHMAILGOODS:
                    {
                        result = ProcessFetchMailGoodsCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_DELETEUSERMAIL:
                    {
                        result = ProcessDeleteUserMailCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_EXECUTEINPUTFANLI:
                    {
                        result = ProcessExcuteInputFanLiCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_EXECUTEINPUTJIASONG:
                    {
                        result = ProcessExcuteInputJiaSongCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_EXECUTEINPUTKING:
                    {
                        result = ProcessExcuteInputKingCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_EXECUTELEVELKING:
                    {
                        result = ProcessExcuteLevelKingCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_EXECUTEEQUIPKING:
                    {
                        result = ProcessExcuteEquipKingCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_EXECUTEHORSEKING:
                    {
                        result = ProcessExcuteHorseKingCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_EXECUTEJINGMAIKING:
                    {
                        result = ProcessExcuteJingMaiKingCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_QUERYINPUTFANLI:
                    {
                        result = ProcessSprQueryInputFanLiCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_QUERYINPUTJIASONG:
                    {
                        result = ProcessSprQueryInputJiaSongCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_QUERYINPUTKING:
                    {
                        result = ProcessSprQueryInputKingCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_QUERYLEVELKING:
                    {
                        result = ProcessSprQueryLevelKingCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_QUERYEQUIPKING:
                    {
                        result = ProcessSprQueryEquipKingCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_QUERYHORSEKING:
                    {
                        result = ProcessSprQueryHorseKingCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_QUERYJINGMAIKING:
                    {
                        result = ProcessSprQueryJingMaiKingCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_QUERYAWARDHIST:
                    {
                        result = ProcessSprQueryAwardHistoryCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_QUERYLIMITGOODSUSEDNUM:
                    {
                        result = ProcessDBQueryLimitGoodsUsedNumCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_UPDATELIMITGOODSUSEDNUM:
                    {
                        result = ProcessDBUpdateLimitGoodsUsedNumCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_UPDATEDAILYVIPDATA:
                    {
                        // result = ProcessUpdateDailyVipDataCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_UPDATEDAILYYANGGONGBKJIFENDATA:
                    {
                        // result = ProcessUpdateYangGongBKDailyJiFenDataCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_UPDATESINGLETIMEAWARDFLAG:
                    {
                        result = ProcessUpdateSingleTimeAwardFlagCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_ADDSHENGXIAOGUESSHIST:
                    {
                        result = ProcessAddShengXiaoGuessHisotryCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_QUERYSHENGXIAOGUESSHISTORY:
                    {
                        // result = ProcessQueryShengXiaoGuessHistCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_QUERYSHENGXIAOGUESSSELFHISTORY:
                    {
                        //  result = ProcessQueryShengXiaoGuessHistCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_UPDATEGOODSLIMIT:
                    {
                        result = ProcessUpdateGoodsLimitCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_UPDATEROLEPARAM:
                    {
                        result = ProcessUpdateRoleParamCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_DB_SETLINGDIWARREQUEST:
                    {
                        // result = ProcessSetLingDiWarRequestCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_TAKELINGDIDAILYAWARD:
                    {
                        //  result = ProcessTakeLingDiDailyAwardCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_ADDQIANGGOUBUYITEM:
                    {
                        //  result = ProcessAddQiangGouBuyItemCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_QUERYQIANGGOUBUYITEMINFO:
                    {
                        result = ProcessQueryQiangGouBuyItemInfoCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_ADDQIANGGOUITEM:
                    {
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_QUERYCURRENTQIANGGOUITEM:
                    {
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_QUERYQIANGGOUBUYITEMS:
                    {
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_UPDATEQIANGGOUTIMEOVER:
                    {
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_GETBANGHUIMINIDATA:
                    {
                        //  result = ProcessGetBangHuiMiniDataCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_ADDZAJINDANHISTORY:
                    {
                        result = ProcessAddZaJinDanHisotryCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_QUERYZAJINDANHISTORY:
                    {
                        // result = ProcessQueryZaJinDanHistoryCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_QUERYSELFZAJINDANHISTORY:
                    {
                        // result = ProcessQueryZaJinDanHistoryCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_QUERYFIRSTCHONGZHIBYUSERID:
                    {
                        result = ProcessQueryFirstChongZhiDaLiByUserIDCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_QUERYDAYCHONGZHIBYUSERID:
                    {
                        result = ProcessQueryDayChongZhiDaLiByUserIDCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_QUERYKAIFUONLINEAWARDROLEID:
                    {
                        result = ProcessQueryKaiFuOnlineAwardRoleIDCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_ADDKAIFUONLINEAWARD:
                    {
                        result = ProcessAddKaiFuOnlineAwardCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_QUERYKAIFUONLINEAWARDLIST:
                    {
                        result = ProcessQueryKaiFuOnlineAwardListCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_ADDGIVEUSERMONEYITEM:
                    {
                        result = ProcessAddGiveUserMoneyItemCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_ADDEXCHANGE1ITEM:
                    {
                        result = ProcessAddExchange1ItemCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_ADDEXCHANGE2ITEM:
                    {
                        result = ProcessAddExchange2ItemCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_ADDEXCHANGE3ITEM:
                    {
                        result = ProcessAddExchange3ItemCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_ADDFALLGOODSITEM:
                    {
                        // result = ProcessAddFallGoodsItemCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_UPDATEROLEPROPS:
                    {
                        result = ProcessUpdateRolePropsCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_QUERYJIERIDALIBAO:
                    {
                        result = ProcessQueryJieriDaLiBaoCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_QUERYJIERIDENGLU:
                    {
                        result = ProcessQueryJieriDengLuCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_QUERYJIERIVIP:
                    {
                        result = ProcessQueryJieriVIPCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_QUERYJIERICZSONG:
                    {
                        result = ProcessQueryJieriCZSongCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_QUERYJIERICZLEIJI:
                    {
                        result = ProcessQueryJieriCZLeiJiCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_QUERYJIERITOTALCONSUME:
                    {
                        result = ProcessQueryJieriTotalConsumeCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_QUERYJIERIXIAOFEIKING:
                    {
                        // result = ProcessQueryJieriXiaoFeiKingCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_QUERYJIERICZKING:
                    {
                        // result = ProcessQueryJieriCZKingCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_EXECUTEJIERIDALIBAO:
                    {
                        result = ProcessExecuteJieriDaLiBaoCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_EXECUTEJIERIDENGLU:
                    {
                        result = ProcessExecuteJieriDengLuCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_EXECUTEJIERIVIP:
                    {
                        result = ProcessExecuteJieriVIPCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_EXECUTEJIERICZSONG:
                    {
                        result = ProcessExecuteJieriCZSongCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_EXECUTEJIERICZLEIJI:
                    {
                        result = ProcessExecuteJieriCZLeiJiCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_EXECUTEJIERITOTALCONSUME:
                    {
                        result = ProcessExecuteJieriTotalConsumeCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_EXECUTEJIERIXIAOFEIKING:
                    {
                        result = ProcessExecuteJieriXiaoFeiKingCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_EXECUTEJIERICZKING:
                    {
                        result = ProcessExecuteJieriCZKingCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_QUERYHEFUDALIBAO:
                    {
                        result = ProcessQueryHeFuDaLiBaoCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_QUERYHEFUVIP:
                    {
                        result = ProcessQueryHeFuVIPCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_QUERYHEFUCZSONG:
                    {
                        result = ProcessQueryHeFuCZSongCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_QUERYHEFUPKKING:
                    {
                        // result = ProcessQueryHeFuPKKingCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_QUERYHEFUWCKING:
                    {
                        //  result = ProcessQueryHeFuWCKingCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_QUERYHEFUFANLI:
                    {
                        result = ProcessQueryHeFuCZFanLiCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_QUERYXINFANLI:
                    {
                        // result = ProcessQueryXinCZFanLiCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_EXECUHEFUDALIBAO:
                    {
                        //  result = ProcessExecuteHeFuDaLiBaoCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_EXECUHEFUVIP:
                    {
                        result = ProcessExecuteHeFuVIPCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_EXECUHEFUCZSONG:
                    {
                        result = ProcessExecuteHeFuCZSongCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_EXECUHEFUPKKING:
                    {
                        result = ProcessExecuteHeFuPKKingCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_EXECUHEFUWCKING:
                    {
                        // result = ProcessExecuteHeFuWCKingCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_EXECUHEFUFANLI:
                    {
                        result = ProcessExecuteHeFuCZFanLiCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_EXECUXINFANLI:
                    {
                        result = ProcessExecuteXinCZFanLiCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_QUERYACTIVITYINFO:
                    {
                        result = ProcessQueryActivityInfoCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_QUERYXINGYUNORYUEDUCHOUJIANGINFO:
                    {
                        result = ProcessQueryXingYunChouJiangInfoCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_EXECUXINGYUNORYUEDUCHOUJIANGINFO:
                    {
                        result = ProcessExcuteXingYunChouJiangInfoCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_ADDYUEDUCHOUJIANGHISTORY:
                    {
                        result = ProcessExcuteAddYueDuChouJiangInfoCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_QUERYYUEDUCHOUJIANGHISTORY:
                    {
                        // result = ProcessQueryYueDuChouJiangHistoryCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_QUERYSELFQUERYYUEDUCHOUJIANGHISTORY:
                    {
                        //   result = ProcessQueryYueDuChouJiangHistoryCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_EXECUTECHANGEOCCUPATION:
                    {
                        result = ProcessExecuteChangeOccupationCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_GETROLEUSINGGOODSDATALIST:
                    {
                        result = ProcessGetUsingGoodsDataListCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_QUERYBLOODCASTLEENTERCOUNT:
                    {
                        result = ProcessQueryBloodCastleEnterCountCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_UPDATEBLOODCASTLEENTERCOUNT:
                    {
                        result = ProcessUpdateBloodCastleEnterCountCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_QUERYFUBENHISINFO:
                    {
                        // result = ProcessQueryFuBenHisInfoCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_COMPLETEFLASHSCENE:
                    {
                        result = ProcessCompleteFlashSceneCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_FINISHFRESHPLAYERSTATUS:
                    {
                        result = ProcessFinishFreshPlayerStatusCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_CLEANDATAWHENFRESHPLAYERLOGOUT:
                    {
                        result = ProcessCleanDataWhenFreshPlayerLogOutCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_EXECUTECHANGETASKSTARLEVEL:
                    {
                        result = ProcessChangeTaskStarLevelCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_EXECUTECHANGELIFE:
                    {
                        // result = ProcessChangeLifeCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_ADMIREDPLAYER:
                    {
                        result = ProcessAdmiredPlayerCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_EXECUTEUPDATEROLESOMEINFO:
                    {
                        // result = ProcessUpdateRoleSomeInfoCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_QUERYDAYACTIVITYTOTALPOINT:
                    {
                        result = ProcessQueryDayActivityPoinCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_QUERYDAYACTIVITYSELFPOINT:
                    {
                        result = ProcessQueryRoleDayActivityPoinCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_UPDATEDAYACTIVITYSELFPOINT:
                    {
                        result = ProcessUpdateRoleDayActivityPoinCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_QUERYEVERYDAYONLINEAWARDGIFTINFO:
                    {
                        result = ProcessQueryEveryDayOnLineAwardGiftInfoCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_SETAUTOASSIGNPROPERTYPOINT:
                    {
                        //  result = ProcessSetAutoAssignPropertyPointCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_UPDATEPUSHMESSAGEINFO:
                    {
                        result = ProcessUpdatePushMessageInfoCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_QUERYPUSHMESSAGEUSERLIST:
                    {
                        result = ProcessQueryPushMsgUerListCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_ADDWING:
                    {
                        // result = ProcessAddWingCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_MODWING:
                    {
                        //  result = ProcessModWingCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_REFERPICTUREJUDGE:
                    {
                        // result = ProcessReferPictureJudgeInfoCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_QUERYMOJINGEXCHANGEINFO:
                    {
                        result = ProcessQueryMoJingExchangeInfoCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_UPDATEMOJINGEXCHANGEINFO:
                    {
                        result = ProcessUpdateMoJingExchangeInfoCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_GETNEWZONEACTIVEAWARD:
                    {
                        result = NewZoneActiveMgr.ProcessGetNewzoneActiveAward(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_QUERYUPLEVELMADMAN:
                    {
                        // NewZoneActiveMgr
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_QUERYNEWZONEACTIVE:
                    {
                        result = NewZoneActiveMgr.ProcessQueryActiveInfo(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_QUERY_REPAYACTIVEINFO:
                    {
                        result = RechargeRepayActiveMgr.ProcessQueryActiveInfo(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_QUERY_USERACTIVITYINFO:
                    {
                        result = ProcessSprQueryUserActivityInfoCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_UPDATE_USERACTIVITYINFO:
                    {
                        result = ProcessSprUpdateUserActivityInfoCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_GET_REPAYACTIVEAWARD:
                    {
                        result = RechargeRepayActiveMgr.ProcessGetActiveAwards(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_QUERY_GETOLDRESINFO:
                    {
                        result = CGetOldResourceManager.ProcessQueryGetResourceInfo(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_UPDATE_OLDRESOURCE:
                    {
                        result = CGetOldResourceManager.ProcessUpdateGetResourceInfo(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_UPDATEGOODS_CMD2:
                    {
                        // result = ProcessUpdateGoodsCmd2(dbMgr, client, nID, data, count, out tcpOutPacket);
                        break;
                    }
#if 移植
                case 30111:
                    {
                        //  result = ProcessUpdateGoodsCmd3(dbMgr, client, nID, data, count, out tcpOutPacket);
                        break;
                    }
#endif
                case (int)TCPGameServerCmds.CMD_DB_SAVECONSUMELOG:
                    {
                        result = Global.SaveConsumeLog(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_QUERYVIPLEVELAWARDFLAG:
                    {
                        result = ProcessQueryVipLevelAwardFlagCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_UPDATEVIPLEVELAWARDFLAG:
                    {
                        result = ProcessUpdateVipLevelAwardFlagCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_LOGDB_ADD_ITEM_LOG:
                    {
                        result = ProcessAddItemLogCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_LOGDB_UPDATE_ROLE_KUAFU_DAY_LOG:
                    {
                        result = ProcessUpdateRoleKuaFuDayLogCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_GETFIRSTCHARGEINFO:
                    {
                        //result = CFirstChargeMgr.ProcessQueryUserFirstCharge(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_FIRSTCHARGE_CONFIG:
                    {
                        // result = CFirstChargeMgr.FirstChargeConfig(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_ADD_STORE_YINLIANG:
                    {
                        result = ProcessAddRoleStoreYinliang(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_ADD_STORE_MONEY:
                    {
                        result = ProcessAddRoleStoreMoney(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_QUERYROLEMONEYINFO:
                    {
                        // result = ProcessQueryRoleMoneyInfo(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_ALL_COMPLETION_OF_TASK_BY_TASKID: // 完成某任务与之前所有任务 [XSea 2015/4/13]
                    {
                        result = ProcessAutoCompletionTaskByTaskID(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_MODIFY_ROLE_HUO_BI_OFFLINE:
                    {
                        result = ProcessRoleHuobiOffline(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_UPDATE_USR_SECOND_PASSWORD:
                    {
                        result = ProcessUsrSetSecPwd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_GET_USR_SECOND_PASSWORD:
                    {
                        result = ProcessGetUsrSecondPassword(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_UPDATE_MARRY_DATA:
                    {
                        //  result = ProcessUpdateMarriageDataCmd(dbMgr, client, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_GET_MARRY_DATA:
                    {
                        result = ProcessGetMarriageDataCmd(dbMgr, pool, client, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_EXECUXJIERIFANLI:
                    {
                        result = ProcessExecuteJieriFanLiCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_INPUTPOINTS_EXCHANGE:
                    {
                        result = ProcessInputPointsExchangeCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_REQUESTNEWGMAILLIST:
                    {
                        result = GroupMailManager.RequestNewGroupMailList(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_MODIFYROLEGMAIL:
                    {
                        result = GroupMailManager.ModifyGMailRecord(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_SPR_CHANGE_NAME:
                    {
                        result = NameManager.Instance().ProcChangeName(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_GM_BAN_CHACK:
                    {
                        result = ProcessGmBanCheck(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_GM_BAN_LOG:
                    {
                        result = ProcessGmBanLog(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_TEN_INIT:
                    {
                        result = ProcessTenInitCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_SPREAD_AWARD_GET:
                    {
                        result = ProcessSpreadAwardGetCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_SPREAD_AWARD_UPDATE:
                    {
                        result = ProcessSpreadAwardUpdateCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_ACTIVATE_GET:
                    {
                        result = ProcessActivateStateGetCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_ACTIVATE_SET:
                    {
                        result = ProcessActivateStateSetCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_QUERY_SYSPARAM:
                    {
                        result = TCPCmdHandler.ProcessQuerySystemGlobalParameters(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_DB_TEAMBATTLE:
                    {
                        result = KTTeamBattleManager.Instance.ProcessTeamBattleCmd(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_KT_FAMILY_CREATE:
                    {
                        result = KT_TCPHandler_Family.CMD_KT_FAMILY_CREATE(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_KT_FAMILY_REQUESTJOIN:
                    {
                        result = KT_TCPHandler_Family.CMD_KT_FAMILY_REQUESTJOIN(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_KT_FAMILY_KICKMEMBER:
                    {
                        result = KT_TCPHandler_Family.CMD_KT_FAMILY_KICKMEMBER(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_KT_FAMILY_GETLISTFAMILY:
                    {
                        result = KT_TCPHandler_Family.CMD_KT_FAMILY_GETLISTFAMILY(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_KT_FAMILY_DESTROY:
                    {
                        result = KT_TCPHandler_Family.CMD_KT_FAMILY_DESTROY(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_KT_FAMILY_CHANGENOTIFY:
                    {
                        result = KT_TCPHandler_Family.CMD_KT_FAMILY_CHANGENOTIFY(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_KT_FAMILY_CHANGE_REQUESTJOIN_NOTIFY:
                    {
                        result = KT_TCPHandler_Family.CMD_KT_FAMILY_CHANGE_REQUESTJOIN_NOTIFY(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_KT_FAMILY_CHANGE_RANK:
                    {
                        result = KT_TCPHandler_Family.CMD_KT_FAMILY_CHANGE_RANK(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_KT_FAMILY_RESPONSE_REQUEST:
                    {
                        result = KT_TCPHandler_Family.CMD_KT_FAMILY_RESPONSE_REQUEST(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_KT_FAMILY_QUIT:
                    {
                        result = KT_TCPHandler_Family.CMD_KT_FAMILY_QUIT(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_KT_FAMILY_OPEN:
                    {
                        result = KT_TCPHandler_Family.CMD_KT_FAMILY_OPEN(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_KT_FAMILY_WEEKLYFUBENCOUNT:
                    {
                        result = KT_TCPHandler_Family.CMD_KT_FAMILY_WEEKLYFUBENCOUNT(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_KT_GUILD_CREATE:
                    {
                        result = KT_TCPHandler_Family.CMD_KT_GUILD_CREATE(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_KT_GUILD_GETINFO:
                    {
                        result = KT_TCPHandler_Family.CMD_KT_GUILD_GETINFO(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_KT_GUILD_GETMEMBERLIST:
                    {
                        result = KT_TCPHandler_Family.CMD_KT_GUILD_GETMEMBERLIST(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_KT_GUILD_CHANGERANK:
                    {
                        result = KT_TCPHandler_Family.CMD_KT_GUILD_CHANGERANK(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_KT_GUILD_KICKFAMILY:
                    {
                        result = KT_TCPHandler_Family.CMD_KT_GUILD_KICKFAMILY(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_KT_GUILD_GETGIFTED:
                    {
                        result = KT_TCPHandler_Family.CMD_KT_GUILD_GETGIFTED(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_KT_GUILD_OFFICE_RANK:
                    {
                        result = KT_TCPHandler_Family.CMD_KT_GUILD_OFFICE_RANK(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_KT_GUILD_VOTEGIFTED:
                    {
                        result = KT_TCPHandler_Family.CMD_KT_GUILD_VOTEGIFTED(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_KT_GUILD_DONATE:
                    {
                        result = KT_TCPHandler_Family.CMD_KT_GUILD_DONATE(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_KT_GUILD_TERRITORY:
                    {
                        result = KT_TCPHandler_Family.CMD_KT_GUILD_TERRITORY(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }



                case (int)TCPGameServerCmds.CMD_KT_GUILD_GETMINIGUILDINFO:
                    {
                        result = KT_TCPHandler_Family.CMD_KT_GUILD_GETMINIGUILDINFO(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }


                //Update thông tin lãnh thổ
                case (int)TCPGameServerCmds.CMD_KT_GUILD_UPDATE_TERRITORY:
                    {
                        result = KT_TCPHandler_Family.CMD_KT_GUILD_UPDATE_TERRITORY(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }


                case (int)TCPGameServerCmds.CMD_KT_GUILD_ALLTERRITORY:
                    {
                        result = KT_TCPHandler_Family.CMD_KT_GUILD_ALLTERRITORY(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_KT_GUILD_SETCITY:
                    {
                        result = KT_TCPHandler_Family.CMD_KT_GUILD_SETCITY(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_KT_GUILD_SETTAX:
                    {
                        result = KT_TCPHandler_Family.CMD_KT_GUILD_SETTAX(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_KT_GUILD_QUIT:
                    {
                        result = KT_TCPHandler_Family.CMD_KT_GUILD_QUIT(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_KT_GUILD_CHANGE_MAXWITHDRAW:
                    {
                        result = KT_TCPHandler_Family.CMD_KT_GUILD_CHANGE_MAXWITHDRAW(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_KT_GUILD_CHANGE_NOTIFY:
                    {
                        result = KT_TCPHandler_Family.CMD_KT_GUILD_CHANGE_NOTIFY(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_KT_GUILD_GETSHARE:
                    {
                        result = KT_TCPHandler_Family.CMD_KT_GUILD_GETSHARE(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_KT_GUILD_ASKJOIN:
                    {
                        //result = KT_TCPHandler.CMD_KT_GUILD_ASKJOIN(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                case (int)TCPGameServerCmds.CMD_KT_GUILD_RESPONSEASK:
                    {
                        result = KT_TCPHandler_Family.CMD_KT_GUILD_RESPONSEASK(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_KT_GUILD_INVITE:
                    {
                        // result = KT_TCPHandler.CMD_KT_GUILD_INVITE(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_KT_GUILD_DOWTIHDRAW:
                    {
                        result = KT_TCPHandler_Family.CMD_KT_GUILD_DOWTIHDRAW(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_KT_GETTERRORY_DATA:
                    {
                        result = KT_TCPHandler_Family.CMD_KT_GETTERRORY_DATA(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_KT_G2C_RECHAGE:
                    {
                        result = UserMoneyMgr.CMT_KT_LOG_RECHAGE(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_KT_GUILD_RESPONSEINVITE:
                    {
                        result = KT_TCPHandler_Family.CMD_KT_GUILD_RESPONSEINVITE(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                case (int)TCPGameServerCmds.CMD_KT_UPDATE_ROLEGUILDMONEY:
                    {
                        result = KT_TCPHandler_Family.CMD_KT_UPDATE_ROLEGUILDMONEY(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                //XONG
                case (int)TCPGameServerCmds.CMD_KT_RANKING_CHECKING:
                    {
                        result = RankingManager.CMD_KT_RANKING_CHECKING(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                //XONG
                case (int)TCPGameServerCmds.CMD_KT_GETRANK_RECORE_BYTYPE:
                    {
                        result = KTRecoreManager.CMD_KT_GETRANK_RECORE_BYTYPE(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                //XONG
                case (int)TCPGameServerCmds.CMD_KT_GETMARKVALUE:
                    {
                        result = KTRecoreManager.CMD_KT_GETMARKVALUE(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                //XONG
                case (int)TCPGameServerCmds.CMD_KT_UPDATEMARKVALUE:
                    {
                        result = KTRecoreManager.CMD_KT_UPDATEMARKVALUE(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                //XONG
                case (int)TCPGameServerCmds.CMD_KT_GET_RECORE_BYTYPE:
                    {
                        result = KTRecoreManager.CMD_KT_GET_RECORE_BYTYPE(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }
                //XONG
                case (int)TCPGameServerCmds.CMD_KT_ADD_RECORE_BYTYPE:
                    {
                        result = KTRecoreManager.CMD_KT_ADD_RECORE_BYTYPE(dbMgr, pool, nID, data, count, out tcpOutPacket);
                        break;
                    }

                default:
                    {
                        result = TCPCmdDispatcher.getInstance().dispathProcessor(client, nID, data, count);
                        break;
                    }
            }

            return result;
        }

        /// <summary>
        /// Xử lý truy vấn thông tin biến toàn cục hệ thống
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessQuerySystemGlobalParameters(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 3)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}", (TCPGameServerCmds)nID, fields.Length, cmdData));
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int type = Convert.ToInt32(fields[0]);
                int id = Convert.ToInt32(fields[1]);
                string value = fields[2];

                switch (type)
                {
                    /// GET
                    case 0:
                        {
                            value = SystemGlobalParametersManager.GetSystemGlobalParameter(id);
                            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, string.Format("{0}:{1}", id, value), nID);
                            return TCPProcessCmdResults.RESULT_DATA;
                        }
                    /// SET
                    case 1:
                        {
                            SystemGlobalParametersManager.SetSystemGlobalParameter(id, value);
                            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "", nID);
                            return TCPProcessCmdResults.RESULT_DATA;
                        }
                }
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 获取usr的二级密码
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessGetUsrSecondPassword(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 设置二级密码
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessUsrSetSecPwd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 后台修改角色货币，但是角色不在线，所以转给db处理
        /// 注意：这里相当于伪造了修改货币的消息，如果消息货币相关的协议改变了，不要忘记这里
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessRoleHuobiOffline(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, string.Format("{0}", -1), nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 3)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, string.Format("{0}", -1), nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //角色名:货币类型:修改值

                int roleid = Convert.ToInt32(fields[0]);
                string huobiType = fields[1];
                int modifyValue = Convert.ToInt32(fields[2]);

                if (string.IsNullOrEmpty(huobiType))
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("指令参数错误, CMD={0}, Recv={1}, CmdData={2}",
                       (TCPGameServerCmds)nID, fields.Length, cmdData));
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, string.Format("{0}", -1), nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleid);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("被修改货币的角色不存在，CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleid));
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, string.Format("{0}", -1), nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }
                string rolename = dbRoleInfo.RoleName;

                if ("jinbi" == huobiType)
                {
                    /*
                    //伪造一个game到db的修改金币的消息包 roleid:modifyvalue
                    string cmd = string.Format("{0}:{1}", dbRoleInfo.RoleID, modifyValue);
                    byte[] bytesCmd = new UTF8Encoding().GetBytes(cmd);
                    return ProcessUpdateUserYinLiangCmd(dbMgr, pool, nID, bytesCmd, bytesCmd.Length, out tcpOutPacket);
                     */
                }
                else if ("bangjin" == huobiType)
                {
                    /*
                    //伪造一个game到db的修改绑金的消息包 roleid:realvalue
                    int bj = dbRoleInfo.Money1 + modifyValue;
                    //bj = Math.Max(0, bj);
                    string cmd = string.Format("{0}:{1}", dbRoleInfo.RoleID, bj);
                    byte[] bytesCmd = new UTF8Encoding().GetBytes(cmd);
                    return ProcessUpdateMoney1Cmd(dbMgr, pool, nID, bytesCmd, bytesCmd.Length, out tcpOutPacket);
                     */
                }
                else if ("zuanshi" == huobiType)
                {
                    /*
                    //伪造一个game到db的修改钻石的消息包 roleid:modifyvalue:type:desc
                    string cmd = string.Format("{0}:{1}:{2}:{3}", dbRoleInfo.RoleID, modifyValue, 0, "GM要求添加");
                    byte[] bytesCmd = new UTF8Encoding().GetBytes(cmd);
                    return ProcessUpdateUserMoneyCmd(dbMgr, pool, nID, bytesCmd, bytesCmd.Length, out tcpOutPacket);
                     */
                }
                else if ("bangzuan" == huobiType)
                {
                    /*
                    //伪造一个game到db的修改绑钻的消息包 roleid:modifyvalue
                    string cmd = string.Format("{0}:{1}", dbRoleInfo.RoleID, modifyValue);
                    byte[] bytesCmd = new UTF8Encoding().GetBytes(cmd);
                    return ProcessUpdateUserGoldCmd(dbMgr, pool, nID, bytesCmd, bytesCmd.Length, out tcpOutPacket);
                     */
                }
                else if ("mojing" == huobiType)
                {
                    //伪造一个game到db的修改魔晶的消息包 roleid:paramkey:paramvalue
                    string key = "TianDiJingYuan";
                    long newVal = Global.ModifyRoleParamLongByName(dbMgr, dbRoleInfo, key, modifyValue);
                    string cmd = string.Format("{0}:{1}:{2}", dbRoleInfo.RoleID, key, newVal);
                    byte[] bytesCmd = new UTF8Encoding().GetBytes(cmd);
                    return ProcessUpdateRoleParamCmd(dbMgr, pool, nID, bytesCmd, bytesCmd.Length, out tcpOutPacket);
                }
                else if ("chengjiu" == huobiType)
                {
                    /*
                    //伪造一个game到db的修改成就的消息包 roleid:paramkey:paramvalue
                    string key = "ChengJiuData";
                    const int chengJiuIdx = 0;
                    RoleParamsData paramData = null;
                    dbRoleInfo.RoleParamsDict.TryGetValue(key, out paramData);
                    List<uint> lsUnit = Global.ParseRoleparamStreamValueToList(paramData != null ? paramData.ParamValue : "");
                    while (lsUnit.Count < (chengJiuIdx + 1))
                    {
                        lsUnit.Add(0);
                    }
                    lsUnit[chengJiuIdx] = (uint)Math.Max(0, lsUnit[chengJiuIdx] + modifyValue);
                    string newVal = Global.ParseListToRoleparamStreamValue(lsUnit);
                    string cmd = string.Format("{0}:{1}:{2}", dbRoleInfo.RoleID, key, newVal);
                    byte[] bytesCmd = new UTF8Encoding().GetBytes(cmd);
                    return ProcessUpdateRoleParamCmd(dbMgr, pool, nID, bytesCmd, bytesCmd.Length, out tcpOutPacket);
                     */
                }
                else if ("shengwang" == huobiType)
                {
                    /*
                    //伪造一个game到db的修改声望的消息包 roleid:paramkey:paramvalue
                    string key = "ShengWang";
                    long newVal = Global.AddRoleParamByName(dbMgr, dbRoleInfo, key, modifyValue);
                    string cmd = string.Format("{0}:{1}:{2}", dbRoleInfo.RoleID, key, newVal);
                    byte[] bytesCmd = new UTF8Encoding().GetBytes(cmd);
                    return ProcessUpdateRoleParamCmd(dbMgr, pool, nID, bytesCmd, bytesCmd.Length, out tcpOutPacket);
                     */
                }
                else if ("xinghun" == huobiType)
                {
                    /*
                    //伪造一个game到db的修改星魂的消息包 roleid:paramkey:paramvalue
                    string key = "StarSoul";
                    long newVal = Global.AddRoleParamByName(dbMgr, dbRoleInfo, key, modifyValue);
                    string cmd = string.Format("{0}:{1}:{2}", dbRoleInfo.RoleID, key, newVal);
                    byte[] bytesCmd = new UTF8Encoding().GetBytes(cmd);
                    return ProcessUpdateRoleParamCmd(dbMgr, pool, nID, bytesCmd, bytesCmd.Length, out tcpOutPacket);
                     */
                }
                else if ("lingjing" == huobiType)
                {
                    //伪造一个game到db的修改灵晶的消息包 roleid:paramkey:paramvalue
                    string key = "MUMoHe";
                    long newVal = Global.ModifyRoleParamLongByName(dbMgr, dbRoleInfo, key, modifyValue);
                    string cmd = string.Format("{0}:{1}:{2}", dbRoleInfo.RoleID, key, newVal);
                    byte[] bytesCmd = new UTF8Encoding().GetBytes(cmd);
                    return ProcessUpdateRoleParamCmd(dbMgr, pool, nID, bytesCmd, bytesCmd.Length, out tcpOutPacket);
                }
                else if ("fenmo" == huobiType)
                {
                    /*
                    //伪造一个game到db的修改粉末的消息包 roleid:paramkey:paramvalue
                    string key = "ElementPowder";
                    long newVal = Global.AddRoleParamByName(dbMgr, dbRoleInfo, key, modifyValue);
                    string cmd = string.Format("{0}:{1}:{2}", dbRoleInfo.RoleID, key, newVal);
                    byte[] bytesCmd = new UTF8Encoding().GetBytes(cmd);
                    return ProcessUpdateRoleParamCmd(dbMgr, pool, nID, bytesCmd, bytesCmd.Length, out tcpOutPacket);
                     */
                }
                else if ("zaizao" == huobiType)
                {
                    //伪造一个game到db的修改再造的消息包 roleid:paramkey:paramvalue
                    string key = "ZaiZaoPoint";
                    long newVal = Global.ModifyRoleParamLongByName(dbMgr, dbRoleInfo, key, modifyValue);
                    string cmd = string.Format("{0}:{1}:{2}", dbRoleInfo.RoleID, key, newVal);
                    byte[] bytesCmd = new UTF8Encoding().GetBytes(cmd);
                    return ProcessUpdateRoleParamCmd(dbMgr, pool, nID, bytesCmd, bytesCmd.Length, out tcpOutPacket);
                }
                else
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("未注册的GM修改货币类型,Rolename:{0},Huobi:{1},Modify:{2}", rolename, huobiType, modifyValue));
                }
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }
            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, string.Format("{0}", -1), nID);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 月卡信息本来该由gameserver处理，但是刚买月卡就掉线了这种情况转交给dbserver处理
        /// 既然买了月卡，那么用户肯定登录了，所以此时的月卡信息也被修正了
        /// 之前没有月卡的，直接加一张月卡
        /// 之前月卡有效的，直接加30天
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessRoleBuyYueKaButOffline(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, string.Format("{0}", -1), nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 3)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, string.Format("{0}", -1), nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int beginOffsetDay = Convert.ToInt32(fields[1]);
                string key = fields[2];

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, string.Format("{0}", -1), nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                lock (dbRoleInfo)
                {
                    string[] oldFields = null;
                    string oldParamValue = "";
                    string paramValue;

                    string oldParamValueStr = Global.GetRoleParamByName(dbRoleInfo, key);
                    if (!string.IsNullOrEmpty(oldParamValueStr))
                    {
                        oldFields = oldParamValueStr.Split(',');
                        if (oldFields.Length == 5 && oldFields[0] == "1")
                        {
                            oldParamValue = oldParamValueStr;
                        }
                    }

                    RoleParamsData value = null;
                    //月卡过期或者无月卡, 新加一个月卡
                    if (string.IsNullOrEmpty(oldParamValue))
                    {
                        paramValue = string.Format("1,{0},{1},{2},0", beginOffsetDay, beginOffsetDay + 30, beginOffsetDay);
                    }
                    else//有月卡且月卡有效, 直接添加30天
                    {
                        paramValue = string.Format("{0},{1},{2},{3},{4}", oldFields[0], oldFields[1], Convert.ToInt32(oldFields[2]) + 30, oldFields[3], oldFields[4]);
                    }

                    Global.UpdateRoleParamByName(dbMgr, dbRoleInfo, key, paramValue);
                }

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, string.Format("{0}", 0), nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }
            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, string.Format("{0}", -1), nID);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 处理注册用户ID的操作
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessRegUserIDCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                //解析用户名称和用户密码
                string[] fields = cmdData.Split(':');
                if (fields.Length != 3)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                string userID = fields[0];
                int serverLineID = Convert.ToInt32(fields[1]);
                int state = Convert.ToInt32(fields[2]);

                int ret = 1;
                long logoutServerTicks = 0;
                if (!UserOnlineManager.RegisterUserID(userID, serverLineID, state))
                {
                    ret = 0;
                }

                DBUserInfo dbUserInfo = dbMgr.GetDBUserInfo(userID);
                if (dbUserInfo != null)
                {
                    lock (dbUserInfo)
                    {
                        logoutServerTicks = dbUserInfo.LogoutServerTicks;
                    }
                }

                string strcmd = string.Format("{0}:{1}", ret, logoutServerTicks);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// Truy vấn danh sách nhân vật
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessGetRoleListCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                string userID = fields[0];
                int zoneID = Convert.ToInt32(fields[1]);

                DBUserInfo dbUserInfo = dbMgr.GetDBUserInfo(userID);
                string strcmd = "";
                if (null == dbUserInfo)
                {
                    strcmd = string.Format("{0}:{1}", 0, "");
                }
                else
                {
                    int nRoleCount = 0;
                    string roleList = "";
                    //lock (dbUserInfo)
                    {
                        for (int i = 0; i < dbUserInfo.ListRoleIDs.Count; i++)
                        {
                            if (dbUserInfo.ListRoleZoneIDs[i] != zoneID)
                            {
                                continue;
                            }

                            // Thời gian xóa nhân vật để là 0 luôn đỡ phải querry
                            int PreDelLeftSeconds = 0;//;GameDBManager.PreDelRoleMgr.CalcPreDeleteRoleLeftSeconds(dbUserInfo.ListRolePreRemoveTime[i]);

                            // Lấy ra role
                            DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(dbUserInfo.ListRoleIDs[i]);

                            /// Danh sách trang bị
                            int armorID = -1, helmID = -1, weaponID = -1, weaponEnhanceLevel = 0, mantleID = -1;
                            /// Nếu có tồn tại danh sách vật phẩm trên người
                            if (dbRoleInfo.GoodsDataList != null)
                            {
                                List<GoodsData> listRoleEquipMini = dbRoleInfo.GoodsDataList.Values.Where(x => x.Using == 1 || x.Using == 0 || x.Using == 3 || x.Using == 15).ToList();
                                foreach (GoodsData itemGD in listRoleEquipMini)
                                {
                                    switch (itemGD.Using)
                                    {
                                        case 0:
                                            {
                                                helmID = itemGD.GoodsID;
                                                break;
                                            }
                                        case 1:
                                            {
                                                armorID = itemGD.GoodsID;
                                                break;
                                            }
                                        case 3:
                                            {
                                                weaponID = itemGD.GoodsID;
                                                weaponEnhanceLevel = itemGD.Forge_level;
                                                break;
                                            }
                                        case 15:
                                            {
                                                mantleID = itemGD.GoodsID;
                                                break;
                                            }
                                    }
                                }
                            }

                            string roleEquipMiniString = string.Format("{0}_{1}_{2}_{3}_{4}", armorID, helmID, weaponID, weaponEnhanceLevel, mantleID);

                            nRoleCount++;
                            roleList += string.Format("{0}${1}${2}${3}${4}${5}${6}|",
                                dbUserInfo.ListRoleIDs[i], dbUserInfo.ListRoleSexes[i], dbUserInfo.ListRoleOccups[i], dbUserInfo.ListRoleNames[i].Replace('|', '*'),
                                dbUserInfo.ListRoleLevels[i], PreDelLeftSeconds, roleEquipMiniString);
                        }
                    }

                    roleList = roleList.Trim('|');
                    strcmd = string.Format("{0}:{1}", nRoleCount, roleList);
                }

                byte[] bytesCmd = new UTF8Encoding().GetBytes(strcmd);
                tcpOutPacket = pool.Pop();
                tcpOutPacket.PacketCmdID = (int)TCPGameServerCmds.CMD_ROLE_LIST;
                tcpOutPacket.FinalWriteData(bytesCmd, 0, bytesCmd.Length);

                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// Thực hiện tạo nhân vật
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessCreateRoleCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 8)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}", (TCPGameServerCmds)nID, fields.Length, cmdData));
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                /// UserID
                string userID = fields[0];
                /// UserName
                string userName = fields[1];
                /// Giới tính
                int sex = Convert.ToInt32(fields[2]);
                /// ID phái
                int factionID = Convert.ToInt32(fields[3]);
                /// Tên nhân vật
                string rolename = fields[4].Split('$')[0];
                /// ID Server
                int serverID = Convert.ToInt32(fields[5]);
                /// ID Tân thủ thôn
                int villageID = Convert.ToInt32(fields[6]);
                /// Thông tin vị trí
                string positionInfo = fields[7].Replace(',', ':');

                string strcmd = "";

                /// Kiểm tra tổng số nhân vật đã đầy chưa
                if (DBWriter.CheckRoleCountFull(dbMgr))
                {
                    strcmd = string.Format("{0}:{1}", -2, string.Format("{0}${1}${2}${3}${4}${5}", "", "", "", "", "", ""));

                    LogManager.WriteLog(LogTypes.Error, string.Format("Server Role data is full，CMD={0}, RoleID={1}", (TCPGameServerCmds)nID, -1));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_CREATE_ROLE);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                /// Kiểm tra tên nhân vật có thể được sử dụng không
                if (!NameManager.Instance().IsNameCanUseInDb(dbMgr, rolename))
                {
                    strcmd = string.Format("{0}:{1}", -3, string.Format("{0}${1}${2}${3}${4}${5}", "", "", "", "", "", ""));

                    LogManager.WriteLog(LogTypes.Error, string.Format("Name is invalid or already exist CMD={0}, RoleName={1}", (TCPGameServerCmds)nID, rolename));
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_CREATE_ROLE);
                    return TCPProcessCmdResults.RESULT_DATA;
                }
                if (!NameUsedMgr.Instance().AddCannotUse_Ex(rolename) || dbMgr.IsRolenameExist(rolename))
                {
                    //添加新角色失败
                    strcmd = string.Format("{0}:{1}", -1, string.Format("{0}${1}${2}${3}${4}${5}", "", "", "", "", "", ""));

                    LogManager.WriteLog(LogTypes.Error, string.Format("Name is already exist or param is invalid CMD={0}, RoleID={1}",
                                       (TCPGameServerCmds)nID, -1));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_CREATE_ROLE);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                /// Cấp độ ban đầu
                int nInitLevel = 1;
                /// Tổng số nhân vật
                int totalRoleCount = 0;
                /// Thông tin nhân vật tương ứng
                DBUserInfo dbUserInfo = dbMgr.GetDBUserInfo(userID);
                if (null != dbUserInfo)
                {
                    //lock (dbUserInfo)
                    {
                        for (int i = 0; i < dbUserInfo.ListRoleIDs.Count; i++)
                        {
                            if (dbUserInfo.ListRoleZoneIDs[i] != serverID)
                            {
                                continue;
                            }

                            totalRoleCount++;
                        }
                    }
                }

                /// Nếu tổng số nhân vật >= 4 thì toác
                if (totalRoleCount >= 4)
                {
                    NameUsedMgr.Instance().DelCannotUse_Ex(rolename);
                    strcmd = string.Format("{0}:{1}", -1000, string.Format("{0}${1}${2}${3}${4}${5}", "", "", "", "", "", ""));
                    LogManager.WriteLog(LogTypes.Warning, string.Format("Can not add more role, full 4 roles already on this account CMD={0}, RoleID={1}", (TCPGameServerCmds)nID, -1000));
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_CREATE_ROLE);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                /// Thực hiện tạo nhân vật
                int roleID = DBWriter.CreateRole(dbMgr, userID, userName, sex, factionID, 0, rolename, serverID, (int)RoleCreateConstant.GridNum, positionInfo, 1);
                if (0 > roleID)
                {
                    NameUsedMgr.Instance().DelCannotUse_Ex(rolename);
                    strcmd = string.Format("{0}:{1}", roleID, string.Format("{0}${1}${2}${3}${4}${5}", "", "", "", "", "", ""));
                    LogManager.WriteLog(LogTypes.Error, string.Format("Create new role faild CMD={0}, RoleID={1}", (TCPGameServerCmds)nID, roleID));
                }
                else
                {
                    /// Cập nhật thông tin
                    DBWriter.UpdateRolePBInfo(dbMgr, roleID, 60);

                    dbUserInfo = dbMgr.GetDBUserInfo(userID);
                    if (null != dbUserInfo)
                    {
                        lock (dbUserInfo)
                        {
                            dbUserInfo.ListRoleIDs.Add(roleID);
                            dbUserInfo.ListRoleSexes.Add(sex);
                            dbUserInfo.ListRoleOccups.Add(factionID);
                            dbUserInfo.ListRoleNames.Add(rolename);
                            dbUserInfo.ListRoleLevels.Add(nInitLevel);
                            dbUserInfo.ListRoleZoneIDs.Add(serverID);
                        }
                    }

                    strcmd = string.Format("{0}:{1}", 1, string.Format("{0}${1}${2}${3}${4}${5}${6}", roleID, sex, factionID, rolename, nInitLevel, 0, "-1_-1_-1_0_-1"));
                }

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, (int)TCPGameServerCmds.CMD_CREATE_ROLE);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// Sự kiện người chơi vào Game
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessInitGameCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 3 && fields.Length != 2 && fields.Length != 4)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}", (TCPGameServerCmds)nID, fields.Length, cmdData));
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                string userID = fields[0];
                int roleID = Convert.ToInt32(fields[1]);

                RoleDataEx roleDataEx = new RoleDataEx();

                bool failed = false;

                if (DBQuery.IsBlackUserID(dbMgr, userID))
                {
                    failed = true;
                    roleDataEx.RoleID = -70;
                    LogManager.WriteLog(LogTypes.Error, string.Format("User is forbidden to log in，CMD={0}, UserID={1}", (TCPGameServerCmds)nID, userID));
                    tcpOutPacket = DataHelper.ObjectToTCPOutPacket<RoleDataEx>(roleDataEx, pool, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                DBUserInfo dbUserInfo = dbMgr.GetDBUserInfo(userID);
                if (null == dbUserInfo)
                {
                    failed = true;
                    roleDataEx.RoleID = -2;

                    LogManager.WriteLog(LogTypes.Error, string.Format("Failed to get user data，CMD={0}, UserID={1}",
                        (TCPGameServerCmds)nID, userID));
                }
                else
                {
                    bool hasrole = false;
                    foreach (var role in dbUserInfo.ListRoleIDs)
                    {
                        if (role == roleID)
                        {
                            hasrole = true;
                            break;
                        }
                    }
                    if (!hasrole)
                    {
                        failed = true;
                        roleDataEx.RoleID = -2;
                        LogManager.WriteLog(LogTypes.Error, string.Format("Failed to get role data，CMD={0}, UserID={1}, RoleID={2}", (TCPGameServerCmds)nID, userID, roleID));
                    }
                    else
                    {
                        lock (dbUserInfo)
                        {
                            roleDataEx.Token = dbUserInfo.Money;
                            roleDataEx.PushMessageID = dbUserInfo.PushMessageID;
                        }
                    }
                }

                if (null != dbUserInfo && !failed)
                {
                    DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                    if (null == dbRoleInfo)
                    {
                        roleDataEx.RoleID = -1;
                        LogManager.WriteLog(LogTypes.Error, string.Format("Failed to get user data，CMD={0}, RoleID={1}", (TCPGameServerCmds)nID, roleID));
                    }
                    else if (BanManager.IsBanRoleName(Global.FormatRoleName(dbRoleInfo)) > 0 || BanManager.IsBanRoleName(dbRoleInfo.RoleID + "$rid") > 0)
                    {
                        roleDataEx.RoleID = -10;
                        LogManager.WriteLog(LogTypes.Error, string.Format("User is banned by Admin，CMD={0}, RoleID={1}", (TCPGameServerCmds)nID, roleID));
                    }
                    else if (dbRoleInfo.BanLogin > 0)
                    {
                        roleDataEx.RoleID = -10;
                        LogManager.WriteLog(LogTypes.Error, string.Format("User is banned by Admin，CMD={0}, RoleID={1}", (TCPGameServerCmds)nID, roleID));
                    }
                    else
                    {
                        dbRoleInfo.LastTime = TimeUtil.NOW();

                        /// Lưu lại thông tin nhân vật
                        CacheManager.AddRoleMiniInfo(roleID, dbRoleInfo.ZoneID, dbRoleInfo.UserID);

                        /// Chuyển thông tin nhân vật thành RoleDataEx
                        Global.DBRoleInfo2RoleDataEx(dbRoleInfo, roleDataEx);

                        /// Thực thi sự kiện đăng nhập
                        GlobalEventSource.getInstance().fireEvent(new PlayerLoginEventObject(dbRoleInfo));

                        /// Gắn dữ liệu
                        roleDataEx.userMiniData = dbUserInfo.GetUserMiniData(userID, roleID, dbRoleInfo.ZoneID);
                    }
                }

                tcpOutPacket = DataHelper.ObjectToTCPOutPacket<RoleDataEx>(roleDataEx, pool, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 处理角色上线的操作
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessRoleOnLineGameCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                //解析用户名称和用户密码
                string[] fields = cmdData.Split(':');
                if (fields.Length != 4)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int serverLineID = Convert.ToInt32(fields[1]);
                int loginNum = Convert.ToInt32(fields[2]);
                string ip = fields[3];

                string strcmd = "";
                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int dayID = DateTime.Now.DayOfYear;

                int loginDayID = 0;
                int loginDayNum = 0;
                lock (dbRoleInfo)
                {
                    dbRoleInfo.ServerLineID = serverLineID;
                    dbRoleInfo.LoginNum = loginNum;
                    dbRoleInfo.LastTime = DateTime.Now.Ticks / 10000;

                    if (dayID != dbRoleInfo.LoginDayID)
                    {
                        dbRoleInfo.LoginDayNum++;
                        dbRoleInfo.LoginDayID = dayID;
                    }

                    loginDayID = dbRoleInfo.LoginDayID;
                    loginDayNum = dbRoleInfo.LoginDayNum;

                    dbRoleInfo.LastIP = ip;
                }

                // DBWriter.UpdateRoleLoginInfo(dbMgr, roleID, loginNum, loginDayID, loginDayNum, dbRoleInfo.UserID, dbRoleInfo.ZoneID, ip);

                RoleOnlineManager.UpdateRoleOnlineTicks(roleID);

                //DBWriter.InsertCityInfo(dbMgr, ip, dbRoleInfo.UserID);

                // Global.WriteRoleInfoLog(dbMgr, dbRoleInfo);

                strcmd = string.Format("{0}:{1}", roleID, 0);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 处理更新活跃统计操作
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessUpdateAccountActiveCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                //解析用户名称和用户密码
                string[] fields = cmdData.Split(':');
                if (fields.Length != 1)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                DBUserActiveInfo.getInstance().UpdateAccountActiveInfo(dbMgr, fields[0]);
                string strcmd = string.Format("{0}", 0);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 处理角色在线的心跳操作
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessRoleHeartGameCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                //解析用户名称和用户密码
                string[] fields = cmdData.Split(':');
                if (fields.Length != 1)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);

                string strcmd = "";
                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //记录心跳信息
                RoleOnlineManager.UpdateRoleOnlineTicks(roleID);

                strcmd = string.Format("{0}:{1}", roleID, 0);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// Gói tin thông báo đối tượng rời mạng
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessRoleOffLineGameCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                //解析用户名称和用户密码
                string[] fields = cmdData.Split(':');
                if (fields.Length < 4)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int serverLineID = Convert.ToInt32(fields[1]);
                string ip = fields[2];
                int activeVal = Convert.ToInt32(fields[3]);

                long logoutServerTicks = 0;
                if (fields.Length >= 5)
                {
                    //来自GameServer的离线时间
                    logoutServerTicks = Convert.ToInt64(fields[4]);
                }

                string strcmd = "";
                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int pkMode = 0, horseDbID = 0, petDbID = 0;
                int onlineSecs = 0;
                lock (dbRoleInfo)
                {
                    dbRoleInfo.ServerLineID = -1;
                    dbRoleInfo.LogOffTime = DateTime.Now.Ticks / 10000;
                    pkMode = dbRoleInfo.PKMode;

                    onlineSecs = Math.Min((int)((dbRoleInfo.LogOffTime - dbRoleInfo.LastTime) / 1000), 86400);
                }
                dbRoleInfo.RankValue.Clear();

                DBUserInfo dbUserInfo = dbMgr.GetDBUserInfo(dbRoleInfo.UserID);
                if (null != dbUserInfo)
                {
                    lock (dbUserInfo)
                    {
                        dbUserInfo.LogoutServerTicks = logoutServerTicks;
                    }
                }

                DBWriter.UpdateRoleLogOff(dbMgr, roleID, dbRoleInfo.UserID, dbRoleInfo.ZoneID, ip, onlineSecs);

                DBWriter.UpdatePKMode(dbMgr, roleID, pkMode);

                RoleOnlineManager.RemoveRoleOnlineTicks(roleID);

                strcmd = string.Format("{0}:{1}", roleID, 0);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 处理获取在线服务器的操作
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessGetServerListCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("解析指令字符串错误 , CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                //解析用户名称和用户密码
                string[] fields = cmdData.Split(':');
                if (fields.Length != 3)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                string lineToken = fields[0];
                string userID = fields[1];
                int verSign = Convert.ToInt32(fields[2]);

                int rolesCount = 0;
                DBUserInfo dbUserInfo = dbMgr.dbUserMgr.FindDBUserInfo(userID);
                if (null != dbUserInfo)
                {
                    lock (dbUserInfo)
                    {
                        rolesCount = dbUserInfo.ListRoleNames.Count;
                    }
                }

                ServerListData serverListData = new ServerListData()
                {
                    RetCode = 0,
                    RolesCount = rolesCount,
                    LineDataList = null,
                };

                List<LineData> lineDataList = new List<LineData>();

                //先判断版本是否匹配
                if (verSign != (int)TCPCmdProtocolVer.VerSign)
                {
                    serverListData.RetCode = -1;
                }
                //else if (Global.GetUserOnlineState(dbUserInfo) > 0) //判断用户是否在线? 直接交给GameServer判断
                //{
                //    serverListData.RetCode = -2;
                //}
                else
                {
                    //线路管理
                    List<LineItem> itemList = LineManager.GetLineItemList();
                    for (int i = 0; i < itemList.Count; i++)
                    {
                        lineDataList.Add(Global.LineItemToLineData(itemList[i]));
                    }
                    itemList = null;

                    serverListData.LineDataList = lineDataList;
                }

                /// 将对象转为TCP协议流
                tcpOutPacket = DataHelper.ObjectToTCPOutPacket<ServerListData>(serverListData, pool, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                // });
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 处理游戏服务器在线心跳的操作
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessOnlineServerHeartCmd(GameServerClient client, DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("解析指令字符串错误 , CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                //解析用户名称和用户密码
                string[] fields = cmdData.Split(':');
                if (fields.Length != 3)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int serverLineID = Convert.ToInt32(fields[0]);
                int serverLineNum = Convert.ToInt32(fields[1]);
                int serverLineCount = Convert.ToInt32(fields[2]);

                if (serverLineCount <= 0) //刚上线的服务器
                {
                    //清空指定线路ID对应的所有用户数据
                    UserOnlineManager.ClearUserIDsByServerLineID(serverLineID);
                }

                //更新服务器状态
                LineManager.UpdateLineHeart(client, serverLineID, serverLineNum);

                string strcmd = string.Format("{0}", 0);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 获取服务器ID
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessGetServerIdCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            byte[] bytes = null;
            try
            {
                bytes = DataHelper.ObjectToBytes(GameDBManager.ZoneID);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, bytes, 0, bytes.Length, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            bytes = DataHelper.ObjectToBytes(GameDBManager.ZoneID);
            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, bytes, 0, bytes.Length, (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 处理添加新任务的操作
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessNewTaskCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int npcID = Convert.ToInt32(fields[1]);
                int taskID = Convert.ToInt32(fields[2]);
                int focus = Convert.ToInt32(fields[3]);
                int nStarLevel = Convert.ToInt32(fields[4]); // 任务星级 [12/5/2013 LiaoWei]

                DateTime now = DateTime.Now;
                string today = now.ToString("yyyy-MM-dd HH:mm:ss");
                long ticks = (now.Ticks / 10000);

                string strcmd = "";
                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //将用户的请求发起写数据库的操作
                int ret = DBWriter.NewTask(dbMgr, roleID, npcID, taskID, today, focus, nStarLevel);
                if (ret < 0)
                {
                    //添加任务失败
                    strcmd = string.Format("{0}:{1}:{2}:{3}", roleID, taskID, ticks, ret);

                    LogManager.WriteLog(LogTypes.Error, string.Format("添加任务失败，CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));
                }
                else
                {
                    if (null == dbRoleInfo.DoingTaskList)
                    {
                        dbRoleInfo.DoingTaskList = new ConcurrentDictionary<int, TaskData>();
                    }

                    TaskData task = new TaskData()
                    {
                        DbID = ret,
                        DoingTaskID = taskID,
                        DoingTaskVal1 = 0,
                        DoingTaskVal2 = 0,
                        DoingTaskFocus = focus,
                        AddDateTime = ticks,
                        StarLevel = nStarLevel,
                    };

                    dbRoleInfo.DoingTaskList[task.DbID] = task;

                    strcmd = string.Format("{0}:{1}:{2}:{3}", roleID, taskID, ticks, ret);
                }

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 处理更新角色位置的操作
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessUpdatePosCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int mapCode = Convert.ToInt32(fields[1]);
                int direction = Convert.ToInt32(fields[2]);
                int posX = Convert.ToInt32(fields[3]);
                int posY = Convert.ToInt32(fields[4]);

                string strcmd = "";
                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                long ticks = DateTime.Now.Ticks / 10000;
                strcmd = string.Format("{0}:{1}:{2}:{3}", mapCode, direction, posX, posY);

                //是否更新数据库
                bool updateDBRolePosition = true;

                //2011-05-31 不在直接处理，有offline处理
                //将用户的请求更新内存缓存
                lock (dbRoleInfo)
                {
                    dbRoleInfo.Position = strcmd;
                    //if (ticks - dbRoleInfo.UpdateDBPositionTicks >= (2 * 60 * 1000))
                    //{
                    //    updateDBRolePosition = true;
                    //    dbRoleInfo.UpdateDBPositionTicks = ticks;
                    //}
                }

                //将用户的请求发起写数据库的操作
                bool ret = true;

                //判断是否写入数据库
                if (updateDBRolePosition)
                {
                    ret = DBWriter.UpdateRolePosition(dbMgr, roleID, strcmd);
                }

                if (!ret)
                {
                    //更新角色位置失败
                    strcmd = string.Format("{0}:{1}", roleID, -1);

                    LogManager.WriteLog(LogTypes.Error, string.Format("更新角色位置失败，CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));
                }
                else
                {
                    strcmd = string.Format("{0}:{1}", roleID, 0);
                }

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// Thêm kinh nghiệm cho người chơi
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessUpdateExpLevelCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 4)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int level = Convert.ToInt32(fields[1]);
                long experience = Convert.ToInt64(fields[2]);
                int Prestige = Convert.ToInt32(fields[3]);

                Console.WriteLine("SAVE ROLE :" + roleID + "LEVEL : " + level + " | experience :" + experience);

                string strcmd = "";
                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                bool ret = DBWriter.UpdateRoleInfo(dbMgr, roleID, level, experience, Prestige);
                if (!ret)
                {
                    strcmd = string.Format("{0}:{1}", roleID, -1);

                    LogManager.WriteLog(LogTypes.Error, string.Format("Có lỗ khi ghi vào csdl，CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));
                }
                else
                {
                    string userID = "";
                    lock (dbRoleInfo)
                    {
                        dbRoleInfo.Level = level;
                        dbRoleInfo.Experience = experience;
                        dbRoleInfo.Prestige = Prestige;
                        userID = dbRoleInfo.UserID;
                    }

                    // DISABLE REDUCT COST
                    //if (userID != "")
                    //{
                    //    DBUserInfo dbUserInfo = dbMgr.GetDBUserInfo(userID);
                    //    if (null != dbUserInfo)
                    //    {
                    //        lock (dbUserInfo)
                    //        {
                    //            for (int i = 0; i < dbUserInfo.ListRoleLevels.Count; i++)
                    //            {
                    //                if (dbUserInfo.ListRoleIDs[i] == roleID)
                    //                {
                    //                    dbUserInfo.ListRoleLevels[i] = level;
                    //                }
                    //            }
                    //        }
                    //    }
                    //}

                    strcmd = string.Format("{0}:{1}", roleID, 0);
                }

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// Cập nhật Avarta nhân vật
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessUpdateRoleAvartaCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int avartaID = Convert.ToInt32(fields[1]);

                string strcmd = "";
                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                bool updateDBRoleAvarta = true;

                dbRoleInfo.RolePic = avartaID;

                bool ret = true;
                if (updateDBRoleAvarta)
                {
                    ret = DBWriter.UpdateRoleAvarta(dbMgr, roleID, avartaID);
                }

                if (!ret)
                {
                    strcmd = string.Format("{0}:{1}", roleID, -1);
                    LogManager.WriteLog(LogTypes.Error, string.Format("Update role avarta faild，CMD={0}, RoleID={1}", (TCPGameServerCmds)nID, roleID));
                }
                else
                {
                    strcmd = string.Format("{0}:{1}", roleID, 0);
                }

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 处理更新角色游戏币1操作
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessUpdateMoney1Cmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            /*
             *  注意：如果协议修改了，一定不要忘记确认下是否需要修改ProcessRoleHuobiOffline这个函数
             *  chenjg 20150422
             */
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int meney1 = Convert.ToInt32(fields[1]);

                string strcmd = "";
                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                bool ret = DBWriter.UpdateRoleMoney1(dbMgr, roleID, meney1);
                if (!ret)
                {
                    strcmd = string.Format("{0}:{1}", roleID, -1);

                    LogManager.WriteLog(LogTypes.Error, string.Format("更新角色金币失败，CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));
                }
                else
                {
                    lock (dbRoleInfo.GetMoneyLock)
                    {
                        dbRoleInfo.Money1 = meney1;
                    }

                    strcmd = string.Format("{0}:{1}", roleID, meney1);
                }

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// Thêm vật phẩm
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessAddGoodsCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 13)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}", (TCPGameServerCmds)nID, fields.Length, cmdData));
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int goodsID = Convert.ToInt32(fields[1]);
                int goodsNum = Convert.ToInt32(fields[2]);
                string props = fields[3];
                int forgeLevel = Convert.ToInt32(fields[4]);
                int binding = Convert.ToInt32(fields[5]);
                int site = Convert.ToInt32(fields[6]);
                int bagindex = Convert.ToInt32(fields[7]);
                string startTime = fields[8];
                startTime = startTime.Replace("$", ":");
                string endTime = fields[9];
                endTime = endTime.Replace("$", ":");
                int strong = Convert.ToInt32(fields[10]);
                int series = Convert.ToInt32(fields[11]);
                string otherpramer = fields[12];

                byte[] Base64Decode = Convert.FromBase64String(otherpramer);

                Dictionary<ItemPramenter, string> _OtherParams = DataHelper.BytesToObject<Dictionary<ItemPramenter, string>>(Base64Decode, 0, Base64Decode.Length);

                string strcmd = "";
                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}", (TCPGameServerCmds)nID, roleID));
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //Ghi vào DB
                int ret = DBWriter.NewGoods(dbMgr, roleID, goodsID, goodsNum, props, forgeLevel, binding, site, bagindex, startTime, endTime, strong, series, otherpramer);
                if (ret < 0)
                {
                    strcmd = string.Format("{0}:{1}", ret, cmdData);
                    LogManager.WriteLog(LogTypes.Error, string.Format("Có lỗi khi ghi vật phẩm vào DB，CMD={0}, RoleID={1}", (TCPGameServerCmds)nID, roleID));
                }
                else
                {
                    GoodsData itemGD = new GoodsData()
                    {
                        Id = ret,
                        GoodsID = goodsID,
                        Using = -1,
                        Forge_level = forgeLevel,
                        Starttime = startTime,
                        Endtime = endTime,
                        Site = site,
                        Props = props,
                        GCount = goodsNum,
                        Binding = binding,
                        BagIndex = bagindex,
                        Strong = strong,
                        OtherParams = _OtherParams,
                        Series = series,
                    };

                    /// Nếu danh sách chưa tồn taiọ
                    if (null == dbRoleInfo.GoodsDataList)
                    {
                        /// Tạo mới
                        dbRoleInfo.GoodsDataList = new ConcurrentDictionary<int, GoodsData>();
                    }

                    /// Thêm vật phẩm vào danh sách
                    dbRoleInfo.GoodsDataList[itemGD.Id] = itemGD;

                    /// NẾu mà sử dụng túi thì POrtalBangData tăng thêm ô
                    if ((int)SaleGoodsConsts.PortableGoodsID == site)
                    {
                        dbRoleInfo.MyPortableBagData.GoodsUsedGridNum++;
                    }

                    strcmd = string.Format("{0}:{1}", ret, cmdData);
                }

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// Hàm update Good CMD
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessUpdateGoodsCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');

                if (fields.Length != 16)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}", (TCPGameServerCmds)nID, fields.Length, cmdData));
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int id = Convert.ToInt32(fields[1]);

                string strcmd = "";

                // nếu sử dụng quee
                if (GameDBManager.IsUsingQueeItem)
                {
                    ItemCacheModel _CacheModel = new ItemCacheModel();
                    _CacheModel.Fields = fields;
                    _CacheModel.ItemID = id;
                    _CacheModel.RoleId = roleID;

                    ItemManager.getInstance().AddItemProsecc(_CacheModel);

                    strcmd = string.Format("{0}:{1}", id, 0);
                }
                else
                {
                    DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                    if (null == dbRoleInfo)
                    {
                        LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                            (TCPGameServerCmds)nID, roleID));

                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }
                    /// Thông tin vật phẩm tương ứng
                    GoodsData itemGD = Global.GetGoodsDataByDbID(dbRoleInfo, id);

                    //Lấy thông tin GOOD TỪ DATA RA Nếu không tìm thấy vật phẩm thì return lỗi -1000
                    if (null == itemGD)
                    {
                        strcmd = string.Format("{0}:{1}", id, -1000);
                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }

                    //Thực hiện update vào DB theo lệnh
                    int ret = DBWriter.UpdateGoods(dbMgr, id, fields, 2);
                    if (ret < 0)
                    {
                        strcmd = string.Format("{0}:{1}", id, ret);
                        LogManager.WriteLog(LogTypes.Error, string.Format("Update role item failed，CMD={0}, RoleID={1}", (TCPGameServerCmds)nID, roleID));
                    }
                    else
                    {
                        int gcount = DataHelper.ConvertToInt32(fields[8], itemGD.GCount);

                        if (gcount > 0)
                        {
                            int newSite = DataHelper.ConvertToInt32(fields[6], itemGD.Site);

                            itemGD.Using = DataHelper.ConvertToInt32(fields[2], itemGD.Using);
                            itemGD.Forge_level = DataHelper.ConvertToInt32(fields[3], itemGD.Forge_level);
                            itemGD.Starttime = DataHelper.ConvertToStr(fields[4].Replace("#", ":"), itemGD.Starttime);
                            itemGD.Endtime = DataHelper.ConvertToStr(fields[5].Replace("#", ":"), itemGD.Endtime);
                            itemGD.Site = newSite;
                            itemGD.Props = DataHelper.ConvertToStr(fields[7], itemGD.Props);
                            itemGD.GCount = gcount;
                            itemGD.Binding = DataHelper.ConvertToInt32(fields[9], itemGD.Binding);
                            itemGD.BagIndex = DataHelper.ConvertToInt32(fields[10], itemGD.BagIndex);
                            itemGD.Strong = DataHelper.ConvertToInt32(fields[11], itemGD.Strong);
                            itemGD.Series = DataHelper.ConvertToInt32(fields[12], itemGD.Series);

                            string otherpramer = fields[13];
                            if (otherpramer.Length > 10)
                            {
                                byte[] Base64Decode = Convert.FromBase64String(otherpramer);

                                Dictionary<ItemPramenter, string> _OtherParams = DataHelper.BytesToObject<Dictionary<ItemPramenter, string>>(Base64Decode, 0, Base64Decode.Length);
                                itemGD.OtherParams = _OtherParams;
                            }

                            itemGD.GoodsID = DataHelper.ConvertToInt32(fields[14], itemGD.GoodsID);
                        }
                        else
                        {
                            if (GameDBManager.Flag_t_goods_delete_immediately)
                            {
                                DBWriter.MoveGoodsDataToBackupTable(dbMgr, id);
                            }

                            /// Xóa vật phẩm khỏi danh sách
                            dbRoleInfo.GoodsDataList.TryRemove(itemGD.Id, out _);
                        }

                        strcmd = string.Format("{0}:{1}", id, 0);
                    }
                }

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// Cập nhật thông tin nhiệm vụ
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessUpdateTaskCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 6)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}", (TCPGameServerCmds)nID, fields.Length, cmdData));
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int taskID = Convert.ToInt32(fields[1]);
                int dbID = Convert.ToInt32(fields[2]);

                string strcmd = "";
                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int ret = DBWriter.UpdateTask(dbMgr, dbID, fields, 3);
                if (ret < 0)
                {
                    strcmd = string.Format("{0}:{1}:{2}", roleID, taskID, -1);
                    LogManager.WriteLog(LogTypes.Error, string.Format("Update role task failed，CMD={0}, RoleID={1}", (TCPGameServerCmds)nID, roleID));
                }
                else
                {
                    if (null != dbRoleInfo.DoingTaskList)
                    {
                        if (dbRoleInfo.DoingTaskList.TryGetValue(dbID, out TaskData task))
                        {
                            task.DoingTaskFocus = DataHelper.ConvertToInt32(fields[3], task.DoingTaskFocus);
                            task.DoingTaskVal1 = DataHelper.ConvertToInt32(fields[4], task.DoingTaskVal1);
                            task.DoingTaskVal2 = DataHelper.ConvertToInt32(fields[4], task.DoingTaskVal2);
                        }
                    }

                    strcmd = string.Format("{0}:{1}:{2}", roleID, taskID, 0);
                }

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// Hoàn thành nhiệm vụ
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessCompleteTaskCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 6)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int npcID = Convert.ToInt32(fields[1]);
                int taskID = Convert.ToInt32(fields[2]);
                int dbID = Convert.ToInt32(fields[3]);
                int isMainTask = Convert.ToInt32(fields[4]);
                int taskclass = Convert.ToInt32(fields[5]);

                string strcmd = "";
                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}", (TCPGameServerCmds)nID, roleID));
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                bool ret = DBWriter.CompleteTask(dbMgr, roleID, npcID, taskID, dbID, taskclass);
                if (!ret)
                {
                    strcmd = string.Format("{0}:{1}:{2}", roleID, taskID, -1);
                    LogManager.WriteLog(LogTypes.Error, string.Format("Complete task failed，CMD={0}, RoleID={1}", (TCPGameServerCmds)nID, roleID));
                }
                else
                {
                    bool needUpdateMainTaskID = false;

                    if (isMainTask > 0 && taskID > dbRoleInfo.MainTaskID)
                    {
                        dbRoleInfo.MainTaskID = taskID;
                        needUpdateMainTaskID = true;
                    }

                    if (null != dbRoleInfo.DoingTaskList)
                    {
                        if (dbRoleInfo.DoingTaskList.ContainsKey(dbID))
                        {
                            dbRoleInfo.DoingTaskList.TryRemove(dbID, out _);
                        }
                    }

                    if (null == dbRoleInfo.OldTasks)
                    {
                        dbRoleInfo.OldTasks = new List<OldTaskData>();
                    }

                    int findIndex = -1;
                    for (int i = 0; i < dbRoleInfo.OldTasks.Count; i++)
                    {
                        if (dbRoleInfo.OldTasks[i].TaskID == taskID)
                        {
                            findIndex = i;
                            break;
                        }
                    }

                    if (findIndex >= 0)
                    {
                        dbRoleInfo.OldTasks[findIndex].DoCount++;
                    }
                    else
                    {
                        dbRoleInfo.OldTasks.Add(new OldTaskData()
                        {
                            TaskID = taskID,
                            DoCount = 1,
                        });
                    }

                    if (needUpdateMainTaskID && isMainTask > 0)
                    {
                        DBWriter.UpdateRoleMainTaskID(dbMgr, roleID, taskID);
                    }

                    strcmd = string.Format("{0}:{1}:{2}", roleID, taskID, 0);
                }

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 获取朋友列表的操作
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessGetFriendsCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 1)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                List<FriendData> friendDataList = null;


                friendDataList = new List<FriendData>();


                if (null == dbRoleInfo.FriendDataList)
                {
                    dbRoleInfo.FriendDataList = new List<FriendData>();
                }


                for (int i = 0; i < dbRoleInfo.FriendDataList.Count; i++)
                {
                    friendDataList.Add(new FriendData()
                    {
                        DbID = dbRoleInfo.FriendDataList[i].DbID,
                        OtherRoleID = dbRoleInfo.FriendDataList[i].OtherRoleID,
                        FriendType = dbRoleInfo.FriendDataList[i].FriendType,
                        Relationship = dbRoleInfo.FriendDataList[i].Relationship,
                    });
                }

                List<FriendData> toSendFriendDataList = new List<FriendData>();
                for (int i = 0; i < friendDataList.Count; i++)
                {
                    DBRoleInfo otherDbRoleInfo = dbMgr.GetDBRoleInfo(friendDataList[i].OtherRoleID);
                    if (null == otherDbRoleInfo)
                    {
                        continue;
                    }

                    // chenjingui. 20150703 获取好友名字从dbroleinfo获取，改名系统不做修改
                    friendDataList[i].OtherRoleName = Global.FormatRoleName(otherDbRoleInfo);
                    friendDataList[i].OtherLevel = otherDbRoleInfo.Level;
                    friendDataList[i].FactionID = otherDbRoleInfo.Occupation;
                    friendDataList[i].OnlineState = Global.GetRoleOnlineState(otherDbRoleInfo);
                    string[] positionInfo = otherDbRoleInfo.Position.Split(':');
                    friendDataList[i].MapCode = Convert.ToInt32(positionInfo[0]);

                    friendDataList[i].GuildID = otherDbRoleInfo.GuildID;
                    friendDataList[i].PicCode = otherDbRoleInfo.RolePic;
                    friendDataList[i].SpouseId = otherDbRoleInfo.MyMarriageData != null ? otherDbRoleInfo.MyMarriageData.nSpouseID : 0;
                    toSendFriendDataList.Add(friendDataList[i]);
                }


                tcpOutPacket = DataHelper.ObjectToTCPOutPacket<List<FriendData>>(toSendFriendDataList, pool, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {

                DataHelper.WriteFormatExceptionLog(ex, "", false);

            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 处理添加朋友的操作
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessAddFriendCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 4)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int dbID = Convert.ToInt32(fields[0]);
                int roleID = Convert.ToInt32(fields[1]);
                string otherName = fields[2];
                int friendType = Convert.ToInt32(fields[3]);

                //将用户的请求更新内存缓存
                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                bool alreadyExists = false;
                int type0Count = 0, type1Count = 0, type2Count = 0;

                List<FriendData> findFriendDataList = new List<FriendData>();

                if (null != dbRoleInfo.FriendDataList)
                {
                    for (int i = 0; i < dbRoleInfo.FriendDataList.Count; i++)
                    {
                        findFriendDataList.Add(dbRoleInfo.FriendDataList[i]);
                    }
                }
                for (int i = 0; i < findFriendDataList.Count; i++)
                {
                    string existsOtherRoleName = string.Empty;

                    DBRoleInfo otherDbRoleInfo = dbMgr.GetDBRoleInfo(findFriendDataList[i].OtherRoleID);
                    if (null != otherDbRoleInfo)
                    {

                        existsOtherRoleName = Global.FormatRoleName(otherDbRoleInfo);
                    }


                    if (!string.IsNullOrEmpty(existsOtherRoleName) && existsOtherRoleName == otherName && findFriendDataList[i].FriendType == friendType)
                    {
                        alreadyExists = true;
                    }

                    if (findFriendDataList[i].FriendType == 0)
                    {
                        type0Count++;
                    }
                    else if (findFriendDataList[i].FriendType == 1)
                    {
                        type1Count++;
                    }
                    else
                    {
                        type2Count++;
                    }
                }

                bool canAdded = !alreadyExists;
                if (canAdded)
                {
                    if (friendType == 0)
                    {
                        if (type0Count >= (int)FriendsConsts.MaxFriendsNum)
                        {
                            canAdded = false;
                        }
                    }
                    else if (friendType == 1)
                    {
                        if (type1Count >= (int)FriendsConsts.MaxBlackListNum)
                        {
                            canAdded = false;
                        }
                    }
                    else
                    {
                        if (type2Count >= (int)FriendsConsts.MaxEnemiesNum)
                        {
                            canAdded = false;
                        }
                    }
                }

                FriendData friendData = null;

                if (canAdded)
                {
                    int otherID = dbMgr.DBRoleMgr.FindDBRoleID(otherName);
                    if (-1 == otherID)
                    {
                        //添加任务失败
                        friendData = new FriendData()
                        {
                            DbID = -10000,
                            OtherRoleID = 0,
                            OtherRoleName = "",
                            OtherLevel = 1,
                            FactionID = 0,
                            OnlineState = 0,
                            MapCode = -1,
                            PosX = -1,
                            PosY = -1,
                            FriendType = friendType,
                            GuildID = -1,
                            PicCode = 0,
                            Relationship = 0,
                        };

                        LogManager.WriteLog(LogTypes.Error, string.Format("添加好友找有时查找对方角色ID失败，CMD={0}, RoleID={1}, OtherName={2}",
                            (TCPGameServerCmds)nID, roleID, otherName));
                    }
                    else
                    {
                        //将用户的请求发起写数据库的操作
                        int ret = DBWriter.AddFriend(dbMgr, dbID, roleID, otherID, friendType, 0);
                        if (ret < 0)
                        {
                            //添加任务失败
                            friendData = new FriendData()
                            {
                                DbID = ret,
                                OtherRoleID = 0,
                                OtherRoleName = "",
                                OtherLevel = 1,
                                FactionID = 0,
                                OnlineState = 0,
                                MapCode = -1,
                                PosX = -1,
                                PosY = -1,
                                FriendType = 0,
                                GuildID = -1,
                                PicCode = 0,
                                Relationship = 0,
                            };

                            LogManager.WriteLog(LogTypes.Error, string.Format("添加好友到数据库失败，CMD={0}, RoleID={1}",
                                (TCPGameServerCmds)nID, roleID));
                        }
                        else
                        {

                            lock (dbRoleInfo)
                            {
                                if (null == dbRoleInfo.FriendDataList)
                                {
                                    dbRoleInfo.FriendDataList = new List<FriendData>();
                                }

                                int findIndex = -1;
                                for (int i = 0; i < dbRoleInfo.FriendDataList.Count; i++)
                                {
                                    if (dbRoleInfo.FriendDataList[i].DbID == ret)
                                    {
                                        findIndex = i;
                                        break;
                                    }
                                }

                                if (findIndex >= 0)
                                {
                                    dbRoleInfo.FriendDataList[findIndex].OtherRoleID = otherID;
                                    dbRoleInfo.FriendDataList[findIndex].FriendType = friendType;
                                }
                                else
                                {
                                    dbRoleInfo.FriendDataList.Add(new FriendData()
                                    {
                                        DbID = ret,
                                        OtherRoleID = otherID,
                                        FriendType = friendType,
                                    });
                                }
                            }

                            DBRoleInfo otherDbRoleInfo = dbMgr.GetDBRoleInfo(otherID);
                            /// Thêm bạn cho thằng này
                            if (otherDbRoleInfo != null)
                            {
                                lock (otherDbRoleInfo)
                                {
                                    if (null == otherDbRoleInfo.FriendDataList)
                                    {
                                        otherDbRoleInfo.FriendDataList = new List<FriendData>();
                                    }

                                    int findIndex = -1;
                                    for (int i = 0; i < otherDbRoleInfo.FriendDataList.Count; i++)
                                    {
                                        if (otherDbRoleInfo.FriendDataList[i].DbID == ret)
                                        {
                                            findIndex = i;
                                            break;
                                        }
                                    }

                                    if (findIndex >= 0)
                                    {
                                        otherDbRoleInfo.FriendDataList[findIndex].OtherRoleID = roleID;
                                        otherDbRoleInfo.FriendDataList[findIndex].FriendType = friendType;
                                    }
                                    else
                                    {
                                        otherDbRoleInfo.FriendDataList.Add(new FriendData()
                                        {
                                            DbID = ret,
                                            OtherRoleID = roleID,
                                            FriendType = friendType,
                                        });
                                    }
                                }
                            }

                            if (null == otherDbRoleInfo)
                            {
                                friendData = new FriendData()
                                {
                                    DbID = -10000,
                                    OtherRoleID = 0,
                                    OtherRoleName = "",
                                    OtherLevel = 1,
                                    FactionID = 0,
                                    OnlineState = 0,
                                    MapCode = -1,
                                    PosX = -1,
                                    PosY = -1,
                                    FriendType = friendType,
                                    GuildID = -1,
                                    PicCode = 0,
                                    Relationship = 0,
                                };
                            }
                            else
                            {
                                string[] positionInfo = otherDbRoleInfo.Position.Split(':');
                                friendData = new FriendData()
                                {
                                    DbID = ret,
                                    OtherRoleID = otherDbRoleInfo.RoleID,
                                    OtherRoleName = Global.FormatRoleName(otherDbRoleInfo),
                                    OtherLevel = otherDbRoleInfo.Level,
                                    FactionID = otherDbRoleInfo.Occupation,
                                    OnlineState = Global.GetRoleOnlineState(otherDbRoleInfo),
                                    MapCode = Convert.ToInt32(positionInfo[0]),
                                    PosX = Convert.ToInt32(positionInfo[2]),
                                    PosY = Convert.ToInt32(positionInfo[3]),
                                    FriendType = friendType,
                                    SpouseId = otherDbRoleInfo.MyMarriageData != null ? otherDbRoleInfo.MyMarriageData.nSpouseID : 0,
                                    GuildID = otherDbRoleInfo.GuildID,
                                    PicCode = otherDbRoleInfo.RolePic,
                                    Relationship = 0,
                                };
                            }
                        }
                    }
                }
                else
                {

                    friendData = new FriendData()
                    {
                        DbID = alreadyExists ? -10002 : -10001,
                        OtherRoleID = 0,
                        OtherRoleName = "",
                        OtherLevel = 1,
                        FactionID = 0,
                        OnlineState = 0,
                        MapCode = -1,
                        PosX = -1,
                        PosY = -1,
                        FriendType = friendType,
                        GuildID = -1,
                        PicCode = 0,
                        Relationship = 0,
                    };

                    LogManager.WriteLog(LogTypes.Error, string.Format("添加好友时已经存在，CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));
                }

                /// 将对象转为TCP协议流
                tcpOutPacket = DataHelper.ObjectToTCPOutPacket<FriendData>(friendData, pool, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 处理删除朋友的操作
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessRemoveFriendCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int dbID = Convert.ToInt32(fields[0]);
                int roleID = Convert.ToInt32(fields[1]);

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                string strcmd = "";

                /// Xóa của thằng
                bool ret = DBWriter.RemoveFriend(dbMgr, dbID, roleID);
                if (!ret)
                {
                    //删除朋友失败
                    strcmd = string.Format("{0}:{1}:{2}", dbID, roleID, -1);

                    LogManager.WriteLog(LogTypes.Error, string.Format("删除好友时失败，CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));
                }
                else
                {
                    /// ID thằng kia
                    int otherID = -1;
                    lock (dbRoleInfo)
                    {
                        if (null == dbRoleInfo.FriendDataList)
                        {
                            dbRoleInfo.FriendDataList = new List<FriendData>();
                        }

                        int findIndex = -1;
                        for (int i = 0; i < dbRoleInfo.FriendDataList.Count; i++)
                        {
                            if (dbRoleInfo.FriendDataList[i].DbID == dbID)
                            {
                                findIndex = i;
                                otherID = dbRoleInfo.FriendDataList[i].OtherRoleID;
                                break;
                            }
                        }

                        if (findIndex >= 0)
                        {
                            dbRoleInfo.FriendDataList.RemoveAt(findIndex);
                        }
                    }

                    /// Thông tin thằng kia
                    DBRoleInfo otherDbRoleInfo = dbMgr.GetDBRoleInfo(otherID);
                    /// Xóa bạn cho cả thằng kia
                    lock (otherDbRoleInfo)
                    {
                        if (null == otherDbRoleInfo.FriendDataList)
                        {
                            otherDbRoleInfo.FriendDataList = new List<FriendData>();
                        }

                        int findIndex = -1;
                        for (int i = 0; i < otherDbRoleInfo.FriendDataList.Count; i++)
                        {
                            if (otherDbRoleInfo.FriendDataList[i].DbID == dbID)
                            {
                                findIndex = i;
                                break;
                            }
                        }

                        if (findIndex >= 0)
                        {
                            otherDbRoleInfo.FriendDataList.RemoveAt(findIndex);
                        }
                    }

                    strcmd = string.Format("{0}:{1}:{2}", dbID, roleID, 0);
                }

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {

                DataHelper.WriteFormatExceptionLog(ex, "", false);

            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 修改角色的PKMode
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessUpdatePKModeCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int pkMode = Convert.ToInt32(fields[1]);

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                string strcmd = "";

                //将用户的请求发起写数据库的操作, 2011-05-31 精简指令，放到offline中写入
                //bool ret = DBWriter.UpdatePKMode(dbMgr, roleID, pkMode);
                bool ret = true;
                if (!ret)
                {
                    //删除朋友失败
                    strcmd = string.Format("{0}:{1}:{2}", roleID, pkMode, -1);

                    LogManager.WriteLog(LogTypes.Error, string.Format("更新角色PK模式时失败，CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));
                }
                else
                {
                    dbRoleInfo.PKMode = pkMode;

                    strcmd = string.Format("{0}:{1}:{2}", roleID, pkMode, -1);
                }

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// Cập nhật trị PK
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessUpdatePKValCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 3)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int pkValue = Convert.ToInt32(fields[1]);
                int pkPoint = Convert.ToInt32(fields[2]);

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                string strcmd = "";

                bool ret = DBWriter.UpdatePKValues(dbMgr, roleID, pkValue, pkPoint);
                if (!ret)
                {
                    strcmd = string.Format("{0}:{1}:{2}:{3}", roleID, pkValue, pkPoint, -1);

                    LogManager.WriteLog(LogTypes.Error, string.Format("Update role PKValue failed，CMD={0}, RoleID={1}", (TCPGameServerCmds)nID, roleID));
                }
                else
                {
                    //lock (dbRoleInfo)
                    {
                        dbRoleInfo.PKValue = pkValue;
                        dbRoleInfo.PKPoint = pkPoint;
                    }

                    strcmd = string.Format("{0}:{1}:{2}:{3}", roleID, pkValue, pkPoint, 0);
                }

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// Bỏ nhiệm vụ
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessAbandonTaskCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 3)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int dbID = Convert.ToInt32(fields[1]);
                int taskID = Convert.ToInt32(fields[2]);

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}", (TCPGameServerCmds)nID, roleID));
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                string strcmd = "";

                bool ret = DBWriter.DeleteTask(dbMgr, roleID, taskID, dbID);
                if (!ret)
                {
                    strcmd = string.Format("{0}", -1);
                    LogManager.WriteLog(LogTypes.Error, string.Format("Abandon task failed，CMD={0}, RoleID={1}, TaskID={2}", (TCPGameServerCmds)nID, roleID, taskID));
                }
                else
                {
                    if (null != dbRoleInfo.DoingTaskList)
                    {
                        if (dbRoleInfo.DoingTaskList.ContainsKey(dbID))
                        {
                            dbRoleInfo.DoingTaskList.TryRemove(dbID, out _);
                        }
                    }

                    strcmd = string.Format("{0}", 0);
                }

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// GM设置主线任务
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessGMSetTaskCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;

            try
            {
                //List<int> taskList = DataHelper.BytesToObject<List<int>>(data, 0, count);
                //if (null != taskList && taskList.Count >= 2)
                //{
                //    int roleID = taskList[0];
                //    int taskID = taskList[taskList.Count - 1];

                //    DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                //    if (null != dbRoleInfo)
                //    {
                //        //将用户的请求更新内存缓存
                //        lock (dbRoleInfo)
                //        {
                //            //清空任务
                //            DBWriter.UpdateRoleTasksForFlashPlayerWhenLogOut(dbMgr, roleID);

                //            //设置历史任务和已完成的主线任务编号
                //            DBWriter.GMSetTask(dbMgr, roleID, taskID, taskList);
                //            DBWriter.UpdateRoleMainTaskID(dbMgr, roleID, taskID);

                //            dbRoleInfo.MainTaskID = taskID;
                //            dbRoleInfo.OldTasks = new List<OldTaskData>();
                //            dbRoleInfo.DoingTaskList = new List<TaskData>();
                //            for (int i = 1; i < taskList.Count; i++)
                //            {
                //                dbRoleInfo.OldTasks.Add(new OldTaskData() { TaskID = taskList[i], DoCount = 1 });
                //            }
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            byte[] bytes = DataHelper.ObjectToBytes(0);
            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, bytes, 0, bytes.Length, nID);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// Thực hiện thiết lập kỹ năng vào khung dùng nhanh
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessModKeysCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 3)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int type = Convert.ToInt32(fields[1]);

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                string keys = fields[2];
                string strcmd = "";

                /// Thực hiện lưu vào DB
                bool ret = DBWriter.UpdateRoleKeys(dbMgr, roleID, type, keys);
                if (!ret)
                {
                    strcmd = string.Format("{0}", -1);

                    LogManager.WriteLog(LogTypes.Error, string.Format("Update role skill Main Quick Key failed，CMD={0}, RoleID={1}", (TCPGameServerCmds)nID, roleID));
                }
                else
                {
                    //lock (dbRoleInfo)
                    {
                        if (type == 0)
                        {
                            dbRoleInfo.MainQuickBarKeys = keys;
                        }
                        else
                        {
                            dbRoleInfo.OtherQuickBarKeys = keys;
                        }
                    }

                    strcmd = string.Format("{0}", 0);
                }

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// Update tiền cho người chơi
        /// 处理更新用户点卷操作
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessUpdateUserMoneyCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            /*
             *  注意：如果协议修改了，一定不要忘记确认下是否需要修改ProcessRoleHuobiOffline这个函数
             *  chenjg 20150422
             */
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                // roleid, addmoney, result
                if (fields.Length != 2 && fields.Length != 3 && fields.Length != 4)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int addOrSubUserMoney = Convert.ToInt32(fields[1]);
                int activeid = 0;
                string param = "";
                if (fields.Length >= 3)
                    activeid = Global.SafeConvertToInt32(fields[2]);
                if (fields.Length >= 4)
                    param = fields[3];

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                string strcmd = "";
                string userID = dbRoleInfo.UserID;

                //将用户的请求更新内存缓存
                DBUserInfo dbUserInfo = dbMgr.GetDBUserInfo(userID);
                if (null == dbUserInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("查找用户数据失败，CMD={0}, UserID={1}",
                        (TCPGameServerCmds)nID, userID));

                    //添加任务失败
                    strcmd = string.Format("{0}:{1}", roleID, -2);

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                // 如果是活动给的钻石奖励，则更新领取记录
                switch (activeid)
                {
                    case (int)(ActivityTypes.HeFuRecharge):
                        {
                            DateTime startTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                            DateTime endTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
                            int hasgettimes = 0;
                            string huoDongKeyStr = param;
                            string lastgettime = "";
                            //判断是否领取过---活动关键字字符串唯一确定了每次活动，针对同类型不同时间的活动
                            DBQuery.GetAwardHistoryForUser(dbMgr, dbRoleInfo.UserID, (int)(ActivityTypes.HeFuRecharge), huoDongKeyStr, out hasgettimes, out lastgettime);
                            // 如果领取过就用最后一次领取时的开始计算
                            if (hasgettimes > 0)
                            {
                                // 如果今天已经领取了
                                int currday = Global.GetOffsetDay(DateTime.Now);
                                if (Global.GetOffsetDay(DateTime.Parse(lastgettime)) == currday)
                                {
                                    // 返回错误信息
                                    strcmd = string.Format("{0}:{1}", roleID, -5);
                                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                                    return TCPProcessCmdResults.RESULT_DATA;
                                }
                            }

                            hasgettimes++;
                            //避免同一角色同时多次操作
                            lock (dbRoleInfo)
                            {
                                int ret = DBWriter.UpdateHongDongAwardRecordForUser(dbMgr, dbRoleInfo.UserID, activeid, huoDongKeyStr, hasgettimes, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                if (ret < 0)
                                    ret = DBWriter.AddHongDongAwardRecordForUser(dbMgr, dbRoleInfo.UserID, activeid, huoDongKeyStr, hasgettimes, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                if (ret < 0)
                                {
                                    // log it

                                    LogManager.WriteLog(LogTypes.Error, string.Format("更新玩家合服充值返利领取记录失败！！！！！！！！，CMD={0}, RoleID={1}",
                                            (TCPGameServerCmds)nID, roleID));
                                    //tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                                    //return TCPProcessCmdResults.RESULT_FAILED;
                                }
                            }
                        }
                        break;

                    default:
                        break;
                }

                bool failed = false;
                int userMoney = 0;
                lock (dbUserInfo)
                {
                    //判断如果是扣除操作，则可能是元宝余额不足而失败
                    if (addOrSubUserMoney < 0 && dbUserInfo.Money < Math.Abs(addOrSubUserMoney))
                    {
                        failed = true;
                    }
                    else //处理元宝的加减
                    {
                        dbUserInfo.Money = Math.Max(0, dbUserInfo.Money + addOrSubUserMoney);
                        userMoney = dbUserInfo.Money;
                    }
                }

                //如果扣除失败
                if (failed)
                {
                    //添加任务失败
                    strcmd = string.Format("{0}:{1}", roleID, -3);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //不等于0，才更新数据库
                if (addOrSubUserMoney != 0)
                {
                    //将用户的请求发起写数据库的操作
                    bool ret = DBWriter.UpdateUserMoney(dbMgr, userID, userMoney);
                    if (!ret)
                    {
                        LogManager.WriteLog(LogTypes.Error, string.Format("更新用户的元宝失败，CMD={0}, UserID={1}",
                            (TCPGameServerCmds)nID, userID));

                        //添加任务失败
                        strcmd = string.Format("{0}:{1}", roleID, -4);

                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }
                }

                // 添加一个返回的数据 -- 玩家的真实充值金额 [4/10/2014 LiaoWei]
                int nUserMoney = 0;
                int nRealMoney = 0;

                DBQuery.QueryUserMoneyByUserID(dbMgr, dbUserInfo.UserID, out nUserMoney, out nRealMoney);

                strcmd = string.Format("{0}:{1}:{2}", roleID, userMoney, nRealMoney);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 处理更新用户绑定银两操作
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessUpdateUserYinLiangCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            /*
             *  注意：如果协议修改了，一定不要忘记确认下是否需要修改ProcessRoleHuobiOffline这个函数
             *  chenjg 20150422
             */
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int addOrSubUserYinLiang = Convert.ToInt32(fields[1]);

                string strcmd = "";
                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                bool failed = false;
                int userYinLiang = 0;
                lock (dbRoleInfo.GetMoneyLock)
                {
                    //判断如果是扣除操作，则可能是银两余额不足而失败
                    if (addOrSubUserYinLiang < 0 && dbRoleInfo.YinLiang < Math.Abs(addOrSubUserYinLiang))
                    {
                        failed = true;
                    }
                    else //处理元宝的加减
                    {
                        dbRoleInfo.YinLiang = Math.Max(0, dbRoleInfo.YinLiang + addOrSubUserYinLiang);
                        userYinLiang = dbRoleInfo.YinLiang;
                    }
                }

                //如果扣除失败
                if (failed)
                {
                    //添加任务失败
                    strcmd = string.Format("{0}:{1}", roleID, -1);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //不等于0，才更新数据库
                if (addOrSubUserYinLiang != 0)
                {
                    //将用户的请求发起写数据库的操作
                    bool ret = DBWriter.UpdateRoleYinLiang(dbMgr, roleID, userYinLiang);
                    if (!ret)
                    {
                        LogManager.WriteLog(LogTypes.Error, string.Format("更新角色银两失败，CMD={0}, RoleID={1}",
                            (TCPGameServerCmds)nID, roleID));

                        //添加任务失败
                        strcmd = string.Format("{0}:{1}", roleID, -2);

                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }
                }

                strcmd = string.Format("{0}:{1}", roleID, userYinLiang);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// Hàm cập nhật lại tiền tệ bang hội
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProseccUpdateGuildMoney(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);

                int AddOrSubGuildMoney = Convert.ToInt32(fields[1]);

                // Lấy ra thông tin nhân vật
                // Nếu đéo lấy được thì thông báo lỗi về
                string strcmd = "";
                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                bool failed = false;
                int RoleGuildMoney = 0;
                lock (dbRoleInfo.GetMoneyLock)
                {
                    // Check xem có đủ tiền để trừ không
                    if (AddOrSubGuildMoney < 0 && dbRoleInfo.RoleGuildMoney < Math.Abs(AddOrSubGuildMoney))
                    {
                        failed = true;
                    }
                }
                // Nếu ko có bang thì cũng cho toạch luôn
                if (dbRoleInfo.GuildID <= 0)
                {
                    failed = true;
                }
                // Nếu mà toạch thì báo luôn là toạch

                if (failed)
                {
                    strcmd = string.Format("{0}:{1}", roleID, -1);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }
                // Lấy ra tổng số tiền đã add vào
                int TOTALADD = Global.GetRoleParamsInt32(dbRoleInfo, "TotalGuildMoneyAdd");

                //LẤY RA TỔNG SỐT IỀN ĐÃ RÚT
                int TOTALWITHDRAW = Global.GetRoleParamsInt32(dbRoleInfo, "TotalGuildMoneyWithDraw");

                // Đoạn này sẽ tính xem thằng này còn có đủ quỹ để mua shop bang hội nữa không
                if (AddOrSubGuildMoney < 0)
                {
                    // Lấy ra thông tin bang hội
                    Guild _GuildFind = GuildManager.getInstance().GetGuildByID(dbRoleInfo.GuildID);

                    if (_GuildFind != null)
                    {
                        int PERCENT = _GuildFind.MaxWithDraw;
                        // TỔNG SỐ TIỀN MUỐN RÚT = TỔNG SỐ TIỀN ĐÃ RÚT + SỐ TIỀN MUỐN RÚT HÔM NAY
                        int SUMWITHDRAWT = TOTALWITHDRAW + Math.Abs(AddOrSubGuildMoney);
                        //TỔNG SỐ % ĐÃ RÚT
                        int PERCENTSELF = SUMWITHDRAWT * 100 / TOTALADD;
                        // NẾU SỐ % MÀ QUÁ THÌ SẼ BÁO LỖI
                        if (PERCENTSELF > PERCENT)
                        {
                            // TỔNG SỐ TIỀN CÓ THỂ RÚT
                            int MAXCANDRAWR = PERCENT * TOTALADD / 100;

                            int MONEYLESS = MAXCANDRAWR - TOTALWITHDRAW;

                            // Báo về đã mua quá số tiền tích lũy
                            strcmd = string.Format("{0}:{1}", MONEYLESS, -3);
                            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                            return TCPProcessCmdResults.RESULT_DATA;
                        }
                    }
                    else
                    {
                        // nếu không có bang thì báo lỗi luôn là ko thể thao tác tsc
                        strcmd = string.Format("{0}:{1}", roleID, -4);
                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }
                }
                if (AddOrSubGuildMoney != 0)
                {
                    bool ret = GuildManager.getInstance().UpdateMemberMoney(AddOrSubGuildMoney, dbRoleInfo.GuildID, dbRoleInfo.RoleID);
                    if (!ret)
                    {
                        LogManager.WriteLog(LogTypes.Error, string.Format("Không cập nhật được tiền bang hội lỗi nghiêm trọng，CMD={0}, RoleID={1}",
                            (TCPGameServerCmds)nID, roleID));

                        strcmd = string.Format("{0}:{1}", roleID, -2);

                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }
                    else
                    {
                        if (AddOrSubGuildMoney > 0)
                        {
                            // Thực hiện ghi lại tích lũy đã cống hiến vào bang bao nhiêu
                            Global.ModifyRoleParamLongByName(dbMgr, dbRoleInfo, "TotalGuildMoneyAdd", AddOrSubGuildMoney);
                        }
                        else
                        {
                            // Thực hiện ghi lại tổng tích lũy đã rút ra khỏi bang bao nhiêu
                            Global.ModifyRoleParamLongByName(dbMgr, dbRoleInfo, "TotalGuildMoneyWithDraw", Math.Abs(AddOrSubGuildMoney));
                        }

                        //Thực hiện update lại thực thể tiền bang hội trên người
                        dbRoleInfo.RoleGuildMoney = Math.Max(0, dbRoleInfo.RoleGuildMoney + AddOrSubGuildMoney);
                        RoleGuildMoney = dbRoleInfo.RoleGuildMoney;
                    }
                }

                strcmd = string.Format("{0}:{1}", roleID, RoleGuildMoney);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// Di chuyển vật phẩm
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessMoveGoodsCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 3)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int goodsOwnerRoleID = Convert.ToInt32(fields[1]);
                int goodsDbID = Convert.ToInt32(fields[2]);

                string strcmd = "";

                //将用户的请求更新内存缓存
                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("移动物品时查找角色失败，CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    //移动物品失败
                    strcmd = string.Format("{0}:{1}:{2}:{3}", roleID, goodsOwnerRoleID, goodsDbID, -1);

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //将用户的请求更新内存缓存
                DBRoleInfo dbRoleInfo2 = dbMgr.GetDBRoleInfo(goodsOwnerRoleID);
                if (null == dbRoleInfo2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("移动物品时查找物品拥有者角色失败，CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, goodsOwnerRoleID));

                    //移动物品失败
                    strcmd = string.Format("{0}:{1}:{2}:{3}", roleID, goodsOwnerRoleID, goodsDbID, -2);

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //根据物品的DBID获取物品
                if (null == Global.GetGoodsDataByDbID(dbRoleInfo2, goodsDbID)) //如果没有找到物品，则表示物品已经不存在了，直接返回失败
                {
                    strcmd = string.Format("{0}:{1}:{2}:{3}", roleID, goodsOwnerRoleID, goodsDbID, -1000);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //移动一个物品
                int ret = DBWriter.MoveGoods(dbMgr, roleID, goodsDbID);
                if (ret < 0)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("移动物品时修改数据库失败，CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    //添加任务失败
                    strcmd = string.Format("{0}:{1}:{2}:{3}", roleID, goodsOwnerRoleID, goodsDbID, ret);

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                /// Vật phẩm tương ứng
                GoodsData gd = Global.GetGoodsDataByDbID(dbRoleInfo2, goodsDbID);
                /// Toác
                if (null == gd)
                {
                    strcmd = string.Format("{0}:{1}:{2}:{3}", roleID, goodsOwnerRoleID, goodsDbID, -1000);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                /// Xóa khỏi danh sách
                dbRoleInfo2.GoodsDataList.TryRemove(goodsDbID, out _);
                gd.Site = 0;

                if (null == dbRoleInfo.GoodsDataList)
                {
                    dbRoleInfo.GoodsDataList = new ConcurrentDictionary<int, GoodsData>();
                }

                dbRoleInfo.GoodsDataList[goodsDbID] = gd;

                strcmd = string.Format("{0}:{1}:{2}:{3}", roleID, goodsOwnerRoleID, goodsDbID, 0);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 处理更新角色剩余自动战斗时间的操作
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessUpdateLeftFightSecsCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int leftFightSecs = Convert.ToInt32(fields[1]);

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                string strcmd = "";

                //将用户的请求发起写数据库的操作
                bool ret = DBWriter.UpdateRoleLeftFightSecs(dbMgr, roleID, leftFightSecs);
                if (!ret)
                {
                    //添加任务失败
                    strcmd = string.Format("{0}:{1}", roleID, -1);

                    LogManager.WriteLog(LogTypes.Error, string.Format("更新角色的剩余挂机时间时失败，CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));
                }
                else
                {
                    dbRoleInfo.LeftFightSeconds = leftFightSecs;

                    strcmd = string.Format("{0}:{1}", roleID, 0);
                }

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                // });
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 处理查询名称的操作
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessQueryNameByIDCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 3)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                string otherName = fields[1];
                int opCode = Convert.ToInt32(fields[2]);

                int myServerLineID = -1;

                if (roleID > 0)
                {
                    DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                    if (null == dbRoleInfo)
                    {
                        LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                            (TCPGameServerCmds)nID, roleID));

                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }

                    myServerLineID = dbRoleInfo.ServerLineID;
                }

                string strcmd = "";

                int onlineState = -1;
                int otherRoleID = dbMgr.DBRoleMgr.FindDBRoleID(otherName); //角色不存在
                if (otherRoleID == -1)
                {
                    var otherDdbRoleInfo = dbMgr.GetDBRoleInfo(otherName);
                    if (otherDdbRoleInfo != null)
                    {
                        otherRoleID = otherDdbRoleInfo.RoleID;
                    }
                }

                if (-1 != otherRoleID)
                {
                    DBRoleInfo otherDbRoleInfo = dbMgr.GetDBRoleInfo(otherRoleID);
                    if (null != otherDbRoleInfo)
                    {
                        int roleOnlineState = Global.GetRoleOnlineState(otherDbRoleInfo);
                        if (1 == roleOnlineState)
                        {
                            onlineState = 0;

                            if (otherDbRoleInfo.ServerLineID != myServerLineID)
                            {
                                onlineState = otherDbRoleInfo.ServerLineID;
                            }
                        }
                    }
                }

                strcmd = string.Format("{0}:{1}:{2}:{3}:{4}", roleID, otherName, opCode, otherRoleID, onlineState);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 处理根据名称查询平台ID、元宝数、充值历史记录值
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessQueryUserMoneyByNameCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                string otherName = fields[1];

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                string strcmd = "";

                //根据角色名称查询平台用户ID
                //应该不需要再次查询
                string userID = dbRoleInfo.UserID;// DBQuery.QueryUserIDByRoleName(dbMgr, otherName, dbRoleInfo.ZoneID);
                if (userID == "")
                {
                    strcmd = string.Format("{0}:{1}:{2}:{3}", roleID, userID, 0, 0);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int userMoney = 0;
                int realMoney = 0;

                //根据平台用户ID查询元宝和真实的充值钱数
                //DBQuery.QueryUserMoneyByUserID(dbMgr, userID, out userMoney, out realMoney);

                //查询时如果当前时间小于结束时间，就用采用结束时间也行
                //活动时间范围内的充值数，真实货币单位
                int inputMoneyInPeriod = DBQuery.GetUserInputMoney(dbMgr, userID, dbRoleInfo.ZoneID, "2000-01-01 00:00:00", "2050-01-01 00:00:00");

                if (inputMoneyInPeriod < 0)
                {
                    inputMoneyInPeriod = 0;
                }

                //时间范围内的元宝数
                int roleYuanBaoInPeriod = Global.TransMoneyToYuanBao(inputMoneyInPeriod);
                userMoney = roleYuanBaoInPeriod;
                realMoney = inputMoneyInPeriod;

                strcmd = string.Format("{0}:{1}:{2}:{3}", roleID, userID, userMoney, realMoney);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 转发客户端发送的聊天消息
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessSpriteChatCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 9)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int serverLineID = Convert.ToInt32(fields[8]);
                List<LineItem> itemList = LineManager.GetLineItemList();
                if (null != itemList)
                {
                    for (int i = 0; i < itemList.Count; i++)
                    {
                        if (itemList[i].LineID == serverLineID)
                        {
                            continue;
                        }

                        if (serverLineID == GameDBManager.ServerLineIdAllIncludeKuaFu)
                        {
                            ChatMsgManager.AddChatMsg(itemList[i].LineID, cmdData);
                        }
                        else if (serverLineID == GameDBManager.ServerLineIdAllLineExcludeSelf)
                        {
                            if (null != TCPManager.CurrentClient && TCPManager.CurrentClient.LineId != itemList[i].LineID)
                            {
                                ChatMsgManager.AddChatMsg(itemList[i].LineID, cmdData);
                            }
                        }
                        else if (itemList[i].LineID < GameDBManager.KuaFuServerIdStartValue)
                        {
                            ChatMsgManager.AddChatMsg(itemList[i].LineID, cmdData);
                        }
                    }
                }

                string strcmd = string.Format("{0}", 0);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 客户端获取转发的聊天消息列表
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessGetChatMsgListCmd(GameServerClient client, DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 4)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int serverLineID = Convert.ToInt32(fields[0]);
                int serverLineNum = Convert.ToInt32(fields[1]);
                int serverLineCount = Convert.ToInt32(fields[2]);

                if (serverLineCount <= 0) //刚上线的服务器
                {
                    //清空指定线路ID对应的所有用户数据
                    UserOnlineManager.ClearUserIDsByServerLineID(serverLineID);
                }

                //更新服务器状态
                LineManager.UpdateLineHeart(client, serverLineID, serverLineNum, fields[3]);

                tcpOutPacket = ChatMsgManager.GetWaitingChatMsg(pool, nID, serverLineID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 获取移动仓库中的物品列表的操作
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessGetGoodsListBySiteCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int site = Convert.ToInt32(fields[1]);

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                List<GoodsData> goodsDataList = null;

                //将用户的请求更新内存缓存
                lock (dbRoleInfo)
                {
                    goodsDataList = Global.GetGoodsDataListBySite(dbRoleInfo, site);
                }

                /// 将对象转为TCP协议流
                tcpOutPacket = DataHelper.ObjectToTCPOutPacket<List<GoodsData>>(goodsDataList, pool, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 设置是否禁止某个角色登录
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessBanRoleNameCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                string roleName = fields[0];
                int state = Convert.ToInt32(fields[1]);

                //设置是否禁止某个角色名称
                BanManager.BanRoleName(roleName, state);

                string strcmd = string.Format("{0}", 0);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 设置是否禁止某个角色聊天发言
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessBanRoleChatCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                string roleName = fields[0];
                int banHours = Convert.ToInt32(fields[1]);

                //添加禁止发言的角色名称到字典中
                BanChatManager.AddBanRoleName(roleName, banHours);

                string strcmd = string.Format("{0}", 0);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// GameServer定时获取禁止角色聊天发言的词典
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessGetBanRoleChatDictCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 1)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int serverLineID = Convert.ToInt32(fields[0]);

                //获取禁止聊天发言的词典发送tcp对象
                tcpOutPacket = BanChatManager.GetBanChatDictTCPOutPacket(pool, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 角色更新在线时长相关的字段
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessUpdateOnlineTimeCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 3)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int totalOnlineSecs = Convert.ToInt32(fields[1]);
                int antiAddictionSecs = Convert.ToInt32(fields[2]);

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //long ticks = DateTime.Now.Ticks / 10000;
                bool updateDBTime = true;
                dbRoleInfo.TotalOnlineSecs = totalOnlineSecs;
                dbRoleInfo.AntiAddictionSecs = antiAddictionSecs;

                if (updateDBTime)
                {
                    DBWriter.UpdateRoleOnlineSecs(dbMgr, roleID, totalOnlineSecs, antiAddictionSecs);
                }

                string strcmd = string.Format("{0}", 0);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// GameServer获取DB的游戏配置参数
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessGameConfigDictCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 1)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int serverLineID = Convert.ToInt32(fields[0]);

                //获取公告消息词典的发送tcp对象
                tcpOutPacket = GameDBManager.GameConfigMgr.GetGameConfigDictTCPOutPacket(pool, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 更新游戏配置参数
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessGameConfigItemCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                string paramName = fields[0];
                string paramValue = fields[1];

                //设置游戏参数
                GameDBManager.GameConfigMgr.UpdateGameConfigItem(paramName, paramValue);

                //更新设置游戏参数
                DBWriter.UpdateGameConfig(dbMgr, paramName, paramValue);

                //添加GM命令消息
                string gmCmdData = string.Format("-config {0} {1}", paramName, paramValue);
                ChatMsgManager.AddGMCmdChatMsg(-1, gmCmdData);

                string strcmd = string.Format("{0}", 0);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// Thực hiện thêm kỹ năng vào DB
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessAddSkillCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int skillID = Convert.ToInt32(fields[1]);

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int ret = DBWriter.AddSkill(dbMgr, roleID, skillID);
                if (ret > 0)
                {
                    if (null == dbRoleInfo.SkillDataList)
                    {
                        dbRoleInfo.SkillDataList = new ConcurrentDictionary<int, SkillData>();
                    }

                    SkillData skillData = new SkillData()
                    {
                        DbID = ret,
                        SkillID = skillID,
                        Cooldown = 0,
                        Exp = 0,
                        LastUsedTick = 0,
                        SkillLevel = 0,
                    };
                    dbRoleInfo.SkillDataList[skillData.SkillID] = skillData;
                }

                string strcmd = string.Format("{0}", ret);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// Thực hiện xóa kỹ năng khỏi DB
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessDelSkillCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int skillID = Convert.ToInt32(fields[1]);

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                bool ret = DBWriter.DeleteSkill(dbMgr, roleID, skillID);
                if (ret)
                {
                    if (null == dbRoleInfo.SkillDataList)
                    {
                        dbRoleInfo.SkillDataList = new ConcurrentDictionary<int, SkillData>();
                    }

                    if (dbRoleInfo.SkillDataList.TryGetValue(skillID, out SkillData skillData))
                    {
                        dbRoleInfo.SkillDataList.TryRemove(skillID, out _);
                    }
                }

                string strcmd = string.Format("{0}", ret ? 1 : 0);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// Thực hiện thay đổi thông tin kỹ năng
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessUpSkillInfoCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 6)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int skillID = Convert.ToInt32(fields[1]);
                int skillLevel = Convert.ToInt32(fields[2]);
                long lastUsedTick = Convert.ToInt64(fields[3]);
                int cooldownTick = Convert.ToInt32(fields[4]);
                int exp = Convert.ToInt32(fields[5]);

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                if (null != dbRoleInfo.SkillDataList)
                {
                    if (dbRoleInfo.SkillDataList.TryGetValue(skillID, out SkillData skill))
                    {
                        skill.SkillLevel = skillLevel;
                        skill.LastUsedTick = lastUsedTick;
                        skill.Cooldown = cooldownTick;
                        skill.Exp = exp;
                    }
                }

                bool ret = DBWriter.UpdateSkillInfo(dbMgr, roleID, skillID, skillLevel, lastUsedTick, cooldownTick, exp);

                string strcmd = string.Format("{0}", ret ? 1 : 0);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// Cập nhật thông tin Buff
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessUpdateBufferItemCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 6)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int bufferID = Convert.ToInt32(fields[1]);
                long startTime = Convert.ToInt64(fields[2]);
                long bufferSecs = Convert.ToInt32(fields[3]);
                long bufferVal = Convert.ToInt64(fields[4]);
                string customProperty = fields[5];

                string strcmd = "";
                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //将用户的请求发起写数据库的操作
                //int intSecs = DataHelper.SysTicksToUnixSeconds(startTime);
                int ret = DBWriter.UpdateRoleBufferItem(dbMgr, roleID, bufferID, startTime, bufferSecs, bufferVal, customProperty);
                if (ret < 0)
                {
                    //添加Buffer项失败
                    strcmd = string.Format("{0}:{1}", roleID, ret);

                    LogManager.WriteLog(LogTypes.Error, string.Format("更新角色Buffer项失败，CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));
                }
                else
                {
                    if (null == dbRoleInfo.BufferDataList)
                    {
                        dbRoleInfo.BufferDataList = new ConcurrentDictionary<int, BufferData>();
                    }

                    /// Nếu tồn tại
                    if (dbRoleInfo.BufferDataList.TryGetValue(bufferID, out BufferData buff))
                    {
                        buff.BufferID = bufferID;
                        buff.StartTime = startTime;
                        buff.BufferSecs = bufferSecs;
                        buff.BufferVal = bufferVal;
                        buff.CustomProperty = customProperty;
                    }
                    /// Nếu không tồn tại
                    else
                    {
                        buff = new BufferData()
                        {
                            BufferID = bufferID,
                            StartTime = startTime,
                            BufferSecs = bufferSecs,
                            BufferVal = bufferVal,
                            BufferType = 0,
                            CustomProperty = customProperty,
                        };
                        dbRoleInfo.BufferDataList[buff.BufferID] = buff;
                    }

                    strcmd = string.Format("{0}:{1}", roleID, ret);
                }

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 处理恢复删除角色的操作
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessUnDelRoleNameCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 1)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                string otherRoleName = fields[0];

                //恢复删除一个用户角色
                DBWriter.UnRemoveRole(dbMgr, otherRoleName);

                string strcmd = string.Format("{0}", 0);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 处理删除角色的操作
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessDelRoleNameCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 1)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                string otherRoleName = fields[0];

                //删除一个用户角色
                DBWriter.RemoveRoleByName(dbMgr, otherRoleName);

                string strcmd = string.Format("{0}", 0);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 处理日跑环数据的更新
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessUpdateDailyTaskDataCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 7)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int huanID = Convert.ToInt32(fields[1]);
                string rectime = fields[2];
                int recnum = Convert.ToInt32(fields[3]);
                int taskClass = Convert.ToInt32(fields[4]);
                int extDayID = Convert.ToInt32(fields[5]);
                int extNum = Convert.ToInt32(fields[6]);

                string strcmd = "";
                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                lock (dbRoleInfo)
                {
                    if (null == dbRoleInfo.MyDailyTaskDataList)
                    {
                        dbRoleInfo.MyDailyTaskDataList = new List<DailyTaskData>();
                    }

                    bool found = false;
                    DailyTaskData dailyTaskData = null;
                    for (int i = 0; i < dbRoleInfo.MyDailyTaskDataList.Count; i++)
                    {
                        if (dbRoleInfo.MyDailyTaskDataList[i].TaskClass == taskClass)
                        {
                            found = true;
                            dailyTaskData = dbRoleInfo.MyDailyTaskDataList[i];
                            break;
                        }
                    }

                    if (!found)
                    {
                        dailyTaskData = new DailyTaskData();
                        dbRoleInfo.MyDailyTaskDataList.Add(dailyTaskData);
                    }

                    dailyTaskData.HuanID = huanID;
                    dailyTaskData.RecTime = rectime;
                    dailyTaskData.RecNum = recnum;
                    dailyTaskData.TaskClass = taskClass;
                    dailyTaskData.ExtDayID = extDayID;
                    dailyTaskData.ExtNum = extNum;
                }

                //更新一个用户角色的日跑环任务数据
                DBWriter.UpdateRoleDailyTaskData(dbMgr, roleID, huanID, rectime, recnum, taskClass, extDayID, extNum);

                strcmd = string.Format("{0}:{1}", roleID, 0);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 处理更新角色移动仓库的信息的操作
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessUpdatePBInfoCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int extGridNum = Convert.ToInt32(fields[1]);

                string strcmd = "";
                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //更新一个用户角色的随身仓库信息的操作
                int ret = DBWriter.UpdateRolePBInfo(dbMgr, roleID, extGridNum);
                if (ret < 0)
                {
                    //添加任务失败
                    strcmd = string.Format("{0}:{1}", roleID, ret);

                    LogManager.WriteLog(LogTypes.Error, string.Format("更新角色随身仓库信息失败，CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));
                }
                else
                {
                    //将用户的请求更新内存缓存
                    lock (dbRoleInfo)
                    {
                        dbRoleInfo.MyPortableBagData.ExtGridNum = extGridNum;
                    }

                    strcmd = string.Format("{0}:{1}", roleID, 0);
                }

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 处理更新角色背包可用格子数的操作
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessUpdateRoleBagNumCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int extGridNum = Convert.ToInt32(fields[1]);

                string strcmd = "";
                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //更新一个用户角色背包格子数的操作
                int ret = DBWriter.UpdateRoleBagNum(dbMgr, roleID, extGridNum);
                if (ret < 0)
                {
                    //添加任务失败
                    strcmd = string.Format("{0}:{1}", roleID, ret);

                    LogManager.WriteLog(LogTypes.Error, string.Format("更新角色随身仓库信息失败，CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));
                }
                else
                {
                    //将用户的请求更新内存缓存
                    lock (dbRoleInfo)
                    {
                        dbRoleInfo.BagNum = extGridNum;
                    }

                    strcmd = string.Format("{0}:{1}", roleID, 0);
                }

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 处理更新角色送礼活动的信息的操作
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessUpdateHuoDongInfoCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 22)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);

                string strcmd = "";
                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                bool existsMyHuodongData = false;
                lock (dbRoleInfo)
                {
                    existsMyHuodongData = dbRoleInfo.ExistsMyHuodongData;
                }

                //如果角色的活动数据还不存在
                if (!existsMyHuodongData)
                {
                    LogManager.WriteLog(LogTypes.SQL, "CREATE NEW  CreateHuoDong :" + roleID);

                    DBWriter.CreateHuoDong(dbMgr, roleID);

                    lock (dbRoleInfo)
                    {
                        dbRoleInfo.ExistsMyHuodongData = true;
                    }
                }

                //更新一个用户角色的随身仓库信息的操作
                int ret = DBWriter.UpdateHuoDong(dbMgr, roleID, fields, 1);
                if (ret < 0)
                {
                    LogManager.WriteLog(LogTypes.SQL, "UpdateHuoDong OK :" + ret);
                    //添加任务失败
                    strcmd = string.Format("{0}:{1}", roleID, ret);

                    LogManager.WriteLog(LogTypes.Error, string.Format("更新角色送礼活动信息失败，CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));
                }
                else
                {
                    //将用户的请求更新内存缓存
                    lock (dbRoleInfo)
                    {
                        dbRoleInfo.MyHuodongData.LastWeekID = DataHelper.ConvertToStr(fields[1], dbRoleInfo.MyHuodongData.LastWeekID);
                        dbRoleInfo.MyHuodongData.LastDayID = DataHelper.ConvertToStr(fields[2], dbRoleInfo.MyHuodongData.LastDayID);
                        dbRoleInfo.MyHuodongData.LoginNum = DataHelper.ConvertToInt32(fields[3], dbRoleInfo.MyHuodongData.LoginNum);
                        dbRoleInfo.MyHuodongData.NewStep = DataHelper.ConvertToInt32(fields[4], dbRoleInfo.MyHuodongData.NewStep);
                        dbRoleInfo.MyHuodongData.StepTime = DataHelper.ConvertToTicks(fields[5], dbRoleInfo.MyHuodongData.StepTime);
                        dbRoleInfo.MyHuodongData.LastMTime = DataHelper.ConvertToInt32(fields[6], dbRoleInfo.MyHuodongData.LastMTime);
                        dbRoleInfo.MyHuodongData.CurMID = DataHelper.ConvertToStr(fields[7], dbRoleInfo.MyHuodongData.CurMID);
                        dbRoleInfo.MyHuodongData.CurMTime = DataHelper.ConvertToInt32(fields[8], dbRoleInfo.MyHuodongData.CurMTime);
                        dbRoleInfo.MyHuodongData.SongLiID = DataHelper.ConvertToInt32(fields[9], dbRoleInfo.MyHuodongData.SongLiID);
                        dbRoleInfo.MyHuodongData.LoginGiftState = DataHelper.ConvertToInt32(fields[10], dbRoleInfo.MyHuodongData.LoginGiftState);
                        dbRoleInfo.MyHuodongData.OnlineGiftState = DataHelper.ConvertToInt32(fields[11], dbRoleInfo.MyHuodongData.OnlineGiftState);
                        dbRoleInfo.MyHuodongData.LastLimitTimeHuoDongID = DataHelper.ConvertToInt32(fields[12], dbRoleInfo.MyHuodongData.LastLimitTimeHuoDongID);
                        dbRoleInfo.MyHuodongData.LastLimitTimeDayID = DataHelper.ConvertToInt32(fields[13], dbRoleInfo.MyHuodongData.LastLimitTimeDayID);
                        dbRoleInfo.MyHuodongData.LimitTimeLoginNum = DataHelper.ConvertToInt32(fields[14], dbRoleInfo.MyHuodongData.LimitTimeLoginNum);
                        dbRoleInfo.MyHuodongData.LimitTimeGiftState = DataHelper.ConvertToInt32(fields[15], dbRoleInfo.MyHuodongData.LimitTimeGiftState);
                        dbRoleInfo.MyHuodongData.EveryDayOnLineAwardStep = DataHelper.ConvertToInt32(fields[16], dbRoleInfo.MyHuodongData.EveryDayOnLineAwardStep);
                        dbRoleInfo.MyHuodongData.GetEveryDayOnLineAwardDayID = DataHelper.ConvertToInt32(fields[17], dbRoleInfo.MyHuodongData.GetEveryDayOnLineAwardDayID);
                        dbRoleInfo.MyHuodongData.SeriesLoginGetAwardStep = DataHelper.ConvertToInt32(fields[18], dbRoleInfo.MyHuodongData.SeriesLoginGetAwardStep);
                        dbRoleInfo.MyHuodongData.SeriesLoginAwardDayID = DataHelper.ConvertToInt32(fields[19], dbRoleInfo.MyHuodongData.SeriesLoginAwardDayID);
                        dbRoleInfo.MyHuodongData.SeriesLoginAwardGoodsID = DataHelper.ConvertToStr(fields[20], dbRoleInfo.MyHuodongData.SeriesLoginAwardGoodsID);
                        dbRoleInfo.MyHuodongData.EveryDayOnLineAwardGoodsID = DataHelper.ConvertToStr(fields[21], dbRoleInfo.MyHuodongData.EveryDayOnLineAwardGoodsID);
                    }

                    strcmd = string.Format("{0}:{1}", roleID, 0);
                }

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 处理使用礼品码的操作
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessUseLiPinMaCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 3)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int songLiID = Convert.ToInt32(fields[1]);
                string liPinMa = fields[2];
                liPinMa = liPinMa.ToUpper();

                string strcmd = "";
                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //使用礼品码
                int ret = 0;
                if (liPinMa.Substring(0, 2) == "NZ")
                {
                    ret = LiPinMaManager.UseLiPinMa2(dbMgr, roleID, songLiID, liPinMa, dbRoleInfo.ZoneID);
                }
                else if (liPinMa.Substring(0, 2) == "NX")
                {
                    ret = LiPinMaManager.UseLiPinMaNX(dbMgr, roleID, songLiID, liPinMa, dbRoleInfo.ZoneID);
                }
                else
                {
                    ret = LiPinMaManager.UseLiPinMa(dbMgr, roleID, songLiID, liPinMa);
                }

                strcmd = string.Format("{0}:{1}", roleID, ret);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 处理专享活动数据的删除
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessDeleteSpecActCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length < 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }
                int roleID = Convert.ToInt32(fields[0]);
                int groupID = Convert.ToInt32(fields[1]);

                string strcmd = "";
                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                lock (dbRoleInfo)
                {
                    if (null != dbRoleInfo.SpecActInfoDict)
                    {
                        if (groupID == 0)
                        {
                            dbRoleInfo.SpecActInfoDict.Clear();
                        }
                        else
                        {
                            List<int> SpecActInfoDelete = new List<int>();
                            foreach (var kvp in dbRoleInfo.SpecActInfoDict)
                            {
                                if (kvp.Value.GroupID == groupID)
                                    SpecActInfoDelete.Add(kvp.Key);
                            }
                            foreach (var key in SpecActInfoDelete)
                            {
                                dbRoleInfo.SpecActInfoDict.Remove(key);
                            }
                        }
                    }
                }

                //删除一个用户角色的专享活动数据
                DBWriter.DeleteSpecialActivityData(dbMgr, roleID, groupID);

                strcmd = string.Format("{0}:{1}", roleID, 0);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 获得专享活动数据
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessGetSpecActInfoCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 1)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }
                int roleID = Convert.ToInt32(fields[0]);

                string strcmd = "";
                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                lock (dbRoleInfo)
                {
                    /// 将对象转为TCP协议流
                    tcpOutPacket = DataHelper.ObjectToTCPOutPacket<Dictionary<int, SpecActInfoDB>>(dbRoleInfo.SpecActInfoDict, pool, nID);
                }
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 处理专享活动数据的更新
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessUpdateSpecActCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 6)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }
                int roleID = Convert.ToInt32(fields[0]);

                SpecActInfoDB SpecAct = new SpecActInfoDB();
                SpecAct.GroupID = Convert.ToInt32(fields[1]);
                SpecAct.ActID = Convert.ToInt32(fields[2]);
                SpecAct.PurNum = Convert.ToInt32(fields[3]);
                SpecAct.CountNum = Convert.ToInt32(fields[4]);
                SpecAct.Active = Convert.ToInt16(fields[5]);

                string strcmd = "";
                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                lock (dbRoleInfo)
                {
                    if (null == dbRoleInfo.SpecActInfoDict)
                    {
                        dbRoleInfo.SpecActInfoDict = new Dictionary<int, SpecActInfoDB>();
                    }
                    dbRoleInfo.SpecActInfoDict[SpecAct.ActID] = SpecAct;
                }

                //更新一个用户角色的专享活动数据
                DBWriter.UpdateSpecialActivityData(dbMgr, roleID, SpecAct);

                strcmd = string.Format("{0}:{1}", roleID, 0);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 修改角色的每日数据值
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessUpdateRoleDailyDataCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 11)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int expDayID = Convert.ToInt32(fields[1]);
                int todayExp = Convert.ToInt32(fields[2]);
                int lingLiDayID = Convert.ToInt32(fields[3]);
                int todayLingLi = Convert.ToInt32(fields[4]);
                int killBossDayID = Convert.ToInt32(fields[5]);
                int todayKillBoss = Convert.ToInt32(fields[6]);
                int fuBenDayID = Convert.ToInt32(fields[7]);
                int todayFuBenNum = Convert.ToInt32(fields[8]);
                int wuXingDayID = Convert.ToInt32(fields[9]);
                int wuXingNum = Convert.ToInt32(fields[10]);

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                string strcmd = "";

                //将用户的请求发起写数据库的操作
                int ret = DBWriter.UpdateRoleDailyData(dbMgr, roleID, expDayID, todayExp, lingLiDayID, todayLingLi, killBossDayID, todayKillBoss, fuBenDayID, todayFuBenNum, wuXingDayID, wuXingNum);
                if (ret < 0)
                {
                    //删除朋友失败
                    strcmd = string.Format("{0}:{1}", roleID, ret);

                    LogManager.WriteLog(LogTypes.Error, string.Format("更新角色日常数据值时失败，CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));
                }
                else
                {
                    //将用户的请求更新内存缓存
                    lock (dbRoleInfo)
                    {
                        if (null == dbRoleInfo.MyRoleDailyData)
                        {
                            dbRoleInfo.MyRoleDailyData = new RoleDailyData();
                        }

                        dbRoleInfo.MyRoleDailyData.ExpDayID = expDayID;
                        dbRoleInfo.MyRoleDailyData.TodayExp = todayExp;
                        dbRoleInfo.MyRoleDailyData.LingLiDayID = lingLiDayID;
                        dbRoleInfo.MyRoleDailyData.TodayLingLi = todayLingLi;
                        dbRoleInfo.MyRoleDailyData.KillBossDayID = killBossDayID;
                        dbRoleInfo.MyRoleDailyData.TodayKillBoss = todayKillBoss;
                        dbRoleInfo.MyRoleDailyData.FuBenDayID = fuBenDayID;
                        dbRoleInfo.MyRoleDailyData.TodayFuBenNum = todayFuBenNum;
                        dbRoleInfo.MyRoleDailyData.WuXingDayID = wuXingDayID;
                        dbRoleInfo.MyRoleDailyData.WuXingNum = wuXingNum;
                    }

                    strcmd = string.Format("{0}:{1}", roleID, 0);
                }

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        private static TCPProcessCmdResults ProseccUpdateRanking(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 10)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                string rname = fields[1];
                int level = Convert.ToInt32(fields[2]);
                int occupation = Convert.ToInt32(fields[3]);
                int sub_id = Convert.ToInt32(fields[4]);
                int monphai = Convert.ToInt32(fields[5]);
                Int64 taiphu = Convert.ToInt64(fields[6]);
                int volam = Convert.ToInt32(fields[7]);
                int liendau = Convert.ToInt32(fields[8]);
                int uydanh = Convert.ToInt32(fields[9]);

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                string strcmd = "";

                int ret = DBWriter.UpdateRoleRanking(dbMgr, roleID, rname, level, occupation, sub_id, monphai, taiphu, volam, liendau, uydanh);
                if (ret < 0)
                {
                    strcmd = string.Format("{0}:{1}", roleID, ret);

                    LogManager.WriteLog(LogTypes.Error, string.Format("更新角色日常数据值时失败，CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));
                }

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 修改角色的杀BOSS总数量
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessUpdateKillBossCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int killBoss = Convert.ToInt32(fields[1]);

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                string strcmd = "";

                //将用户的请求发起写数据库的操作
                bool ret = DBWriter.UpdateKillBoss(dbMgr, roleID, killBoss);
                if (!ret)
                {
                    //删除朋友失败
                    strcmd = string.Format("{0}:{1}:{2}", roleID, killBoss, -1);

                    LogManager.WriteLog(LogTypes.Error, string.Format("更新角色杀BOSS总数量时失败，CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));
                }
                else
                {
                    //将用户的请求更新内存缓存
                    lock (dbRoleInfo)
                    {
                        dbRoleInfo.KillBoss = killBoss;
                    }

                    strcmd = string.Format("{0}:{1}:{2}", roleID, killBoss, 0);
                }

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// Lấy danh sách bảng xếp hạng
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessGetPaiHangListCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 3)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int paiHangType = Convert.ToInt32(fields[1]);
                int pagennumber = Convert.ToInt32(fields[2]);

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (roleID != 0 && null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                RankMode _RankMode = (RankMode)paiHangType;

                List<PlayerRanking> RankData = RankingManager.getInstance().GetRank(_RankMode, roleID, pagennumber);

                Ranking _Rank = new Ranking();
                _Rank.Players = RankData;
                _Rank.Type = paiHangType;
                _Rank.TotalPlayers = RankData.Count;
                _Rank.TotalPlayers = RankingManager.getInstance().Count(_RankMode);

                tcpOutPacket = DataHelper.ObjectToTCPOutPacket<Ranking>(_Rank, pool, nID);

                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 获得其他角色的数据
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessGetOtherAttrib2DataCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //int roleID = Convert.ToInt32(fields[0]);
                int otherRoleID = Convert.ToInt32(fields[1]);

                RoleDataEx roleDataEx = new RoleDataEx();

                /// 获取指定的角色信息
                /// ChenXiaojun 没有时从数据库中获取，不管角色是否删除 【21/5/2014】
                DBRoleInfo dbRoleInfo = dbMgr.GetDBAllRoleInfo(otherRoleID); // dbMgr.DBRoleMgr.FindDBRoleInfo(otherRoleID);
                if (null == dbRoleInfo)
                {
                    roleDataEx.RoleID = -1;
                }
                else
                {
                    //数据库角色信息到用户数据的转换
                    Global.DBRoleInfo2RoleDataEx(dbRoleInfo, roleDataEx);
                }

                /// 将对象转为TCP协议流
                tcpOutPacket = DataHelper.ObjectToTCPOutPacket<RoleDataEx>(roleDataEx, pool, nID);

                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 添加商城购买记录
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessAddMallBuyItemCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int goodsID = Convert.ToInt32(fields[1]);
                int goodsNum = Convert.ToInt32(fields[2]);
                int totalPrice = Convert.ToInt32(fields[3]);
                int leftMoney = Convert.ToInt32(fields[4]);

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                string strcmd = "";

                //添加一个新的商城购买记录
                int ret = DBWriter.AddNewMallBuyItem(dbMgr, roleID, goodsID, goodsNum, totalPrice, leftMoney);
                if (ret < 0)
                {
                    //删除朋友失败
                    strcmd = string.Format("{0}:{1}", roleID, -1);

                    LogManager.WriteLog(LogTypes.Error, string.Format("添加新的商城购买记录失败，CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));
                }
                else
                {
                    strcmd = string.Format("{0}:{1}", roleID, 0);
                }

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 添加奇珍阁购买记录
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessAddQiZhenGeBuyItemCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int goodsID = Convert.ToInt32(fields[1]);
                int goodsNum = Convert.ToInt32(fields[2]);
                int totalPrice = Convert.ToInt32(fields[3]);
                int leftMoney = Convert.ToInt32(fields[4]);

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                string strcmd = "";

                //添加一个新的奇珍阁购买记录
                int ret = DBWriter.AddNewQiZhenGeBuyItem(dbMgr, roleID, goodsID, goodsNum, totalPrice, leftMoney);
                if (ret < 0)
                {
                    //删除朋友失败
                    strcmd = string.Format("{0}:{1}", roleID, -1);

                    LogManager.WriteLog(LogTypes.Error, string.Format("添加新的奇珍阁购买记录失败，CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));
                }
                else
                {
                    strcmd = string.Format("{0}:{1}", roleID, 0);
                }

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 获取礼品码的信息i
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessGetLiPinMaInfoCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 3)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int songLiID = Convert.ToInt32(fields[1]);
                string liPinMa = fields[2];

                string strcmd = "";
                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //获取某个礼品码的平台信息
                int ret = 0;
                if (liPinMa.Length < 3)
                {
                    ret = -1020;
                }
                else if (liPinMa.Substring(0, 2) == "NZ")
                {
                    ret = LiPinMaManager.GetLiPinMaPingTaiID2(dbMgr, songLiID, liPinMa, dbRoleInfo.ZoneID);
                }
                else if (liPinMa.Substring(0, 2) == "NX")
                {
                    ret = LiPinMaManager.GetLiPinMaPingTaiIDNX(dbMgr, songLiID, liPinMa, dbRoleInfo.ZoneID);
                }
                else
                {
                    ret = LiPinMaManager.GetLiPinMaPingTaiID(dbMgr, songLiID, liPinMa);
                }

                strcmd = string.Format("{0}:{1}", roleID, ret);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 修改角色的充值任务ID的值
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessUpdateCZTaskIDCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int czTaskID = Convert.ToInt32(fields[1]);

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                string strcmd = "";

                //更新一个用户角色的充值TaskID
                bool ret = DBWriter.UpdateRoleCZTaskID(dbMgr, roleID, czTaskID);
                if (!ret)
                {
                    //删除朋友失败
                    strcmd = string.Format("{0}:{1}", roleID, -1);

                    LogManager.WriteLog(LogTypes.Error, string.Format("更新角色充值任务ID时失败，CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));
                }
                else
                {
                    //将用户的请求更新内存缓存
                    lock (dbRoleInfo)
                    {
                        dbRoleInfo.CZTaskID = czTaskID;
                    }

                    strcmd = string.Format("{0}:{1}", roleID, 0);
                }

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 获取总的在线人数
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessGetTotalOnlineNumCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 1)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                /*int roleID = Convert.ToInt32(fields[0]);

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }*/

                int totalOnlineNum = LineManager.GetTotalOnlineNum();
                string strcmd = string.Format("{0}", totalOnlineNum);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 强制重新加载排行榜数据
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessForceReloadPaiHangCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 1)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);

                string strcmd = string.Format("{0}:{1}", roleID, 0);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 添加银票购买记录
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessAddYinPiaoBuyItemCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int goodsID = Convert.ToInt32(fields[1]);
                int goodsNum = Convert.ToInt32(fields[2]);
                int totalPrice = Convert.ToInt32(fields[3]);
                int leftYinPiaoNum = Convert.ToInt32(fields[4]);

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                string strcmd = "";

                //添加一个新的银票购买记录
                int ret = DBWriter.AddNewYinPiaoBuyItem(dbMgr, roleID, goodsID, goodsNum, totalPrice, leftYinPiaoNum);
                if (ret < 0)
                {
                    //删除朋友失败
                    strcmd = string.Format("{0}:{1}", roleID, -1);

                    LogManager.WriteLog(LogTypes.Error, string.Format("添加新的银票购买记录失败，CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));
                }
                else
                {
                    strcmd = string.Format("{0}:{1}", roleID, 0);
                }

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 在内存中搜索在线的角色
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessSearchRolesFromDBCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 3)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                string searchText = fields[1];
                int startIndex = Convert.ToInt32(fields[2]);

                List<SearchRoleData> searchRoleDataList = null;
                int otherID = -1;

                if (searchText.Length > 0)
                {
                    otherID = dbMgr.DBRoleMgr.FindDBRoleID(searchText);
                    if (-1 != otherID)
                    {
                        DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(otherID);
                        if (null != dbRoleInfo)
                        {
                            searchRoleDataList = new List<SearchRoleData>();
                            SearchRoleData searchRoleData = new SearchRoleData()
                            {
                                RoleID = dbRoleInfo.RoleID,
                                RoleName = Global.FormatRoleName(dbRoleInfo.ZoneID, dbRoleInfo.RoleName),
                                RoleSex = dbRoleInfo.RoleSex,
                                Level = dbRoleInfo.Level,
                                Occupation = dbRoleInfo.Occupation,
                                Faction = dbRoleInfo.GuildID,
                                BHName = dbRoleInfo.GuildName
                            };

                            searchRoleDataList.Add(searchRoleData);
                        }
                    }
                }
                ////根据字符串搜索在线的角色
                //List<SearchRoleData> searchRoleDataList = DBQuery.SearchOnlineRoleByName(dbMgr, searchText, startIndex, (int)SearchResultConsts.MaxSearchRolesNum);

                /// 将对象转为TCP协议流
                tcpOutPacket = DataHelper.ObjectToTCPOutPacket<List<SearchRoleData>>(searchRoleDataList, pool, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 获取皇帝角色的数据
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessGetHuangDiRoleDataCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int huangDiRoleID = Convert.ToInt32(fields[1]);

                RoleDataEx roleDataEx = new RoleDataEx();

                /// 获取指定的角色信息
                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(huangDiRoleID);
                if (null == dbRoleInfo)
                {
                    roleDataEx.RoleID = -1;
                }
                else
                {
                    //数据库角色信息到用户数据的转换
                    Global.DBRoleInfo2RoleDataEx(dbRoleInfo, roleDataEx);
                }

                /// 将对象转为TCP协议流
                tcpOutPacket = DataHelper.ObjectToTCPOutPacket<RoleDataEx>(roleDataEx, pool, nID);

                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 处理写入刷新奇珍阁的记录
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessAddRefreshQiZhenRecCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 3)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int oldUserMoney = Convert.ToInt32(fields[1]);
                int leftUserMoney = Convert.ToInt32(fields[2]);

                string strcmd = "";
                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //添加刷新奇珍阁的记录
                DBWriter.AddRefreshQiZhenGeRec(dbMgr, roleID, oldUserMoney, leftUserMoney);

                strcmd = string.Format("{0}:{1}", 0, roleID);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 处理清空某个缓存的角色数据
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessClrCachingRoleDataCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int otherRoleID = Convert.ToInt32(fields[0]);
                string otherRoleName = fields[1];

                string strcmd = "";
                /*DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }*/

                // 没有角色id就用角色名称查找角色id
                if (otherRoleID == 0)
                {
                    otherRoleID = dbMgr.DBRoleMgr.FindDBRoleID(otherRoleName);
                    if (otherRoleID > 0)
                    {
                        //释放某个指定的角色信息
                        dbMgr.DBRoleMgr.ReleaseDBRoleInfoByID(otherRoleID);
                    }
                }
                else if (otherRoleID == 1)
                {
                    //释放帐号缓存
                    dbMgr.dbUserMgr.RemoveDBUserInfo(otherRoleName);
                }

                strcmd = string.Format("{0}:{1}", 0, otherRoleID);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 处理清空所有缓存的角色数据
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessClrAllCachingRoleDataCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 1)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int randNum = Convert.ToInt32(fields[0]);

                string strcmd = "";

                //释放某个指定的角色信息
                dbMgr.DBRoleMgr.ClearAllDBroleInfo();

                strcmd = string.Format("{0}:{1}", 0, randNum);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 处理添加元宝消费告警的记录
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessAddMoneyWarningCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 3)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int usedMoney = Convert.ToInt32(fields[1]);
                int goodsMoney = Convert.ToInt32(fields[2]);

                string strcmd = "";
                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //添加元宝消费记录告警记录
                DBWriter.AddMoneyWarning(dbMgr, roleID, usedMoney, goodsMoney);

                strcmd = string.Format("{0}:{1}", 0, roleID);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 获取物品数据
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessGetGoodsByDbIDCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int goodsDbID = Convert.ToInt32(fields[1]);

                GoodsData goodsData = null;
                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    /// 将对象转为TCP协议流
                    tcpOutPacket = DataHelper.ObjectToTCPOutPacket<GoodsData>(goodsData, pool, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //根据物品的DBID获取物品
                goodsData = Global.GetGoodsDataByDbID(dbRoleInfo, goodsDbID);

                /// 将对象转为TCP协议流
                tcpOutPacket = DataHelper.ObjectToTCPOutPacket<GoodsData>(goodsData, pool, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 查询充值金额
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessQueryChongZhiMoneyCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                string userID = fields[0];
                int zoneID = Convert.ToInt32(fields[1]);

                int userMoney = 0;
                int realMoney = 0;

                DBQuery.QueryUserMoneyByUserID(dbMgr, userID, out userMoney, out realMoney);

                string strcmd = string.Format("{0}", realMoney);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 查询今日充值金额
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessQueryDayChongZhiMoneyCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                string userID = fields[0];
                int zoneID = Convert.ToInt32(fields[1]);

                int userMoney1 = 0;
                int realMoney1 = 0;

                //根据平台用户ID查询元宝和真实的充值钱数
                DBQuery.QueryTodayUserMoneyByUserID(dbMgr, userID, zoneID, out userMoney1, out realMoney1);

                int userMoney2 = 0;
                int realMoney2 = 0;

                //根据平台用户ID查询元宝和真实的充值钱数
                DBQuery.QueryTodayUserMoneyByUserID2(dbMgr, userID, zoneID, out userMoney2, out realMoney2);

                string strcmd = string.Format("{0}", realMoney1 + realMoney2);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 添加购买记录
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessAddBuyItemFromNpcCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 6)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int goodsID = Convert.ToInt32(fields[1]);
                int goodsNum = Convert.ToInt32(fields[2]);
                int totalPrice = Convert.ToInt32(fields[3]);
                int leftMoney = Convert.ToInt32(fields[4]);
                int moneyType = Convert.ToInt32(fields[5]);

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                string strcmd = "";

                //添加一个新的银两购买记录
                int ret = DBWriter.AddNewBuyItemFromNpc(dbMgr, roleID, goodsID, goodsNum, totalPrice, leftMoney, moneyType);
                if (ret < 0)
                {
                    //删除朋友失败
                    strcmd = string.Format("{0}:{1}", roleID, -1);

                    LogManager.WriteLog(LogTypes.Error, string.Format("添加新的购买记录失败，CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));
                }
                else
                {
                    strcmd = string.Format("{0}:{1}", roleID, 0);
                }

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 添加银两购买记录
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessAddYinLiangBuyItemCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int goodsID = Convert.ToInt32(fields[1]);
                int goodsNum = Convert.ToInt32(fields[2]);
                int totalPrice = Convert.ToInt32(fields[3]);
                int leftYinLiang = Convert.ToInt32(fields[4]);

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                string strcmd = "";

                //添加一个新的银两购买记录
                int ret = DBWriter.AddNewYinLiangBuyItem(dbMgr, roleID, goodsID, goodsNum, totalPrice, leftYinLiang);
                if (ret < 0)
                {
                    //删除朋友失败
                    strcmd = string.Format("{0}:{1}", roleID, -1);

                    LogManager.WriteLog(LogTypes.Error, string.Format("添加新的银两购买记录失败，CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));
                }
                else
                {
                    strcmd = string.Format("{0}:{1}", roleID, 0);
                }

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// Lấy dữ liệu danh sách thư của người chơi
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessGetUserMailListCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 1)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);

                /// Danh sách thư
                List<MailData> mailItemDataList = Global.LoadUserMailItemDataList(dbMgr, roleID);

                tcpOutPacket = DataHelper.ObjectToTCPOutPacket<List<MailData>>(mailItemDataList, pool, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 获取用户的邮件数量
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessGetUserMailCountCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 3)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int excludeReadState = Convert.ToInt32(fields[1]);
                int limitCount = Convert.ToInt32(fields[2]);

                //获取邮件列表数据
                int emailCount = Global.LoadUserMailItemDataCount(dbMgr, roleID, excludeReadState, limitCount);

                /// 将对象转为TCP协议流
                tcpOutPacket = DataHelper.ObjectToTCPOutPacket<int>(emailCount, pool, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                // });
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// Lấy dữ liệu thư tương ứng
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessGetUserMailDataCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int mailID = Convert.ToInt32(fields[1]);

                /// Thông tin thư tương ứng
                MailData mailItemData = Global.LoadMailItemData(dbMgr, roleID, mailID);

                tcpOutPacket = DataHelper.ObjectToTCPOutPacket<MailData>(mailItemData, pool, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// Xử lý gửi thư cho người chơi
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessSendUserMailCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 9 && fields.Length != 10)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                /// ID đối tượng gửi thư
                int roleID = Convert.ToInt32(fields[0]);
                int receiverrid = Convert.ToInt32(fields[2]);
                /// Nếu độ dài 10 nghĩa là giá trị cuối quy định phải kiểm tra người nhận tồn tại không
                if (fields.Length == 10)
                {
                    int.TryParse(fields[9], out int checkReceiverExist);
                    if (checkReceiverExist != 0 && DBManager.getInstance().GetDBRoleInfo(receiverrid) == null)
                    {
                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }
                }

                int addGoodsCount = 0;
                int mailID = Global.AddMail(dbMgr, fields, out addGoodsCount);

                string strcmd = string.Format("{0}:{1}:{2}", roleID, mailID, addGoodsCount);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// Lấy vật phẩm trong thư
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessFetchMailGoodsCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int mailID = Convert.ToInt32(fields[1]);

                /// Thực hiện xóa vật phẩm và cập nhật trạng thái cho thư
                bool ret = Global.UpdateHasFetchMailGoodsStat(dbMgr, roleID, mailID);

                string strcmd = string.Format("{0}:{1}:{2}", roleID, mailID, ret ? 1 : -1);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// Xóa thư
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessDeleteUserMailCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}", (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int mailID = Convert.ToInt32(fields[1]);

                /// Thông tin thư tương ứng
                MailData mailItemData = Global.LoadMailItemData(dbMgr, roleID, mailID);
                /// Nếu thư không tồn tại
                if (mailItemData == null)
                {
                    string strcmd = string.Format("{0}:{1}:{2}", roleID, mailItemData.MailID, -100);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }
                /// Nếu chưa lấy vật phẩm đính kèm
                if (mailItemData.HasFetchAttachment == 1)
                {
                    string strcmd = string.Format("{0}:{1}:{2}", roleID, mailItemData.MailID, -101);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                /// Thực hiện xóa thư
                bool ret = Global.DeleteMail(dbMgr, roleID, mailItemData.MailID.ToString());

                string _strcmd = string.Format("{0}:{1}:{2}", roleID, mailItemData.MailID, ret ? 1 : -1);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, _strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 根据角色名称查询角色ID
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessGetRoleIDByRoleNameCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 1)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //传输过程中 : 被 $替换
                string roleName = fields[0].Replace('$', ':');
                int roleID = Global.FindDBRoleID(dbMgr, roleName);

                string strcmd = string.Format("{0}:{1}", roleID, roleID, roleName);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 充值返利
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessSprQueryInputFanLiCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                string fromDate = fields[1].Replace('$', ':');
                string toDate = fields[2].Replace('$', ':');
                //返利百分比,已经由gameserver验证
                double addPercent = Convert.ToDouble(fields[3]);

                string strcmd = "";

                DBRoleInfo roleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == roleInfo)
                {
                    strcmd = string.Format("{0}:{1}:0", -1001, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //查询时如果当前时间小于结束时间，就用采用结束时间也行
                //活动时间范围内的充值数，真实货币单位
                //int inputMoneyInPeriod = DBQuery.GetUserInputMoney(dbMgr, roleInfo.UserID, roleInfo.ZoneID, fromDate, toDate);
                RankDataKey key = new RankDataKey(RankType.Charge, fromDate, toDate, null);
                int inputMoneyInPeriod = roleInfo.RankValue.GetRankValue(key);
                if (inputMoneyInPeriod < 0)
                {
                    inputMoneyInPeriod = 0;
                }

                //时间范围内的元宝数
                int roleYuanBaoInPeriod = inputMoneyInPeriod; //Global.TransMoneyToYuanBao(inputMoneyInPeriod);

                //返利元宝数
                int fanliYuanBao = (int)(roleYuanBaoInPeriod * addPercent);

                strcmd = string.Format("{0}:{1}:{2}", 1, roleID, fanliYuanBao);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 充值送礼
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessSprQueryInputJiaSongCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                string fromDate = fields[1].Replace('$', ':');
                string toDate = fields[2].Replace('$', ':');
                //最小元宝值,已经由gameserver验证
                int gateYuanBao = Convert.ToInt32(fields[3]);

                string strcmd = "";

                DBRoleInfo roleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == roleInfo)
                {
                    strcmd = string.Format("{0}:{1}:0", -1001, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //必须大于0
                if (gateYuanBao <= 0)
                {
                    strcmd = string.Format("{0}:{1}:0", -1002, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //查询时如果当前时间小于结束时间，就用采用结束时间也行
                //活动时间范围内的充值数，真实货币单位
                //int inputMoneyInPeriod = DBQuery.GetUserInputMoney(dbMgr, roleInfo.UserID, roleInfo.ZoneID, fromDate, toDate);
                RankDataKey key = new RankDataKey(RankType.Charge, fromDate, toDate, null);
                int inputMoneyInPeriod = roleInfo.RankValue.GetRankValue(key);
                //时间范围内没冲过值
                if (inputMoneyInPeriod <= 0)
                {
                    strcmd = string.Format("{0}:{1}:0", -1004, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //时间范围内的元宝数,充值了，但没达到最低标准
                int roleYuanBaoInPeriod = inputMoneyInPeriod; //Global.TransMoneyToYuanBao(inputMoneyInPeriod);
                if (roleYuanBaoInPeriod < gateYuanBao)
                {
                    strcmd = string.Format("{0}:{1}:0", -1005, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //告诉角色，可以领取充值附加物品奖励
                strcmd = string.Format("{0}:{1}:{2}", 1, roleID, roleYuanBaoInPeriod);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                // });
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 充值王
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessSprQueryInputKingCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                string fromDate = fields[1].Replace('$', ':');
                string toDate = fields[2].Replace('$', ':');
                //排名最低元宝要求列表，依次为第一名的最小元宝，第二名的最小元宝......
                string[] minYuanBaoArr = fields[3].Split('_');

                List<int> minGateValueList = new List<int>();
                foreach (var item in minYuanBaoArr)
                {
                    minGateValueList.Add(Global.SafeConvertToInt32(item));
                }

                //返回排行信息,通过活动限制值过滤后的排名,可能没有第一名等名次
                List<InputKingPaiHangData> listPaiHang = Global.GetInputKingPaiHangListByHuoDongLimit(dbMgr, fromDate, toDate, minGateValueList);

                //更新最大等级角色名称 和区号
                foreach (var item in listPaiHang)
                {
                    Global.GetUserMaxLevelRole(dbMgr, item.UserID, out item.MaxLevelRoleName, out item.MaxLevelRoleZoneID);
                }

                //生成排行信息的tcp对象流
                tcpOutPacket = DataHelper.ObjectToTCPOutPacket<List<InputKingPaiHangData>>(listPaiHang, pool, nID);

                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 冲级王
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessSprQueryLevelKingCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                return Global.GetHuoDongPaiHangForKing(dbMgr, pool, nID, fields, (int)ActivityTypes.LevelKing, out tcpOutPacket);
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 装备王
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessSprQueryEquipKingCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                return Global.GetHuoDongPaiHangForKing(dbMgr, pool, nID, fields, (int)ActivityTypes.EquipKing, out tcpOutPacket);
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 坐骑王
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessSprQueryHorseKingCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                return Global.GetHuoDongPaiHangForKing(dbMgr, pool, nID, fields, (int)ActivityTypes.HorseKing, out tcpOutPacket);
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 经脉王
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessSprQueryJingMaiKingCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                return Global.GetHuoDongPaiHangForKing(dbMgr, pool, nID, fields, (int)ActivityTypes.JingMaiKing, out tcpOutPacket);
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        //***************************************************************************************************************************************
        /// <summary>
        /// 充值返利奖励
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessExcuteInputFanLiCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                string fromDate = fields[1].Replace('$', ':');
                string toDate = fields[2].Replace('$', ':');
                //返利百分比,已经由gameserver验证
                double addPercent = Convert.ToDouble(fields[3]);

                string strcmd = "";

                DBRoleInfo roleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == roleInfo)
                {
                    strcmd = string.Format("{0}:{1}:0", -1001, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //活动时间范围内的充值数，真实货币单位
                //int inputMoneyInPeriod = DBQuery.GetUserInputMoney(dbMgr, roleInfo.UserID, roleInfo.ZoneID, fromDate, toDate);
                RankDataKey key = new RankDataKey(RankType.Charge, fromDate, toDate, null);
                int inputMoneyInPeriod = roleInfo.RankValue.GetRankValue(key);
                if (inputMoneyInPeriod <= 0)
                {
                    strcmd = string.Format("{0}:{1}:0", -10006, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //活动关键字，能够唯一标志某内活动的一个实例【在某段时间内的活动】
                string huoDongKeyStr = Global.GetHuoDongKeyString(fromDate, toDate);

                int hasgettimes = 0;
                string lastgettime = "";

                DBUserInfo userInfo = dbMgr.GetDBUserInfo(roleInfo.UserID);

                //避免同一用户账号同时多次操作
                lock (userInfo)
                {
                    //判断是否领取过---活动关键字字符串唯一确定了每次活动，针对同类型不同时间的活动
                    DBQuery.GetAwardHistoryForUser(dbMgr, roleInfo.UserID, (int)(ActivityTypes.InputFanLi), huoDongKeyStr, out hasgettimes, out lastgettime);

                    //这个活动每次每个用户最多领取一次
                    if (hasgettimes > 0)
                    {
                        strcmd = string.Format("{0}:{1}:0", -10005, roleID);
                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }

                    //更新已领取状态
                    int ret = DBWriter.AddHongDongAwardRecordForUser(dbMgr, roleInfo.UserID, (int)(ActivityTypes.InputFanLi), huoDongKeyStr, 1, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    if (ret < 0)
                    {
                        strcmd = string.Format("{0}:{1}:0", -1006, roleID);
                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }
                }

                //时间范围内的元宝数
                int roleYuanBaoInPeriod = inputMoneyInPeriod; //Global.TransMoneyToYuanBao(inputMoneyInPeriod);

                strcmd = string.Format("{0}:{1}:{2}", 1, roleID, roleYuanBaoInPeriod);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 充值送礼奖励
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessExcuteInputJiaSongCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                string fromDate = fields[1].Replace('$', ':');
                string toDate = fields[2].Replace('$', ':');
                //最小元宝值,已经由gameserver验证
                int gateYuanBao = Convert.ToInt32(fields[3]);

                string strcmd = "";

                DBRoleInfo roleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == roleInfo)
                {
                    strcmd = string.Format("{0}:{1}:0", -1001, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //必须大于0
                if (gateYuanBao <= 0)
                {
                    strcmd = string.Format("{0}:{1}:0", -1002, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //活动时间范围内的充值数，真实货币单位
                //int inputMoneyInPeriod = DBQuery.GetUserInputMoney(dbMgr, roleInfo.UserID, roleInfo.ZoneID, fromDate, toDate);
                RankDataKey key = new RankDataKey(RankType.Charge, fromDate, toDate, null);
                int inputMoneyInPeriod = roleInfo.RankValue.GetRankValue(key);
                //时间范围内没冲过值
                if (inputMoneyInPeriod <= 0)
                {
                    strcmd = string.Format("{0}:{1}:0", -10006, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //时间范围内的元宝数,充值了，但没达到最低标准
                int roleYuanBaoInPeriod = inputMoneyInPeriod; //Global.TransMoneyToYuanBao(inputMoneyInPeriod);
                if (roleYuanBaoInPeriod < gateYuanBao)
                {
                    strcmd = string.Format("{0}:{1}:0", -10007, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //活动关键字，能够唯一标志某内活动的一个实例【在某段时间内的活动】
                string huoDongKeyStr = Global.GetHuoDongKeyString(fromDate, toDate);

                int hasgettimes = 0;
                string lastgettime = "";

                DBUserInfo userInfo = dbMgr.GetDBUserInfo(roleInfo.UserID);

                //避免同一用户账号同时多次操作
                lock (userInfo)
                {
                    //判断是否领取过---活动关键字字符串唯一确定了每次活动，针对同类型不同时间的活动
                    DBQuery.GetAwardHistoryForUser(dbMgr, roleInfo.UserID, (int)(ActivityTypes.InputJiaSong), huoDongKeyStr, out hasgettimes, out lastgettime);

                    //这个活动每次每个用户最多领取一次
                    if (hasgettimes > 0)
                    {
                        strcmd = string.Format("{0}:{1}:0", -10005, roleID);
                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }

                    //更新已领取状态
                    int ret = DBWriter.AddHongDongAwardRecordForUser(dbMgr, roleInfo.UserID, (int)(ActivityTypes.InputJiaSong), huoDongKeyStr, 1, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    if (ret < 0)
                    {
                        strcmd = string.Format("{0}:{1}:0", -1007, roleID);
                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }
                }

                strcmd = string.Format("{0}:{1}:{2}", 1, roleID, roleYuanBaoInPeriod);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 充值王奖励
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessExcuteInputKingCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                string fromDate = fields[1].Replace('$', ':');
                string toDate = fields[2].Replace('$', ':');
                //排名最低元宝要求列表，依次为第一名的最小元宝，第二名的最小元宝......
                string[] minYuanBaoArr = fields[3].Split('_');

                List<int> minGateValueList = new List<int>();
                foreach (var item in minYuanBaoArr)
                {
                    minGateValueList.Add(Global.SafeConvertToInt32(item));
                }

                string strcmd = "";

                DBRoleInfo roleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == roleInfo)
                {
                    strcmd = string.Format("{0}:{1}:0", -1001, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //返回排行信息,通过活动限制值过滤后的排名,可能没有第一名等名次
                List<InputKingPaiHangData> listPaiHang = Global.GetInputKingPaiHangListByHuoDongLimit(dbMgr, fromDate, toDate, minGateValueList);
                //排行值
                int paiHang = -1;
                //活动时间范围的充值，真实货币单位
                int inputMoneyInPeriod = 0;

                for (int n = 0; n < listPaiHang.Count; n++)
                {
                    if (roleInfo.UserID == listPaiHang[n].UserID)
                    {
                        paiHang = listPaiHang[n].PaiHang;//得到排行值
                        inputMoneyInPeriod = listPaiHang[n].PaiHangValue;
                    }
                }

                //判断是否在排行内
                if (paiHang <= 0)
                {
                    strcmd = string.Format("{0}:{1}:0", -1003, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //在排行内但未充值，GetUserInputPaiHang()内已经做了过滤
                if (inputMoneyInPeriod <= 0)
                {
                    strcmd = string.Format("{0}:{1}:0", -10006, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //时间范围内获取的元宝数
                int roleGetYuanBaoInPeriod = inputMoneyInPeriod;//Global.TransMoneyToYuanBao(inputMoneyInPeriod);

                //对排行进行修正之后，超出了奖励范围,即充值元宝数量没有达到最低的元宝数量要求
                if (paiHang <= 0)
                {
                    strcmd = string.Format("{0}:{1}:0", -10007, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //活动关键字，能够唯一标志某内活动的一个实例【在某段时间内的活动】
                string huoDongKeyStr = Global.GetHuoDongKeyString(fromDate, toDate);

                int hasgettimes = 0;
                string lastgettime = "";

                DBUserInfo userInfo = dbMgr.GetDBUserInfo(roleInfo.UserID);

                //避免同一用户账号同时多次操作
                lock (userInfo)
                {
                    //判断是否领取过---活动关键字字符串唯一确定了每次活动，针对同类型不同时间的活动
                    DBQuery.GetAwardHistoryForUser(dbMgr, roleInfo.UserID, (int)(ActivityTypes.InputKing), huoDongKeyStr, out hasgettimes, out lastgettime);

                    //这个活动每次每个用户最多领取一次
                    if (hasgettimes > 0)
                    {
                        strcmd = string.Format("{0}:{1}:0", -10005, roleID);
                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }

                    //更新已领取状态
                    int ret = DBWriter.AddHongDongAwardRecordForUser(dbMgr, roleInfo.UserID, (int)(ActivityTypes.InputKing), huoDongKeyStr, 1, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    if (ret < 0)
                    {
                        strcmd = string.Format("{0}:{1}:0", -1008, roleID);
                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }
                }

                strcmd = string.Format("{0}:{1}:{2}", 1, roleID, paiHang);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 冲级王奖励
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessExcuteLevelKingCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                return Global.ProcessHuoDongForKing(dbMgr, pool, nID, fields, (int)ActivityTypes.LevelKing, out tcpOutPacket);
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 装备王奖励
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessExcuteEquipKingCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                return Global.ProcessHuoDongForKing(dbMgr, pool, nID, fields, (int)ActivityTypes.EquipKing, out tcpOutPacket);
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 坐骑王奖励
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessExcuteHorseKingCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                return Global.ProcessHuoDongForKing(dbMgr, pool, nID, fields, (int)ActivityTypes.HorseKing, out tcpOutPacket);
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 经脉王奖励
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessExcuteJingMaiKingCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                return Global.ProcessHuoDongForKing(dbMgr, pool, nID, fields, (int)ActivityTypes.JingMaiKing, out tcpOutPacket);
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        //**************************************************************************************************************************************

        /// <summary>
        /// 查询活动奖励领取记录
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessSprQueryAwardHistoryCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                string fromDate = fields[1].Replace('$', ':');
                string toDate = fields[2].Replace('$', ':');
                int activityType = Global.SafeConvertToInt32(fields[3]);

                string strcmd = "";

                DBRoleInfo roleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == roleInfo)
                {
                    strcmd = string.Format("{0}:{1}:0:0", -1001, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //活动关键字，能够唯一标志某内活动的一个实例【在某段时间内的活动】
                string huoDongKeyStr = Global.GetHuoDongKeyString(fromDate, toDate);

                int hasgettimes = 0;
                string lastgettime = "";

                if ((int)ActivityTypes.InputFanLi == activityType || (int)ActivityTypes.InputJiaSong == activityType ||
                    (int)ActivityTypes.InputKing == activityType)
                {
                    //针对用户的奖励---活动关键字字符串唯一确定了每次活动，针对同类型不同时间的活动
                    DBQuery.GetAwardHistoryForUser(dbMgr, roleInfo.UserID, activityType, huoDongKeyStr, out hasgettimes, out lastgettime);
                }
                else
                {
                    //针对角色的奖励
                    DBQuery.GetAwardHistoryForRole(dbMgr, roleInfo.RoleID, roleInfo.ZoneID, activityType, huoDongKeyStr, out hasgettimes, out lastgettime);
                }

                strcmd = string.Format("{0}:{1}:{2}:{3}", 1, roleID, activityType, hasgettimes);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 查询帐号活动奖励领取记录
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessSprQueryUserActivityInfoCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 4)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                string huoDongKeyStr = fields[1];
                int activityType = Global.SafeConvertToInt32(fields[2]);

                string strcmd = "";

                DBRoleInfo roleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == roleInfo)
                {
                    strcmd = string.Format("{0}:{1}:0:0", -1001, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                long hasgettimes = 0;
                string lastgettime = "";

                //针对用户的奖励---活动关键字字符串唯一确定了每次活动，针对同类型不同时间的活动
                int ret = DBQuery.GetAwardHistoryForUser(dbMgr, roleInfo.UserID, activityType, huoDongKeyStr, out hasgettimes, out lastgettime);

                strcmd = string.Format("{0}:{1}:{2}:{3}", ret, roleID, activityType, hasgettimes);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 更新帐号活动奖励领取记录
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessSprUpdateUserActivityInfoCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                string huoDongKeyStr = fields[1];
                int activityType = Global.SafeConvertToInt32(fields[2]);
                long hasgettimes = Global.SafeConvertToInt64(fields[3]);
                string lastgettime = fields[4];

                string strcmd = "";

                DBRoleInfo roleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == roleInfo)
                {
                    strcmd = string.Format("{0}:{1}:0:0", -1001, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int ret = 0;
                lock (roleInfo)
                {
                    ret = DBWriter.UpdateHongDongAwardRecordForUser(dbMgr, roleInfo.UserID, activityType, huoDongKeyStr, hasgettimes, lastgettime);
                    if (ret < 0)
                    {
                        ret = DBWriter.AddHongDongAwardRecordForUser(dbMgr, roleInfo.UserID, activityType, huoDongKeyStr, hasgettimes, lastgettime);
                    }
                }

                strcmd = string.Format("{0}:{1}:{2}:{3}", ret, roleID, activityType, hasgettimes);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 查询限量购买的物品已经购买的次数
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessDBQueryLimitGoodsUsedNumCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int goodsID = Convert.ToInt32(fields[1]);

                int dayID = 0;
                int usedNum = 0;

                string strcmd = "";

                //通过角色ID和物品ID查询物品每日的已经购买数量
                int ret = DBQuery.QueryLimitGoodsUsedNumByRoleID(dbMgr, roleID, goodsID, out dayID, out usedNum);
                if (ret < 0)
                {
                    //删除朋友失败
                    strcmd = string.Format("{0}:{1}:{2}:{3}", roleID, ret, dayID, usedNum);

                    LogManager.WriteLog(LogTypes.Error, string.Format("通过角色ID和物品ID查询物品每日的已经购买数量失败，CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));
                }
                else
                {
                    strcmd = string.Format("{0}:{1}:{2}:{3}", roleID, 0, dayID, usedNum);
                }

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 更新限量购买的物品已经购买的次数
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessDBUpdateLimitGoodsUsedNumCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 4)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int goodsID = Convert.ToInt32(fields[1]);
                int dayID = Convert.ToInt32(fields[2]);
                int usedNum = Convert.ToInt32(fields[3]);

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                string strcmd = "";

                //添加限购物品的历史记录
                int ret = DBWriter.AddLimitGoodsBuyItem(dbMgr, roleID, goodsID, dayID, usedNum);
                if (ret < 0)
                {
                    //删除朋友失败
                    strcmd = string.Format("{0}:{1}", roleID, -1);

                    LogManager.WriteLog(LogTypes.Error, string.Format("添加限购物品的历史记录失败，CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));
                }
                else
                {
                    strcmd = string.Format("{0}:{1}", roleID, 0);
                }

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 更新针对角色的单次奖励标志位[具体每一位表示什么由gameserver决定]
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessUpdateSingleTimeAwardFlagCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                long onceAwardFlag = Convert.ToInt64(fields[1]);

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                string strcmd = "";

                //更新一个用户角色的充值TaskID
                bool ret = DBWriter.UpdateRoleOnceAwardFlag(dbMgr, roleID, onceAwardFlag);
                if (!ret)
                {
                    //删除朋友失败
                    strcmd = string.Format("{0}:{1}", roleID, -1);

                    LogManager.WriteLog(LogTypes.Error, string.Format("更新角色充值任务ID时失败，CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));
                }
                else
                {
                    //将用户的请求更新内存缓存
                    lock (dbRoleInfo)
                    {
                        dbRoleInfo.OnceAwardFlag = onceAwardFlag;
                    }

                    strcmd = string.Format("{0}:{1}", roleID, 0);
                }

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 添加生肖运程竞猜记录
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessAddShengXiaoGuessHisotryCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 1)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                string[] sGuessResults = fields[0].Split(';');

                //依次遍历每一个角色的竞猜信息
                for (int i = 0; i < sGuessResults.Length; i++)
                {
                    string[] singleRoleResults = sGuessResults[0].Split(','); ;

                    if (singleRoleResults.Length != 2)
                    {
                        continue;
                    }

                    int roleID = Convert.ToInt32(singleRoleResults[0]);

                    DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                    if (null == dbRoleInfo)
                    {
                        LogManager.WriteLog(LogTypes.Error, string.Format("记录竞猜历史是需要处理的的角色不存在，CMD={0}, RoleID={1}",
                            (TCPGameServerCmds)nID, roleID));
                        continue;
                    }

                    string roleName = dbRoleInfo.RoleName;
                    int zoneID = dbRoleInfo.ZoneID;

                    string[] guessItems = singleRoleResults[1].Split('|');

                    //依次处理角色的每一个竞猜项
                    for (int n = 0; n < guessItems.Length; n++)
                    {
                        string[] itemFields = guessItems[n].Split('_');

                        if (itemFields.Length != 5)
                        {
                            continue;
                        }

                        int guessKey = Convert.ToInt32(itemFields[0]);
                        int mortgage = Convert.ToInt32(itemFields[1]);
                        int resultKey = Convert.ToInt32(itemFields[2]);
                        int gainNum = Convert.ToInt32(itemFields[3]);
                        int leftMortgage = Convert.ToInt32(itemFields[4]);

                        //添加一个新的生肖竞猜记录
                        int ret = DBWriter.AddNewShengXiaoGuessHistory(dbMgr, roleID, roleName, zoneID, guessKey, mortgage, resultKey, gainNum, leftMortgage);
                        if (ret < 0)
                        {
                            //添加生肖竞猜失败,简单记录一下日志就行
                            LogManager.WriteLog(LogTypes.Error, string.Format("添加新的生肖竞猜记录失败，CMD={0}, RoleID={1}",
                                (TCPGameServerCmds)nID, roleID));
                        }
                    }
                }

                string strcmd = string.Format("{0}:{1}", 1, 0);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 处理更新用户金币相关操作
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessUpdateUserGoldCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            /*
             *  注意：如果协议修改了，一定不要忘记确认下是否需要修改ProcessRoleHuobiOffline这个函数
             *  chenjg 20150422
             */
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int addOrSubUserGold = Convert.ToInt32(fields[1]);

                string strcmd = "";
                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                bool failed = false;
                int userGold = 0;
                lock (dbRoleInfo)
                {
                    //判断如果是扣除操作，则可能是银两余额不足而失败
                    if (addOrSubUserGold < 0 && dbRoleInfo.Gold < Math.Abs(addOrSubUserGold))
                    {
                        failed = true;
                    }
                    else //处理元宝的加减
                    {
                        dbRoleInfo.Gold = Math.Max(0, dbRoleInfo.Gold + addOrSubUserGold);
                        userGold = dbRoleInfo.Gold;
                    }
                }

                //如果扣除失败
                if (failed)
                {
                    //添加任务失败
                    strcmd = string.Format("{0}:{1}", roleID, -1);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //不等于0，才更新数据库
                if (addOrSubUserGold != 0)
                {
                    //将用户的请求发起写数据库的操作
                    bool ret = DBWriter.UpdateRoleGold(dbMgr, roleID, userGold);
                    if (!ret)
                    {
                        LogManager.WriteLog(LogTypes.Error, string.Format("更新角色金币失败，CMD={0}, RoleID={1}",
                            (TCPGameServerCmds)nID, roleID));

                        //添加任务失败
                        strcmd = string.Format("{0}:{1}", roleID, -2);

                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }
                }


                strcmd = string.Format("{0}:{1}", roleID, userGold);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 添加金币购买记录
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessAddGoldBuyItemCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int goodsID = Convert.ToInt32(fields[1]);
                int goodsNum = Convert.ToInt32(fields[2]);
                int totalPrice = Convert.ToInt32(fields[3]);
                int leftGold = Convert.ToInt32(fields[4]);

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                string strcmd = "";

                //添加一个新的银两购买记录[这儿可能存在]
                int ret = DBWriter.AddNewGoldBuyItem(dbMgr, roleID, goodsID, goodsNum, totalPrice, leftGold);
                if (ret < 0)
                {
                    //失败
                    strcmd = string.Format("{0}:{1}", roleID, -1);

                    LogManager.WriteLog(LogTypes.Error, string.Format("添加新的金币购买记录失败，CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));
                }
                else
                {
                    strcmd = string.Format("{0}:{1}", roleID, 0);
                }

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 更新物品限制使用次数表
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessUpdateGoodsLimitCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 4)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int goodsID = Convert.ToInt32(fields[1]);
                int dayID = Convert.ToInt32(fields[2]);
                int usedNum = Convert.ToInt32(fields[3]);

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                string strcmd = "";

                //更新一个角色的物品使用次数限制
                bool ret = DBWriter.UpdateGoodsLimit(dbMgr, roleID, goodsID, dayID, usedNum);
                if (!ret)
                {
                    //删除朋友失败
                    strcmd = string.Format("{0}:{1}", roleID, -1);

                    LogManager.WriteLog(LogTypes.Error, string.Format("更新角色物品限制时失败，CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));
                }
                else
                {
                    //物品限制列表中加入指定的物品
                    Global.UpdateGoodsLimitByID(dbRoleInfo, goodsID, dayID, usedNum);

                    strcmd = string.Format("{0}:{1}", roleID, 0);
                }

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// Thực hiện update RolePRamMenter
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessUpdateRoleParamCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {

            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 3)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                string name = fields[1];
                string value = fields[2];

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                string strcmd = "";


                Global.UpdateRoleParamByName(dbMgr, dbRoleInfo, name, value);


                strcmd = string.Format("{0}:{1}", roleID, 0);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }


        ///
        /// 查询限时抢购购买记录信息
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessQueryQiangGouBuyItemInfoCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int goodsID = Convert.ToInt32(fields[1]);
                int qiangGouID = Convert.ToInt32(fields[2]);
                int random = Convert.ToInt32(fields[3]);
                int actStartDay = Convert.ToInt32(fields[4]);

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleBuyNum = 0, totalBuyNum = 0;

                //查询新的抢购记录信息
                Global.QueryQiangGouBuyItemInfo(dbMgr, roleID, goodsID, qiangGouID, random, actStartDay, out roleBuyNum, out totalBuyNum);

                string strcmd = string.Format("{0}:{1}:{2}", roleID, roleBuyNum, totalBuyNum);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 添加砸金蛋记录
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessAddZaJinDanHisotryCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 1)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                string[] lines = fields[0].Split(';');
                String rname, uid;
                for (int n = 0; n < lines.Length; n++)
                {
                    string[] lineFields = lines[n].Split('_');

                    if (lineFields.Length < 12)
                    {
                        continue;
                    }

                    int rid = Convert.ToInt32(lineFields[0]);
                    Global.GetRoleNameAndUserID(dbMgr, rid, out rname, out uid);
                    int zoneid = Convert.ToInt32(lineFields[2]);
                    int timesselected = Convert.ToInt32(lineFields[3]);
                    int usedyuanbao = Convert.ToInt32(lineFields[4]);
                    int usedjindan = Convert.ToInt32(lineFields[5]);
                    int gaingoodsid = Convert.ToInt32(lineFields[6]);
                    int gaingoodsnum = Convert.ToInt32(lineFields[7]);
                    int gaingold = Convert.ToInt32(lineFields[8]);
                    int gainyinliang = Convert.ToInt32(lineFields[9]);
                    int gainexp = Convert.ToInt32(lineFields[10]);
                    string strPorp = lineFields[11];
                    /*int nGoodsLevel = Convert.ToInt32(lineFields[11]);
                    int nAppendProp = Convert.ToInt32(lineFields[12]);
                    int nLuckyProp = Convert.ToInt32(lineFields[13]);
                    int nExcellenceProp = Convert.ToInt32(lineFields[14]);*/

                    //添加一个新的砸金蛋记录
                    int ret = DBWriter.AddNewZaJinDanHistory(dbMgr, rid, rname, zoneid, timesselected, usedyuanbao,
                        usedjindan, gaingoodsid, gaingoodsnum, gaingold, gainyinliang, gainexp, strPorp); //nGoodsLevel, nAppendProp, nLuckyProp, nExcellenceProp);
                    if (ret < 0)
                    {
                        //添加生肖竞猜失败,简单记录一下日志就行
                        LogManager.WriteLog(LogTypes.Error, string.Format("添加新的砸金蛋记录失败，CMD={0}, RoleID={1}",
                            (TCPGameServerCmds)nID, rid));
                    }
                }

                string strcmd = string.Format("{0}:{1}", 1, 0);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 查询首冲大礼的领取状态
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessQueryFirstChongZhiDaLiByUserIDCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 1)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //参数0是roleid
                int roleID = Convert.ToInt32(fields[0]);

                string strcmd = "";
                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    strcmd = string.Format("{0}", -1);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                /// 通过用户ID，查询是否已经领取过首充大礼
                int totalNum = DBQuery.GetFirstChongZhiDaLiNum(dbMgr, dbRoleInfo.UserID);

                strcmd = string.Format("{0}", totalNum);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 查询每日充值大礼的领取状态
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessQueryDayChongZhiDaLiByUserIDCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //参数0是roleid
                int roleID = Convert.ToInt32(fields[0]);
                int dayID = Convert.ToInt32(fields[1]);

                string strcmd = "";
                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    strcmd = string.Format("{0}", -1);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                /// 通过用户ID，查询是否已经领取过每日充值大礼
                int totalNum = 0;

                strcmd = string.Format("{0}", totalNum);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 查询今天要奖励的角色ID
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessQueryKaiFuOnlineAwardRoleIDCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 1)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int dayID = Convert.ToInt32(fields[0]);

                string strcmd = "";

                int totalRoleNum = 0;

                /// 通过用户ID，查询是否已经领取过首充大礼
                int roleID = DBQuery.GetKaiFuOnlineAwardRoleID(dbMgr, dayID, out totalRoleNum);

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    strcmd = string.Format("{0}:{1}:{2}:{3}", -1, 0, "", 0);
                }
                else
                {
                    strcmd = string.Format("{0}:{1}:{2}:{3}", roleID, dbRoleInfo.ZoneID, dbRoleInfo.RoleName, totalRoleNum);
                }

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 添加开服在线奖励项
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessAddKaiFuOnlineAwardCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int dayID = Convert.ToInt32(fields[1]);
                int yuanBao = Convert.ToInt32(fields[2]);
                int totalRoleNum = Convert.ToInt32(fields[3]);
                int zoneID = Convert.ToInt32(fields[4]);

                int ret = DBWriter.AddKaiFuOnlineAward(dbMgr, roleID, dayID, yuanBao, totalRoleNum, zoneID);

                string strcmd = "";
                strcmd = string.Format("{0}", ret);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 查询开服在线奖励项列表
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessQueryKaiFuOnlineAwardListCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int zoneID = Convert.ToInt32(fields[1]);

                List<KaiFuOnlineAwardData> list = DBQuery.GetKaiFuOnlineAwardDataList(dbMgr, zoneID);
                tcpOutPacket = DataHelper.ObjectToTCPOutPacket<List<KaiFuOnlineAwardData>>(list, pool, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 添加系统给予的元宝记录奖励项
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessAddGiveUserMoneyItemCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 3)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int yuanBao = Convert.ToInt32(fields[1]);
                string giveType = fields[2];

                int ret = DBWriter.AddSystemGiveUserMoney(dbMgr, roleID, yuanBao, giveType);

                string strcmd = "";
                strcmd = string.Format("{0}", ret);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 添加系统交易1日志
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessAddExchange1ItemCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 6)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int rid = Convert.ToInt32(fields[0]);
                int goodsid = Convert.ToInt32(fields[1]);
                int goodsnum = Convert.ToInt32(fields[2]);
                int leftgoodsnum = Convert.ToInt32(fields[3]);
                int otherroleid = Convert.ToInt32(fields[4]);
                string result = fields[5];

                int ret = DBWriter.AddExchange1Item(dbMgr, rid, goodsid, goodsnum, leftgoodsnum, otherroleid, result);

                string strcmd = "";
                strcmd = string.Format("{0}", ret);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 添加系统交易2日志
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessAddExchange2ItemCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 4)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int rid = Convert.ToInt32(fields[0]);
                int yinliang = Convert.ToInt32(fields[1]);
                int leftyinliang = Convert.ToInt32(fields[2]);
                int otherroleid = Convert.ToInt32(fields[3]);

                int ret = DBWriter.AddExchange2Item(dbMgr, rid, yinliang, leftyinliang, otherroleid);

                string strcmd = "";
                strcmd = string.Format("{0}", ret);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 添加系统交易3日志
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessAddExchange3ItemCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 4)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int rid = Convert.ToInt32(fields[0]);
                int yuanbao = Convert.ToInt32(fields[1]);
                int leftyuanbao = Convert.ToInt32(fields[2]);
                int otherroleid = Convert.ToInt32(fields[3]);

                int ret = DBWriter.AddExchange3Item(dbMgr, rid, yuanbao, leftyuanbao, otherroleid);

                string strcmd = "";
                strcmd = string.Format("{0}", ret);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 更新针对角色的一些属性
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessUpdateRolePropsCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 3)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int rolePropIindex = Convert.ToInt32(fields[1]);
                long propValue = Convert.ToInt64(fields[2]);

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                string strcmd = "";

                bool ret = false;
                switch ((RolePropIndexs)rolePropIindex)
                {
                    case RolePropIndexs.BanChat:
                        ret = DBWriter.UpdateRoleBanProps(dbMgr, roleID, "banchat", propValue);
                        break;

                    case RolePropIndexs.BanLogin:
                        ret = DBWriter.UpdateRoleBanProps(dbMgr, roleID, "banlogin", propValue);
                        break;

                    case RolePropIndexs.BanTrade:
                        ret = DBWriter.UpdateRoleBanProps(dbMgr, roleID, "ban_trade_to_ticks", propValue);
                        break;

                    default:
                        break;
                }

                if (!ret)
                {
                    //删除朋友失败
                    strcmd = string.Format("{0}:{1}", roleID, -1);

                    LogManager.WriteLog(LogTypes.Error, string.Format("更新角色属性值时失败，CMD={0}, RoleID={1}, PropIndex={2}",
                        (TCPGameServerCmds)nID, roleID, rolePropIindex));
                }
                else
                {
                    //将用户的请求更新内存缓存
                    lock (dbRoleInfo)
                    {
                        switch ((RolePropIndexs)rolePropIindex)
                        {
                            case RolePropIndexs.BanChat:
                                dbRoleInfo.BanChat = (int)propValue;
                                break;

                            case RolePropIndexs.BanLogin:
                                dbRoleInfo.BanLogin = (int)propValue;
                                break;

                            case RolePropIndexs.BanTrade:
                                dbRoleInfo.BanTradeToTicks = propValue;
                                break;

                            default:
                                break;
                        }
                    }

                    strcmd = string.Format("{0}:{1}", roleID, 0);
                }

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 节日礼包查询
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessQueryJieriDaLiBaoCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                string fromDate = fields[1].Replace('$', ':');
                string toDate = fields[2].Replace('$', ':');

                string strcmd = "";

                DBRoleInfo roleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == roleInfo)
                {
                    strcmd = string.Format("{0}:{1}:0", -1001, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //活动关键字，能够唯一标志某内活动的一个实例【在某段时间内的活动】
                string huoDongKeyStr = Global.GetHuoDongKeyString(fromDate, toDate);

                int hasgettimes = 0;
                string lastgettime = "";

                //判断是否领取过---活动关键字字符串唯一确定了每次活动，针对同类型不同时间的活动
                DBQuery.GetAwardHistoryForRole(dbMgr, roleID, roleInfo.ZoneID, (int)(ActivityTypes.JieriDaLiBao), huoDongKeyStr, out hasgettimes, out lastgettime);

                strcmd = string.Format("{0}:{1}:{2}", 1, roleID, hasgettimes);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 节日登录查询
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessQueryJieriDengLuCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                string fromDate = fields[1].Replace('$', ':');
                string toDate = fields[2].Replace('$', ':');

                int dengLuTimes = Convert.ToInt32(fields[3]); //GameServer传递过来的登录次数，需要返还给客户端

                string strcmd = "";

                DBRoleInfo roleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == roleInfo)
                {
                    strcmd = string.Format("{0}:{1}:0", -1001, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //活动关键字，能够唯一标志某内活动的一个实例【在某段时间内的活动】
                string huoDongKeyStr = Global.GetHuoDongKeyString(fromDate, toDate);

                int hasgettimes = 0;
                string lastgettime = "";

                //判断是否领取过---活动关键字字符串唯一确定了每次活动，针对同类型不同时间的活动
                DBQuery.GetAwardHistoryForRole(dbMgr, roleID, roleInfo.ZoneID, (int)(ActivityTypes.JieriDengLuHaoLi), huoDongKeyStr, out hasgettimes, out lastgettime);

                strcmd = string.Format("{0}:{1}:{2}:{3}", 1, roleID, hasgettimes, dengLuTimes);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 节日VIP查询
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessQueryJieriVIPCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                string fromDate = fields[1].Replace('$', ':');
                string toDate = fields[2].Replace('$', ':');

                int isVip = Convert.ToInt32(fields[3]); //GameServer传递过来的登录次数，需要返还给客户端

                string strcmd = "";

                DBRoleInfo roleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == roleInfo)
                {
                    strcmd = string.Format("{0}:{1}:0", -1001, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //活动关键字，能够唯一标志某内活动的一个实例【在某段时间内的活动】
                string huoDongKeyStr = Global.GetHuoDongKeyString(fromDate, toDate);

                int hasgettimes = 0;
                string lastgettime = "";

                //判断是否领取过---活动关键字字符串唯一确定了每次活动，针对同类型不同时间的活动
                DBQuery.GetAwardHistoryForRole(dbMgr, roleID, roleInfo.ZoneID, (int)(ActivityTypes.JieriVIP), huoDongKeyStr, out hasgettimes, out lastgettime);

                strcmd = string.Format("{0}:{1}:{2}:{3}", 1, roleID, hasgettimes, isVip);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 节日充值加送查询
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessQueryJieriCZSongCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                string fromDate = fields[1].Replace('$', ':');
                string toDate = fields[2].Replace('$', ':');

                //返利百分比,已经由gameserver验证
                int minYuanBao = Convert.ToInt32(fields[3]);

                string strcmd = "";

                DBRoleInfo roleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == roleInfo)
                {
                    strcmd = string.Format("{0}:{1}:0", -1001, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //活动关键字，能够唯一标志某内活动的一个实例【在某段时间内的活动】
                string huoDongKeyStr = Global.GetHuoDongKeyString(fromDate, toDate);

                int hasgettimes = 0;
                string lastgettime = "";

                //判断是否领取过---活动关键字字符串唯一确定了每次活动，针对同类型不同时间的活动
                DBQuery.GetAwardHistoryForUser(dbMgr, roleInfo.UserID, (int)(ActivityTypes.JieriCZSong), huoDongKeyStr, out hasgettimes, out lastgettime);

                //查询时如果当前时间小于结束时间，就用采用结束时间也行
                //活动时间范围内的充值数，真实货币单位
                //int inputMoneyInPeriod = DBQuery.GetUserInputMoney(dbMgr, roleInfo.UserID, roleInfo.ZoneID, fromDate, toDate);
                RankDataKey key = new RankDataKey(RankType.Charge, fromDate, toDate, null);
                int inputMoneyInPeriod = roleInfo.RankValue.GetRankValue(key);
                if (inputMoneyInPeriod < 0)
                {
                    inputMoneyInPeriod = 0;
                }

                //时间范围内的元宝数
                int roleYuanBaoInPeriod = inputMoneyInPeriod; //Global.TransMoneyToYuanBao(inputMoneyInPeriod);

                strcmd = string.Format("{0}:{1}:{2}:{3}:{4}", 1, roleID, minYuanBao, roleYuanBaoInPeriod, hasgettimes);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 节日充值累计查询
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessQueryJieriCZLeiJiCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                string fromDate = fields[1].Replace('$', ':');
                string toDate = fields[2].Replace('$', ':');
                //排名最低元宝要求列表，依次为第一名的最小元宝，第二名的最小元宝......
                string[] minYuanBaoArr = fields[3].Split('_');

                string strcmd = "";
                DBRoleInfo roleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == roleInfo)
                {
                    strcmd = string.Format("{0}:{1}:0", -1001, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                /*List<int> minGateValueList = new List<int>();
                foreach (var item in minYuanBaoArr)
                {
                    minGateValueList.Add(Global.SafeConvertToInt32(item));
                }*/

                //活动关键字，能够唯一标志某内活动的一个实例【在某段时间内的活动】
                string huoDongKeyStr = Global.GetHuoDongKeyString(fromDate, toDate);

                int hasgettimes = 0;
                string lastgettime = "";

                //判断是否领取过---活动关键字字符串唯一确定了每次活动，针对同类型不同时间的活动
                DBQuery.GetAwardHistoryForUser(dbMgr, roleInfo.UserID, (int)(ActivityTypes.JieriLeiJiCZ), huoDongKeyStr, out hasgettimes, out lastgettime);

                //查询时如果当前时间小于结束时间，就用采用结束时间也行
                //活动时间范围内的充值数，真实货币单位
                //int inputMoneyInPeriod = DBQuery.GetUserInputMoney(dbMgr, roleInfo.UserID, roleInfo.ZoneID, fromDate, toDate);
                RankDataKey key = new RankDataKey(RankType.Charge, fromDate, toDate, null);
                int inputMoneyInPeriod = roleInfo.RankValue.GetRankValue(key);
                if (inputMoneyInPeriod < 0)
                {
                    inputMoneyInPeriod = 0;
                }

                //时间范围内的元宝数
                int roleYuanBaoInPeriod = inputMoneyInPeriod; //Global.TransMoneyToYuanBao(inputMoneyInPeriod);
                strcmd = string.Format("{0}:{1}:{2}:{3}", 1, roleID, roleYuanBaoInPeriod, hasgettimes);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 节日消费累计查询
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessQueryJieriTotalConsumeCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                string fromDate = fields[1].Replace('$', ':');
                string toDate = fields[2].Replace('$', ':');
                //排名最低元宝要求列表，依次为第一名的最小元宝，第二名的最小元宝......
                string[] minYuanBaoArr = fields[3].Split('_');

                string strcmd = "";
                DBRoleInfo roleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == roleInfo)
                {
                    strcmd = string.Format("{0}:{1}:0", -1001, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                /*List<int> minGateValueList = new List<int>();
                foreach (var item in minYuanBaoArr)
                {
                    minGateValueList.Add(Global.SafeConvertToInt32(item));
                }*/

                //活动关键字，能够唯一标志某内活动的一个实例【在某段时间内的活动】
                string huoDongKeyStr = Global.GetHuoDongKeyString(fromDate, toDate);

                int hasgettimes = 0;
                string lastgettime = "";

                //判断是否领取过---活动关键字字符串唯一确定了每次活动，针对同类型不同时间的活动
                DBQuery.GetAwardHistoryForUser(dbMgr, roleInfo.UserID, (int)(ActivityTypes.JieriTotalConsume), huoDongKeyStr, out hasgettimes, out lastgettime);

                //查询时如果当前时间小于结束时间，就用采用结束时间也行
                //活动时间范围内的消费数，真实货币单位
                //DBQuery.GetUserUsedMoney(dbMgr, roleID, fromDate, toDate);
                RankDataKey key = new RankDataKey(RankType.Consume, fromDate, toDate, null);
                int inputMoneyInPeriod = roleInfo.RankValue.GetRankValue(key);
                if (inputMoneyInPeriod < 0)
                {
                    inputMoneyInPeriod = 0;
                }

                //时间范围内的元宝数
                int roleYuanBaoInPeriod = inputMoneyInPeriod;
                strcmd = string.Format("{0}:{1}:{2}:{3}", 1, roleID, roleYuanBaoInPeriod, hasgettimes);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 充值点兑换
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessInputPointsExchangeCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                string fromDate = fields[1].Replace('$', ':');
                string toDate = fields[2].Replace('$', ':');
                int nActType = Convert.ToInt32(fields[3]);
                int extTag = Convert.ToInt32(fields[4]);
                extTag = Math.Max(0, extTag);

                string strcmd = "";
                DBRoleInfo roleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == roleInfo)
                {
                    strcmd = string.Format("{0}:{1}:0", -1001, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //避免同一角色同时多次操作
                DBUserInfo userInfo = dbMgr.GetDBUserInfo(roleInfo.UserID);
                if (null == userInfo)
                {
                    strcmd = string.Format("{0}:{1}:0", -1001, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //活动关键字，能够唯一标志某内活动的一个实例【在某段时间内的活动】
                string huoDongKeyStr = Global.GetHuoDongKeyString(fromDate, toDate);

                int hasgettimes = 0;
                string lastgettime = "";

                //判断是否领取过---活动关键字字符串唯一确定了每次活动，针对同类型不同时间的活动
                int histForRole = DBQuery.GetAwardHistoryForUser(dbMgr, roleInfo.UserID, (int)(ActivityTypes.JieriInputPointsExchg), huoDongKeyStr, out hasgettimes, out lastgettime);
                if (hasgettimes > 0)
                {
                }
                else
                {
                    //更新已领取状态
                    int ret = DBWriter.AddHongDongAwardRecordForUser(dbMgr, roleInfo.UserID, (int)(ActivityTypes.JieriInputPointsExchg), huoDongKeyStr, 1, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    if (ret < 0)
                    {
                        strcmd = string.Format("{0}:{1}", roleID, -1006);
                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }
                }

                strcmd = string.Format("{0}:{1}:{2}", hasgettimes, roleID, extTag);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 节日礼包领取
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessExecuteJieriDaLiBaoCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                string fromDate = fields[1].Replace('$', ':');
                string toDate = fields[2].Replace('$', ':');

                string strcmd = "";

                DBRoleInfo roleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == roleInfo)
                {
                    strcmd = string.Format("{0}:{1}:0", -1001, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //活动关键字，能够唯一标志某内活动的一个实例【在某段时间内的活动】
                string huoDongKeyStr = Global.GetHuoDongKeyString(fromDate, toDate);

                int hasgettimes = 0;
                string lastgettime = "";

                //避免同一角色同时多次操作
                lock (roleInfo)
                {
                    //判断是否领取过---活动关键字字符串唯一确定了每次活动，针对同类型不同时间的活动
                    DBQuery.GetAwardHistoryForRole(dbMgr, roleInfo.RoleID, roleInfo.ZoneID, (int)(ActivityTypes.JieriDaLiBao), huoDongKeyStr, out hasgettimes, out lastgettime);

                    //这个活动每次每个用户最多领取一次
                    if (hasgettimes > 0)
                    {
                        strcmd = string.Format("{0}:{1}:0", -10005, roleID);
                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }

                    hasgettimes = 1;

                    //更新已领取状态
                    int ret = DBWriter.AddHongDongAwardRecordForRole(dbMgr, roleInfo.RoleID, roleInfo.ZoneID, (int)(ActivityTypes.JieriDaLiBao), huoDongKeyStr, 1, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    if (ret < 0)
                    {
                        strcmd = string.Format("{0}:{1}:0", -1008, roleID);
                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }
                }

                strcmd = string.Format("{0}:{1}:{2}", 1, roleID, hasgettimes);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 节日登录领取
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessExecuteJieriDengLuCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                string fromDate = fields[1].Replace('$', ':');
                string toDate = fields[2].Replace('$', ':');

                int dengLuTimes = Convert.ToInt32(fields[3]); //GameServer传递过来的登录次数，需要返还给客户端
                int extTag = Convert.ToInt32(fields[4]);
                extTag = Math.Max(0, extTag);
                extTag = Math.Min(7, extTag);

                string strcmd = "";
                DBRoleInfo roleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == roleInfo)
                {
                    strcmd = string.Format("{0}:{1}:0", -1001, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //活动关键字，能够唯一标志某内活动的一个实例【在某段时间内的活动】
                string huoDongKeyStr = Global.GetHuoDongKeyString(fromDate, toDate);

                int hasgettimes = 0;
                string lastgettime = "";

                //判断是否领取过---活动关键字字符串唯一确定了每次活动，针对同类型不同时间的活动
                int histForRole = DBQuery.GetAwardHistoryForRole(dbMgr, roleID, roleInfo.ZoneID, (int)(ActivityTypes.JieriDengLuHaoLi), huoDongKeyStr, out hasgettimes, out lastgettime);

                int bitVal = Global.GetBitValue(extTag);
                if ((hasgettimes & bitVal) == bitVal)
                {
                    strcmd = string.Format("{0}:{1}:0", -10005, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                if (dengLuTimes < extTag)
                {
                    strcmd = string.Format("{0}:{1}:0", -10077, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //避免同一角色同时多次操作
                lock (roleInfo)
                {
                    hasgettimes |= (1 << (extTag - 1));

                    if (histForRole < 0)
                    {
                        //更新已领取状态
                        int ret = DBWriter.AddHongDongAwardRecordForRole(dbMgr, roleInfo.RoleID, roleInfo.ZoneID, (int)(ActivityTypes.JieriDengLuHaoLi), huoDongKeyStr, hasgettimes, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        if (ret < 0)
                        {
                            strcmd = string.Format("{0}:{1}:0", -1008, roleID);
                            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                            return TCPProcessCmdResults.RESULT_DATA;
                        }
                    }
                    else
                    {
                        int ret = DBWriter.UpdateHongDongAwardRecordForRole(dbMgr, roleInfo.RoleID, roleInfo.ZoneID, (int)(ActivityTypes.JieriDengLuHaoLi), huoDongKeyStr, hasgettimes, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        if (ret < 0)
                        {
                            strcmd = string.Format("{0}:{1}:0", -1008, roleID);
                            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                            return TCPProcessCmdResults.RESULT_DATA;
                        }
                    }
                }

                strcmd = string.Format("{0}:{1}:{2}", hasgettimes, roleID, extTag);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 节日VIP领取
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessExecuteJieriVIPCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                string fromDate = fields[1].Replace('$', ':');
                string toDate = fields[2].Replace('$', ':');

                int isVip = Convert.ToInt32(fields[3]);

                string strcmd = "";

                DBRoleInfo roleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == roleInfo)
                {
                    strcmd = string.Format("{0}:{1}:0", -1001, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                if (isVip <= 0)
                {
                    strcmd = string.Format("{0}:{1}:0", -10099, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //活动关键字，能够唯一标志某内活动的一个实例【在某段时间内的活动】
                string huoDongKeyStr = Global.GetHuoDongKeyString(fromDate, toDate);

                int hasgettimes = 0;
                string lastgettime = "";

                //避免同一角色同时多次操作
                lock (roleInfo)
                {
                    //判断是否领取过---活动关键字字符串唯一确定了每次活动，针对同类型不同时间的活动
                    DBQuery.GetAwardHistoryForRole(dbMgr, roleInfo.RoleID, roleInfo.ZoneID, (int)(ActivityTypes.JieriVIP), huoDongKeyStr, out hasgettimes, out lastgettime);

                    //这个活动每次每个用户最多领取一次
                    if (hasgettimes > 0)
                    {
                        strcmd = string.Format("{0}:{1}:0", -10005, roleID);
                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }

                    hasgettimes = 1;

                    //更新已领取状态
                    int ret = DBWriter.AddHongDongAwardRecordForRole(dbMgr, roleInfo.RoleID, roleInfo.ZoneID, (int)(ActivityTypes.JieriVIP), huoDongKeyStr, 1, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    if (ret < 0)
                    {
                        strcmd = string.Format("{0}:{1}:0", -1008, roleID);
                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }
                }

                strcmd = string.Format("{0}:{1}:{2}", 1, roleID, hasgettimes);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 节日充值加送领取
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessExecuteJieriCZSongCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                string fromDate = fields[1].Replace('$', ':');
                string toDate = fields[2].Replace('$', ':');

                int minYuanBao = Convert.ToInt32(fields[3]);
                int extTag = Convert.ToInt32(fields[4]);

                string strcmd = "";

                DBRoleInfo roleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == roleInfo)
                {
                    strcmd = string.Format("{0}:{1}:0", -1001, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //活动关键字，能够唯一标志某内活动的一个实例【在某段时间内的活动】
                string huoDongKeyStr = Global.GetHuoDongKeyString(fromDate, toDate);

                int hasgettimes = 0;
                string lastgettime = "";

                //判断是否领取过---活动关键字字符串唯一确定了每次活动，针对同类型不同时间的活动
                int histForRole = DBQuery.GetAwardHistoryForUser(dbMgr, roleInfo.UserID, (int)(ActivityTypes.JieriCZSong), huoDongKeyStr, out hasgettimes, out lastgettime);
                // 奖励分档了，所以存储也按位啦
                int bitVal = Global.GetBitValue(extTag);
                if ((hasgettimes & bitVal) == bitVal)
                {
                    strcmd = string.Format("{0}:{1}:0", -10005, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //查询时如果当前时间小于结束时间，就用采用结束时间也行
                //活动时间范围内的充值数，真实货币单位
                //int inputMoneyInPeriod = DBQuery.GetUserInputMoney(dbMgr, roleInfo.UserID, roleInfo.ZoneID, fromDate, toDate);
                RankDataKey key = new RankDataKey(RankType.Charge, fromDate, toDate, null);
                int inputMoneyInPeriod = roleInfo.RankValue.GetRankValue(key);
                if (inputMoneyInPeriod < 0)
                {
                    inputMoneyInPeriod = 0;
                }

                //时间范围内的元宝数
                int roleYuanBaoInPeriod = inputMoneyInPeriod; //Global.TransMoneyToYuanBao(inputMoneyInPeriod);

                if (roleYuanBaoInPeriod < minYuanBao)
                {
                    strcmd = string.Format("{0}:{1}:0", -10088, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //避免同一角色同时多次操作
                DBUserInfo userInfo = dbMgr.GetDBUserInfo(roleInfo.UserID);
                if (null == userInfo)
                {
                    strcmd = string.Format("{0}:{1}:0", -1001, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                lock (userInfo)
                {
                    //hasgettimes = 1;
                    hasgettimes |= (1 << (extTag - 1));

                    //更新已领取状态
                    /*int ret = DBWriter.AddHongDongAwardRecordForUser(dbMgr, roleInfo.UserID, (int)(ActivityTypes.JieriCZSong), huoDongKeyStr, hasgettimes, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    if (ret < 0)
                    {
                        strcmd = string.Format("{0}:{1}:0", -1008, roleID);
                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }*/
                    if (histForRole < 0)
                    {
                        //更新已领取
                        int ret = DBWriter.AddHongDongAwardRecordForUser(dbMgr, roleInfo.UserID, (int)(ActivityTypes.JieriCZSong), huoDongKeyStr, hasgettimes, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        if (ret < 0)
                        {
                            strcmd = string.Format("{0}:{1}:0", -1008, roleID);
                            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                            return TCPProcessCmdResults.RESULT_DATA;
                        }
                    }
                    else
                    {
                        int ret = DBWriter.UpdateHongDongAwardRecordForUser(dbMgr, roleInfo.UserID, (int)(ActivityTypes.JieriCZSong), huoDongKeyStr, hasgettimes, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        if (ret < 0)
                        {
                            strcmd = string.Format("{0}:{1}:0", -1008, roleID);
                            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                            return TCPProcessCmdResults.RESULT_DATA;
                        }
                    }
                }

                strcmd = string.Format("{0}:{1}:{2}", 1, roleID, extTag);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 节日充值累计领取
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessExecuteJieriCZLeiJiCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                string fromDate = fields[1].Replace('$', ':');
                string toDate = fields[2].Replace('$', ':');
                //排名最低元宝要求列表，依次为第一名的最小元宝，第二名的最小元宝......
                string[] minYuanBaoArr = fields[3].Split('_');
                int extTag = Convert.ToInt32(fields[4]);
                extTag = Math.Max(0, extTag);
                //extTag = Math.Min(7, extTag);

                string strcmd = "";
                DBRoleInfo roleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == roleInfo)
                {
                    strcmd = string.Format("{0}:{1}:0", -1001, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                List<int> minGateValueList = new List<int>();
                foreach (var item in minYuanBaoArr)
                {
                    minGateValueList.Add(Global.SafeConvertToInt32(item));
                }

                //活动关键字，能够唯一标志某内活动的一个实例【在某段时间内的活动】
                string huoDongKeyStr = Global.GetHuoDongKeyString(fromDate, toDate);

                int hasgettimes = 0;
                string lastgettime = "";

                //判断是否领取过---活动关键字字符串唯一确定了每次活动，针对同类型不同时间的活动
                int histForRole = DBQuery.GetAwardHistoryForUser(dbMgr, roleInfo.UserID, (int)(ActivityTypes.JieriLeiJiCZ), huoDongKeyStr, out hasgettimes, out lastgettime);

                int bitVal = Global.GetBitValue(extTag);
                if ((hasgettimes & bitVal) == bitVal)
                {
                    strcmd = string.Format("{0}:{1}:0", -10005, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //查询时如果当前时间小于结束时间，就用采用结束时间也行
                //活动时间范围内的充值数，真实货币单位
                //int inputMoneyInPeriod = DBQuery.GetUserInputMoney(dbMgr, roleInfo.UserID, roleInfo.ZoneID, fromDate, toDate);
                RankDataKey key = new RankDataKey(RankType.Charge, fromDate, toDate, null);
                int inputMoneyInPeriod = roleInfo.RankValue.GetRankValue(key);
                if (inputMoneyInPeriod < 0)
                {
                    inputMoneyInPeriod = 0;
                }

                //时间范围内的元宝数
                int roleYuanBaoInPeriod = inputMoneyInPeriod; //Global.TransMoneyToYuanBao(inputMoneyInPeriod);

                int findIndex = -1;
                for (int i = 0; i < minGateValueList.Count; i++)
                {
                    if (roleYuanBaoInPeriod < minGateValueList[i])
                    {
                        break;
                    }

                    findIndex = i;
                }

                if (findIndex < (extTag - 1))
                {
                    strcmd = string.Format("{0}:{1}:0", -10088, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                DBUserInfo userInfo = dbMgr.GetDBUserInfo(roleInfo.UserID);
                if (null == userInfo)
                {
                    strcmd = string.Format("{0}:{1}:0", -1001, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                lock (userInfo)
                {
                    hasgettimes |= (1 << (extTag - 1));

                    if (histForRole < 0)
                    {
                        //更新已领取
                        int ret = DBWriter.AddHongDongAwardRecordForUser(dbMgr, roleInfo.UserID, (int)(ActivityTypes.JieriLeiJiCZ), huoDongKeyStr, hasgettimes, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        if (ret < 0)
                        {
                            strcmd = string.Format("{0}:{1}:0", -1008, roleID);
                            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                            return TCPProcessCmdResults.RESULT_DATA;
                        }
                    }
                    else
                    {
                        int ret = DBWriter.UpdateHongDongAwardRecordForUser(dbMgr, roleInfo.UserID, (int)(ActivityTypes.JieriLeiJiCZ), huoDongKeyStr, hasgettimes, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        if (ret < 0)
                        {
                            strcmd = string.Format("{0}:{1}:0", -1008, roleID);
                            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                            return TCPProcessCmdResults.RESULT_DATA;
                        }
                    }
                }

                strcmd = string.Format("{0}:{1}:{2}", hasgettimes, roleID, extTag);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 节日消费累计领取
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessExecuteJieriTotalConsumeCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                string fromDate = fields[1].Replace('$', ':');
                string toDate = fields[2].Replace('$', ':');
                //排名最低元宝要求列表，依次为第一名的最小元宝，第二名的最小元宝......
                string[] minYuanBaoArr = fields[3].Split('_');
                int extTag = Convert.ToInt32(fields[4]);
                extTag = Math.Max(0, extTag);
                //extTag = Math.Min(7, extTag);

                string strcmd = "";
                DBRoleInfo roleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == roleInfo)
                {
                    strcmd = string.Format("{0}:{1}:0", -1001, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                List<int> minGateValueList = new List<int>();
                foreach (var item in minYuanBaoArr)
                {
                    minGateValueList.Add(Global.SafeConvertToInt32(item));
                }

                //活动关键字，能够唯一标志某内活动的一个实例【在某段时间内的活动】
                string huoDongKeyStr = Global.GetHuoDongKeyString(fromDate, toDate);

                int hasgettimes = 0;
                string lastgettime = "";

                //判断是否领取过---活动关键字字符串唯一确定了每次活动，针对同类型不同时间的活动
                int histForRole = DBQuery.GetAwardHistoryForUser(dbMgr, roleInfo.UserID, (int)(ActivityTypes.JieriTotalConsume), huoDongKeyStr, out hasgettimes, out lastgettime);

                int bitVal = Global.GetBitValue(extTag);
                if ((hasgettimes & bitVal) == bitVal)
                {
                    strcmd = string.Format("{0}:{1}:0", -10005, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //查询时如果当前时间小于结束时间，就用采用结束时间也行
                //活动时间范围内的充值数，真实货币单位
                //DBQuery.GetUserUsedMoney(dbMgr, roleID, fromDate, toDate);
                RankDataKey key = new RankDataKey(RankType.Consume, fromDate, toDate, null);
                int inputMoneyInPeriod = roleInfo.RankValue.GetRankValue(key);
                if (inputMoneyInPeriod < 0)
                {
                    inputMoneyInPeriod = 0;
                }

                //时间范围内的元宝数
                int roleYuanBaoInPeriod = inputMoneyInPeriod;

                int findIndex = -1;
                for (int i = 0; i < minGateValueList.Count; i++)
                {
                    if (roleYuanBaoInPeriod < minGateValueList[i])
                    {
                        break;
                    }

                    findIndex = i;
                }

                if (findIndex < (extTag - 1))
                {
                    strcmd = string.Format("{0}:{1}:0", -10088, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                DBUserInfo userInfo = dbMgr.GetDBUserInfo(roleInfo.UserID);
                if (null == userInfo)
                {
                    strcmd = string.Format("{0}:{1}:0", -1001, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                lock (userInfo)
                {
                    hasgettimes |= (1 << (extTag - 1));

                    if (histForRole < 0)
                    {
                        //更新已领取
                        int ret = DBWriter.AddHongDongAwardRecordForUser(dbMgr, roleInfo.UserID, (int)(ActivityTypes.JieriTotalConsume), huoDongKeyStr, hasgettimes, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        if (ret < 0)
                        {
                            strcmd = string.Format("{0}:{1}:0", -1008, roleID);
                            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                            return TCPProcessCmdResults.RESULT_DATA;
                        }
                    }
                    else
                    {
                        int ret = DBWriter.UpdateHongDongAwardRecordForUser(dbMgr, roleInfo.UserID, (int)(ActivityTypes.JieriTotalConsume), huoDongKeyStr, hasgettimes, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        if (ret < 0)
                        {
                            strcmd = string.Format("{0}:{1}:0", -1008, roleID);
                            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                            return TCPProcessCmdResults.RESULT_DATA;
                        }
                    }
                }

                strcmd = string.Format("{0}:{1}:{2}", hasgettimes, roleID, extTag);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 节日消费王领取
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessExecuteJieriXiaoFeiKingCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                string fromDate = fields[1].Replace('$', ':');
                string toDate = fields[2].Replace('$', ':');
                //排名最低元宝要求列表，依次为第一名的最小元宝，第二名的最小元宝......
                string[] minYuanBaoArr = fields[3].Split('_');

                List<int> minGateValueList = new List<int>();
                foreach (var item in minYuanBaoArr)
                {
                    minGateValueList.Add(Global.SafeConvertToInt32(item));
                }

                string strcmd = "";

                DBRoleInfo roleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == roleInfo)
                {
                    strcmd = string.Format("{0}:{1}:0", -1001, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //返回排行信息,返回消费王排行数据列表,可能没有第一名等名次
                List<InputKingPaiHangData> listPaiHang = Global.GetUsedMoneyKingPaiHangListByHuoDongLimit(dbMgr, fromDate, toDate, minGateValueList, minGateValueList.Count);
                //排行值
                int paiHang = -1;
                //活动时间范围的充值，真实货币单位
                int inputMoneyInPeriod = 0;

                for (int n = 0; n < listPaiHang.Count; n++)
                {
                    if (roleInfo.RoleID.ToString() == listPaiHang[n].UserID) //这里返回的是角色的ID
                    {
                        paiHang = listPaiHang[n].PaiHang;//得到排行值
                        inputMoneyInPeriod = listPaiHang[n].PaiHangValue;
                    }
                }

                //判断是否在排行内
                if (paiHang <= 0)
                {
                    strcmd = string.Format("{0}:{1}:0", -1003, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //在排行内但未充值，GetUserInputPaiHang()内已经做了过滤
                if (inputMoneyInPeriod <= 0)
                {
                    strcmd = string.Format("{0}:{1}:0", -10006, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //时间范围内获取的元宝数
                int roleGetYuanBaoInPeriod = inputMoneyInPeriod;

                //对排行进行修正之后，超出了奖励范围,即充值元宝数量没有达到最低的元宝数量要求
                if (paiHang <= 0)
                {
                    strcmd = string.Format("{0}:{1}:0", -10007, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //活动关键字，能够唯一标志某内活动的一个实例【在某段时间内的活动】
                string huoDongKeyStr = Global.GetHuoDongKeyString(fromDate, toDate);

                int hasgettimes = 0;
                string lastgettime = "";

                DBUserInfo userInfo = dbMgr.GetDBUserInfo(roleInfo.UserID);

                //避免同一用户账号同时多次操作
                lock (userInfo)
                {
                    //判断是否领取过---活动关键字字符串唯一确定了每次活动，针对同类型不同时间的活动
                    DBQuery.GetAwardHistoryForRole(dbMgr, roleInfo.RoleID, roleInfo.ZoneID, (int)(ActivityTypes.JieriPTXiaoFeiKing), huoDongKeyStr, out hasgettimes, out lastgettime);

                    //这个活动每次每个用户最多领取一次
                    if (hasgettimes > 0)
                    {
                        strcmd = string.Format("{0}:{1}:0", -10005, roleID);
                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }

                    //更新已领取状态
                    int ret = DBWriter.AddHongDongAwardRecordForRole(dbMgr, roleInfo.RoleID, roleInfo.ZoneID, (int)(ActivityTypes.JieriPTXiaoFeiKing), huoDongKeyStr, 1, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    if (ret < 0)
                    {
                        strcmd = string.Format("{0}:{1}:0", -1008, roleID);
                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }
                }

                strcmd = string.Format("{0}:{1}:{2}", 1, roleID, paiHang);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 节日充值王领取
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessExecuteJieriCZKingCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                string fromDate = fields[1].Replace('$', ':');
                string toDate = fields[2].Replace('$', ':');
                //排名最低元宝要求列表，依次为第一名的最小元宝，第二名的最小元宝......
                string[] minYuanBaoArr = fields[3].Split('_');

                List<int> minGateValueList = new List<int>();
                foreach (var item in minYuanBaoArr)
                {
                    minGateValueList.Add(Global.SafeConvertToInt32(item));
                }

                string strcmd = "";

                DBRoleInfo roleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == roleInfo)
                {
                    strcmd = string.Format("{0}:{1}:0", -1001, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //返回排行信息,通过活动限制值过滤后的排名,可能没有第一名等名次
                List<InputKingPaiHangData> listPaiHang = Global.GetInputKingPaiHangListByHuoDongLimit(dbMgr, fromDate, toDate, minGateValueList, minGateValueList.Count);
                //排行值
                int paiHang = -1;
                //活动时间范围的充值，真实货币单位
                int inputMoneyInPeriod = 0;

                for (int n = 0; n < listPaiHang.Count; n++)
                {
                    if (roleInfo.UserID == listPaiHang[n].UserID)
                    {
                        paiHang = listPaiHang[n].PaiHang;//得到排行值
                        inputMoneyInPeriod = listPaiHang[n].PaiHangValue;
                    }
                }

                //判断是否在排行内
                if (paiHang <= 0)
                {
                    strcmd = string.Format("{0}:{1}:0", -1003, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //在排行内但未充值，GetUserInputPaiHang()内已经做了过滤
                if (inputMoneyInPeriod <= 0)
                {
                    strcmd = string.Format("{0}:{1}:0", -10006, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //时间范围内获取的元宝数
                int roleGetYuanBaoInPeriod = inputMoneyInPeriod;// Global.TransMoneyToYuanBao(inputMoneyInPeriod);

                //对排行进行修正之后，超出了奖励范围,即充值元宝数量没有达到最低的元宝数量要求
                if (paiHang <= 0)
                {
                    strcmd = string.Format("{0}:{1}:0", -10007, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //活动关键字，能够唯一标志某内活动的一个实例【在某段时间内的活动】
                string huoDongKeyStr = Global.GetHuoDongKeyString(fromDate, toDate);

                int hasgettimes = 0;
                string lastgettime = "";

                DBUserInfo userInfo = dbMgr.GetDBUserInfo(roleInfo.UserID);

                //避免同一用户账号同时多次操作
                lock (userInfo)
                {
                    //判断是否领取过---活动关键字字符串唯一确定了每次活动，针对同类型不同时间的活动
                    DBQuery.GetAwardHistoryForUser(dbMgr, roleInfo.UserID, (int)(ActivityTypes.JieriPTCZKing), huoDongKeyStr, out hasgettimes, out lastgettime);

                    //这个活动每次每个用户最多领取一次
                    if (hasgettimes > 0)
                    {
                        strcmd = string.Format("{0}:{1}:0", -10005, roleID);
                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }

                    //更新已领取状态
                    int ret = DBWriter.AddHongDongAwardRecordForUser(dbMgr, roleInfo.UserID, (int)(ActivityTypes.JieriPTCZKing), huoDongKeyStr, 1, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    if (ret < 0)
                    {
                        strcmd = string.Format("{0}:{1}:0", -1008, roleID);
                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }
                }

                strcmd = string.Format("{0}:{1}:{2}", 1, roleID, paiHang);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 合服礼包查询
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessQueryHeFuDaLiBaoCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            /*try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                string fromDate = fields[1].Replace('$', ':');
                string toDate = fields[2].Replace('$', ':');

                string strcmd = "";

                DBRoleInfo roleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == roleInfo)
                {
                    strcmd = string.Format("{0}:{1}:0", -1001, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //活动关键字，能够唯一标志某内活动的一个实例【在某段时间内的活动】
                string huoDongKeyStr = Global.GetHuoDongKeyString(fromDate, toDate);

                int hasgettimes = 0;
                string lastgettime = "";

                //判断是否领取过---活动关键字字符串唯一确定了每次活动，针对同类型不同时间的活动
                DBQuery.GetAwardHistoryForRole(dbMgr, roleID, roleInfo.ZoneID, (int)(ActivityTypes.HeFuDaLiBao), huoDongKeyStr, out hasgettimes, out lastgettime);

                strcmd = string.Format("{0}:{1}:{2}", 1, roleID, hasgettimes);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);*/
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 合服VIP查询
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessQueryHeFuVIPCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            /*try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                string fromDate = fields[1].Replace('$', ':');
                string toDate = fields[2].Replace('$', ':');

                int isVip = Convert.ToInt32(fields[3]); //GameServer传递过来的登录次数，需要返还给客户端

                string strcmd = "";

                DBRoleInfo roleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == roleInfo)
                {
                    strcmd = string.Format("{0}:{1}:0", -1001, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //活动关键字，能够唯一标志某内活动的一个实例【在某段时间内的活动】
                string huoDongKeyStr = Global.GetHuoDongKeyString(fromDate, toDate);

                int hasgettimes = 0;
                string lastgettime = "";

                //判断是否领取过---活动关键字字符串唯一确定了每次活动，针对同类型不同时间的活动
                DBQuery.GetAwardHistoryForRole(dbMgr, roleID, roleInfo.ZoneID, (int)(ActivityTypes.HeFuVIP), huoDongKeyStr, out hasgettimes, out lastgettime);

                strcmd = string.Format("{0}:{1}:{2}:{3}", 1, roleID, hasgettimes, isVip);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);*/
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 合服充值加送查询
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessQueryHeFuCZSongCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            /*try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                string fromDate = fields[1].Replace('$', ':');
                string toDate = fields[2].Replace('$', ':');

                //返利百分比,已经由gameserver验证
                int minYuanBao = Convert.ToInt32(fields[3]);

                string strcmd = "";

                DBRoleInfo roleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == roleInfo)
                {
                    strcmd = string.Format("{0}:{1}:0", -1001, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //活动关键字，能够唯一标志某内活动的一个实例【在某段时间内的活动】
                string huoDongKeyStr = Global.GetHuoDongKeyString(fromDate, toDate);

                int hasgettimes = 0;
                string lastgettime = "";

                //判断是否领取过---活动关键字字符串唯一确定了每次活动，针对同类型不同时间的活动
                DBQuery.GetAwardHistoryForUser(dbMgr, roleInfo.UserID, (int)(ActivityTypes.HeFuCZSong), huoDongKeyStr, out hasgettimes, out lastgettime);

                //查询时如果当前时间小于结束时间，就用采用结束时间也行
                //活动时间范围内的充值数，真实货币单位
                int inputMoneyInPeriod = DBQuery.GetUserInputMoney(dbMgr, roleInfo.UserID, roleInfo.ZoneID, fromDate, toDate);

                if (inputMoneyInPeriod < 0)
                {
                    inputMoneyInPeriod = 0;
                }

                //时间范围内的元宝数
                int roleYuanBaoInPeriod = Global.TransMoneyToYuanBao(inputMoneyInPeriod);

                strcmd = string.Format("{0}:{1}:{2}:{3}:{4}", 1, roleID, minYuanBao, roleYuanBaoInPeriod, hasgettimes);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);*/
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 合服充值返利查询
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessQueryHeFuCZFanLiCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            /*try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                string fromDate = fields[1].Replace('$', ':');
                string toDate = fields[2].Replace('$', ':');
                //排名最低元宝要求列表，依次为第一名的最小元宝，第二名的最小元宝......
                string[] minYuanBaoArr = fields[3].Split('_');

                DBRoleInfo roleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == roleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                List<int> minGateValueList = new List<int>();
                List<int> fanLiRateValueList = new List<int>();
                foreach (var item in minYuanBaoArr)
                {
                    string[] itemFields = item.Split(',');

                    minGateValueList.Add(Global.SafeConvertToInt32(itemFields[0]));
                    fanLiRateValueList.Add(Global.SafeConvertToInt32(itemFields[1]));
                }

                DateTime now = DateTime.Now;
                string startTime = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0).ToString("yyyy-MM-dd HH:mm:ss");
                string endTime = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59).ToString("yyyy-MM-dd HH:mm:ss");

                //返回排行信息,通过活动限制值过滤后的排名,可能没有第一名等名次
                List<InputKingPaiHangData> listPaiHang = Global.GetInputKingPaiHangListByHuoDongLimit(dbMgr, startTime, endTime, minGateValueList, 5);

                //更新最大等级角色名称 和区号
                foreach (var item in listPaiHang)
                {
                    Global.GetUserMaxLevelRole(dbMgr, item.UserID, out item.MaxLevelRoleName, out item.MaxLevelRoleZoneID);
                }

                int hasgettimes = 0;
                string lastgettime = "";
                List<InputKingPaiHangData> listPaiHang2 = null;

                DateTime huodongStartTime = new DateTime(2000, 1, 1, 0, 0, 0);
                DateTime.TryParse(fromDate, out huodongStartTime);
                int roleYuanBaoInPeriod = 0;
                if (now.Ticks > (huodongStartTime.Ticks + (10000L * 1000L * 24L * 60L * 60L)))
                {
                    /// 获取一个增加了几天时间的DateTime
                    DateTime sub1DayDateTime = Global.GetAddDaysDataTime(now, -1, true);

                    startTime = new DateTime(sub1DayDateTime.Year, sub1DayDateTime.Month, sub1DayDateTime.Day, 0, 0, 0).ToString("yyyy-MM-dd HH:mm:ss");
                    endTime = new DateTime(sub1DayDateTime.Year, sub1DayDateTime.Month, sub1DayDateTime.Day, 23, 59, 59).ToString("yyyy-MM-dd HH:mm:ss");

                    //返回排行信息,通过活动限制值过滤后的排名,可能没有第一名等名次
                    listPaiHang2 = Global.GetInputKingPaiHangListByHuoDongLimit(dbMgr, startTime, endTime, minGateValueList, 5);

                    //查询时如果当前时间小于结束时间，就用采用结束时间也行
                    //活动时间范围内的充值数，真实货币单位
                    int inputMoneyInPeriod = DBQuery.GetUserInputMoney(dbMgr, roleInfo.UserID, roleInfo.ZoneID, startTime, endTime);

                    if (inputMoneyInPeriod < 0)
                    {
                        inputMoneyInPeriod = 0;
                    }

                    //时间范围内的元宝数
                    roleYuanBaoInPeriod = Global.TransMoneyToYuanBao(inputMoneyInPeriod);

                    int selfPaiHang = 0;
                    for (int i = 0; i < listPaiHang2.Count; i++)
                    {
                        if (listPaiHang2[i].UserID == roleInfo.UserID)
                        {
                            selfPaiHang = listPaiHang2[i].PaiHang;
                            break;
                        }
                    }

                    if (selfPaiHang > 0)
                    {
                        double fanLiPercent = fanLiRateValueList[selfPaiHang - 1] / 100.0;
                        roleYuanBaoInPeriod = (int)(fanLiPercent * roleYuanBaoInPeriod);
                    }
                    else
                    {
                        roleYuanBaoInPeriod = 0;
                    }

                    //活动关键字，能够唯一标志某内活动的一个实例【在某段时间内的活动】
                    string huoDongKeyStr = Global.GetHuoDongKeyString(startTime, endTime);

                    //判断是否领取过---活动关键字字符串唯一确定了每次活动，针对同类型不同时间的活动
                    DBQuery.GetAwardHistoryForUser(dbMgr, roleInfo.UserID, (int)(ActivityTypes.HeFuCZFanLi), huoDongKeyStr, out hasgettimes, out lastgettime);

                    //更新最大等级角色名称 和区号
                    foreach (var item in listPaiHang2)
                    {
                        Global.GetUserMaxLevelRole(dbMgr, item.UserID, out item.MaxLevelRoleName, out item.MaxLevelRoleZoneID);
                    }
                }

                JieriCZKingData jieriCZKingData = new JieriCZKingData()
                {
                    YuanBao = roleYuanBaoInPeriod,
                    ListPaiHang = listPaiHang,
                    State = hasgettimes,
                    ListPaiHangYestoday = listPaiHang2,
                };

                //生成排行信息的tcp对象流
                tcpOutPacket = DataHelper.ObjectToTCPOutPacket<JieriCZKingData>(jieriCZKingData, pool, nID);

                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);*/
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 合服VIP领取
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessExecuteHeFuVIPCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            /*try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                string fromDate = fields[1].Replace('$', ':');
                string toDate = fields[2].Replace('$', ':');

                int isVip = Convert.ToInt32(fields[3]);

                string strcmd = "";

                DBRoleInfo roleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == roleInfo)
                {
                    strcmd = string.Format("{0}:{1}:0", -1001, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                if (isVip <= 0)
                {
                    strcmd = string.Format("{0}:{1}:0", -10099, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //活动关键字，能够唯一标志某内活动的一个实例【在某段时间内的活动】
                string huoDongKeyStr = Global.GetHuoDongKeyString(fromDate, toDate);

                int hasgettimes = 0;
                string lastgettime = "";

                //避免同一角色同时多次操作
                lock (roleInfo)
                {
                    //判断是否领取过---活动关键字字符串唯一确定了每次活动，针对同类型不同时间的活动
                    DBQuery.GetAwardHistoryForRole(dbMgr, roleInfo.RoleID, roleInfo.ZoneID, (int)(ActivityTypes.HeFuVIP), huoDongKeyStr, out hasgettimes, out lastgettime);

                    //这个活动每次每个用户最多领取一次
                    if (hasgettimes > 0)
                    {
                        strcmd = string.Format("{0}:{1}:0", -10005, roleID);
                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }

                    hasgettimes = 1;

                    //更新已领取状态
                    int ret = DBWriter.AddHongDongAwardRecordForRole(dbMgr, roleInfo.RoleID, roleInfo.ZoneID, (int)(ActivityTypes.HeFuVIP), huoDongKeyStr, 1, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    if (ret < 0)
                    {
                        strcmd = string.Format("{0}:{1}:0", -1008, roleID);
                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }
                }

                strcmd = string.Format("{0}:{1}:{2}", 1, roleID, hasgettimes);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);*/
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 合服充值加送领取
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessExecuteHeFuCZSongCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            /*try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                string fromDate = fields[1].Replace('$', ':');
                string toDate = fields[2].Replace('$', ':');

                int minYuanBao = Convert.ToInt32(fields[3]);

                string strcmd = "";

                DBRoleInfo roleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == roleInfo)
                {
                    strcmd = string.Format("{0}:{1}:0", -1001, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //查询时如果当前时间小于结束时间，就用采用结束时间也行
                //活动时间范围内的充值数，真实货币单位
                int inputMoneyInPeriod = DBQuery.GetUserInputMoney(dbMgr, roleInfo.UserID, roleInfo.ZoneID, fromDate, toDate);

                if (inputMoneyInPeriod < 0)
                {
                    inputMoneyInPeriod = 0;
                }

                //时间范围内的元宝数
                int roleYuanBaoInPeriod = Global.TransMoneyToYuanBao(inputMoneyInPeriod);

                if (roleYuanBaoInPeriod < minYuanBao)
                {
                    strcmd = string.Format("{0}:{1}:0", -10088, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //活动关键字，能够唯一标志某内活动的一个实例【在某段时间内的活动】
                string huoDongKeyStr = Global.GetHuoDongKeyString(fromDate, toDate);

                int hasgettimes = 0;
                string lastgettime = "";

                //避免同一角色同时多次操作
                DBUserInfo userInfo = dbMgr.GetDBUserInfo(roleInfo.UserID);
                if (null == userInfo)
                {
                    strcmd = string.Format("{0}:{1}:0", -1001, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                lock (userInfo)
                {
                    //判断是否领取过---活动关键字字符串唯一确定了每次活动，针对同类型不同时间的活动
                    DBQuery.GetAwardHistoryForUser(dbMgr, roleInfo.UserID, (int)(ActivityTypes.HeFuCZSong), huoDongKeyStr, out hasgettimes, out lastgettime);

                    //这个活动每次每个用户最多领取一次
                    if (hasgettimes > 0)
                    {
                        strcmd = string.Format("{0}:{1}:0", -10005, roleID);
                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }

                    hasgettimes = 1;

                    //更新已领取状态
                    int ret = DBWriter.AddHongDongAwardRecordForUser(dbMgr, roleInfo.UserID, (int)(ActivityTypes.HeFuCZSong), huoDongKeyStr, 1, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    if (ret < 0)
                    {
                        strcmd = string.Format("{0}:{1}:0", -1008, roleID);
                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }
                }

                strcmd = string.Format("{0}:{1}:{2}", 1, roleID, hasgettimes);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);*/
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 合服PK王领取
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessExecuteHeFuPKKingCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                string fromDate = fields[1].Replace('$', ':');
                string toDate = fields[2].Replace('$', ':');

                int kingID = Convert.ToInt32(fields[3]);

                string strcmd = "";

                DBRoleInfo roleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == roleInfo)
                {
                    strcmd = string.Format("{0}:{1}:0", -1001, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                if (kingID != roleInfo.RoleID)
                {
                    strcmd = string.Format("{0}:{1}:0", -10089, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //活动关键字，能够唯一标志某内活动的一个实例【在某段时间内的活动】
                string huoDongKeyStr = Global.GetHuoDongKeyString(fromDate, toDate);

                int hasgettimes = 0;
                string lastgettime = "";

                //避免同一角色同时多次操作
                lock (roleInfo)
                {
                    //判断是否领取过---活动关键字字符串唯一确定了每次活动，针对同类型不同时间的活动
                    DBQuery.GetAwardHistoryForRole(dbMgr, roleInfo.RoleID, roleInfo.ZoneID, (int)(ActivityTypes.HeFuPKKing), huoDongKeyStr, out hasgettimes, out lastgettime);

                    //这个活动每次每个用户最多领取一次
                    if (hasgettimes > 0)
                    {
                        strcmd = string.Format("{0}:{1}:0", -10005, roleID);
                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }

                    hasgettimes = 1;

                    //更新已领取状态
                    int ret = DBWriter.AddHongDongAwardRecordForRole(dbMgr, roleInfo.RoleID, roleInfo.ZoneID, (int)(ActivityTypes.HeFuPKKing), huoDongKeyStr, 1, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    if (ret < 0)
                    {
                        strcmd = string.Format("{0}:{1}:0", -1008, roleID);
                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }
                }

                strcmd = string.Format("{0}:{1}:{2}", 1, roleID, hasgettimes);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 合服充值返利领取
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessExecuteHeFuCZFanLiCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            /*try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                string fromDate = fields[1].Replace('$', ':');
                string toDate = fields[2].Replace('$', ':');
                //排名最低元宝要求列表，依次为第一名的最小元宝，第二名的最小元宝......
                string[] minYuanBaoArr = fields[3].Split('_');

                List<int> minGateValueList = new List<int>();
                List<int> fanLiRateValueList = new List<int>();
                foreach (var item in minYuanBaoArr)
                {
                    string[] itemFields = item.Split(',');

                    minGateValueList.Add(Global.SafeConvertToInt32(itemFields[0]));
                    fanLiRateValueList.Add(Global.SafeConvertToInt32(itemFields[1]));
                }

                string strcmd = "";

                DBRoleInfo roleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == roleInfo)
                {
                    strcmd = string.Format("{0}:{1}:0", -1001, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int hasgettimes = 0;
                string lastgettime = "";

                DateTime now = DateTime.Now;
                DateTime huodongStartTime = new DateTime(2000, 1, 1, 0, 0, 0);
                DateTime.TryParse(fromDate, out huodongStartTime);
                int roleYuanBaoInPeriod = 0;
                if (now.Ticks <= (huodongStartTime.Ticks + (10000L * 1000L * 24L * 60L * 60L)))
                {
                    strcmd = string.Format("{0}:{1}:0", -1002, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                /// 获取一个增加了几天时间的DateTime
                DateTime sub1DayDateTime = Global.GetAddDaysDataTime(now, -1, true);

                string startTime = new DateTime(sub1DayDateTime.Year, sub1DayDateTime.Month, sub1DayDateTime.Day, 0, 0, 0).ToString("yyyy-MM-dd HH:mm:ss");
                string endTime = new DateTime(sub1DayDateTime.Year, sub1DayDateTime.Month, sub1DayDateTime.Day, 23, 59, 59).ToString("yyyy-MM-dd HH:mm:ss");

                //返回排行信息,通过活动限制值过滤后的排名,可能没有第一名等名次
                List<InputKingPaiHangData> listPaiHang = Global.GetInputKingPaiHangListByHuoDongLimit(dbMgr, startTime, endTime, minGateValueList, 5);

                //查询时如果当前时间小于结束时间，就用采用结束时间也行
                //活动时间范围内的充值数，真实货币单位
                int inputMoneyInPeriod = DBQuery.GetUserInputMoney(dbMgr, roleInfo.UserID, roleInfo.ZoneID, startTime, endTime);

                if (inputMoneyInPeriod < 0)
                {
                    inputMoneyInPeriod = 0;
                }

                //时间范围内的元宝数
                roleYuanBaoInPeriod = Global.TransMoneyToYuanBao(inputMoneyInPeriod);

                int selfPaiHang = 0;
                for (int i = 0; i < listPaiHang.Count; i++)
                {
                    if (listPaiHang[i].UserID == roleInfo.UserID)
                    {
                        selfPaiHang = listPaiHang[i].PaiHang;
                        break;
                    }
                }

                //判断是否在排行内
                if (selfPaiHang <= 0)
                {
                    strcmd = string.Format("{0}:{1}:0", -1003, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                double fanLiPercent = fanLiRateValueList[selfPaiHang - 1] / 100.0;
                roleYuanBaoInPeriod = (int)(fanLiPercent * roleYuanBaoInPeriod);

                //活动关键字，能够唯一标志某内活动的一个实例【在某段时间内的活动】
                string huoDongKeyStr = Global.GetHuoDongKeyString(startTime, endTime);

                //在排行内但未充值，GetUserInputPaiHang()内已经做了过滤
                if (roleYuanBaoInPeriod <= 0)
                {
                    strcmd = string.Format("{0}:{1}:0", -10006, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                DBUserInfo userInfo = dbMgr.GetDBUserInfo(roleInfo.UserID);

                //避免同一用户账号同时多次操作
                lock (userInfo)
                {
                    //判断是否领取过---活动关键字字符串唯一确定了每次活动，针对同类型不同时间的活动
                    DBQuery.GetAwardHistoryForUser(dbMgr, roleInfo.UserID, (int)(ActivityTypes.HeFuCZFanLi), huoDongKeyStr, out hasgettimes, out lastgettime);

                    //这个活动每次每个用户最多领取一次
                    if (hasgettimes > 0)
                    {
                        strcmd = string.Format("{0}:{1}:0", -10005, roleID);
                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }

                    //更新已领取状态
                    int ret = DBWriter.AddHongDongAwardRecordForUser(dbMgr, roleInfo.UserID, (int)(ActivityTypes.HeFuCZFanLi), huoDongKeyStr, 1, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    if (ret < 0)
                    {
                        strcmd = string.Format("{0}:{1}:0", -1008, roleID);
                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }
                }

                strcmd = string.Format("{0}:{1}:{2}", 1, roleID, roleYuanBaoInPeriod);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);*/
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 新区充值返利领取
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessExecuteXinCZFanLiCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                string fromDate = fields[1].Replace('$', ':');
                string toDate = fields[2].Replace('$', ':');
                //排名最低元宝要求列表，依次为第一名的最小元宝，第二名的最小元宝......
                string[] minYuanBaoArr = fields[3].Split('_');

                List<int> minGateValueList = new List<int>();
                foreach (var item in minYuanBaoArr)
                {
                    minGateValueList.Add(Global.SafeConvertToInt32(item));
                }

                string strcmd = "";

                DBRoleInfo roleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == roleInfo)
                {
                    strcmd = string.Format("{0}:{1}:0", -1001, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int hasgettimes = 0;
                string lastgettime = "";

                DateTime now = DateTime.Now;
                DateTime huodongStartTime = new DateTime(2000, 1, 1, 0, 0, 0);
                DateTime.TryParse(fromDate, out huodongStartTime);
                int roleYuanBaoInPeriod = 0;
                if (now.Ticks <= (huodongStartTime.Ticks + (10000L * 1000L * 24L * 60L * 60L)))
                {
                    strcmd = string.Format("{0}:{1}:0", -1002, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                /// 获取一个增加了几天时间的DateTime
                DateTime sub1DayDateTime = Global.GetAddDaysDataTime(now, -1, true);

                string startTime = new DateTime(sub1DayDateTime.Year, sub1DayDateTime.Month, sub1DayDateTime.Day, 0, 0, 0).ToString("yyyy-MM-dd HH:mm:ss");
                string endTime = new DateTime(sub1DayDateTime.Year, sub1DayDateTime.Month, sub1DayDateTime.Day, 23, 59, 59).ToString("yyyy-MM-dd HH:mm:ss");

                //返回排行信息,通过活动限制值过滤后的排名,可能没有第一名等名次
                List<InputKingPaiHangData> listPaiHang = Global.GetInputKingPaiHangListByHuoDongLimit(dbMgr, startTime, endTime, minGateValueList, 5);

                //查询时如果当前时间小于结束时间，就用采用结束时间也行
                //活动时间范围内的充值数，真实货币单位
                //int inputMoneyInPeriod = DBQuery.GetUserInputMoney(dbMgr, roleInfo.UserID, roleInfo.ZoneID, startTime, endTime);
                RankDataKey key = new RankDataKey(RankType.Charge, startTime, endTime, null);
                int inputMoneyInPeriod = roleInfo.RankValue.GetRankValue(key);
                if (inputMoneyInPeriod < 0)
                {
                    inputMoneyInPeriod = 0;
                }

                //时间范围内的元宝数
                roleYuanBaoInPeriod = inputMoneyInPeriod;//Global.TransMoneyToYuanBao(inputMoneyInPeriod);

                int selfPaiHang = 0;
                for (int i = 0; i < listPaiHang.Count; i++)
                {
                    if (listPaiHang[i].UserID == roleInfo.UserID)
                    {
                        selfPaiHang = listPaiHang[i].PaiHang;
                        break;
                    }
                }

                //判断是否在排行内
                if (selfPaiHang <= 0)
                {
                    strcmd = string.Format("{0}:{1}:0", -1003, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                double fanLiPercent = minGateValueList[selfPaiHang - 1] / 100.0;
                roleYuanBaoInPeriod = (int)(fanLiPercent * roleYuanBaoInPeriod);

                //活动关键字，能够唯一标志某内活动的一个实例【在某段时间内的活动】
                string huoDongKeyStr = Global.GetHuoDongKeyString(startTime, endTime);

                //在排行内但未充值，GetUserInputPaiHang()内已经做了过滤
                if (roleYuanBaoInPeriod <= 0)
                {
                    strcmd = string.Format("{0}:{1}:0", -10006, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                DBUserInfo userInfo = dbMgr.GetDBUserInfo(roleInfo.UserID);

                //避免同一用户账号同时多次操作
                lock (userInfo)
                {
                    //判断是否领取过---活动关键字字符串唯一确定了每次活动，针对同类型不同时间的活动
                    DBQuery.GetAwardHistoryForUser(dbMgr, roleInfo.UserID, (int)(ActivityTypes.XinCZFanLi), huoDongKeyStr, out hasgettimes, out lastgettime);

                    //这个活动每次每个用户最多领取一次
                    if (hasgettimes > 0)
                    {
                        strcmd = string.Format("{0}:{1}:0", -10005, roleID);
                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }

                    //更新已领取状态
                    int ret = DBWriter.AddHongDongAwardRecordForUser(dbMgr, roleInfo.UserID, (int)(ActivityTypes.XinCZFanLi), huoDongKeyStr, 1, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    if (ret < 0)
                    {
                        strcmd = string.Format("{0}:{1}:0", -1008, roleID);
                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }
                }

                strcmd = string.Format("{0}:{1}:{2}", 1, roleID, roleYuanBaoInPeriod);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 客户端请求活动的相关信息
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessQueryActivityInfoCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                string strcmd = "";

                int roleID = Convert.ToInt32(fields[0]);
                string sFromTime = fields[1].Replace("$", ":");

                DBRoleInfo roleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == roleInfo)
                {
                    strcmd = string.Format("{0}:{1}:0", -1001, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                if (null == sFromTime)
                {
                    strcmd = string.Format("{0}:{1}:0", -1001, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }
                // 3.幸运大抽奖次数
                // 我把用户抽奖的次数放在t_huodongawarduserhist表里 并且根据t_inputlog取得该用户能抽奖的总次数 相减得到剩余次数

                // 修改代码 把取开服时间 放在GameServer里
                //string sFrom = GameDBManager.GameConfigMgr.GetGameConifgItem("kaifutime");
                DateTime tStartTime = new DateTime(2000, 1, 1, 0, 0, 0);
                DateTime.TryParse(sFromTime, out tStartTime);
                DateTime dEndTime = Global.GetAddDaysDataTime(tStartTime, 3, true);
                string sEnd = dEndTime.ToString("yyyy-MM-dd HH:mm:ss");

                string sKeyStr = Global.GetHuoDongKeyString(sFromTime, sEnd);
                int nInputMoney = DBQuery.GetUserInputMoney(dbMgr, roleInfo.UserID, roleInfo.ZoneID, sFromTime, sEnd); //取得开服开始3天内 玩家冲值金额
                int nIputYuanBao = Global.TransMoneyToYuanBao(nInputMoney); // 取元宝

                int nhasPlaytimes = 0;
                string slastgettimes = "";
                DBQuery.GetAwardHistoryForUser(dbMgr, roleInfo.UserID, (int)(ActivityTypes.XingYunChouJiang), sKeyStr, out nhasPlaytimes, out slastgettimes);

                DateTime now = DateTime.Now;
                tStartTime = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
                string sFrom = tStartTime.ToString("yyyy-MM-dd HH:mm:ss");
                dEndTime = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);
                sEnd = dEndTime.ToString("yyyy-MM-dd HH:mm:ss");
                nInputMoney = DBQuery.GetUserInputMoney(dbMgr, roleInfo.UserID, roleInfo.ZoneID, sFrom, sEnd);
                int nIputYuanBao2 = Global.TransMoneyToYuanBao(nInputMoney); // 取元宝

                strcmd = string.Format("{0}:{1}:{2}", nIputYuanBao, nhasPlaytimes, nIputYuanBao2);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 请求幸运抽奖的相关信息
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessQueryXingYunChouJiangInfoCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                string strcmd = "";

                int roleID = Convert.ToInt32(fields[0]);
                int nType = Convert.ToInt32(fields[1]);
                int nXingYunChouJiangYB = Convert.ToInt32(fields[2]);
                string sFromTime = fields[3].Replace("$", ":");
                string sToTime = fields[4].Replace("$", ":");

                DBRoleInfo roleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == roleInfo)
                {
                    strcmd = string.Format("{0}:{1}::", -1001, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int nInputMoney = DBQuery.GetUserInputMoney(dbMgr, roleInfo.UserID, roleInfo.ZoneID, sFromTime, sToTime);
                int nIputYuanBao = Global.TransMoneyToYuanBao(nInputMoney); // 取元宝
                int nCanPlayCount = nIputYuanBao / nXingYunChouJiangYB;

                int nTimes = 0;
                string sLastgettime = "";
                string sKeyStr = Global.GetHuoDongKeyString(sFromTime, sToTime);
                int nActive = 0;
                if (nType == 1)
                    nActive = (int)(ActivityTypes.XingYunChouJiang);
                else if (nType == 2)
                    nActive = (int)(ActivityTypes.YuDuZhuanPanChouJiang);

                DBQuery.GetAwardHistoryForUser(dbMgr, roleInfo.UserID, nActive, sKeyStr, out nTimes, out sLastgettime);

                // 返回 1.roleid  2.充值的金额能玩几次  3.已经玩了几次 (根据2 和 3 就能知道剩余几次 发给客户端 显示在界面上) 4.充值元宝的金额--要显示在界面上
                strcmd = string.Format("{0}:{1}:{2}:{3}", roleID, nCanPlayCount, nTimes, nIputYuanBao);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 执行幸运抽奖
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessExcuteXingYunChouJiangInfoCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                string strcmd = "";
                int roleID = Convert.ToInt32(fields[0]);
                int nTpye = Convert.ToInt32(fields[1]);
                int nHasPlayTime = Convert.ToInt32(fields[2]);
                string sFromTime = fields[3].Replace("$", ":");
                string sToTime = fields[4].Replace("$", ":");
                int nActTpye = 0;
                //string sFrom      = "";
                //string sEnd       = "";

                if (nTpye == 1) //幸运抽奖
                    nActTpye = (int)(ActivityTypes.XingYunChouJiang);
                else if (nTpye == 2) //月度转盘
                    nActTpye = (int)(ActivityTypes.YuDuZhuanPanChouJiang);

                string sKeyStr = Global.GetHuoDongKeyString(sFromTime, sToTime);

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    strcmd = string.Format("{0}:{1}", -1001, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                DBUserInfo userInfo = dbMgr.GetDBUserInfo(dbRoleInfo.UserID);
                if (null == userInfo)
                {
                    strcmd = string.Format("{0}:{1}", -1001, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                // 如果这是第一次玩则INSERT INTO  不然则UPDATE 注意多线程问题
                lock (userInfo)
                {
                    if (nHasPlayTime == 0)
                    {
                        int ret = DBWriter.AddHongDongAwardRecordForUser(dbMgr, userInfo.UserID, nActTpye, sKeyStr, nHasPlayTime + 1, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        if (ret < 0)
                        {
                            strcmd = string.Format("{0}:{1}", -1006, roleID);
                            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                            return TCPProcessCmdResults.RESULT_DATA;
                        }
                    }
                    else
                    {
                        int ret = DBWriter.UpdateHongDongAwardRecordForUser(dbMgr, userInfo.UserID, nActTpye, sKeyStr, nHasPlayTime + 1, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        if (ret < 0)
                        {
                            strcmd = string.Format("{0}:{1}", -1008, roleID);
                            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                            return TCPProcessCmdResults.RESULT_DATA;
                        }
                    }
                }

                strcmd = string.Format("{0}:{1}", 1, roleID);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 添加月度转盘记录
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessExcuteAddYueDuChouJiangInfoCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 1)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                string[] lines = fields[0].Split(';');

                for (int n = 0; n < lines.Length; n++)
                {
                    string[] lineFields = lines[n].Split('_');

                    if (lineFields.Length < 8)
                    {
                        continue;
                    }

                    int rid = Convert.ToInt32(lineFields[0]);
                    String rname = lineFields[1];
                    int zoneid = Convert.ToInt32(lineFields[2]);
                    int gaingoodsid = Convert.ToInt32(lineFields[3]);
                    int gaingoodsnum = Convert.ToInt32(lineFields[4]);
                    int gaingold = Convert.ToInt32(lineFields[5]);
                    int gainyinliang = Convert.ToInt32(lineFields[6]);
                    int gainexp = Convert.ToInt32(lineFields[7]);

                    //添加新的月度抽奖记录
                    int ret = DBWriter.AddNewYueDuChouJiangHistory(dbMgr, rid, rname, zoneid, gaingoodsid, gaingoodsnum, gaingold, gainyinliang, gainexp);
                    if (ret < 0)
                    {
                        LogManager.WriteLog(LogTypes.Error, string.Format("添加新的月度抽奖记录失败，CMD={0}, RoleID={1}",
                            (TCPGameServerCmds)nID, rid));
                    }
                }

                string strcmd = string.Format("{0}:{1}", 1, 0);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// Đổi phái
        /// </summary>
        private static TCPProcessCmdResults ProcessExecuteChangeOccupationCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 3)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int factionID = Convert.ToInt32(fields[1]);
                int routeID = Convert.ToInt32(fields[2]);

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                bool ret = DBWriter.UpdateRoleFactionAndRoute(dbMgr, roleID, factionID, routeID);
                if (!ret)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Update role faction and route faild. CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));
                }
                else
                {
                    string userID = "";
                    lock (dbRoleInfo)
                    {
                        dbRoleInfo.Occupation = factionID;
                        dbRoleInfo.SubID = routeID;
                        userID = dbRoleInfo.UserID;
                    }


                    //将用户的请求更新内存缓存
                    if (userID != "")
                    {
                        DBUserInfo dbUserInfo = dbMgr.GetDBUserInfo(userID);
                        if (null != dbUserInfo)
                        {
                            lock (dbUserInfo)
                            {
                                for (int i = 0; i < dbUserInfo.ListRoleLevels.Count; i++)
                                {
                                    if (dbUserInfo.ListRoleIDs[i] == roleID)
                                    {
                                        dbUserInfo.ListRoleOccups[i] = factionID;
                                    }
                                }
                            }
                        }
                    }
                }

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "1", nID);

                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 获取角色的使用物品信息
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessGetUsingGoodsDataListCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                //解析用户名称和用户密码
                string[] fields = cmdData.Split(':');
                if (fields.Length != 1)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);

                RoleData4Selector roleData4Selector = new RoleData4Selector();
                roleData4Selector.RoleID = -1;

                // 将用户的请求更新内存缓存
                // ChenXiaojun 没有时从数据库中获取，不管角色是否删除 【21/5/2014】
                DBRoleInfo dbRoleInfo = dbMgr.GetDBAllRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    /// 将对象转为TCP协议流
                    tcpOutPacket = DataHelper.ObjectToTCPOutPacket<RoleData4Selector>(roleData4Selector, pool, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                lock (dbRoleInfo)
                {
                    //数据库角色信息到选择用户数据的转换
                    Global.DBRoleInfo2RoleData4Selector(dbMgr, dbRoleInfo, roleData4Selector);
                }

                /// 将对象转为TCP协议流
                tcpOutPacket = DataHelper.ObjectToTCPOutPacket<RoleData4Selector>(roleData4Selector, pool, nID);

                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 血色堡垒 请求当天进入的次数
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessQueryBloodCastleEnterCountCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                //解析用户名称和用户密码
                string[] fields = cmdData.Split(':');
                if (fields.Length != 3)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int nDate = Convert.ToInt32(fields[1]);
                int nType = Convert.ToInt32(fields[2]);

                string strcmd = "";

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    strcmd = string.Format("{0}:{1}", -1, 0);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                // 查询DB
                int nCount = DBQuery.GetBloodCastleEnterCount(dbMgr, roleID, nDate, nType);

                strcmd = string.Format("{0}:{1}", 1, nCount);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);

                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 血色堡垒 更新当天进入的次数
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessUpdateBloodCastleEnterCountCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                //解析用户名称和用户密码
                string[] fields = cmdData.Split(':');
                if (fields.Length != 4)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int nDate = Convert.ToInt32(fields[1]);
                int nType = Convert.ToInt32(fields[2]);
                int nCount = Convert.ToInt32(fields[3]);

                string strcmd = "";

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    strcmd = string.Format("{0}:{1}", -1, 0);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                // 更新DB
                bool bRet = DBWriter.UpdateBloodCastleEnterCount(dbMgr, roleID, nDate, nType, nCount, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                strcmd = string.Format("{0}:{1}", 1, bRet);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);

                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 完成新手场景
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessCompleteFlashSceneCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 1)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);

                string strcmd = "";

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    strcmd = string.Format("{0}:{1}", "", -1);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                // 更新DB
                bool bRet = DBWriter.UpdateRoleInfoForFlashPlayerFlag(dbMgr, roleID, 0);

                strcmd = string.Format("{0}:{1}", roleID, 1);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);

                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 结束新手阶段
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessFinishFreshPlayerStatusCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 1)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);

                string strcmd = "";

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    strcmd = string.Format("{0}:{1}", "", -1);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                DBUserInfo userInfo = dbMgr.GetDBUserInfo(dbRoleInfo.UserID);
                if (null == userInfo)
                {
                    strcmd = string.Format("{0}:{1}:0", -1001, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int nIndex = -1;
                for (int i = 0; i < userInfo.ListRoleIDs.Count; ++i)
                {
                    if (userInfo.ListRoleIDs[i] == dbRoleInfo.RoleID)
                    {
                        nIndex = i;
                        break;
                    }
                }

                dbRoleInfo.IsFlashPlayer = 0;

                // 更新DB
                // 1 清空经验
                DBWriter.UpdateRoleExpForFlashPlayerWhenLogOut(dbMgr, roleID);
                dbRoleInfo.Experience = 0;
                dbRoleInfo.MainTaskID = 0;
                dbRoleInfo.MainQuickBarKeys = "";

                // 2 清空等级
                DBWriter.UpdateRoleLevForFlashPlayerWhenLogOut(dbMgr, roleID);
                dbRoleInfo.Level = 1;
                userInfo.ListRoleLevels[nIndex] = 1;

                if (DBWriter.UpdateRoleInfoForFlashPlayerFlag(dbMgr, roleID, 0))
                {
                    strcmd = string.Format("{0}:{1}", roleID, 1);
                }
                else
                {
                    strcmd = string.Format("{0}:{1}", roleID, 0);
                }

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);

                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// Làm rỗng dữ liệu khi người chơi đăng xuất
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessCleanDataWhenFreshPlayerLogOutCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 1)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);

                string strcmd = "";

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    strcmd = string.Format("{0}:{1}", "", -1);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                DBUserInfo userInfo = dbMgr.GetDBUserInfo(dbRoleInfo.UserID);
                if (null == userInfo)
                {
                    strcmd = string.Format("{0}:{1}:0", -1001, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int nIndex = -1;
                for (int i = 0; i < userInfo.ListRoleIDs.Count; ++i)
                {
                    if (userInfo.ListRoleIDs[i] == dbRoleInfo.RoleID)
                    {
                        nIndex = i;
                        break;
                    }
                }

                /// Kinh nghiệm
                DBWriter.UpdateRoleExpForFlashPlayerWhenLogOut(dbMgr, roleID);
                dbRoleInfo.Experience = 0;
                dbRoleInfo.MainTaskID = 0;
                dbRoleInfo.MainQuickBarKeys = "";

                /// Cấp độ
                DBWriter.UpdateRoleLevForFlashPlayerWhenLogOut(dbMgr, roleID);
                dbRoleInfo.Level = 1;
                userInfo.ListRoleLevels[nIndex] = 1;

                /// Danh sách vật phẩm
                DBWriter.UpdateRoleGoodsForFlashPlayerWhenLogOut(dbMgr, roleID);
                dbRoleInfo.GoodsDataList = new ConcurrentDictionary<int, GoodsData>();

                /// Danh sách nhiệm vụ
                DBWriter.UpdateRoleTasksForFlashPlayerWhenLogOut(dbMgr, roleID);
                dbRoleInfo.DoingTaskList = new ConcurrentDictionary<int, TaskData>();
                dbRoleInfo.OldTasks = new List<OldTaskData>();

                strcmd = string.Format("{0}:{1}", roleID, 1);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// Thay đổi cấp độ của nhiệm vụ
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessChangeTaskStarLevelCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 3)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int TaskID = Convert.ToInt32(fields[1]);
                int StarLevel = Convert.ToInt32(fields[2]);

                string strcmd = "";

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    strcmd = string.Format("{0}:{1}", "", -1);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                bool bRet = false;
                if (DBWriter.UpdateRoleTasksStarLevel(dbMgr, roleID, TaskID, StarLevel))
                {
                    if (dbRoleInfo.DoingTaskList != null)
                    {
                        if (dbRoleInfo.DoingTaskList.TryGetValue(TaskID, out TaskData task))
                        {
                            task.StarLevel = StarLevel;
                            strcmd = string.Format("{0}:{1}", roleID, 1);
                            bRet = true;
                        }
                    }
                }

                if (bRet == false)
                {
                    strcmd = string.Format("{0}:{1}", roleID, -1);
                }

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 崇拜某人操作
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessAdmiredPlayerCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 3)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleAID = Convert.ToInt32(fields[0]);
                int roleBID = Convert.ToInt32(fields[1]);
                int nDate = Convert.ToInt32(fields[2]);

                string strcmd = "";

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleAID);
                if (null == dbRoleInfo)
                {
                    strcmd = string.Format("{0}:{1}:{2}", roleAID, -1, 0);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                DBRoleInfo dbRoleInfo1 = dbMgr.GetDBRoleInfo(roleBID);
                if (null == dbRoleInfo1)
                {
                    strcmd = string.Format("{0}:{1}:{2}", roleAID, -1, 0);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                // 先查询下 今天是否崇拜了此人
                int nId = -1;
                nId = DBQuery.QueryPlayerAdmiredAnother(dbMgr, roleAID, roleBID, nDate);

                if (nId == roleBID)
                {
                    strcmd = string.Format("{0}:{1}:{2}", roleAID, -2, 0);                 // 今天已经崇拜了此人
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                // 更新DB 1.崇拜者相关数据--t_adorationinfo表 2.roleBID被崇拜的次数
                int nCount = dbRoleInfo1.AdmiredCount + 1;
                if (DBWriter.UpdateRoleAdmiredInfo1(dbMgr, roleBID, nCount) && DBWriter.UpdateRoleAdmiredInfo2(dbMgr, roleAID, roleBID, nDate))
                {
                    dbRoleInfo1.AdmiredCount = nCount;
                    strcmd = string.Format("{0}:{1}:{2}", roleAID, 1, dbRoleInfo1.AdmiredCount);
                }
                else
                    strcmd = string.Format("{0}:{1}:{2}", roleAID, -1, 0);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);

                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        private static TCPProcessCmdResults ProcessQueryRoleMiniInfoCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            long rid = 0;
            RoleMiniInfo roleMiniInfo = null;

            try
            {
                rid = DataHelper.BytesToObject<long>(data, 0, count);
                if (rid > 0)
                {
                    roleMiniInfo = CacheManager.GetRoleMiniInfo(rid);
                }

                tcpOutPacket = DataHelper.ObjectToTCPOutPacket<RoleMiniInfo>(roleMiniInfo, pool, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }

            tcpOutPacket = DataHelper.ObjectToTCPOutPacket<RoleMiniInfo>(roleMiniInfo, pool, (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 请求日常活动最高得分
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessQueryDayActivityPoinCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 1)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int nTpye = Convert.ToInt32(fields[0]); // 日常活动类型

                bool nRet = true;

                List<int> lData = null;

                lData = DBQuery.GetDayActivityTotlePoint(dbMgr, nTpye);

                string strcmd = "";

                DBRoleInfo dbRoleInfo = null;

                if (lData != null && lData.Count == 2)
                {
                    dbRoleInfo = dbMgr.GetDBRoleInfo(lData[0]);
                    while (dbRoleInfo == null)
                    {
                        DBWriter.DeleteRoleDayActivityInfo(dbMgr, lData[0], nTpye);

                        lData = DBQuery.GetDayActivityTotlePoint(dbMgr, nTpye);

                        if (lData != null && lData.Count == 2)
                        {
                            dbRoleInfo = dbMgr.GetDBRoleInfo(lData[0]);
                        }
                        else
                        {
                            nRet = false;
                            break;
                        }
                    }

                    /*if (null == dbRoleInfo)
                    {
                        DBWriter.DeleteRoleDayActivityInfo(dbMgr, lData[0], nTpye);

                        LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                                            (TCPGameServerCmds)nID, lData[0]));

                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }*/
                }

                if (nRet == true && dbRoleInfo != null)
                {
                    string strName = Global.FormatRoleName(dbRoleInfo);
                    strcmd = string.Format("{0}:{1}", lData[1], strName);
                }
                else
                    strcmd = string.Format("{0}:{1}", -1, null);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);

                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                // });
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 请求玩家日常活动最高得分
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessQueryRoleDayActivityPoinCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int nRoleid = Convert.ToInt32(fields[0]);
                int nType = Convert.ToInt32(fields[1]); // 日常活动类型 如果是0就全部查询

                string strcmd = "";
                int nVlue = -1;

                if (nType < 0 || nType > 5)
                {
                    strcmd = string.Format("{0}:{1}:{2}", nVlue, -1, -1);

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);

                    return TCPProcessCmdResults.RESULT_DATA;
                }

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(nRoleid);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, nRoleid));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int nBloodValue = 0;
                int nDaimonValue = 0;
                int nCampValue = 0;
                int nKingOfPK = 0;
                int nAngelTemple = 0;

                if (nType == 0)
                {
                    nBloodValue = DBQuery.GetRoleDayActivityPoint(dbMgr, nRoleid, 1);

                    nDaimonValue = DBQuery.GetRoleDayActivityPoint(dbMgr, nRoleid, 2);

                    nCampValue = DBQuery.GetRoleDayActivityPoint(dbMgr, nRoleid, 3);

                    nKingOfPK = DBQuery.GetRoleDayActivityPoint(dbMgr, nRoleid, 4);

                    nAngelTemple = DBQuery.GetRoleDayActivityPoint(dbMgr, nRoleid, 5);

                    strcmd = string.Format("{0}:{1}:{2}:{3}:{4}", nBloodValue, nDaimonValue, nCampValue, nKingOfPK, nAngelTemple);
                }
                else
                {
                    nVlue = DBQuery.GetRoleDayActivityPoint(dbMgr, nRoleid, nType);

                    if (nVlue == 1)
                    {
                        strcmd = string.Format("{0}:{1}:{2}:{3}:{4}", nVlue, -1, -1, -1, -1);
                    }
                    else if (nVlue == 2)
                    {
                        strcmd = string.Format("{0}:{1}:{2}:{3}:{4}", -1, nVlue, -1, -1, -1);
                    }
                    else if (nVlue == 3)
                    {
                        strcmd = string.Format("{0}:{1}:{2}:{3}:{4}", -1, -1, nVlue, -1, -1);
                    }
                    else if (nVlue == 4)
                    {
                        strcmd = string.Format("{0}:{1}:{2}:{3}:{4}", -1, -1, -1, nVlue, -1);
                    }
                    else if (nVlue == 5)
                    {
                        strcmd = string.Format("{0}:{1}:{2}:{3}:{4}", -1, -1, -1, -1, nVlue);
                    }
                }

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);

                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 更新玩家日常活动最高得分
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessUpdateRoleDayActivityPoinCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 8)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int nRoleid = Convert.ToInt32(fields[0]);
                int nType = Convert.ToInt32(fields[1]);
                int nDate = Convert.ToInt32(fields[2]);
                int nBloodValue = Convert.ToInt32(fields[3]);
                int nDaimonValue = Convert.ToInt32(fields[4]);
                int nCampValue = Convert.ToInt32(fields[5]);
                int nKingOfPk = Convert.ToInt32(fields[6]);
                long nAngelTemple = Convert.ToInt64(fields[7]);

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(nRoleid);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, nRoleid));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                string strcmd = "";
                int nCount = 0;

                if (nType == 0)
                {
                    nCount = DBQuery.GetBloodCastleEnterCount(dbMgr, nRoleid, nDate, 1);

                    DBWriter.UpdateRoleDayActivityPoint(dbMgr, nRoleid, nDate, 1, nCount, nBloodValue);

                    nCount = DBQuery.GetBloodCastleEnterCount(dbMgr, nRoleid, nDate, 2);

                    DBWriter.UpdateRoleDayActivityPoint(dbMgr, nRoleid, nDate, 2, nCount, nDaimonValue);

                    nCount = DBQuery.GetBloodCastleEnterCount(dbMgr, nRoleid, nDate, 3);

                    DBWriter.UpdateRoleDayActivityPoint(dbMgr, nRoleid, nDate, 3, nCount, nCampValue);

                    nCount = DBQuery.GetBloodCastleEnterCount(dbMgr, nRoleid, nDate, 4);

                    DBWriter.UpdateRoleDayActivityPoint(dbMgr, nRoleid, nDate, 4, nCount, nKingOfPk);

                    nCount = DBQuery.GetBloodCastleEnterCount(dbMgr, nRoleid, nDate, 5);

                    DBWriter.UpdateRoleDayActivityPoint(dbMgr, nRoleid, nDate, 5, nCount, nAngelTemple);
                }
                else
                {
                    nCount = DBQuery.GetBloodCastleEnterCount(dbMgr, nRoleid, nDate, nType);

                    if (nType == 1)
                    {
                        DBWriter.UpdateRoleDayActivityPoint(dbMgr, nRoleid, nDate, nType, nCount + 1, nBloodValue);
                    }
                    else if (nType == 2)
                    {
                        DBWriter.UpdateRoleDayActivityPoint(dbMgr, nRoleid, nDate, nType, nCount + 1, nDaimonValue);
                    }
                    else if (nType == 3)
                    {
                        DBWriter.UpdateRoleDayActivityPoint(dbMgr, nRoleid, nDate, nType, nCount + 1, nCampValue);
                    }
                    else if (nType == 4)
                    {
                        DBWriter.UpdateRoleDayActivityPoint(dbMgr, nRoleid, nDate, nType, nCount + 1, nKingOfPk);
                    }
                    else if (nType == 5)
                    {
                        DBWriter.UpdateRoleDayActivityPoint(dbMgr, nRoleid, nDate, nType, nCount + 1, nAngelTemple);
                    }
                }

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);

                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 查询玩家每日在线奖励的相关数据
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessQueryEveryDayOnLineAwardGiftInfoCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 1)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int nRoleid = Convert.ToInt32(fields[0]);

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(nRoleid);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, nRoleid));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                string strcmd = "";

                List<int> lListData = null;

                lListData = DBQuery.QueryPlayerEveryDayOnLineAwardGiftInfo(dbMgr, nRoleid);

                if (lListData != null)
                    strcmd = string.Format("{0}:{1}:{2}", 1, lListData[0], lListData[1]);
                else
                    strcmd = string.Format("{0}:{1}:{2}", -1, -1, -1);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);

                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 更新消息推送ID
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessUpdatePushMessageInfoCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int nRoleid = Convert.ToInt32(fields[0]);
                string strPushMsgID = fields[1];

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(nRoleid);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, nRoleid));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                string strcmd = "";

                int nRet = 0;
                nRet = DBWriter.SetUserPushMessageID(dbMgr, dbRoleInfo.UserID, strPushMsgID);

                if (nRet == 1)
                    dbRoleInfo.PushMsgID = strPushMsgID;

                strcmd = string.Format("{0}:{1}", nRoleid, nRet);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);

                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 请求推送玩家列表
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessQueryPushMsgUerListCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 1)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int nCondition = Convert.ToInt32(fields[0]);

                List<PushMessageData> list = new List<PushMessageData>();

                list = DBQuery.QueryPushMsgUerList(dbMgr, nCondition);

                tcpOutPacket = DataHelper.ObjectToTCPOutPacket<List<PushMessageData>>(list, pool, nID);

                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 请求魔晶兑换信息
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessQueryMoJingExchangeInfoCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int nDayID = Convert.ToInt32(fields[1]);

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //将用户的请求发起写数据库的操作
                Dictionary<int, int> TmpDict = DBQuery.QueryMoJingExchangeDict(dbMgr, roleID, nDayID);

                tcpOutPacket = DataHelper.ObjectToTCPOutPacket<Dictionary<int, int>>(TmpDict, pool, nID);

                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 更新魔晶兑换信息
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessUpdateMoJingExchangeInfoCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 4)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int nExchangeID = Convert.ToInt32(fields[1]);
                int nDayid = Convert.ToInt32(fields[2]);
                int nNum = Convert.ToInt32(fields[3]);

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //将用户的请求发起写数据库的操作
                int ret = DBWriter.UpdateMoJingExchangeDict(dbMgr, roleID, nExchangeID, nDayid, nNum);
                if (ret <= 0)
                {
                    //添加任务失败
                    LogManager.WriteLog(LogTypes.Error, string.Format("更新图鉴提交信息失败，CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));
                }

                string strcmd = string.Format("{0}", ret);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 请求VIP等级奖励标记
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessQueryVipLevelAwardFlagCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 1)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int nFlag = 0;
                nFlag = DBQuery.QueryVipLevelAwardFlagInfo(dbMgr, dbRoleInfo.RoleID, dbRoleInfo.ZoneID);

                string strcmd = string.Format("{0}", nFlag);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 更新玩家的vip等级奖励领取标记
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessUpdateVipLevelAwardFlagCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                int nFlag = Convert.ToInt32(fields[1]);

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                string strcmd = "";

                //将用户的请求发起写数据库的操作

                //int nValue = 0;
                //nValue = DBQuery.QueryVipLevelAwardFlagInfo(dbMgr, dbRoleInfo.RoleID);

                bool ret = DBWriter.UpdateVipLevelAwardFlagInfo(dbMgr, dbRoleInfo.UserID, nFlag, dbRoleInfo.ZoneID);

                if (!ret)
                {
                    strcmd = string.Format("{0}:{1}", roleID, -1);

                    LogManager.WriteLog(LogTypes.Error, string.Format("更新角色VIP等级奖励标记事时失败，CMD={0}, RoleID={1}", (TCPGameServerCmds)nID, roleID));
                }
                else
                {
                    lock (dbRoleInfo)
                    {
                        dbRoleInfo.VipAwardFlag = nFlag;
                    }

                    strcmd = string.Format("{0}:{1}", roleID, 0);
                }

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        private static TCPProcessCmdResults ProcessAddItemLogCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                //if (fields.Length != 9)
                //[bing] t_usemoney_log 增加字段 optSurplus 属性操作后剩余值
                if (fields.Length != 10)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                DBWriter.insertItemLog(dbMgr, fields);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "1", nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        private static TCPProcessCmdResults ProcessUpdateRoleKuaFuDayLogCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            RoleKuaFuDayLogData cmdData = null;

            try
            {
                cmdData = DataHelper.BytesToObject<RoleKuaFuDayLogData>(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                if (null == cmdData)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("解析数据结果RoleKuaFuDayLogData失败, CMD={0}, Recv={1}",
                        (TCPGameServerCmds)nID, data.Length));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                DBWriter.UpdateRoleKuaFuDayLog(dbMgr, cmdData);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "1", nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 处理更新用户仓库金币操作
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessAddRoleStoreYinliang(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                long value = Convert.ToInt64(fields[1]);

                string strcmd = "";
                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}",
                        (TCPGameServerCmds)nID, roleID));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                bool failed = false;
                long userYinLiang = 0;

                lock (dbRoleInfo.GetMoneyLock)
                {

                    if (value < 0 && dbRoleInfo.store_yinliang < Math.Abs(value))
                    {
                        failed = true;
                    }
                    else
                    {
                        dbRoleInfo.store_yinliang = Math.Max(0, dbRoleInfo.store_yinliang + value);
                        userYinLiang = dbRoleInfo.store_yinliang;
                    }
                }


                if (failed)
                {

                    strcmd = string.Format("{0}:{1}", roleID, -1);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }


                if (value != 0)
                {

                    bool ret = DBWriter.UpdateRoleStoreYinLiang(dbMgr, roleID, userYinLiang);
                    if (!ret)
                    {
                        LogManager.WriteLog(LogTypes.Error, string.Format("更新角色仓库金币失败，CMD={0}, RoleID={1}",
                            (TCPGameServerCmds)nID, roleID));

                        //添加失败
                        strcmd = string.Format("{0}:{1}", roleID, -2);

                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }
                }



                strcmd = string.Format("{0}:{1}", roleID, userYinLiang);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {

                DataHelper.WriteFormatExceptionLog(ex, "", false);

            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// Thêm bạc vào thương khố cho người chơi
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessAddRoleStoreMoney(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                /// ID người chơi
                int roleID = Convert.ToInt32(fields[0]);
                /// Số bạc thêm vào thương khố
                int value = Convert.ToInt32(fields[1]);

                string strcmd = "";
                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}", (TCPGameServerCmds)nID, roleID));
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                bool failed = false;
                long userMoney = 0;
                lock (dbRoleInfo)
                {
                    if (value < 0 && dbRoleInfo.store_money < Math.Abs(value))
                    {
                        failed = true;
                    }
                    else
                    {
                        dbRoleInfo.store_money = Math.Max(0, dbRoleInfo.store_money + value);
                        userMoney = dbRoleInfo.store_money;
                    }
                }

                /// Nếu toang
                if (failed)
                {
                    /// Gửi gói tin thao tác thất bại
                    strcmd = string.Format("{0}:{1}", roleID, -1);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                /// Nếu khác 0 thì cập nhật DB
                if (value != 0)
                {
                    bool ret = DBWriter.UpdateRoleStoreMoney(dbMgr, roleID, userMoney);
                    if (!ret)
                    {
                        LogManager.WriteLog(LogTypes.Error, string.Format("Update role store money faild，CMD={0}, RoleID={1}", (TCPGameServerCmds)nID, roleID));
                        strcmd = string.Format("{0}:{1}", roleID, -2);
                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                        return TCPProcessCmdResults.RESULT_DATA;
                    }
                }


                /// Gửi lại gói tin về GS
                strcmd = string.Format("{0}:{1}", roleID, userMoney);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }



        /// <summary>
        /// 完成某任务与之前所有任务(仅限对db操作完成，不走任务流程) [XSea 2015/4/23]
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessAutoCompletionTaskByTaskID(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;

            try
            {
                List<int> taskList = DataHelper.BytesToObject<List<int>>(data, 0, count); // 任务id列表
                if (null == taskList || taskList.Count < 2)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("任务列表异常, CMD={0}", (TCPGameServerCmds)nID));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                int roleID = taskList[0]; // 角色id
                int taskID = taskList[taskList.Count - 1]; //最后一个任务

                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(roleID);

                // 检查角色
                if (null == dbRoleInfo)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Source player is not exist, CMD={0}, RoleID={1}", (TCPGameServerCmds)nID, roleID));
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                //将用户的请求更新内存缓存
                lock (dbRoleInfo)
                {
                    //设置历史任务和已完成的主线任务编号
                    bool bWriteRes = DBWriter.WirterAutoCompletionTaskByTaskID(dbMgr, roleID, taskList);
                    if (!bWriteRes)
                    {
                        LogManager.WriteLog(LogTypes.Error, string.Format("插入历史任务标记失败，CMD={0}, RoleID={1}", (TCPGameServerCmds)nID, roleID));
                        tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                        return TCPProcessCmdResults.RESULT_FAILED;
                    }

                    // 更新已经完成主线任务id
                    DBWriter.UpdateRoleMainTaskID(dbMgr, roleID, taskID);

                    // 更新缓存
                    dbRoleInfo.MainTaskID = taskID; // 主线任务id

                    if (null == dbRoleInfo.OldTasks)
                        dbRoleInfo.OldTasks = new List<OldTaskData>();

                    // 加入所有已完成的任务
                    for (int i = 1; i < taskList.Count; i++)
                    {
                        dbRoleInfo.OldTasks.Add(new OldTaskData() { TaskID = taskList[i], DoCount = 1 });
                    }
                }

                // 返回成功
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, string.Format("{0}", 0), nID);
                return TCPProcessCmdResults.RESULT_DATA;

                /* client.sendCmd(nID, str);
                 return TCPProcessCmdResults.RESULT_OK;*/
            }
            catch (Exception ex)
            {
                //System.Windows.Application.Current.Dispatcher.Invoke((MethodInvoker)delegate
                //{
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
                //throw ex;
                //});
            }

            // 返回失败
            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, string.Format("{0}", -1), nID);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 更新结婚数据
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>

        /// <summary>
        /// 获取结婚数据
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessGetMarriageDataCmd(DBManager dbMgr, TCPOutPacketPool pool, GameServerClient client, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            MarriageData updateMarriageData = null;
            int nRoleID = -1;

            bool bRet = false;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                nRoleID = Convert.ToInt32(cmdData);

                //查缓存数据
                DBRoleInfo dbRoleInfo = dbMgr.GetDBRoleInfo(nRoleID);
                if (null != dbRoleInfo)
                {
                    lock (dbRoleInfo)
                    {
                        updateMarriageData = dbRoleInfo.MyMarriageData;
                    }
                }
                else
                //查询数据库
                {
                    updateMarriageData = DBQuery.GetMarriageData(dbMgr, nRoleID);
                }

                if (null != updateMarriageData)
                {
                    tcpOutPacket = DataHelper.ObjectToTCPOutPacket<MarriageData>(updateMarriageData, pool, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }
                else
                {
                    //没找到回个空的回去
                    updateMarriageData = new MarriageData();

                    tcpOutPacket = DataHelper.ObjectToTCPOutPacket<MarriageData>(updateMarriageData, pool, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }
            }
            catch (Exception ex)
            {
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        /// <summary>
        /// 节日返利活动类领取
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="pool"></param>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="tcpOutPacket"></param>
        /// <returns></returns>
        private static TCPProcessCmdResults ProcessExecuteJieriFanLiCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split(':');
                if (fields.Length != 5)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                string fromDate = fields[1].Replace('$', ':');
                string toDate = fields[2].Replace('$', ':');
                int nActType = Convert.ToInt32(fields[3]);
                int extTag = Convert.ToInt32(fields[4]);
                extTag = Math.Max(0, extTag);

                string strcmd = "";
                DBRoleInfo roleInfo = dbMgr.GetDBRoleInfo(roleID);
                if (null == roleInfo)
                {
                    strcmd = string.Format("{0}:{1}:0", -1001, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //活动关键字，能够唯一标志某内活动的一个实例【在某段时间内的活动】
                string huoDongKeyStr = Global.GetHuoDongKeyString(fromDate, toDate);

                int hasgettimes = 0;
                string lastgettime = "";

                //判断是否领取过---活动关键字字符串唯一确定了每次活动，针对同类型不同时间的活动
                int histForRole = DBQuery.GetAwardHistoryForRole(dbMgr, roleInfo.RoleID, roleInfo.ZoneID, nActType, huoDongKeyStr, out hasgettimes, out lastgettime);

                //如果extTag == 0 表示是查询
                if (extTag == 0)
                {
                    strcmd = string.Format("{0}:{1}", nActType, hasgettimes);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int bitVal = Global.GetBitValue(extTag);
                if ((hasgettimes & bitVal) == bitVal)
                {
                    strcmd = string.Format("{0}:{1}:0", -10005, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                DBUserInfo userInfo = dbMgr.GetDBUserInfo(roleInfo.UserID);
                if (null == userInfo)
                {
                    strcmd = string.Format("{0}:{1}:0", -1001, roleID);
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                //避免同一角色同时多次操作
                lock (roleInfo)
                {
                    hasgettimes |= (1 << (extTag - 1));

                    if (histForRole < 0)
                    {
                        //更新已领取状态
                        int ret = DBWriter.AddHongDongAwardRecordForRole(dbMgr, roleInfo.RoleID, roleInfo.ZoneID, nActType, huoDongKeyStr, hasgettimes, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        if (ret < 0)
                        {
                            strcmd = string.Format("{0}:{1}:0", -1008, roleID);
                            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                            return TCPProcessCmdResults.RESULT_DATA;
                        }
                    }
                    else
                    {
                        int ret = DBWriter.UpdateHongDongAwardRecordForRole(dbMgr, roleInfo.RoleID, roleInfo.ZoneID, nActType, huoDongKeyStr, hasgettimes, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        if (ret < 0)
                        {
                            strcmd = string.Format("{0}:{1}:0", -1008, roleID);
                            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                            return TCPProcessCmdResults.RESULT_DATA;
                        }
                    }
                }

                strcmd = string.Format("{0}:{1}:{2}", hasgettimes, roleID, extTag);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                // 格式化异常错误信息
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_ERR_RETURN);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        private static TCPProcessCmdResults ProcessGmBanCheck(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string[] fields = null;

            try
            {
                int length = 2;
                char span = '#';
                if (!CheckHelper.CheckTCPCmdFields2(nID, data, count, out fields, length, span))
                {
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, ((int)TCPProcessCmdResults.RESULT_FAILED).ToString(), nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int roleID = Convert.ToInt32(fields[0]);
                string banIDs = fields[1];

                string strcmd = BanManager.GmBanCheckAdd(dbMgr, roleID, banIDs).ToString();
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, ((int)TCPProcessCmdResults.RESULT_FAILED).ToString(), nID);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        private static TCPProcessCmdResults ProcessGmBanLog(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string[] fields = null;

            try
            {
                int length = 7;
                if (!CheckHelper.CheckTCPCmdFields(nID, data, count, out fields, length))
                {
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, ((int)TCPProcessCmdResults.RESULT_FAILED).ToString(), nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int zoneID = Convert.ToInt32(fields[0]);
                string userID = fields[1];
                int roleID = Convert.ToInt32(fields[2]);
                int banType = Convert.ToInt32(fields[3]);
                string banID = fields[4];
                int banCount = Convert.ToInt32(fields[5]);
                string deviceID = fields[6];

                string strcmd = BanManager.GmBanLogAdd(dbMgr, zoneID, userID, roleID, banType, banID, banCount, deviceID).ToString();
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, ((int)TCPProcessCmdResults.RESULT_FAILED).ToString(), nID);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        private static TCPProcessCmdResults ProcessTenInitCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string cmdData = null;

            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Wrong socket data CMD={0}", (TCPGameServerCmds)nID));

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_TEN_INIT);
                return TCPProcessCmdResults.RESULT_DATA;
            }

            try
            {
                string[] fields = cmdData.Split('#');
                if (fields.Length <= 0)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("Error Socket params count not fit CMD={0}, Recv={1}, CmdData={2}",
                        (TCPGameServerCmds)nID, fields.Length, cmdData));

                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_TEN_INIT);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                TenManager.initTen(fields);

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "1", nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, "0", (int)TCPGameServerCmds.CMD_DB_TEN_INIT);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        private static TCPProcessCmdResults ProcessSpreadAwardGetCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string[] fields = null;

            try
            {
                int length = 2;
                if (!CheckHelper.CheckTCPCmdFields(nID, data, count, out fields, length))
                {
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, ((int)TCPProcessCmdResults.RESULT_FAILED).ToString(), nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int zoneID = Convert.ToInt32(fields[0]);
                int roleID = Convert.ToInt32(fields[1]);

                string strcmd = SpreadManager.GetAward(dbMgr, zoneID, roleID).ToString();
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, ((int)TCPProcessCmdResults.RESULT_FAILED).ToString(), nID);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        private static TCPProcessCmdResults ProcessSpreadAwardUpdateCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string[] fields = null;

            try
            {
                int length = 4;
                if (!CheckHelper.CheckTCPCmdFields(nID, data, count, out fields, length))
                {
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, ((int)TCPProcessCmdResults.RESULT_FAILED).ToString(), nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int zoneID = Convert.ToInt32(fields[0]);
                int roleID = Convert.ToInt32(fields[1]);
                int type = Convert.ToInt32(fields[2]);
                string award = fields[3];

                string strcmd = SpreadManager.UpdateAward(dbMgr, zoneID, roleID, type, award).ToString();
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, ((int)TCPProcessCmdResults.RESULT_FAILED).ToString(), nID);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        private static TCPProcessCmdResults ProcessActivateStateGetCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string[] fields = null;

            try
            {
                int length = 1;
                if (!CheckHelper.CheckTCPCmdFields(nID, data, count, out fields, length))
                {
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, ((int)TCPProcessCmdResults.RESULT_FAILED).ToString(), nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                string userID = fields[0];

                string strcmd = DBQuery.ActivateStateGet(dbMgr, userID) ? "1" : "0";
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, ((int)TCPProcessCmdResults.RESULT_FAILED).ToString(), nID);
            return TCPProcessCmdResults.RESULT_DATA;
        }

        private static TCPProcessCmdResults ProcessActivateStateSetCmd(DBManager dbMgr, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;
            string[] fields = null;

            try
            {
                int length = 3;
                if (!CheckHelper.CheckTCPCmdFields(nID, data, count, out fields, length))
                {
                    tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, ((int)TCPProcessCmdResults.RESULT_FAILED).ToString(), nID);
                    return TCPProcessCmdResults.RESULT_DATA;
                }

                int zoneID = Convert.ToInt32(fields[0]);
                string userID = fields[1];
                int roleID = Convert.ToInt32(fields[2]);

                string strcmd = DBWriter.ActivateStateSet(dbMgr, zoneID, userID, roleID) ? "1" : "0";
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, strcmd, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "", false);
            }

            tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, ((int)TCPProcessCmdResults.RESULT_FAILED).ToString(), nID);
            return TCPProcessCmdResults.RESULT_DATA;
        }
    }
}