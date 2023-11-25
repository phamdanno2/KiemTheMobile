using System.Collections.Generic;
using System.Windows;

//using System.Windows.Threading;

namespace GameServer.Logic
{
    /// <summary>
    /// 防外挂类型
    /// </summary>
    public enum CheckCheatTypes
    {
        MismatchMapCode = 0,
        CheatPosition = 1,
        DisableMovingOnAttack = 2,
        CheckClientDataOne = 98,
        CheckClientData = 99,
        Max = 100,
    }

    /// <summary>
    /// 线程安全的客户端数据
    /// </summary>
    public class CheckCheat
    {
        #region 防外挂校验

        /// <summary>
        /// 禁止攻击状态
        /// </summary>
        public bool DisableAttack = false;

        /// <summary>
        /// 地图编号不匹配状态
        /// </summary>
        public bool MismatchingMapCode = false;

        /// <summary>
        /// 上次校验坐标的时间
        /// </summary>
        public long LastValidateTicks;

        /// <summary>
        /// 上次记录的合理地图编号
        /// </summary>
        public int LastValidMapCode = 0;

        /// <summary>
        /// 上次记录的坐标X
        /// </summary>
        public int LastValidPosX { get; set; } = 0;

        /// <summary>
        /// 上次记录的坐标Y
        /// </summary>
        public int LastValidPosY { get; set; } = 0;

        /// <summary>
        /// 第一次通知客户端离开古墓地图的时间
        /// </summary>
        public long LastNotifyLeaveGuMuTick = 0;

        /// <summary>
        /// 最大速度的参数
        /// </summary>
        public double MaxClientSpeed = 0.0;

        /// <summary>
        /// 检测到可能进程加速的时间
        /// </summary>
        public long ProcessBoosterTicks = 0;

        /// <summary>
        /// 确认开启了进程加速
        /// </summary>
        public bool ProcessBooster = false;

        /// <summary>
        /// 反外挂数据
        /// </summary>
        //public long NextTaskListTimeout = 0;
        //public int RobotAlwaysLogCount = 0;
        //public int RobotDetectCount = 0;
        //public int GeniusDetectCount = 0;
        //public bool RobotDetectedAllowVirtual = false;
        //public long RobotDetectedKickTime = 0;
        //public string RobotDetectedReason = "";

        //public bool RobotTaskDetected = false;
        //public bool RobotTaskAllow = false;
        //public string RobotTaskStr = "";

        //public bool DetectedVm = false;
        //public bool DetectedGenius = false;
        //public bool DetectedBan = false;
        //public string RobotTaskListData = "";
        //public bool KickState = false;

        //public Dictionary<int, int> LogCountDic = new Dictionary<int, int>();

        public string RobotTaskListData = "";

        public int BanCheckMaxCount = 0;
        public int KickWarnMaxCount = 0;

        public bool DropRateDown = false;
        public bool KickState = false;

        public long RobotDetectedKickTime = 0;
        public string RobotDetectedReason = "";

        public long NextTaskListTimeout = 0;

        public Dictionary<int, int> LogCountDic = new Dictionary<int, int>();

        #endregion 防外挂校验

        #region 封号相关

        /// <summary>
        /// 是否被踢下线的角色
        /// </summary>
        public bool IsKickedRole;

        #endregion 封号相关

        #region

        // 最后一次使用的技能
        public int LastMagicCode = 0;

        // 最后一次伤害的数值
        public long LastDamage = 0;

        // 最后一次伤害的类型
        public int LastDamageType = 0;

        // 被攻击者的信息
        public int LastEnemyID = 0;

        public string LastEnemyName = "";
        public Point LastEnemyPos = new Point(0, 0);
        #endregion

        #region GM操作状态

        /// <summary>
        /// GM命令传送到非常规地图的影像地图
        /// </summary>
        public int GmGotoShadowMapCode;

        #endregion GM操作状态
    }
}