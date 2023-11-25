using GameDBServer.Logic.Rank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Task.Tool;

namespace GameDBServer.Logic
{
    /// <summary>
    /// Quản lý GameDB
    /// </summary>
    public class GameDBManager
    {
        /// <summary>
        /// Thống kê loại
        /// </summary>
        public const int StatisticsMode = 3;

        /// <summary>
        /// ID khu vực
        /// </summary>
        public static int ZoneID { get; set; } = 1;

        /// <summary>
        /// Tên DB
        /// </summary>
        public static string DBName { get; set; } = "kt_gamedb";

        /// <summary>
        /// Sự kiện lịch sử SQL
        /// </summary>
        public static ServerEvents SystemServerSQLEvents = new ServerEvents() { EventRootPath = "SQLs", EventPreFileName = "sql" };

        /// <summary>
        /// Cấu hình GameDB
        /// </summary>
        public static GameConfig GameConfigMgr = new GameConfig();

        /// <summary>
        /// 充值消费排行榜缓存管理器
        /// </summary>
        public static RankCacheManager RankCacheMgr = new RankCacheManager();


        /// <summary>
        /// 数据库自增长单区基础范围步长值 默认 一百万
        /// </summary>
        public static int DBAutoIncreaseStepValue = 1000000;


        public static int Guild_FamilyIncreaseStepValue = 100000;

        /// <summary>
        /// 跨服服务器的服务器LineID起始值
        /// </summary>
        public const int KuaFuServerIdStartValue = 9000; //或10000

        /// <summary>
        /// 代表所有线路,包括跨服线路
        /// </summary>
        public const int ServerLineIdAllIncludeKuaFu = 0;

        /// <summary>
        /// 代表所有线路,包括跨服线路,但不包括自己
        /// </summary>
        public const int ServerLineIdAllLineExcludeSelf = -1000;

        /// <summary>
        /// 根据需求 对充值排行进行管理
        /// </summary>
        public static DayRechargeRankManager DayRechargeRankMgr = new DayRechargeRankManager();

        /// <summary>
        /// 是否立即删除物品表的个数为0的数据行
        /// </summary>
        public static bool Flag_t_goods_delete_immediately = true;


        public static bool IsUsingQueeItem = false;


        /// <summary>
        /// 是否拆分角色参数表
        /// </summary>
        public static int Flag_Splite_RoleParams_Table = 0;

		/// <summary>
		/// 查询服务器剩余总钻石数的时间间隔(秒)
		/// </summary>
		public static int Flag_Query_Total_UserMoney_Minute = 60;

        public static Dictionary<string, int> IPRange2AutoIncreaseStepDict = new Dictionary<string, int>()
            {
                { "101.251", 1000000 },
                { "192.168", 0 },
            };
    }
}
