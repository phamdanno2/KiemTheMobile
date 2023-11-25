using GameDBServer.Logic;
using GameDBServer.Logic.Rank;
using MySQLDriverCS;
using Server.Data;
using Server.Tools;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;

namespace GameDBServer.DB
{
    /// <summary>
    /// Code lại cho đỡ toác
    /// </summary>
    public class DBRoleInfo
    {
        #region Các thực thể

        /// <summary>
        /// RoleiD
        /// </summary>
        public int RoleID
        {
            get;
            set;
        }



        private object _MoneyLock = new object();

        public object GetMoneyLock
        {
            get { return _MoneyLock; }
        }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID
        {
            get;
            set;
        }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName
        {
            get;
            set;
        }

        /// <summary>
        /// 角色性别
        /// </summary>
        public int RoleSex
        {
            get;
            set;
        }

        /// <summary>
        /// ID phái
        /// </summary>
        public int Occupation
        {
            get;
            set;
        }

        /// <summary>
        /// ID nhánh
        /// </summary>
        public int SubID
        {
            get;
            set;
        }

        /// <summary>
        /// 角色级别
        /// </summary>
        public int Level
        {
            get;
            set;
        }

        /// <summary>
        /// 角色头像
        /// </summary>
        public int RolePic
        {
            get;
            set;
        }

        /// <summary>
        /// 角色帮派
        /// </summary>
        public int GuildID
        {
            get;
            set;
        }

        /// <summary>
        /// 角色的绑定钱币
        /// </summary>
        public int Money1
        {
            get;
            set;
        }

        /// <summary>
        /// 角色的非绑定钱币
        /// </summary>
        public int Money2
        {
            get;
            set;
        }

        /// <summary>
        /// 角色的当前经验
        /// </summary>
        public long Experience
        {
            get;
            set;
        }

        /// <summary>
        /// 角色的PK模式
        /// </summary>
        public int PKMode
        {
            get;
            set;
        }

        /// <summary>
        /// 角色的PK值
        /// </summary>
        public int PKValue
        {
            get;
            set;
        }

        /// <summary>
        /// 角色的位置
        /// </summary>
        public string Position
        {
            get;
            set;
        }

        /// <summary>
        /// 注册时间
        /// </summary>
        public string RegTime
        {
            get;
            set;
        }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        public long LastTime
        {
            get;
            set;
        }

        /// <summary>
        /// 当前背包的页数(总个数 - 1)
        /// </summary>
        public int BagNum
        {
            get;
            set;
        }

        /// <summary>
        /// 主快捷面板的映射
        /// </summary>
        public string MainQuickBarKeys
        {
            get;
            set;
        }

        /// <summary>
        /// 辅助快捷面板的映射
        /// </summary>
        public string OtherQuickBarKeys
        {
            get;
            set;
        }

        /// <summary>
        /// 登录的次数
        /// </summary>
        public int LoginNum
        {
            get;
            set;
        }

        /// <summary>
        /// 剩余的自动挂机时间
        /// </summary>
        public int LeftFightSeconds
        {
            get;
            set;
        }

        /// <summary>
        /// 线路ID
        /// </summary>
        public int ServerLineID
        {
            get;
            set;
        }

        /// <summary>
        /// 总的在线秒数
        /// </summary>
        public int TotalOnlineSecs
        {
            get;
            set;
        }

        /// <summary>
        /// 防止沉迷在线秒数
        /// </summary>
        public int AntiAddictionSecs
        {
            get;
            set;
        }

        /// <summary>
        /// 上次离线时间
        /// </summary>
        public long LogOffTime
        {
            get;
            set;
        }

        /// <summary>
        /// 系统绑定的银两
        /// </summary>
        public int YinLiang
        {
            get;
            set;
        }

        /// <summary>
        /// 已经完成的主线任务的ID
        /// </summary>
        public int MainTaskID
        {
            get;
            set;
        }

        /// <summary>
        /// 当前的PK点
        /// </summary>
        public int PKPoint
        {
            get;
            set;
        }

        /// <summary>
        /// ID gia tộc
        /// </summary>
        public int FamilyID { get; set; }

        /// <summary>
        /// Tên gia tộc
        /// </summary>
        public string FamilyName { get; set; }

        /// <summary>
        /// Chức vị trong gia tộc
        /// </summary>
        public int FamilyRank { get; set; }

        /// <summary>
        /// Uy danh
        /// </summary>
        public int Prestige { get; set; }

        /// <summary>
        /// 杀BOSS的总个数
        /// </summary>
        public int KillBoss
        {
            get;
            set;
        }

        /// <summary>
        /// 充值TaskID
        /// </summary>
        public int CZTaskID
        {
            get;
            set;
        }

        /// <summary>
        /// 登录日ID
        /// </summary>
        public int LoginDayID
        {
            get;
            set;
        }

        /// <summary>
        /// 登录日次数
        /// </summary>
        public int LoginDayNum
        {
            get;
            set;
        }

        /// <summary>
        /// 区ID
        /// </summary>
        public int ZoneID
        {
            get;
            set;
        }

        /// <summary>
        /// 帮会名称
        /// </summary>
        public string GuildName
        {
            get;
            set;
        }

        /// <summary>
        /// 帮会职务
        /// </summary>
        public int GuildRank
        {
            get;
            set;
        }


        public int RoleGuildMoney
        {
            get;
            set;
        }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName
        {
            get;
            set;
        }

        /// <summary>
        /// 上次的mailID
        /// </summary>
        public int LastMailID
        {
            get;
            set;
        }

        /// <summary>
        /// 单次奖励记录标志位
        /// </summary>
        public long OnceAwardFlag
        {
            get;
            set;
        }

        /// <summary>
        /// 系统绑定的金币
        /// </summary>
        public int Gold
        {
            get;
            set;
        }

        /// <summary>
        /// 永久禁止聊天
        /// </summary>
        public int BanChat
        {
            get;
            set;
        }

        /// <summary>
        /// 永久禁止登陆
        /// </summary>
        public int BanLogin
        {
            get;
            set;
        }

        // MU项目增加字段 [11/30/2013 LiaoWei]
        /// <summary>
        /// 新人标记
        /// </summary>
        public int IsFlashPlayer
        {
            get;
            set;
        }

        // MU项目增加字段 [12/10/2013 LiaoWei]
        /// <summary>
        /// 被崇拜计数
        /// </summary>
        public int AdmiredCount
        {
            get;
            set;
        }

        // MU项目增加字段 [4/23/2014 LiaoWei]
        /// <summary>
        /// 消息推送ID
        /// </summary>
        public string PushMsgID
        {
            get;
            set;
        }

        // MU项目增加字段 [8/21/2014 LiaoWei]
        /// <summary>
        /// vip奖励领取标记
        /// </summary>
        public int VipAwardFlag
        {
            get;
            set;
        }

        /// <summary>
        /// VIP等级
        /// </summary>
        public int VIPLevel
        {
            get;
            set;
        }

        /// <summary>
        /// 角色的当前金币
        /// </summary>
        public long store_yinliang
        {
            get;
            set;
        }

        /// <summary>
        /// 角色的当前绑定金币
        /// </summary>
        public int store_money
        {
            get;
            set;
        }

        /// <summary>

        // 玩家充值/消费数值缓存
        private UserRankValueCache rankValue = new UserRankValueCache();

        /// <summary>
        /// 玩家领取奖励的状态
        /// </summary>
        public UserRankValueCache RankValue
        {
            get { return rankValue; }
            set { rankValue = value; }
        }

        #endregion 基本数据

