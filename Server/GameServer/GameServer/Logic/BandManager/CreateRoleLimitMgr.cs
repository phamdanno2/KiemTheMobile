using GameServer.Core.Executor;
using Server.Tools;
using Server.Tools.Pattern;
using System;
using System.Collections.Generic;
using System.Net;
using System.Xml.Linq;
using Tmsk.Tools.Tools;

namespace GameServer.Logic
{
    /// <summary>
    /// 地址白名单 IPWhiteList.xml
    /// </summary>
    public class IPWhiteList
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int ID = 0;

        /// <summary>
        /// MinIP
        /// </summary>
        public uint MinIP = 0;

        /// <summary>
        /// MaxIP
        /// </summary>
        public uint MaxIP = 0;
    }

    /// <summary>
    /// 玩家创建角色限制分析数据
    /// </summary>
    public class LimitAnalysisData
    {
        // 数据创建时间
        public DateTime Timestamp = TimeUtil.NowDateTime();

        // kvp数据
        public Dictionary<string, int> dict = new Dictionary<string, int>();
    }

    /// <summary>
    /// 检查结果
    /// </summary>
    public class LimitResultData
    {
        // 是否可创建
        public bool CanCreate = true;

        // 最早的有效分析数据时间
        public DateTime AnalysisDataTime;
    }

    /// <summary>
    /// 玩家创建角色限制管理
    /// </summary>
    public class CreateRoleLimitManager : SingletonTemplate<CreateRoleLimitManager>
    {
        /// <summary>
        /// 整理背包时间间隔
        /// </summary>
        public int ResetBagSlotTicks = 0;

        /// <summary>
        /// 刷新交易市场时间间隔
        /// </summary>
        public int RefreshMarketSlotTicks = 0;

        /// <summary>
        /// 宠物出战时间间隔
        /// </summary>
        public int SpriteFightSlotTicks = 0;

        /// <summary>
        /// 添加帮派成员时间间隔
        /// </summary>
        public int AddBHMemberSlotTicks = 0;

        /// <summary>
        /// 添加添加好友/黑名单/仇人时间间隔
        /// </summary>
        public int AddFriendSlotTicks = 0;

        /// <summary>
        /// 创建角色时间限制 默认24小时
        /// </summary>
        public int CreateRoleLimitMinutes = 60 * 24;

        // 配置文件
        private const string IPWhiteListfileName = "Config/IPWhiteList.xml";

        // 配置文件
        private const string UserWhiteListfileName = "Config/UserWhiteList.xml";

        /// <summary>
        /// 设备号限制创建角色数量
        /// </summary>
        private int DeviceIDRestrictNum = -1;

        /// <summary>
        /// IP限制创建角色数量 (24小时)
        /// </summary>
        private int IPRestrictNum = -1;

        /// <summary>
        /// IPLimitData[0]、DeviceIDLimitData[0] 数据的时间点
        /// </summary>
        private DateTime WorkDateTime = TimeUtil.NowDateTime();

        /// <summary>
        /// 地址白名单 (24小时)
        /// </summary>
        private List<IPWhiteList> _IPWhiteList = new List<IPWhiteList>();

        /// <summary>
        /// 帐号白名单 (24小时)
        /// </summary>
        private HashSet<string> _UserWhiteList = new HashSet<string>();

        /// <summary>
        /// 创角记录按小时汇总 Dictionary<DeviceID, CreateRoleNum>
        /// </summary>
        private LinkedList<LimitAnalysisData> DeviceIDLimitData = new LinkedList<LimitAnalysisData>();

        /// <summary>
        /// 创角记录按小时汇总 Dictionary<IP, CreateRoleNum>
        /// </summary>
        private LinkedList<LimitAnalysisData> IPLimitData = new LinkedList<LimitAnalysisData>();

        /// <summary>
        /// 是否可以创建角色
        /// </summary>
        public bool IfCanCreateRole(string UserID, string UserName, string DeviceID, string IP, out int NotifyLeftTime)
        {
            NotifyLeftTime = 0;

            // 检查数据
            LimitResultData CheckData = new LimitResultData();
            do
            {
                //是否在帐号白名单
                if (_UserWhiteList.Contains(UserID.ToLower()))
                {
                    break;
                }

                // 根据设备号检查
                CheckByDeviceID(UserID, UserName, DeviceID, CheckData);
                if (!CheckData.CanCreate)
                    break;

                // 根据IP检查
                CheckByIP(UserID, UserName, IP, CheckData);
                if (!CheckData.CanCreate)
                    break;
            } while (false);

            // 提示时间
            if (!CheckData.CanCreate)
            {
                NotifyLeftTime = CaculateNextAvailableTime(CheckData);
            }
            return CheckData.CanCreate;
        }

        /// <summary>
        /// 增加计数
        /// </summary>
        public void ModifyCreateRoleNum(string UserID, string UserName, string DeviceID, string IP)
        {
            lock (DeviceIDLimitData) // 设备号限制
            {
                if (-1 != DeviceIDRestrictNum && !string.IsNullOrEmpty(DeviceID))
                    ModifyTotalNum(DeviceIDLimitData, DeviceID);
            }

            lock (IPLimitData) // ip限制
            {
                if (-1 != IPRestrictNum && !string.IsNullOrEmpty(IP))
                    ModifyTotalNum(IPLimitData, IP);
            }
        }

        /// <summary>
        /// 计算到何时才可以再次尝试创建角色
        /// </summary>
        private int CaculateNextAvailableTime(LimitResultData CheckData)
        {
            if (CheckData.CanCreate)
                return 0;

            DateTime nextDayTime = CheckData.AnalysisDataTime.AddMinutes(CreateRoleLimitMinutes);
            DateTime Now = TimeUtil.NowDateTime();
            return (int)(nextDayTime - Now).TotalSeconds;
        }

        /// <summary>
        /// 添加统计数据
        /// </summary>
        private void ModifyTotalNum(LinkedList<LimitAnalysisData> list, string key)
        {
            Dictionary<string, int> DataNow = GetHourAnalysisData(list);
            if (null != DataNow)
            {
                int CountNum = 0;
                if (!DataNow.TryGetValue(key, out CountNum))
                {
                    DataNow.Add(key, 1);
                }
                else
                {
                    DataNow[key] = CountNum + 1;
                }
            }
        }

        /// <summary>
        /// 统计创建总数
        /// </summary>
        private int ComputeTotalNum(LinkedList<LimitAnalysisData> list, string key, LimitResultData CheckData)
        {
            int result = 0;
            if (list.Count == 0)
                return result;

            // 数据清理
            DoHouseKeepingForAnalysisData(list);

            foreach (var data in list)
            {
                int count = 0;
                if (data.dict.TryGetValue(key, out count))
                {
                    result += count;
                }
            }
            if (list.Count != 0)
                CheckData.AnalysisDataTime = list.First.Value.Timestamp;
            return result;
        }

        /// <summary>
        /// 获得当前时间点的统计数据
        /// </summary>
        private Dictionary<string, int> GetHourAnalysisData(LinkedList<LimitAnalysisData> list)
        {
            DateTime Now = TimeUtil.NowDateTime();
            if (list.Count == 0 || WorkDateTime.Hour != Now.Hour)
            {
                WorkDateTime = Now;
                list.AddLast(new LimitAnalysisData());
            }

            // return
            return list.Last.Value.dict;
        }

        /// <summary>
        /// 分析数据清理
        /// </summary>
        private void DoHouseKeepingForAnalysisData(LinkedList<LimitAnalysisData> list)
        {
            if (list == null || list.Count == 0)
                return;

            DateTime Now = TimeUtil.NowDateTime();
            LimitAnalysisData oldData = list.First.Value;

            // 数据超过24小时
            int SpanMinutes = (int)(Now - oldData.Timestamp).TotalMinutes;
            if (SpanMinutes >= CreateRoleLimitMinutes)
            {
                list.RemoveFirst();
            }
        }

        /// <summary>
        /// 根据设备号检查
        /// </summary>
        private void CheckByDeviceID(string UserID, string UserName, string DeviceID, LimitResultData CheckData)
        {
            if (-1 == DeviceIDRestrictNum || string.IsNullOrEmpty(DeviceID))
                return;

            lock (DeviceIDLimitData)
            {
                int CountNum = ComputeTotalNum(DeviceIDLimitData, DeviceID, CheckData);
                if (CountNum >= DeviceIDRestrictNum)
                {
                    CheckData.CanCreate = false;
                    LogManager.WriteLog(LogTypes.Error, string.Format("玩家创建角色被限制, UserID={0}, UserName={1}, DeviceID={2}, CountNum={3}", UserID, UserName, DeviceID, CountNum));
                    return;
                }
            }
        }

        /// <summary>
        /// IP是否在白名单中
        /// </summary>
        private bool IfIPInWhiteList(string IP)
        {
            List<IPWhiteList> MyIPWhiteList = null;
            lock (this)
            {
                MyIPWhiteList = _IPWhiteList;
            }

            if (MyIPWhiteList == null || MyIPWhiteList.Count == 0)
                return false;

            IPAddress IPAdd = IPAddress.Parse(IP);
            if (IPAdd == null)
                return false;

            byte[] byteMyIP = IPAdd.GetAddressBytes();
            uint myIP = (uint)(byteMyIP[0] << 24) | (uint)(byteMyIP[1] << 16) | (uint)(byteMyIP[2] << 8) | (uint)(byteMyIP[3]);

            foreach (var data in MyIPWhiteList)
            {
                if (data.MinIP <= myIP && data.MaxIP >= myIP)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 根据IP检查
        /// </summary>
        private void CheckByIP(string UserID, string UserName, string IP, LimitResultData CheckData)
        {
            if (-1 == IPRestrictNum || string.IsNullOrEmpty(IP))
                return;

            if (IfIPInWhiteList(IP))
                return;

            lock (IPLimitData)
            {
                int CountNum = ComputeTotalNum(IPLimitData, IP, CheckData);
                if (CountNum >= IPRestrictNum)
                {
                    CheckData.CanCreate = false;
                    LogManager.WriteLog(LogTypes.Error, string.Format("玩家创建角色被限制, UserID={0}, UserName={1}, IP={2}, CountNum={3}", UserID, UserName, IP, CountNum));
                    return;
                }
            }
        }

        #region 资源初始化

        /// <summary>
        /// 加载资源
        /// </summary>
        public void LoadConfig()
        {
            string strDeviceIDRestrict = "";
            if (!string.IsNullOrEmpty(strDeviceIDRestrict))
            {
                DeviceIDRestrictNum = Global.SafeConvertToInt32(strDeviceIDRestrict);
            }

            // IP限制创建角色数量
            string strIPRestrict = "";
            if (!string.IsNullOrEmpty(strIPRestrict))
            {
                IPRestrictNum = Global.SafeConvertToInt32(strIPRestrict);
            }

            string strResetBagSlotTicks = "";
            if (!string.IsNullOrEmpty(strResetBagSlotTicks))
            {
                ResetBagSlotTicks = Global.SafeConvertToInt32(strResetBagSlotTicks);
            }

            string strRefreshMarketSlotTicks = "";
            if (!string.IsNullOrEmpty(strRefreshMarketSlotTicks))
            {
                RefreshMarketSlotTicks = Global.SafeConvertToInt32(strRefreshMarketSlotTicks);
            }

            string strAddSlotTicks = "";
            if (!string.IsNullOrEmpty(strAddSlotTicks))
            {
                AddFriendSlotTicks = Global.SafeConvertToInt32(strAddSlotTicks);
                AddBHMemberSlotTicks = Global.SafeConvertToInt32(strAddSlotTicks);
            }

            string strSpriteFightSlotTicks = "";
            if (!string.IsNullOrEmpty(strSpriteFightSlotTicks))
            {
                SpriteFightSlotTicks = Global.SafeConvertToInt32(strSpriteFightSlotTicks);
            }
        }

        #endregion 资源初始化
    }
}