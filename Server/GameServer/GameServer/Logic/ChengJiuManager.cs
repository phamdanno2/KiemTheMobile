using GameServer.Core.Executor;
using GameServer.Core.GameEvent;

using GameServer.Server;
using GameServer.Server.CmdProcesser;
using Server.Data;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GameServer.Logic
{
    /// <summary>
    /// Classs quản lý liên quan đến thành tích, chẳng hạn như điểm thành tích, tổng số lần tiêu diệt, số lần đăng nhập hàng ngày liên tiếp, tổng số lần đăng nhập hàng ngày, mỗi dữ liệu được lưu trữ trong 4 byte
    /// </summary>
    public class ChengJiuManager : IManager
    {
        public const String EncodingLatin1 = "latin1";

        private static Dictionary<int, int> _DictFlagIndex = new Dictionary<int, int>();



        private static int _runeRate = 1;

        private static ChengJiuManager Instance = new ChengJiuManager();

        public static ChengJiuManager GetInstance()
        {
            return Instance;
        }

        public bool initialize()
        {
            TCPCmdDispatcher.getInstance().registerProcessor((int)TCPGameServerCmds.CMD_SPR_UPGRADE_CHENGJIU, 2, UpGradeChengLevelCmdProcessor.getInstance(TCPGameServerCmds.CMD_SPR_UPGRADE_CHENGJIU));
            return true;
        }

        public bool startup()
        {
            return true;
        }

        public bool showdown()
        {
            return true;
        }

        public bool destroy()
        {
            return true;
        }


        #region Khởi tạo nhật ký lưu lại chuỗi đăng nhập

        /// <summary>
        /// 初始化成就相关数据
        /// </summary>
        public static void InitRoleChengJiuData(KPlayer client)
        {
            client.ContinuousDayLoginNum = GetChengJiuExtraDataByField(client, ChengJiuExtraDataField.ContinuousDayLogin);
            client.TotalDayLoginNum = GetChengJiuExtraDataByField(client, ChengJiuExtraDataField.TotalDayLogin);


            client.TotalKilledMonsterNum = GetChengJiuExtraDataByField(client, ChengJiuExtraDataField.TotalKilledMonsterNum);



        }

        #endregion 初始成就相关数据

        #region GHI LẠI NHẬT KÝ KHI LOGIN

        public static void OnRoleLogin(KPlayer client, int preLoginDay)
        {
            int dayID = TimeUtil.NowDateTime().DayOfYear;
            if (dayID == preLoginDay)
            {
                return;
            }

            client.TotalDayLoginNum = GetChengJiuExtraDataByField(client, ChengJiuExtraDataField.TotalDayLogin);
            client.ContinuousDayLoginNum = GetChengJiuExtraDataByField(client, ChengJiuExtraDataField.ContinuousDayLogin);

            client.TotalDayLoginNum++;

            DateTime tm = TimeUtil.NowDateTime().AddDays(-1);
            int preDay = tm.DayOfYear;

            if (preDay == preLoginDay)
            {
                client.ContinuousDayLoginNum++;

                client.SeriesLoginNum++;
            }
            else
            {
                client.ContinuousDayLoginNum = 1;

                client.SeriesLoginNum = 1;
            }

            //Cập nhật trạng thái active USER
            if ("" != client.strUserID)
            {
                GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_DB_UPDATE_ACCOUNT_ACTIVE,
                                                string.Format("{0}", client.strUserID),
                                                null, client.ServerId);
            }

            Global.UpdateSeriesLoginInfo(client);

            ModifyChengJiuExtraData(client, (uint)client.TotalDayLoginNum, ChengJiuExtraDataField.TotalDayLogin, true);
            ModifyChengJiuExtraData(client, (uint)client.ContinuousDayLoginNum, ChengJiuExtraDataField.ContinuousDayLogin, true);

            CheckSingleConditionChengJiu(client, ChengJiuTypes.ContinuousLoginChengJiuStart, ChengJiuTypes.ContinuousLoginChengJiuEnd,
                client.ContinuousDayLoginNum, "LoginDayOne");

            CheckSingleConditionChengJiu(client, ChengJiuTypes.TotalLoginChengJiuStart, ChengJiuTypes.TotalLoginChengJiuEnd,
                client.TotalDayLoginNum, "LoginDayTwo");



            if (client.DailyActiveDayLginSetFlag != true)
            {
                bool bIsCompleted = false;

            }

            client.DailyActiveDayLginSetFlag = true;
        }

        #endregion GHI LẠI NHẬT KÝ KHI LOGIN

        #region 初始化配置

        /// <summary>
        /// 为成就项配置类型"Type"字段,不要每次添加新的成就时,要程序改这些常量
        /// </summary>
        public static void InitChengJiuConfig()
        {
            foreach (var kv in GameManager.systemChengJiu.SystemXmlItemDict)
            {
                int type = kv.Value.GetIntValue("ID");
                switch (type)
                {
                    case ChengJiuTypes.Task:
                        {
                            int chengJiuID = kv.Value.GetIntValue("ChengJiuID");
                            if (chengJiuID > ChengJiuTypes.MainLineTaskEnd)
                            {
                                ChengJiuTypes.MainLineTaskEnd = kv.Key;
                            }
                            else if (chengJiuID < ChengJiuTypes.MainLineTaskStart)
                            {
                                ChengJiuTypes.MainLineTaskStart = kv.Key;
                            }
                        }
                        break;
                }
            }
        }

        #endregion 初始化配置

        #region 成就符文相关

        /// <summary>
        /// 成就符文状态
        /// </summary>
        private enum AchievementRuneResultType
        {
            End = 3,            //提升达到极限
            Next = 2,           //成功，开启下一个
            Success = 1,        //成功，未生效
            Efail = 0,           //失败
            EnoOpen = -1,       //未开放
            EnoAchievement = -2,//成就不足
            EnoDiamond = -3,    //钻石不足
            EOver = -4,         //全部开启
        };




        /// <summary>
        /// 加载成就符文基本信息
        /// </summary>




        /// <summary>
        /// 获得今天成就符文提示次数
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static int GetAchievementRuneUpCount(KPlayer client)
        {
            int count = 0;
            int dayOld = 0;
            List<int> data = Global.GetRoleParamsIntListFromDB(client, RoleParamName.AchievementRuneUpCount);
            if (data != null && data.Count > 0)
                dayOld = data[0];

            int day = TimeUtil.NowDateTime().DayOfYear;
            if (dayOld == day)
                count = data[1];
            else
                ModifyAchievementRuneUpCount(client, count, true);

            return count;
        }

        /// <summary>
        /// 修改成就符文次数数据
        /// </summary>
        /// <returns></returns>
        public static void ModifyAchievementRuneUpCount(KPlayer client, int count, bool writeToDB = false)
        {
            List<int> dataList = new List<int>();
            dataList.AddRange(new int[] { TimeUtil.NowDateTime().DayOfYear, count });

            Global.SaveRoleParamsIntListToDB(client, dataList, RoleParamName.AchievementRuneUpCount, writeToDB);
        }





        #endregion 成就符文相关

        #region 成就数据存盘

        /// <summary>
        /// 成就数据存盘
        /// </summary>
        public static void SaveRoleChengJiuData(KPlayer client)
        {
            //ModifyChengJiuExtraData(killer, (uint)++nKillBoss, ChengJiuExtraDataField.TotalKilledBossNum, true);
        }

        #endregion 成就数据存盘

        #region 标志位索引生成 与 获取

        /// <summary>
        /// 初始化标志位索引
        /// </summary>
        public static void InitFlagIndex()
        {
            _DictFlagIndex.Clear();

            // 索引必须手动生成，每一个id对应的索引位置不能变
            int index = 0;

            // 第一次
            for (int n = ChengJiuTypes.FirstKillMonster; n <= ChengJiuTypes.FirstBaiTan; n++)
            {
                _DictFlagIndex.Add(n, index);
                index += 2;//完成与否 和 是否领取奖励共需要两个标志位
            }

            // 等级需求成就
            for (int n = ChengJiuTypes.LevelStart; n <= ChengJiuTypes.LevelEnd; n++)
            {
                _DictFlagIndex.Add(n, index);
                index += 2;//完成与否 和 是否领取奖励共需要两个标志位
            }

            // 转生等级需求成就
            for (int n = ChengJiuTypes.LevelChengJiuStart; n <= ChengJiuTypes.LevelChengJiuEnd; n++)
            {
                _DictFlagIndex.Add(n, index);
                index += 2;//完成与否 和 是否领取奖励共需要两个标志位
            }

            // 技能升级成就开始 MU新增 [3/30/2014 LiaoWei]
            for (int n = ChengJiuTypes.SkillLevelUpStart; n <= ChengJiuTypes.SkillLevelUpEnd; n++)
            {
                _DictFlagIndex.Add(n, index);
                index += 2;//完成与否 和 是否领取奖励共需要两个标志位
            }

            //连续登录成就
            for (int n = ChengJiuTypes.ContinuousLoginChengJiuStart; n <= ChengJiuTypes.ContinuousLoginChengJiuEnd; n++)
            {
                _DictFlagIndex.Add(n, index);
                index += 2;//完成与否 和 是否领取奖励共需要两个标志位
            }

            //累积登录成就
            for (int n = ChengJiuTypes.TotalLoginChengJiuStart; n <= ChengJiuTypes.TotalLoginChengJiuEnd; n++)
            {
                _DictFlagIndex.Add(n, index);
                index += 2;//完成与否 和 是否领取奖励共需要两个标志位
            }

            //铜钱成就开始
            for (int n = ChengJiuTypes.ToQianChengJiuStart; n <= ChengJiuTypes.ToQianChengJiuEnd; n++)
            {
                _DictFlagIndex.Add(n, index);
                index += 2;//完成与否 和 是否领取奖励共需要两个标志位
            }

            //怪物成就开始
            for (int n = ChengJiuTypes.MonsterChengJiuStart; n <= ChengJiuTypes.MonsterChengJiuEnd; n++)
            {
                _DictFlagIndex.Add(n, index);
                index += 2;//完成与否 和 是否领取奖励共需要两个标志位
            }

            //boss成就开始
            for (int n = ChengJiuTypes.BossChengJiuStart; n <= ChengJiuTypes.BossChengJiuEnd; n++)
            {
                _DictFlagIndex.Add(n, index);
                index += 2;//完成与否 和 是否领取奖励共需要两个标志位
            }

            // 副本通关成就
            for (int n = ChengJiuTypes.CompleteCopyMapCountNormalStart; n <= ChengJiuTypes.CompleteCopyMapCountNormalEnd; n++)
            {
                _DictFlagIndex.Add(n, index);
                index += 2;//完成与否 和 是否领取奖励共需要两个标志位
            }

            for (int n = ChengJiuTypes.CompleteCopyMapCountHardStart; n <= ChengJiuTypes.CompleteCopyMapCountHardEnd; n++)
            {
                _DictFlagIndex.Add(n, index);
                index += 2;//完成与否 和 是否领取奖励共需要两个标志位
            }

            for (int n = ChengJiuTypes.CompleteCopyMapCountDifficltStart; n <= ChengJiuTypes.CompleteCopyMapCountDifficltEnd; n++)
            {
                _DictFlagIndex.Add(n, index);
                index += 2;//完成与否 和 是否领取奖励共需要两个标志位
            }

            //强化成就
            for (int n = ChengJiuTypes.QiangHuaChengJiuStart; n <= ChengJiuTypes.QianHuaChengJiuEnd; n++)
            {
                _DictFlagIndex.Add(n, index);
                index += 2;//完成与否 和 是否领取奖励共需要两个标志位
            }

            //追加成就
            for (int n = ChengJiuTypes.ZhuiJiaChengJiuStart; n <= ChengJiuTypes.ZhuiJiaChengJiuEnd; n++)
            {
                _DictFlagIndex.Add(n, index);
                index += 2;//完成与否 和 是否领取奖励共需要两个标志位
            }

            //合成成就
            for (int n = ChengJiuTypes.HeChengChengJiuStart; n <= ChengJiuTypes.HeChengChengJiuEnd; n++)
            {
                _DictFlagIndex.Add(n, index);
                index += 2;//完成与否 和 是否领取奖励共需要两个标志位
            }

            // 战盟成就 MU新增 [3/30/2014 LiaoWei]
            for (int n = ChengJiuTypes.GuildChengJiuStart; n <= ChengJiuTypes.GuildChengJiuEnd; n++)
            {
                _DictFlagIndex.Add(n, index);
                index += 2;//完成与否 和 是否领取奖励共需要两个标志位
            }

            // 军衔成就开始 MU新增 [3/30/2014 LiaoWei]
            for (int n = ChengJiuTypes.JunXianChengJiuStart; n <= ChengJiuTypes.JunXianChengJiuEnd; n++)
            {
                _DictFlagIndex.Add(n, index);
                index += 2;//完成与否 和 是否领取奖励共需要两个标志位
            }

            // 主线任务成就 MU新增 [8/6/2014 LiaoWei]
            for (int n = ChengJiuTypes.MainLineTaskStart; n <= ChengJiuTypes.MainLineTaskEnd; n++)
            {
                _DictFlagIndex.Add(n, index);
                index += 2;//完成与否 和 是否领取奖励共需要两个标志位
            }

            /*//经脉成就
            for (int n = ChengJiuTypes.JingMaiChengJiuStart; n <= ChengJiuTypes.JingMaiChengJiuEnd; n++)
            {
                _DictFlagIndex.Add(n, index);
                index += 2;//完成与否 和 是否领取奖励共需要两个标志位
            }

            //武学成就
            for (int n = ChengJiuTypes.WuXueChengJiuStart; n <= ChengJiuTypes.WuXueChengJiuEnd; n++)
            {
                _DictFlagIndex.Add(n, index);
                index += 2;//完成与否 和 是否领取奖励共需要两个标志位
            }*/

            //如果新加成就，必须加在后面!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        }

        /// <summary>
        /// 通过成就索引位置返回成就ID
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected static ushort GetChengJiuIDByIndex(int index)
        {
            for (int n = 0; n < _DictFlagIndex.Count; n++)
            {
                if (_DictFlagIndex.ElementAt(n).Value == index)
                {
                    return (ushort)_DictFlagIndex.ElementAt(n).Key;
                }
            }
            return 0;
        }

        /// <summary>
        /// 根据成就id返回是否完成索引 失败返回-1
        /// </summary>
        /// <param name="chengJiuID"></param>
        /// <returns></returns>
        protected static int GetCompletedFlagIndex(int chengJiuID)
        {
            int index = -1;

            if (_DictFlagIndex.TryGetValue(chengJiuID, out index))
            {
                return index;
            }

            return -1;
        }

        /// <summary>
        /// 根据成就id返回是否领取索引 失败返回-1
        /// </summary>
        /// <param name="chengJiuID"></param>
        /// <returns></returns>
        protected static int GetAwardFlagIndex(int chengJiuID)
        {
            int index = -1;

            if (_DictFlagIndex.TryGetValue(chengJiuID, out index))
            {
                return index + 1;
            }

            return -1;
        }

        #endregion 标志位索引生成 与 获取

        #region 数据库操作 和 成就点数加减 总击杀怪物数量 总日登录次数 总连续登录天数 等处理

        /// <summary>
        /// 修改成就点数的值，modifyValue 可以是正数或者负数,相应的 增量和 减少量
        /// </summary>
        /// <param name="client"></param>
        /// <param name="modifyValue"></param>
        public static void AddChengJiuPoints(KPlayer client, string strFrom, int modifyValue = 1, Boolean forceUpdateBuffer = true, bool writeToDB = false)
        {

        }

        /// <summary>
        /// Lấy ra số ngày đã tích lũy đăng nhập
        /// </summary>
        /// <returns></returns>
        public static uint GetChengJiuExtraDataByField(KPlayer client, ChengJiuExtraDataField field)
        {
            List<uint> lsUint = Global.GetRoleParamsUIntListFromDB(client, RoleParamName.ChengJiuExtraData);

            int index = (int)field;

            if (index >= lsUint.Count)
            {
                return 0;
            }

            return lsUint[index];
        }

        /// <summary>
        /// 修改成就额外数据
        /// </summary>
        /// <returns></returns>
        public static void ModifyChengJiuExtraData(KPlayer client, UInt32 value, ChengJiuExtraDataField field, bool writeToDB = false)
        {
            List<uint> lsUint = Global.GetRoleParamsUIntListFromDB(client, RoleParamName.ChengJiuExtraData);

            int index = (int)field;

            while (lsUint.Count < (index + 1))
            {
                lsUint.Add(0);
            }

            lsUint[index] = value;

            Global.SaveRoleParamsUintListToDB(client, lsUint, RoleParamName.ChengJiuExtraData, writeToDB);
        }

        /// <summary>
        /// 返回成就等级
        /// </summary>
        /// <returns></returns>
        public static int GetChengJiuLevel(KPlayer client)
        {
            int uChengJiuLevel = Global.GetRoleParamsInt32FromDB(client, RoleParamName.ChengJiuLevel);

            return uChengJiuLevel;
        }



        #endregion 数据库操作 和 成就点数加减 总击杀怪物数量 总日登录次数 总连续登录天数 等处理



        #region 成就完成与否 与 存储判断

        /// <summary>
        /// 通过成就ID提取成就存储位置，index 表示竖线分开的第几项，subIndex 表示某一项中的某一个标志位
        /// 成就id规则，采用成就类型乘以100加上一个子序号
        /// </summary>
        /// <param name="chengJiuID"></param>
        /// <param name="index"></param>
        /// <param name="subIndex"></param>
        /// <returns></returns>
        public static Boolean IsChengJiuCompleted(KPlayer client, int chengJiuID)
        {
            return IsFlagIsTrue(client, chengJiuID);
        }

        /// <summary>
        /// 判断成就奖励是否被领取，index 表示竖线分开的第几项，subIndex 表示某一项中的某一个标志位
        /// 成就id规则，采用成就类型乘以100加上一个子序号
        /// </summary>
        /// <param name="chengJiuID"></param>
        /// <param name="index"></param>
        /// <param name="subIndex"></param>
        /// <returns></returns>
        public static Boolean IsChengJiuAwardFetched(KPlayer client, int chengJiuID)
        {
            return IsFlagIsTrue(client, chengJiuID, true);
        }

        /// <summary>
        /// 成就完成提示
        /// </summary>
        /// <param name="client"></param>
        /// <param name="chengJiuID"></param>
        public static void OnChengJiuCompleted(KPlayer client, int chengJiuID)
        {
            //设置成就完成标志
            UpdateChengJiuFlag(client, chengJiuID);

            // 给奖励 [3/14/2014 LiaoWei]
            ChengJiuManager.GiveChengJiuAward(client, chengJiuID, "完成成就ID：" + chengJiuID);

            //刚刚完成的成就
            NotifyClientChengJiuData(client, chengJiuID);

        }

        /// <summary>
        /// 通知客户端成就数据
        /// </summary>
        /// <param name="client"></param>
        /// <param name="justCompletedChengJiu"></param>
        public static void NotifyClientChengJiuData(KPlayer client, int justCompletedChengJiu = -1)
        {
        }

        /// <summary>
        /// 返回成就信息无符号整数数组，用于传递给客户端，每个成就 14位的成就id， 一位完成标志，1位领取标准
        /// </summary>
        /// <param name="chengJiuString"></param>
        /// <returns></returns>
        protected static List<ushort> GetChengJiuInfoArray(KPlayer client)
        {
            //这儿不采用循环判断IsFlagIsTrue, 使用对成就列表进行循环处理，那样运算少

            List<ulong> lsLong = Global.GetRoleParamsUlongListFromDB(client, RoleParamName.ChengJiuFlags);

            //索引位置
            int curIndex = 0;

            List<ushort> lsUshort = new List<ushort>();

            for (int n = 0; n < lsLong.Count; n++)
            {
                ulong uValue = lsLong[n];

                for (int subIndex = 0; subIndex < 64; subIndex += 2)
                {
                    //采用 11 移动
                    ulong flag = (ulong)(((ulong)0x3) << (subIndex));//完成与否 领取与否

                    //得到2bit标志位
                    ushort realFlag = (ushort)((uValue & flag) >> (subIndex));//提取到两个标志位表示的数 到最右边，得到一个数

                    ushort chengJiuID = GetChengJiuIDByIndex(curIndex);

                    //14bit 的chengJiuID
                    ushort preFix = (ushort)(chengJiuID << 2);

                    //14bit成就ID + 2bit标志位
                    ushort chengJiu = (ushort)(preFix | realFlag);

                    lsUshort.Add(chengJiu);

                    curIndex += 2;//注 索引也是2递增的，因为标志位是两个一组，一个是否完成，一个是否领取

                    //System.Diagnostics.Debug.WriteLine(String.Format("{0}--{1}--{2}--{3}", chengJiuID, chengJiu, preFix, realFlag));
                }
            }

            return lsUshort;
        }

        /// <summary>
        /// 给予完成成就的奖励 会进行完成与否 与是否已经领取的判断
        /// </summary>
        /// <param name="client"></param>
        /// <param name="chengJiuID"></param>
        public static int GiveChengJiuAward(KPlayer client, int chengJiuID, string strFrom)
        {
            //未完成成就不能领取
            if (!IsChengJiuCompleted(client, chengJiuID))
            {
                return -1;
            }

            //奖励领取过了不能再领
            if (IsChengJiuAwardFetched(client, chengJiuID))
            {
                return -2;
            }

            //设置领取标志位
            UpdateChengJiuFlag(client, chengJiuID, true);

            int bindZuanShi = 0, awardBindMoney = 0, awardChengJiuPoints = 0;

            //读取奖励参数
            SystemXmlItem itemChengJiu = null;
            if (GameManager.systemChengJiu.SystemXmlItemDict.TryGetValue(chengJiuID, out itemChengJiu))
            {
                bindZuanShi = Math.Max(0, itemChengJiu.GetIntValue("BindZuanShi"));
                awardBindMoney = Math.Max(0, itemChengJiu.GetIntValue("BindMoney"));
                awardChengJiuPoints = Math.Max(0, itemChengJiu.GetIntValue("ChengJiu"));
            }

            //奖励绑钻
            if (bindZuanShi > 0)
            {
                GameManager.ClientMgr.AddUserBoundMoney(client, bindZuanShi, strFrom);
            }

            //奖励绑定铜钱
            if (awardBindMoney > 0)
            {
                //更新用户的绑定铜钱
                GameManager.ClientMgr.AddMoney(Global._TCPManager.MySocketListener, Global._TCPManager.tcpClientPool, Global._TCPManager.TcpOutPacketPool, client, awardBindMoney, "完成成就：" + chengJiuID, false);

                GameManager.SystemServerEvents.AddEvent(string.Format("角色完成成就获取绑定铜钱, roleID={0}({1}), Money={2}, newMoney={3}, chengJiuID={4}", client.RoleID, client.RoleName, client.Money, awardBindMoney, chengJiuID), EventLevels.Record);
            }

            //给予成就奖励
            if (awardChengJiuPoints > 0)
            {
                AddChengJiuPoints(client, strFrom, awardChengJiuPoints, true, true);
            }

            return 0;
        }

        #endregion 成就完成与否 与 存储判断

        #region 成就 是否完成 与成就奖励是否领取公共函数部分

        /// <summary>
        /// 判断chengJiuID对应标志位是否是true ，forAward=false 成就是否完成 和 forAward = true成就奖励是否领取
        /// </summary>
        /// <param name="chengJiuHexString"></param>
        /// <param name="chengJiuID"></param>
        /// <returns></returns>
        public static Boolean IsFlagIsTrue(KPlayer client, int chengJiuID, Boolean forAward = false)
        {
            //完成标志索引
            int index = GetCompletedFlagIndex(chengJiuID);
            if (index < 0)
            {
                return false;
            }

            if (forAward)
            {
                index++;
            }

            List<ulong> lsLong = Global.GetRoleParamsUlongListFromDB(client, RoleParamName.ChengJiuFlags);

            if (lsLong.Count <= 0)
            {
                return false;
            }

            //根据 index 定位到相应的整数
            int arrPosIndex = index / 64;

            if (arrPosIndex >= lsLong.Count)
            {
                return false;
            }

            //定位到整数内部的某个具体位置
            int subIndex = index % 64;

            UInt64 destLong = lsLong[arrPosIndex];

            //这个flag值比较特殊，这样写意味着 在 8 字节的 64位中处于从最小值开始，根据subIndex增加而增加
            //从64位存储的角度看，设计到大端序列和小端序列，看起来在不同的机器样子不一样
            ulong flag = ((ulong)(1)) << subIndex;

            //进行标志位判断
            bool bResult = (destLong & flag) > 0;

            return bResult;
        }

        /// <summary>
        /// 更新chengJiuID对应的成就项的标准位，并返回修改后的十六进制字符串，用于成就是否完成和成就奖励是否领取的统一处理
        /// </summary>
        /// <param name="chengJiuHexString"></param>
        /// <param name="chengJiuID"></param>
        /// <returns></returns>
        public static bool UpdateChengJiuFlag(KPlayer client, int chengJiuID, bool forAward = false)
        {
            //chengJiuString 长度必须是 8 的倍数，一个长整形是 8 字节

            //完成标志索引
            int index = GetCompletedFlagIndex(chengJiuID);
            if (index < 0)
            {
                return false;
            }

            if (forAward)
            {
                index++;
            }

            List<ulong> lsLong = Global.GetRoleParamsUlongListFromDB(client, RoleParamName.ChengJiuFlags);

            //根据 index 定位到相应的整数
            int arrPosIndex = index / 64;

            //填充64位整数
            while (arrPosIndex > lsLong.Count - 1)
            {
                lsLong.Add(0);
            }

            //定位到整数内部的某个具体位置
            int subIndex = index % 64;

            ulong destLong = lsLong[arrPosIndex];

            //这个flag值比较特殊，这样写意味着 在 8 字节的 64位中处于从最小值开始，根据subIndex增加而增加
            //从64位存储的角度看，设计到大端序列和小端序列，看起来在不同的机器样子不一样
            ulong flag = ((ulong)(1)) << subIndex;

            //设置标志位 为 1
            lsLong[arrPosIndex] = destLong | flag;

            //存储到数据库
            Global.SaveRoleParamsUlongListToDB(client, lsLong, RoleParamName.ChengJiuFlags, true);

            return true;
        }

        #endregion 成就 是否完成 与成就奖励是否领取公共函数部分

        #region 第一次相关成就触发处理

        /// <summary>
        /// 第一次杀怪
        /// </summary>
        /// <param name="client"></param>
        public static void OnFirstKillMonster(KPlayer client)
        {
            if (!IsChengJiuCompleted(client, ChengJiuTypes.FirstKillMonster))
            {
                OnChengJiuCompleted(client, ChengJiuTypes.FirstKillMonster);
            }
        }

        /// <summary>
        /// 第一次添加好友
        /// </summary>
        /// <param name="client"></param>
        public static void OnFirstAddFriend(KPlayer client)
        {
            if (!IsChengJiuCompleted(client, ChengJiuTypes.FirstAddFriend))
            {
                OnChengJiuCompleted(client, ChengJiuTypes.FirstAddFriend);
            }
        }

        /// <summary>
        /// 第一次入会
        /// </summary>
        /// <param name="client"></param>
        public static void OnFirstInFaction(KPlayer client)
        {
            if (!IsChengJiuCompleted(client, ChengJiuTypes.FirstInFaction))
            {
                OnChengJiuCompleted(client, ChengJiuTypes.FirstInFaction);
            }
        }

        /// <summary>
        /// 第一次组队
        /// </summary>
        /// <param name="client"></param>
        public static void OnFirstInTeam(KPlayer client)
        {
            if (!IsChengJiuCompleted(client, ChengJiuTypes.FirstInTeam))
            {
                OnChengJiuCompleted(client, ChengJiuTypes.FirstInTeam);
            }
        }

        /// <summary>
        /// 第一次合成
        /// </summary>
        /// <param name="client"></param>
        public static void OnFirstHeCheng(KPlayer client)
        {
            if (!IsChengJiuCompleted(client, ChengJiuTypes.FirstHeCheng))
            {
                OnChengJiuCompleted(client, ChengJiuTypes.FirstHeCheng);
            }
        }

        /// <summary>
        /// 第一次强化
        /// </summary>
        /// <param name="client"></param>
        public static void OnFirstQiangHua(KPlayer client)
        {
            if (!IsChengJiuCompleted(client, ChengJiuTypes.FirstQiangHua))
            {
                OnChengJiuCompleted(client, ChengJiuTypes.FirstQiangHua);
            }
        }

        /// <summary>
        /// 第一次追加
        /// </summary>
        /// <param name="client"></param>
        public static void OnFirstAppend(KPlayer client)
        {
            if (!IsChengJiuCompleted(client, ChengJiuTypes.FirstZhuiJia))
            {
                OnChengJiuCompleted(client, ChengJiuTypes.FirstZhuiJia);
            }
        }

        /// <summary>
        /// 第一次继承
        /// </summary>
        /// <param name="client"></param>
        public static void OnFirstJiCheng(KPlayer client)
        {
            if (!IsChengJiuCompleted(client, ChengJiuTypes.FirstJiCheng))
            {
                OnChengJiuCompleted(client, ChengJiuTypes.FirstJiCheng);
            }
        }

        /// <summary>
        /// 第一次摆摊
        /// </summary>
        /// <param name="client"></param>
        public static void OnFirstBaiTan(KPlayer client)
        {
            if (!IsChengJiuCompleted(client, ChengJiuTypes.FirstBaiTan))
            {
                OnChengJiuCompleted(client, ChengJiuTypes.FirstBaiTan);
            }
        }

        /*/// <summary>
        /// 第一次洗练
        /// </summary>
        /// <param name="client"></param>
        public static void OnFirstXiLian(KPlayer client)
        {
            if (!IsChengJiuCompleted(client, ChengJiuTypes.FirstXiLian))
            {
                OnChengJiuCompleted(client, ChengJiuTypes.FirstXiLian);
            }
        }

        /// <summary>
        /// 第一次炼化
        /// </summary>
        /// <param name="client"></param>
        public static void OnFirstLianHua(KPlayer client)
        {
            if (!IsChengJiuCompleted(client, ChengJiuTypes.FirstLianHua))
            {
                OnChengJiuCompleted(client, ChengJiuTypes.FirstLianHua);
            }
        }*/

        #endregion 第一次相关成就触发处理

        #region 等级相关成就触发处理

        /// <summary>
        /// 当角色升级的时候
        /// </summary>
        /// <param name="client"></param>
        public static void OnRoleLevelUp(KPlayer client)
        {
            // 成就改造 等级变成转生等级 [3/12/2014 LiaoWei]
            CheckSingleConditionChengJiu(client, ChengJiuTypes.LevelStart, ChengJiuTypes.LevelEnd,
                client.m_Level, "LevelLimit");
        }

        /// <summary>
        /// 当角色转生的时候
        /// </summary>
        /// <param name="client"></param>
        public static void OnRoleChangeLife(KPlayer client)
        {
        }

        #endregion 等级相关成就触发处理

        #region 强化相关成就触发处理

        /// <summary>
        /// 当角色装备强化的时候
        /// </summary>
        /// <param name="client"></param>
        public static void OnRoleEquipmentQiangHua(KPlayer client, int equipStarsNum)
        {
            int nCompletedID = -1;

            // 强化装备
            nCompletedID = CheckEquipmentChengJiu(client, ChengJiuTypes.QiangHuaChengJiuStart, ChengJiuTypes.QianHuaChengJiuEnd, equipStarsNum, "QiangHuaLimit");//具体值要配置

            /*if (nCompletedID != -1)
            {
                int nFlag = -1;
                nFlag = (int)GetChengJiuExtraDataByField(client, ChengJiuExtraDataField.ForgeNum);

                if (nFlag != -1 && equipStarsNum > nFlag)
                {
                    ModifyChengJiuExtraData(client, (uint)equipStarsNum, ChengJiuExtraDataField.ForgeNum, true);
                }
            }*/
        }

        #endregion 强化相关成就触发处理

        #region 追加相关成就触发处理

        /// <summary>
        /// 当角色物品追加的时候
        /// </summary>
        /// <param name="client"></param>
        public static void OnRoleGoodsAppend(KPlayer client, int AppendLev)
        {
            int nCompletedID = -1;

            // 合成得到物品的时候判断
            nCompletedID = CheckEquipmentChengJiu(client, ChengJiuTypes.ZhuiJiaChengJiuStart, ChengJiuTypes.ZhuiJiaChengJiuEnd,
                AppendLev, "ZhuiJiaLimit");

            /*if (nCompletedID != -1)
            {
                int nFlag = -1;
                nFlag = (int)GetChengJiuExtraDataByField(client, ChengJiuExtraDataField.AppendNum);

                if (nFlag != -1 && AppendLev > nFlag)
                {
                    ModifyChengJiuExtraData(client, (uint)AppendLev, ChengJiuExtraDataField.AppendNum, true);
                }
            }*/
        }

        #endregion 追加相关成就触发处理

        #region 合成相关成就触发处理

        /// <summary>
        /// 当角色物品合成的时候
        /// </summary>
        /// <param name="client"></param>
        public static void OnRoleGoodsHeCheng(KPlayer client, int goodsIDCreated)
        {
            int nCompletedID = -1;

            //合成得到物品的时候判断
            nCompletedID = CheckEquipmentChengJiu(client, ChengJiuTypes.HeChengChengJiuStart, ChengJiuTypes.HeChengChengJiuEnd,
                goodsIDCreated, "HeChengLimit");

            /*if (nCompletedID != -1)
            {
                int nFlag = -1;
                nFlag = (int)GetChengJiuExtraDataByField(client, ChengJiuExtraDataField.MergeData);

                if (nFlag != -1 && goodsIDCreated != nFlag)
                {
                    ModifyChengJiuExtraData(client, (uint)goodsIDCreated, ChengJiuExtraDataField.MergeData, true);
                }
            }*/
        }

        #endregion 合成相关成就触发处理

        #region 副本通关相关成就触发处理 -- 它属于浴血沙场



        #endregion 副本通关相关成就触发处理 -- 它属于浴血沙场




        #region 单一字段成就检查通用处理--比如boss数量，怪物数量，铜钱数量等

        /// <summary>
        /// 单一条件成就检查，成就的完成条件只有一个，而且，此类别成就对数量要求由少到多，比如boss击杀数量，怪物击杀数量
        /// 玩家金币数量等 返回下一个需要完成目标的数值
        /// </summary>
        protected static uint CheckSingleConditionChengJiu(KPlayer client, int chengJiuMinID, int chengJiuMaxID, long roleCurrentValue, String strCheckField)
        {
            SystemXmlItem itemChengJiu = null;

            //完成成就需要的最少目标值
            uint needMinValue = 0;

            //for (int chengJiuID = chengJiuMinID; chengJiuID <= chengJiuMaxID; chengJiuID++)
            //{
            //    if (GameManager.systemChengJiu.SystemXmlItemDict.TryGetValue(chengJiuID, out itemChengJiu))
            //    {
            //        if (null == itemChengJiu) continue;

            //        needMinValue = (uint)itemChengJiu.GetIntValue(strCheckField);

            //        if (roleCurrentValue >= needMinValue)
            //        {
            //            如果成就没完成
            //            if (!IsChengJiuCompleted(client, chengJiuID))
            //            {
            //                触发成就完成事件
            //                OnChengJiuCompleted(client, chengJiuID);
            //            }
            //        }
            //        else
            //        {
            //            break;//连最少的都没完成，直接退出
            //        }
            //    }
            //}

            return needMinValue;
        }

        #endregion 单一字段成就检查通用处理--比如boss数量，怪物数量，铜钱数量等

        #region 强化得到一件两星装备 或者 和成得到 一块强化石的判断类似 做装备做物品的通用判断,做限制，这儿只需要得到一件就ok，不需要判断多件

        /// <summary>
        /// 通过【装备强化】获得1件7星装备 通过【合成】功能 roleCurrentValue 是装备星级，成功合成1块三品强化石 roleCurrentValue 是物品ID,
        /// 每次传递，都表明通过某些操作得到这样星级的装备，或者这样的强化石, strCheckField 对应的字段应该是 EquipBornLimit="" 或 GoodsLimit="35002,1"
        /// </summary>
        protected static int CheckEquipmentChengJiu(KPlayer client, int chengJiuMinID, int chengJiuMaxID, long roleCurrentValue, String strCheckField)
        {
            SystemXmlItem itemChengJiu = null;

            int maxCompletedID = -1;

            //完成成就需要的最少目标值
            int needMinValue = 0;

            for (int chengJiuID = chengJiuMinID; chengJiuID <= chengJiuMaxID; chengJiuID++)
            {
                if (GameManager.systemChengJiu.SystemXmlItemDict.TryGetValue(chengJiuID, out itemChengJiu))
                {
                    if (null == itemChengJiu) continue;

                    String[] needMinValueArray = itemChengJiu.GetStringValue(strCheckField).Split(',');

                    if (needMinValueArray.Length != 2) continue;

                    needMinValue = Global.SafeConvertToInt32(needMinValueArray[0]);//多少星， 或者物品ID

                    if (roleCurrentValue == needMinValue)
                    {
                        //这儿，其实还涉及到数量的判断，比如 3星的装备三件， 或者 4品强化石 3个,如果没达到要求，就对成就进度进行累加
                        //考虑到数据储存的复杂度和策划的变通程度，要求needMinNum必须为1，不为1，则需要对成就进度数据进行额外存放，需要更大的空间
                        //同时，成就数据比较分散，涉及时由于空间限制，没有涉及通用的成就进度存储方案
                        int needMinNum = Global.SafeConvertToInt32(needMinValueArray[1]);//多少星， 或者物品ID 的个数

                        if (needMinNum > 1) continue;//大于1个的要求暂时不实现

                        //如果成就没完成
                        if (!IsChengJiuCompleted(client, chengJiuID))
                        {
                            //触发成就完成事件
                            OnChengJiuCompleted(client, chengJiuID);

                            maxCompletedID = chengJiuID;
                        }
                    }
                    else
                    {
                        //break;//这儿不用退出，对于宝石物品id的判断，没有最小值的说法
                    }
                }
            }

            return maxCompletedID;
        }

        #endregion 强化得到一件两星装备 或者 和成得到 一块强化石的判断类似 做装备做物品的通用判断,做限制，这儿只需要得到一件就ok，不需要判断多件
    }
}