        #region 扩展数据

        /// <summary>
        /// 角色参数表
        /// </summary>
        public ConcurrentDictionary<string, RoleParamsData> RoleParamsDict { get; set; } = new ConcurrentDictionary<string, RoleParamsData>();

        /// <summary>
        /// Danh sách nhiệm vụ đã hoàn thành
        /// </summary>
        public List<OldTaskData> OldTasks
        {
            get;
            set;
        }

        /// <summary>
        /// Danh sách nhiệm vụ
        /// </summary>
        public ConcurrentDictionary<int, TaskData> DoingTaskList
        {
            get;
            set;
        }

        /// <summary>
        /// Danh sách vật phẩm
        /// </summary>
        public ConcurrentDictionary<int, GoodsData> GoodsDataList
        {
            get;
            set;
        }

        /// <summary>
        /// 已经使用的物品限制列表
        /// </summary>
        public List<GoodsLimitData> GoodsLimitDataList
        {
            get;
            set;
        }

        /// <summary>
        /// Danh sách bạn bè
        /// </summary>
        public List<FriendData> FriendDataList
        {
            get;
            set;
        }

        /// <summary>
        /// Danh sách kỹ năng
        /// </summary>
        public ConcurrentDictionary<int, SkillData> SkillDataList
        {
            get;
            set;
        }

        /// <summary>
        /// Danh sách Buff
        /// </summary>
        public ConcurrentDictionary<int, BufferData> BufferDataList
        {
            get;
            set;
        }

        /// <summary>
        /// 跑环任务的数据
        /// </summary>
        public List<DailyTaskData> MyDailyTaskDataList
        {
            get;
            set;
        }

        /// <summary>
        /// 随身仓库数据
        /// </summary>
        public PortableBagData MyPortableBagData
        {
            get;
            set;
        }

        /// <summary>
        /// 活动送礼相关数据是否已经存在？
        /// </summary>
        public bool ExistsMyHuodongData
        {
            get;
            set;
        }

        /// <summary>
        /// 活动送礼相关数据
        /// </summary>
        public HuodongData MyHuodongData
        {
            get;
            set;
        }

        /// <summary>
        /// [bing] 结婚数据
        /// </summary>
        public MarriageData MyMarriageData
        {
            get;
            set;
        }

        public Dictionary<int, int> MyMarryPartyJoinList
        {
            get;
            set;
        }



        /// <summary>
        /// 角色每日数据
        /// </summary>
        public RoleDailyData MyRoleDailyData
        {
            get;
            set;
        }


        /// <summary>
        /// 上次使用访问的时间
        /// </summary>
        private long _LastReferenceTicks = DateTime.Now.Ticks / 10000;

        /// <summary>
        /// 上次使用访问的时间
        /// </summary>
        public long LastReferenceTicks
        {
            get { return _LastReferenceTicks; }
            set { _LastReferenceTicks = value; }
        }




        /// <summary>
        /// 最后一次登陆的IP
        /// </summary>
        public string LastIP
        {
            get;
            set;
        }

        public List<int> GroupMailRecordList
        {
            get;
            set;
        }

        /// <summary>
        ///  七日活动
        /// </summary>
        public Dictionary<int, Dictionary<int, SevenDayItemData>> SevenDayActDict
        {
            get;
            set;
        }

        /// <summary>
        /// 封停交易
        /// </summary>
        public long BanTradeToTicks
        {
            get;
            set;
        }

        /// <summary>
        /// 专享活动数据
        /// </summary>
        public Dictionary<int, SpecActInfoDB> SpecActInfoDict
        {
            get;
            set;
        }

        #endregion 扩展数据

        #region 从数据库查询信息

        /// <summary>
        /// 将数据库中获取的数据转换为角色数据
        /// </summary>
        /// <param name="dbRoleInfo"></param>
        /// <param name="cmd"></param>
        /// <param name="index"></param>
        public static void DBTableRow2RoleInfo(DBRoleInfo dbRoleInfo, MySQLSelectCommand cmd, int index)
        {
            dbRoleInfo.RoleID = Convert.ToInt32(cmd.Table.Rows[index]["rid"]);
            dbRoleInfo.UserID = cmd.Table.Rows[index]["userid"].ToString();
            dbRoleInfo.RoleName = cmd.Table.Rows[index]["rname"].ToString();
            dbRoleInfo.RoleSex = Convert.ToInt32(cmd.Table.Rows[index]["sex"]);
            dbRoleInfo.Occupation = Convert.ToInt32(cmd.Table.Rows[index]["occupation"]);
            dbRoleInfo.SubID = Convert.ToInt32(cmd.Table.Rows[index]["sub_id"]);
            dbRoleInfo.Level = Convert.ToInt32(cmd.Table.Rows[index]["level"]);
            dbRoleInfo.RolePic = Convert.ToInt32(cmd.Table.Rows[index]["pic"]);

            dbRoleInfo.Money1 = Convert.ToInt32(cmd.Table.Rows[index]["money1"]);
            dbRoleInfo.Money2 = Convert.ToInt32(cmd.Table.Rows[index]["money2"]);
            dbRoleInfo.Experience = Convert.ToInt64(cmd.Table.Rows[index]["experience"]);
            dbRoleInfo.PKMode = Convert.ToInt32(cmd.Table.Rows[index]["pkmode"]);
            dbRoleInfo.PKValue = Convert.ToInt32(cmd.Table.Rows[index]["pkvalue"]);
            dbRoleInfo.Position = cmd.Table.Rows[index]["position"].ToString();
            dbRoleInfo.RegTime = cmd.Table.Rows[index]["regtime"].ToString();
            dbRoleInfo.LastTime = DataHelper.ConvertToTicks(cmd.Table.Rows[index]["lasttime"].ToString());
            dbRoleInfo.BagNum = Convert.ToInt32(cmd.Table.Rows[index]["bagnum"]);

            dbRoleInfo.MainQuickBarKeys = cmd.Table.Rows[index]["main_quick_keys"].ToString();
            dbRoleInfo.OtherQuickBarKeys = cmd.Table.Rows[index]["other_quick_keys"].ToString();
            dbRoleInfo.LoginNum = Convert.ToInt32(cmd.Table.Rows[index]["loginnum"].ToString());
            dbRoleInfo.LeftFightSeconds = Convert.ToInt32(cmd.Table.Rows[index]["leftfightsecs"].ToString());
            dbRoleInfo.TotalOnlineSecs = Convert.ToInt32(cmd.Table.Rows[index]["totalonlinesecs"].ToString());
            dbRoleInfo.AntiAddictionSecs = Convert.ToInt32(cmd.Table.Rows[index]["antiaddictionsecs"].ToString());
            dbRoleInfo.LogOffTime = DataHelper.ConvertToTicks(cmd.Table.Rows[index]["logofftime"].ToString());

            dbRoleInfo.YinLiang = Convert.ToInt32(cmd.Table.Rows[index]["yinliang"].ToString());

            dbRoleInfo.MainTaskID = Convert.ToInt32(cmd.Table.Rows[index]["maintaskid"].ToString());
            dbRoleInfo.PKPoint = Convert.ToInt32(cmd.Table.Rows[index]["pkpoint"].ToString());

            dbRoleInfo.KillBoss = Convert.ToInt32(cmd.Table.Rows[index]["killboss"].ToString());

            dbRoleInfo.CZTaskID = Convert.ToInt32(cmd.Table.Rows[index]["cztaskid"].ToString());

            dbRoleInfo.LoginDayID = Convert.ToInt32(cmd.Table.Rows[index]["logindayid"].ToString());
            dbRoleInfo.LoginDayNum = Convert.ToInt32(cmd.Table.Rows[index]["logindaynum"].ToString());
            dbRoleInfo.ZoneID = Convert.ToInt32(cmd.Table.Rows[index]["zoneid"].ToString());

            dbRoleInfo.UserName = cmd.Table.Rows[index]["username"].ToString();
            dbRoleInfo.LastMailID = Convert.ToInt32(cmd.Table.Rows[index]["lastmailid"].ToString());
            dbRoleInfo.OnceAwardFlag = Convert.ToInt64(cmd.Table.Rows[index]["onceawardflag"].ToString());
            dbRoleInfo.Gold = Convert.ToInt32(cmd.Table.Rows[index]["money2"].ToString());
            dbRoleInfo.BanChat = Convert.ToInt32(cmd.Table.Rows[index]["banchat"].ToString());
            dbRoleInfo.BanLogin = Convert.ToInt32(cmd.Table.Rows[index]["banlogin"].ToString());
            dbRoleInfo.IsFlashPlayer = Convert.ToInt32(cmd.Table.Rows[index]["isflashplayer"].ToString());

            dbRoleInfo.AdmiredCount = Convert.ToInt32(cmd.Table.Rows[index]["admiredcount"].ToString());

            dbRoleInfo.store_yinliang = Convert.ToInt64(cmd.Table.Rows[index]["store_yinliang"]);
            dbRoleInfo.store_money = Convert.ToInt32(cmd.Table.Rows[index]["store_money"]);

            dbRoleInfo.BanTradeToTicks = Convert.ToInt64(cmd.Table.Rows[index]["ban_trade_to_ticks"].ToString());

            //KT ADD 8/9/2021

            dbRoleInfo.FamilyID = Convert.ToInt32(cmd.Table.Rows[index]["familyid"].ToString());
            dbRoleInfo.FamilyName = cmd.Table.Rows[index]["familyname"].ToString();
            dbRoleInfo.FamilyRank = Convert.ToInt32(cmd.Table.Rows[index]["familyrank"].ToString());

            dbRoleInfo.Prestige = Convert.ToInt32(cmd.Table.Rows[index]["roleprestige"].ToString());


            dbRoleInfo.RoleGuildMoney = Convert.ToInt32(cmd.Table.Rows[index]["guildmoney"]);
            dbRoleInfo.GuildRank = Convert.ToInt32(cmd.Table.Rows[index]["guildrank"].ToString());
            dbRoleInfo.GuildID = Convert.ToInt32(cmd.Table.Rows[index]["guildid"]);
            dbRoleInfo.GuildName = cmd.Table.Rows[index]["guildname"].ToString();
        }

        /// <summary>
        /// 将数据库中获取的数据转换为角色数据_角色参数表
        /// </summary>
        /// <param name="dbRoleInfo"></param>
        /// <param name="cmd"></param>
        /// <param name="index"></param>
        /// <param name="normalOnly">是否总是保存在t_roleparams常规表中</param>
        public static void DBTableRow2RoleInfo_Params(DBRoleInfo dbRoleInfo, MySQLSelectCommand cmd, bool normalOnly)
        {
            if (cmd.Table.Rows.Count > 0)
            {
                ConcurrentDictionary<string, RoleParamsData> dict = dbRoleInfo.RoleParamsDict;
                for (int i = 0; i < cmd.Table.Rows.Count; i++)
                {
                    RoleParamsData roleParamsData = new RoleParamsData()
                    {
                        ParamName = cmd.Table.Rows[i]["pname"].ToString(),
                        ParamValue = cmd.Table.Rows[i]["pvalue"].ToString(),
                    };

                    roleParamsData.ParamType = RoleParamNameInfo.GetRoleParamType(roleParamsData.ParamName, roleParamsData.ParamValue);
                    if (roleParamsData.ParamType.Type > 0 && normalOnly)
                    {
                        continue;
                    }

                    dict[roleParamsData.ParamName] = roleParamsData;
                }
            }
        }

        /// <summary>
        /// Truy vấn tên nhân vật theo ID
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="roleID"></param>
        public static string QueryRoleNameByRoleID(DBManager dbManager, int roleID)
        {
            /// Tên nhân vật tương ứng
            string roleName = "";

            /// Thông tin nhân vật được cache
            DBRoleInfo roleInfo = dbManager.GetDBRoleInfo(roleID);
            /// Nếu đã được Cache
            if (roleInfo != null)
            {
                roleName = roleInfo.RoleName;
            }
            /// Nếu chưa được Cache
            else
            {
                MySQLConnection conn = null;

                try
                {
                    conn = dbManager.DBConns.PopDBConnection();
                    string queryString = string.Format("SELECT rname FROM t_roles WHERE rid = {0}", roleID);

                    MySQLCommand cmd = new MySQLCommand(queryString, conn);
                    MySQLDataReader reader = cmd.ExecuteReaderEx();

                    if (reader.Read())
                    {
                        roleName = reader["rname"].ToString();
                    }

                    cmd.Dispose();
                    cmd = null;
                }
                catch (Exception ex)
                {
                    LogManager.WriteLog(LogTypes.TeamBattle, ex.ToString());
                }
                finally
                {
                    if (null != conn)
                    {
                        dbManager.DBConns.PushDBConnection(conn);
                    }
                }
            }

            /// Trả về kết quả
            return roleName;
        }

        public static void DBTableRow2RoleInfo_ParamsEx(DBRoleInfo dbRoleInfo, int roleId)
        {
            using (MyDbConnection3 conn = new MyDbConnection3())
            {
                string cmdText = string.Format("select * from t_roleparams_long where rid={0};", roleId);
                DataTable dataTable = conn.ExecuteReader(cmdText).GetSchemaTable();
                if (dataTable.Rows.Count > 0)
                {
                    ConcurrentDictionary<string, RoleParamsData> dict = dbRoleInfo.RoleParamsDict;
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        DataRow dataRow = dataTable.Rows[i];
                        int idx = Convert.ToInt32(dataRow["idx"].ToString());
                        int columnCount = dataRow.ItemArray.Length;
                        for (int columnIndex = 2; columnIndex < columnCount; columnIndex++)
                        {
                            RoleParamType roleParamType = RoleParamNameInfo.GetRoleParamType(idx, columnIndex - 2);
                            if (null != roleParamType)
                            {
                                RoleParamsData roleParamsData = new RoleParamsData()
                                {
                                    ParamName = roleParamType.ParamName,
                                    ParamValue = dataRow[columnIndex].ToString(),
                                    ParamType = roleParamType,
                                };

                                dict[roleParamsData.ParamName] = roleParamsData;
                            }
                        }
                    }
                }

                cmdText = string.Format("select * from t_roleparams_char where rid={0};", roleId);
                dataTable = conn.ExecuteReader(cmdText).GetSchemaTable();
                if (dataTable.Rows.Count > 0)
                {
                    ConcurrentDictionary<string, RoleParamsData> dict = dbRoleInfo.RoleParamsDict;
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        DataRow dataRow = dataTable.Rows[i];
                        int idx = Convert.ToInt32(dataRow["idx"].ToString());
                        int columnCount = dataRow.ItemArray.Length;
                        for (int columnIndex = 2; columnIndex < columnCount; columnIndex++)
                        {
                            RoleParamType roleParamType = RoleParamNameInfo.GetRoleParamType(idx, columnIndex - 2);
                            if (null != roleParamType)
                            {
                                RoleParamsData roleParamsData = new RoleParamsData()
                                {
                                    ParamName = roleParamType.ParamName,
                                    ParamValue = dataRow[columnIndex].ToString(),
                                    ParamType = roleParamType,
                                };

                                dict[roleParamsData.ParamName] = roleParamsData;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 将数据库中获取的数据转换为角色数据_旧任务数据
        /// </summary>
        /// <param name="dbRoleInfo"></param>
        /// <param name="cmd"></param>
        /// <param name="index"></param>
        public static void DBTableRow2RoleInfo_OldTasks(DBRoleInfo dbRoleInfo, MySQLSelectCommand cmd)
        {
            if (cmd.Table.Rows.Count > 0)
            {
                List<OldTaskData> oldTasks = new List<OldTaskData>(cmd.Table.Rows.Count);
                for (int i = 0; i < cmd.Table.Rows.Count; i++)
                {
                    string TaskClass = cmd.Table.Rows[i]["taskclass"].ToString();

                    int taskint = 0;

                    if (TaskClass.Length > 0)
                    {
                        taskint = Convert.ToInt32(cmd.Table.Rows[i]["taskclass"].ToString());
                    }

                    oldTasks.Add(new OldTaskData()
                    {
                        TaskID = Convert.ToInt32(cmd.Table.Rows[i]["taskid"].ToString()),
                        DoCount = Convert.ToInt32(cmd.Table.Rows[i]["count"].ToString()),
                        TaskClass = taskint,
                    });
                }

                dbRoleInfo.OldTasks = oldTasks;
            }
        }

        /// <summary>
        /// 将数据库中获取的数据转换为角色数据_正在做任务数据
        /// </summary>
        /// <param name="dbRoleInfo"></param>
        /// <param name="cmd"></param>
        /// <param name="index"></param>
        public static void DBTableRow2RoleInfo_DoingTasks(DBRoleInfo dbRoleInfo, MySQLSelectCommand cmd)
        {
            if (cmd.Table.Rows.Count > 0)
            {
                dbRoleInfo.DoingTaskList = new ConcurrentDictionary<int, TaskData>();
                for (int i = 0; i < cmd.Table.Rows.Count; i++)
                {
                    TaskData task = new TaskData()
                    {
                        DbID = Convert.ToInt32(cmd.Table.Rows[i]["id"].ToString()),
                        DoingTaskID = Convert.ToInt32(cmd.Table.Rows[i]["taskid"].ToString()),
                        DoingTaskVal1 = Convert.ToInt32(cmd.Table.Rows[i]["value1"].ToString()),
                        DoingTaskVal2 = Convert.ToInt32(cmd.Table.Rows[i]["value2"].ToString()),
                        DoingTaskFocus = Convert.ToInt32(cmd.Table.Rows[i]["focus"].ToString()),
                        AddDateTime = DataHelper.ConvertToTicks(cmd.Table.Rows[i]["addtime"].ToString()),
                        StarLevel = Convert.ToInt32(cmd.Table.Rows[i]["starlevel"].ToString()),
                    };
                    dbRoleInfo.DoingTaskList[task.DbID] = task;
                }
            }
        }

        /// <summary>
        /// Đọc toàn bộ GOODs CỦA NGƯỜI CHƠI LƯU RA LIST
        /// </summary>
        /// <param name="dbRoleInfo"></param>
        /// <param name="cmd"></param>
        /// <param name="index"></param>
        public static void DBTableRow2RoleInfo_Goods(DBRoleInfo dbRoleInfo, MySQLSelectCommand cmd)
        {
            if (cmd.Table.Rows.Count > 0)
            {
                dbRoleInfo.GoodsDataList = new ConcurrentDictionary<int, GoodsData>();
                for (int i = 0; i < cmd.Table.Rows.Count; i++)
                {
                    string otherPramenter = cmd.Table.Rows[i]["otherpramer"].ToString();

                    byte[] Base64Decode = Convert.FromBase64String(otherPramenter);

                    Dictionary<ItemPramenter, string> _OtherParams = DataHelper.BytesToObject<Dictionary<ItemPramenter, string>>(Base64Decode, 0, Base64Decode.Length);

                    GoodsData goodsData = new GoodsData()
                    {
                        Id = Convert.ToInt32(cmd.Table.Rows[i]["Id"].ToString()),
                        GoodsID = Convert.ToInt32(cmd.Table.Rows[i]["goodsid"].ToString()),
                        Using = Convert.ToInt32(cmd.Table.Rows[i]["isusing"].ToString()),
                        Forge_level = Convert.ToInt32(cmd.Table.Rows[i]["forge_level"].ToString()),
                        Starttime = cmd.Table.Rows[i]["starttime"].ToString(),
                        Endtime = cmd.Table.Rows[i]["endtime"].ToString(),
                        Site = Convert.ToInt32(cmd.Table.Rows[i]["site"].ToString()),
                        Props = cmd.Table.Rows[i]["Props"].ToString(),
                        GCount = Convert.ToInt32(cmd.Table.Rows[i]["gcount"].ToString()),
                        Binding = Convert.ToInt32(cmd.Table.Rows[i]["binding"].ToString()),
                        BagIndex = Convert.ToInt32(cmd.Table.Rows[i]["bagindex"].ToString()),
                        Strong = Convert.ToInt32(cmd.Table.Rows[i]["strong"].ToString()),
                        Series = Convert.ToInt32(cmd.Table.Rows[i]["series"].ToString()),
                        OtherParams = _OtherParams

                        //TODO : ĐỌC NGŨ HÀNH + TIỀN GIAO BÁN LƯU RA
                    };

                    dbRoleInfo.GoodsDataList[goodsData.Id] = goodsData;
                }
            }
        }

        /// <summary>
        /// 将数据库中获取的数据转换为角色数据_物品限制数据
        /// </summary>
        /// <param name="dbRoleInfo"></param>
        /// <param name="cmd"></param>
        /// <param name="index"></param>
        public static void DBTableRow2RoleInfo_GoodsLimit(DBRoleInfo dbRoleInfo, MySQLSelectCommand cmd)
        {
            if (cmd.Table.Rows.Count > 0)
            {
                dbRoleInfo.GoodsLimitDataList = new List<GoodsLimitData>();
                for (int i = 0; i < cmd.Table.Rows.Count; i++)
                {
                    dbRoleInfo.GoodsLimitDataList.Add(new GoodsLimitData()
                    {
                        GoodsID = Convert.ToInt32(cmd.Table.Rows[i]["goodsid"].ToString()),
                        DayID = Convert.ToInt32(cmd.Table.Rows[i]["dayid"].ToString()),
                        UsedNum = Convert.ToInt32(cmd.Table.Rows[i]["usednum"].ToString()),
                    });
                }
            }
        }

        /*
        /// <summary>
        /// 将数据库中获取的数据转换为角色数据_好友数据
        /// </summary>
        /// <param name="dbRoleInfo"></param>
        /// <param name="cmd"></param>
        /// <param name="index"></param>
        public static void DBTableRow2RoleInfo_Friends(DBRoleInfo dbRoleInfo, MySQLSelectCommand cmd)
        {
            if (cmd.Table.Rows.Count > 0)
            {
                dbRoleInfo.FriendDataList = new List<FriendData>();
                for (int i = 0; i < cmd.Table.Rows.Count; i++)
                {
                    dbRoleInfo.FriendDataList.Add(new FriendData()
                    {
                        DbID = Convert.ToInt32(cmd.Table.Rows[i]["Id"].ToString()),
                        OtherRoleID = Convert.ToInt32(cmd.Table.Rows[i]["otherid"].ToString()),
                        FriendType = Convert.ToInt32(cmd.Table.Rows[i]["friendType"].ToString()),
                        Relationship = Convert.ToInt32(cmd.Table.Rows[i]["relationship"].ToString()),
                    });
                }
            }
        }
        */

        /// <summary>
        /// Đọc dữ liệu kỹ năng
        /// </summary>
        /// <param name="dbRoleInfo"></param>
        /// <param name="cmd"></param>
        /// <param name="index"></param>
        public static void DBTableRow2RoleInfo_Skills(DBRoleInfo dbRoleInfo, MySQLSelectCommand cmd)
        {
            if (cmd.Table.Rows.Count > 0)
            {
                dbRoleInfo.SkillDataList = new ConcurrentDictionary<int, SkillData>();
                for (int i = 0; i < cmd.Table.Rows.Count; i++)
                {
                    SkillData skillData = new SkillData()
                    {
                        DbID = Convert.ToInt32(cmd.Table.Rows[i]["Id"].ToString()),
                        SkillID = Convert.ToInt32(cmd.Table.Rows[i]["skillid"].ToString()),
                        SkillLevel = Convert.ToInt32(cmd.Table.Rows[i]["skilllevel"].ToString()),
                        LastUsedTick = Convert.ToInt64(cmd.Table.Rows[i]["lastusedtick"].ToString()),
                        Cooldown = Convert.ToInt32(cmd.Table.Rows[i]["cooldowntick"].ToString()),
                        Exp = Convert.ToInt32(cmd.Table.Rows[i]["exp"].ToString()),
                    };
                    dbRoleInfo.SkillDataList[skillData.SkillID] = skillData;
                }
            }
        }

        /// <summary>
        /// 将数据库中获取的数据转换为角色数据_Buffer数据
        /// </summary>
        /// <param name="dbRoleInfo"></param>
        /// <param name="cmd"></param>
        /// <param name="index"></param>
        public static void DBTableRow2RoleInfo_Buffers(DBRoleInfo dbRoleInfo, MySQLSelectCommand cmd)
        {
            if (cmd.Table.Rows.Count > 0)
            {
                dbRoleInfo.BufferDataList = new ConcurrentDictionary<int, BufferData>();
                for (int i = 0; i < cmd.Table.Rows.Count; i++)
                {
                    BufferData buff = new BufferData()
                    {
                        BufferID = Convert.ToInt32(cmd.Table.Rows[i]["bufferid"].ToString()),
                        StartTime = Convert.ToInt64(cmd.Table.Rows[i]["starttime"].ToString()),
                        BufferSecs = Convert.ToInt64(cmd.Table.Rows[i]["buffersecs"].ToString()),
                        CustomProperty = cmd.Table.Rows[i]["custom_property"].ToString(),
                        BufferVal = Convert.ToInt64(cmd.Table.Rows[i]["bufferval"].ToString()),
                        BufferType = 0,
                    };
                    dbRoleInfo.BufferDataList[buff.BufferID] = buff;
                }
            }
        }

        /// <summary>
        /// 将数据库中获取的数据转换为角色数据_跑环任务数据
        /// </summary>
        /// <param name="dbRoleInfo"></param>
        /// <param name="cmd"></param>
        /// <param name="index"></param>
        public static void DBTableRow2RoleInfo_DailyTasks(DBRoleInfo dbRoleInfo, MySQLSelectCommand cmd)
        {
            if (null == dbRoleInfo.MyDailyTaskDataList)
            {
                dbRoleInfo.MyDailyTaskDataList = new List<DailyTaskData>();
            }

            for (int i = 0; i < cmd.Table.Rows.Count; i++)
            {
                DailyTaskData dailyTaskData = new DailyTaskData()
                {
                    HuanID = Convert.ToInt32(cmd.Table.Rows[i]["huanid"].ToString()),
                    RecTime = cmd.Table.Rows[i]["rectime"].ToString(),
                    RecNum = Convert.ToInt32(cmd.Table.Rows[i]["recnum"].ToString()),
                    TaskClass = Convert.ToInt32(cmd.Table.Rows[i]["taskClass"].ToString()),
                    ExtDayID = Convert.ToInt32(cmd.Table.Rows[i]["extdayid"].ToString()),
                    ExtNum = Convert.ToInt32(cmd.Table.Rows[i]["extnum"].ToString()),
                };

                dbRoleInfo.MyDailyTaskDataList.Add(dailyTaskData);
            }
        }

        /// <summary>
        /// 将数据库中获取的数据转换为角色数据_随身仓库数据
        /// </summary>
        /// <param name="dbRoleInfo"></param>
        /// <param name="cmd"></param>
        /// <param name="index"></param>
        public static void DBTableRow2RoleInfo_PortableBag(DBRoleInfo dbRoleInfo, MySQLSelectCommand cmd)
        {
            dbRoleInfo.MyPortableBagData = new PortableBagData()
            {
                GoodsUsedGridNum = 0,
            };

            if (cmd.Table.Rows.Count > 0)
            {
                dbRoleInfo.MyPortableBagData.ExtGridNum = Convert.ToInt32(cmd.Table.Rows[0]["extgridnum"].ToString());
            }
        }

        /// <summary>
        /// 将数据库中获取的数据转换为角色数据_送礼活动数据
        /// </summary>
        /// <param name="dbRoleInfo"></param>
        /// <param name="cmd"></param>
        /// <param name="index"></param>
        public static void DBTableRow2RoleInfo_HuodongData(DBRoleInfo dbRoleInfo, MySQLSelectCommand cmd)
        {
            dbRoleInfo.ExistsMyHuodongData = false;
            dbRoleInfo.MyHuodongData = new HuodongData();

            if (cmd.Table.Rows.Count > 0)
            {
                dbRoleInfo.ExistsMyHuodongData = true;

                dbRoleInfo.MyHuodongData.LastWeekID = cmd.Table.Rows[0]["loginweekid"].ToString();
                dbRoleInfo.MyHuodongData.LastDayID = cmd.Table.Rows[0]["logindayid"].ToString();
                dbRoleInfo.MyHuodongData.LoginNum = Convert.ToInt32(cmd.Table.Rows[0]["loginnum"].ToString());
                dbRoleInfo.MyHuodongData.NewStep = Convert.ToInt32(cmd.Table.Rows[0]["newstep"].ToString());
                dbRoleInfo.MyHuodongData.StepTime = DataHelper.ConvertToTicks(cmd.Table.Rows[0]["steptime"].ToString());
                dbRoleInfo.MyHuodongData.LastMTime = Convert.ToInt32(cmd.Table.Rows[0]["lastmtime"].ToString());
                dbRoleInfo.MyHuodongData.CurMID = cmd.Table.Rows[0]["curmid"].ToString();
                dbRoleInfo.MyHuodongData.CurMTime = Convert.ToInt32(cmd.Table.Rows[0]["curmtime"].ToString());
                dbRoleInfo.MyHuodongData.SongLiID = Convert.ToInt32(cmd.Table.Rows[0]["songliid"].ToString());
                dbRoleInfo.MyHuodongData.LoginGiftState = Convert.ToInt32(cmd.Table.Rows[0]["logingiftstate"].ToString());
                dbRoleInfo.MyHuodongData.OnlineGiftState = Convert.ToInt32(cmd.Table.Rows[0]["onlinegiftstate"].ToString());
                dbRoleInfo.MyHuodongData.LastLimitTimeHuoDongID = Convert.ToInt32(cmd.Table.Rows[0]["lastlimittimehuodongid"].ToString());
                dbRoleInfo.MyHuodongData.LastLimitTimeDayID = Convert.ToInt32(cmd.Table.Rows[0]["lastlimittimedayid"].ToString());
                dbRoleInfo.MyHuodongData.LimitTimeLoginNum = Convert.ToInt32(cmd.Table.Rows[0]["limittimeloginnum"].ToString());
                dbRoleInfo.MyHuodongData.LimitTimeGiftState = Convert.ToInt32(cmd.Table.Rows[0]["limittimegiftstate"].ToString());
                dbRoleInfo.MyHuodongData.EveryDayOnLineAwardStep = Convert.ToInt32(cmd.Table.Rows[0]["everydayonlineawardstep"].ToString());
                dbRoleInfo.MyHuodongData.GetEveryDayOnLineAwardDayID = Convert.ToInt32(cmd.Table.Rows[0]["geteverydayonlineawarddayid"].ToString());
                dbRoleInfo.MyHuodongData.SeriesLoginGetAwardStep = Convert.ToInt32(cmd.Table.Rows[0]["serieslogingetawardstep"].ToString());
                dbRoleInfo.MyHuodongData.SeriesLoginAwardDayID = Convert.ToInt32(cmd.Table.Rows[0]["seriesloginawarddayid"].ToString());
                dbRoleInfo.MyHuodongData.SeriesLoginAwardGoodsID = cmd.Table.Rows[0]["seriesloginawardgoodsid"].ToString();
                dbRoleInfo.MyHuodongData.EveryDayOnLineAwardGoodsID = cmd.Table.Rows[0]["everydayonlineawardgoodsid"].ToString();
            }
        }



        /// <summary>
        /// Truy vấn danh sách bạn bè
        /// </summary>
        /// <param name="dbRoleInfo"></param>
        /// <param name="roleID"></param>
        public static void DBTableRow2RoleInfo_Friends(DBRoleInfo dbRoleInfo, int roleID)
        {
            dbRoleInfo.FriendDataList = new List<FriendData>();
            string str = string.Format("SELECT Id, myid, otherid, relationship, friendType FROM t_friends WHERE myid = {0} OR otherid = {1}", roleID, roleID);
            GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", str), EventLevels.Important);
            MySQLConnection connection = DBManager.getInstance().DBConns.PopDBConnection();
            try
            {
                MySQLCommand command = new MySQLCommand(str, connection);
                MySQLDataReader reader = command.ExecuteReaderEx();
                while (reader.Read())
                {
                    int myID = Convert.ToInt32(reader["myid"].ToString());
                    int otherID = Convert.ToInt32(reader["otherid"].ToString());

                    /// Nếu bản thân là myid
                    if (roleID == myID)
                    {
                        dbRoleInfo.FriendDataList.Add(new FriendData()
                        {
                            DbID = Convert.ToInt32(reader["Id"].ToString()),
                            OtherRoleID = otherID,
                            Relationship = Convert.ToInt32(reader["relationship"].ToString()),
                            FriendType = Convert.ToInt32(reader["friendType"].ToString()),
                        });
                    }
                    /// Nếu bản thân là otherid
                    else
                    {
                        dbRoleInfo.FriendDataList.Add(new FriendData()
                        {
                            DbID = Convert.ToInt32(reader["Id"].ToString()),
                            OtherRoleID = myID,
                            Relationship = Convert.ToInt32(reader["relationship"].ToString()),
                            FriendType = Convert.ToInt32(reader["friendType"].ToString()),
                        });
                    }
                }
                command.Dispose();
                command = null;
            }
            finally
            {
                DBManager.getInstance().DBConns.PushDBConnection(connection);
            }
        }

        /// <summary>
        /// [bing] 将数据库中获取的数据转换为角色数据_结婚数据
        /// </summary>
        /// <param name="dbRoleInfo"></param>
        /// <param name="cmd"></param>
        /// <param name="index"></param>
        public static void DBTableRow2RoleInfo_MarriageData(DBRoleInfo dbRoleInfo, MySQLSelectCommand cmd)
        {
            if (cmd.Table.Rows.Count > 0)
            {
                dbRoleInfo.MyMarriageData = new MarriageData()
                {
                    nSpouseID = Convert.ToInt32(cmd.Table.Rows[0]["spouseid"].ToString()),
                    byMarrytype = Convert.ToSByte(cmd.Table.Rows[0]["marrytype"].ToString()),
                    nRingID = Convert.ToInt32(cmd.Table.Rows[0]["ringid"].ToString()),
                    nGoodwillexp = Convert.ToInt32(cmd.Table.Rows[0]["goodwillexp"].ToString()),
                    byGoodwillstar = Convert.ToSByte(cmd.Table.Rows[0]["goodwillstar"].ToString()),
                    byGoodwilllevel = Convert.ToSByte(cmd.Table.Rows[0]["goodwilllevel"].ToString()),
                    nGivenrose = Convert.ToInt32(cmd.Table.Rows[0]["givenrose"].ToString()),
                    strLovemessage = cmd.Table.Rows[0]["lovemessage"].ToString(),
                    byAutoReject = Convert.ToSByte(cmd.Table.Rows[0]["autoreject"].ToString()),
                    ChangTime = cmd.Table.Rows[0]["changtime"].ToString(),
                };
            }
            else
            {
                //[bing] 应该无论如何也会创建MyMarriageData
                dbRoleInfo.MyMarriageData = new MarriageData();
            }
        }

        public static void DBTableRow2RoleInfo_MarryPartyJoinList(DBRoleInfo dbRoleInfo, MySQLSelectCommand cmd)
        {
            dbRoleInfo.MyMarryPartyJoinList = new Dictionary<int, int>();

            if (cmd.Table.Rows.Count > 0)
            {
                for (int i = 0; i < cmd.Table.Rows.Count; i++)
                {
                    dbRoleInfo.MyMarryPartyJoinList.Add(
                        Convert.ToInt32(cmd.Table.Rows[i]["partyroleid"].ToString()),
                        Convert.ToInt32(cmd.Table.Rows[i]["joincount"].ToString())
                        );
                }
            }
        }

        /// <summary>
        /// 将数据库中获取的数据转换为角色数据_日常数据
        /// </summary>
        /// <param name="dbRoleInfo"></param>
        /// <param name="cmd"></param>
        /// <param name="index"></param>
        public static void DBTableRow2RoleInfo_DailyData(DBRoleInfo dbRoleInfo, MySQLSelectCommand cmd)
        {
            if (cmd.Table.Rows.Count > 0)
            {
                dbRoleInfo.MyRoleDailyData = new RoleDailyData()
                {
                    ExpDayID = Convert.ToInt32(cmd.Table.Rows[0]["expdayid"].ToString()),
                    TodayExp = Convert.ToInt32(cmd.Table.Rows[0]["todayexp"].ToString()),
                    LingLiDayID = Convert.ToInt32(cmd.Table.Rows[0]["linglidayid"].ToString()),
                    TodayLingLi = Convert.ToInt32(cmd.Table.Rows[0]["todaylingli"].ToString()),
                    KillBossDayID = Convert.ToInt32(cmd.Table.Rows[0]["killbossdayid"].ToString()),
                    TodayKillBoss = Convert.ToInt32(cmd.Table.Rows[0]["todaykillboss"].ToString()),
                    FuBenDayID = Convert.ToInt32(cmd.Table.Rows[0]["fubendayid"].ToString()),
                    TodayFuBenNum = Convert.ToInt32(cmd.Table.Rows[0]["todayfubennum"].ToString()),
                    WuXingDayID = Convert.ToInt32(cmd.Table.Rows[0]["wuxingdayid"].ToString()),
                    WuXingNum = Convert.ToInt32(cmd.Table.Rows[0]["wuxingnum"].ToString()),
                };
            }
        }








        /// <summary>
        /// 根据角色名查询角色ID
        /// 不要根据这个方法检查角色名是否存在
        /// </summary>
        public static int QueryRoleID_ByRolename(MySQLConnection conn, String strRoleName)
        {
            List<Tuple<int, string>> idList = QueryRoleIdList_ByRolename_IgnoreDbCmp(conn, strRoleName);

            int roleId = -1;
            if (idList != null)
            {
                var tuple = idList.Find(_t => _t.Item2 == strRoleName);
                roleId = tuple != null ? tuple.Item1 : -1;
            }

            return roleId;
        }

        /// <summary>
        /// 查询名字对应的一系列角色id ！！！
        /// 因为现在数据库比较名字未区分大小写，风轻云淡 和 风清云淡 在数据库比较时竟然一样！
        /// 所以查询名字的时候把能查出来的角色id都列出来
        /// </summary>
        public static List<Tuple<int, string>> QueryRoleIdList_ByRolename_IgnoreDbCmp(MySQLConnection conn, string rolename)
        {
            List<Tuple<int, string>> resultList = new List<Tuple<int, string>>();

            string sql = string.Format("SELECT rid,rname FROM t_roles where rname='{0}'", rolename);
            GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", sql), EventLevels.Important);
            MySQLCommand cmd = new MySQLCommand(sql, conn);

            MySQLDataReader reader = cmd.ExecuteReaderEx();
            while (reader.Read())
            {
                int oneRoleId = Convert.ToInt32(reader["rid"].ToString());
                string oneRolename = reader["rname"].ToString();

                resultList.Add(new Tuple<int, string>(oneRoleId, oneRolename));
            }

            cmd.Dispose();
            cmd = null;


            return resultList;
        }

        /// <summary>
        /// 角色群邮件记录信息 [5/17/2014 LiaoWei]
        /// </summary>
        /// <param name="dbRoleInfo"></param>
        /// <param name="cmd"></param>
        /// <param name="index"></param>
        public static void DBTableRow2RoleInfo_GMailInfo(DBRoleInfo dbRoleInfo, MySQLSelectCommand cmd)
        {
            if (cmd.Table.Rows.Count > 0)
            {
                dbRoleInfo.GroupMailRecordList = new List<int>();
                for (int i = 0; i < cmd.Table.Rows.Count; i++)
                {
                    dbRoleInfo.GroupMailRecordList.Add(Convert.ToInt32(cmd.Table.Rows[i]["gmailid"].ToString()));
                }
            }
        }


        /// <summary>
        /// 将数据库中获取的数据转换为角色数据_七日活动数据
        /// </summary>
        /// <param name="dbRoleInfo"></param>
        /// <param name="cmd"></param>
        /// <param name="index"></param>
        public static void DBTableRow2RoleInfo_SevenDayActData(DBRoleInfo dbRoleInfo, MySQLSelectCommand cmd)
        {
            dbRoleInfo.SevenDayActDict = new Dictionary<int, Dictionary<int, SevenDayItemData>>();

            if (cmd.Table.Rows.Count > 0)
            {
                for (int i = 0; i < cmd.Table.Rows.Count; i++)
                {
                    SevenDayItemData itemData = new SevenDayItemData();
                    itemData.AwardFlag = Convert.ToInt32(cmd.Table.Rows[i]["award_flag"].ToString());
                    itemData.Params1 = Convert.ToInt32(cmd.Table.Rows[i]["param1"].ToString());
                    itemData.Params2 = Convert.ToInt32(cmd.Table.Rows[i]["param2"].ToString());

                    int roleid = Convert.ToInt32(cmd.Table.Rows[i]["roleid"].ToString());
                    int actType = Convert.ToInt32(cmd.Table.Rows[i]["act_type"].ToString());
                    int id = Convert.ToInt32(cmd.Table.Rows[i]["id"].ToString());

                    Dictionary<int, SevenDayItemData> itemDict = null;
                    if (!dbRoleInfo.SevenDayActDict.TryGetValue(actType, out itemDict))
                    {
                        itemDict = new Dictionary<int, SevenDayItemData>();
                        dbRoleInfo.SevenDayActDict[actType] = itemDict;
                    }
                    itemDict[id] = itemData;
                }
            }
        }

        /// <summary>
        /// 将数据库中获取的数据转换为角色数据_七日活动数据
        /// </summary>
        /// <param name="dbRoleInfo"></param>
        /// <param name="cmd"></param>
        public static void DBTableRow2RoleInfo_SpecialActivityData(DBRoleInfo dbRoleInfo, MySQLSelectCommand cmd)
        {
            if (cmd.Table.Rows.Count > 0)
            {
                dbRoleInfo.SpecActInfoDict = new Dictionary<int, SpecActInfoDB>();
                for (int i = 0; i < cmd.Table.Rows.Count; i++)
                {
                    SpecActInfoDB itemData = new SpecActInfoDB();
                    itemData.GroupID = Convert.ToInt32(cmd.Table.Rows[i]["groupid"].ToString());
                    itemData.ActID = Convert.ToInt32(cmd.Table.Rows[i]["actid"].ToString());
                    itemData.PurNum = Convert.ToInt32(cmd.Table.Rows[i]["purchaseNum"].ToString());
                    itemData.CountNum = Convert.ToInt32(cmd.Table.Rows[i]["countNum"].ToString());
                    itemData.Active = Convert.ToInt16(cmd.Table.Rows[i]["active"].ToString());

                    dbRoleInfo.SpecActInfoDict[itemData.ActID] = itemData;
                }
            }
        }

        /// <summary>
        /// Truy vấn thông tin nhân vật từ DB
        /// </summary>
        /// <param name="bUseIsdel">Có phải nhân vật chưa bị xóa không</param>
        public bool Query(MySQLConnection conn, int roleID, bool bUseIsdel = true)
        {
            LogManager.WriteLog(LogTypes.Info, string.Format("Load role data from DB: {0}", roleID));

            MySQLSelectCommand cmd = null;

            if (bUseIsdel)
            {
                cmd = new MySQLSelectCommand(conn,
                     new string[] { "rid", "userid", "rname", "sex", "occupation", "sub_id", "level", "pic", "money1", "money2", "experience", "pkmode", "pkvalue", "position", "regtime", "lasttime", "bagnum", "main_quick_keys", "other_quick_keys", "loginnum", "leftfightsecs", "totalonlinesecs", "antiaddictionsecs", "logofftime", "yinliang", "maintaskid", "pkpoint", "killboss", "cztaskid", "logindayid", "logindaynum", "zoneid", "guildname", "guildrank", "guildid", "guildmoney", "username", "lastmailid", "onceawardflag", "banchat", "banlogin", "isflashplayer", "admiredcount", "store_yinliang", "store_money", "ban_trade_to_ticks", "familyid", "familyname", "familyrank", "roleprestige" },
                     new string[] { "t_roles" }, new object[,] { { "rid", "=", roleID }, { "isdel", "=", 0 } }, null, new string[,] { { "level", "desc" } }, true, 0, 4, false);
            }
            else
            {
                cmd = new MySQLSelectCommand(conn,
                    new string[] { "rid", "userid", "rname", "sex", "occupation", "sub_id", "level", "pic", "money1", "money2", "experience", "pkmode", "pkvalue", "position", "regtime", "lasttime", "bagnum", "main_quick_keys", "other_quick_keys", "loginnum", "leftfightsecs", "totalonlinesecs", "antiaddictionsecs", "logofftime", "yinliang", "maintaskid", "pkpoint", "killboss", "cztaskid", "logindayid", "logindaynum", "zoneid", "guildname", "guildrank", "guildid", "guildmoney", "username", "lastmailid", "onceawardflag", "banchat", "banlogin", "isflashplayer", "admiredcount", "store_yinliang", "store_money", "ban_trade_to_ticks", "familyid", "familyname", "familyrank", "roleprestige" },
                     new string[] { "t_roles" }, new object[,] { { "rid", "=", roleID } }, null, new string[,] { { "level", "desc" } }, true, 0, 4, false);
            }

            if (cmd.Table.Rows.Count <= 0)
            {
                return false;
            }


            DBRoleInfo.DBTableRow2RoleInfo(this, cmd, 0);

            if (GameDBManager.Flag_Splite_RoleParams_Table == 0)
            {
                //查询已经完成的任务信息
                cmd = new MySQLSelectCommand(conn,
                     new string[] { "pname", "pvalue" },
                     new string[] { "t_roleparams" }, new object[,] { { "rid", "=", roleID } }, null, null);

                /// 将数据库中获取的数据转换为角色数据_角色参数表
                DBRoleInfo.DBTableRow2RoleInfo_Params(this, cmd, true);
            }
            else
            {
                //查询已经完成的任务信息
                cmd = new MySQLSelectCommand(conn,
                     new string[] { "pname", "pvalue" },
                     new string[] { "t_roleparams_2" }, new object[,] { { "rid", "=", roleID } }, null, null);

                /// 将数据库中获取的数据转换为角色数据_角色参数表
                DBRoleInfo.DBTableRow2RoleInfo_Params(this, cmd, false);

                /// 将数据库中获取的数据转换为角色数据_角色参数表
                DBRoleInfo.DBTableRow2RoleInfo_ParamsEx(this, roleID);
            }

            //查询已经完成的任务信息
            cmd = new MySQLSelectCommand(conn,
                 new string[] { "rid", "taskid", "count", "taskclass" },
                 new string[] { "t_taskslog" }, new object[,] { { "rid", "=", roleID } }, null, null);

            /// 将数据库中获取的数据转换为角色数据_旧任务数据
            DBRoleInfo.DBTableRow2RoleInfo_OldTasks(this, cmd);

            //查询正在做的任务信息
            cmd = new MySQLSelectCommand(conn,
                 new string[] { "Id", "rid", "taskid", "focus", "value1", "value2", "addtime", "starlevel" },
                 new string[] { "t_tasks" }, new object[,] { { "rid", "=", roleID }, { "isdel", "=", 0 } }, null, null);

            /// 将数据库中获取的数据转换为角色数据_正在做任务数据
            DBRoleInfo.DBTableRow2RoleInfo_DoingTasks(this, cmd);

            //Cầu hình đọc ra vật phaarmm trong DB
            cmd = new MySQLSelectCommand(conn,
                     new string[] { "id", "goodsid", "isusing", "forge_level", "starttime", "endtime", "site", "Props", "gcount", "binding", "bagindex", "strong", "series", "otherpramer" },
                     new string[] { "t_goods" }, new object[,] { { "rid", "=", roleID }, { "gcount", ">", 0 } }, null, new string[,] { { "id", "asc" } });

            /// Load Ra List GoodData
            DBRoleInfo.DBTableRow2RoleInfo_Goods(this, cmd);


            DBRoleInfo.DBTableRow2RoleInfo_Friends(this, roleID);

            //查询经脉的列表信息
            cmd = new MySQLSelectCommand(conn,
                     new string[] { "Id", "skillid", "skilllevel", "lastusedtick", "cooldowntick", "exp" },
                     new string[] { "t_skills" }, new object[,] { { "rid", "=", roleID } }, null, null);

            /// 将数据库中获取的数据转换为角色数据_技能数据
            DBRoleInfo.DBTableRow2RoleInfo_Skills(this, cmd);

            //查询Buffer的列表信息
            cmd = new MySQLSelectCommand(conn,
                     new string[] { "bufferid", "starttime", "buffersecs", "bufferval", "custom_property" },
                     new string[] { "t_buffer" }, new object[,] { { "rid", "=", roleID } }, null, null);

            /// 将数据库中获取的数据转换为角色数据_Buffer数据
            DBRoleInfo.DBTableRow2RoleInfo_Buffers(this, cmd);

            //查询跑环任务的列表信息
            cmd = new MySQLSelectCommand(conn,
                     new string[] { "huanid", "rectime", "recnum", "taskClass", "extdayid", "extnum" },
                     new string[] { "t_dailytasks" }, new object[,] { { "rid", "=", roleID } }, null, null);

            /// 将数据库中获取的数据转换为角色数据_跑环任务数据
            DBRoleInfo.DBTableRow2RoleInfo_DailyTasks(this, cmd);

            //查询随身仓库的信息
            cmd = new MySQLSelectCommand(conn,
                     new string[] { "extgridnum" },
                     new string[] { "t_ptbag" }, new object[,] { { "rid", "=", roleID } }, null, null);


            DBRoleInfo.DBTableRow2RoleInfo_PortableBag(this, cmd);


            cmd = new MySQLSelectCommand(conn,
                     new string[] { "loginweekid", "logindayid", "loginnum", "newstep", "steptime", "lastmtime", "curmid", "curmtime", "songliid", "logingiftstate", "onlinegiftstate", "lastlimittimehuodongid", "lastlimittimedayid", "limittimeloginnum", "limittimegiftstate", "everydayonlineawardstep", "geteverydayonlineawarddayid", "serieslogingetawardstep", "seriesloginawarddayid", "seriesloginawardgoodsid", "everydayonlineawardgoodsid" },
                     new string[] { "t_huodong" }, new object[,] { { "rid", "=", roleID } }, null, null);


            DBRoleInfo.DBTableRow2RoleInfo_HuodongData(this, cmd);






            cmd = new MySQLSelectCommand(conn,
                     new string[] { "expdayid", "todayexp", "linglidayid", "todaylingli", "killbossdayid", "todaykillboss", "fubendayid", "todayfubennum", "wuxingdayid", "wuxingnum" },
                     new string[] { "t_dailydata" }, new object[,] { { "rid", "=", roleID } }, null, null);


            DBRoleInfo.DBTableRow2RoleInfo_DailyData(this, cmd);





            cmd = new MySQLSelectCommand(conn,
                     new string[] { "roleid", "gmailid" },
                     new string[] { "t_rolegmail_record" }, new object[,] { { "roleid", "=", roleID } }, null, null);

            DBRoleInfo.DBTableRow2RoleInfo_GMailInfo(this, cmd);




            cmd = new MySQLSelectCommand(conn,
                     new string[] { "roleid", "act_type", "id", "award_flag", "param1", "param2" },
                     new string[] { "t_seven_day_act" }, new object[,] { { "roleid", "=", roleID } }, null, null);


            DBRoleInfo.DBTableRow2RoleInfo_SevenDayActData(this, cmd);


            cmd = new MySQLSelectCommand(conn,
                     new string[] { "rid", "groupid", "actid", "purchaseNum", "countNum", "active" },
                     new string[] { "t_special_activity" }, new object[,] { { "rid", "=", roleID } }, null, null);


            DBRoleInfo.DBTableRow2RoleInfo_SpecialActivityData(this, cmd);

            cmd = null;

            RankValue.Init(roleID);
            return true;
        }

        #endregion 从数据库查询信息
    }
}