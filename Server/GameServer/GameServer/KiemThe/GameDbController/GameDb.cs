using GameServer.Core.Executor;
using GameServer.Logic;
using GameServer.Server;
using Server.Data;
using Server.Protocol;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace GameServer.KiemThe.GameDbController
{
    /// <summary>
    /// Lớp chuyên xử lý việc ghi đọc CSDL
    /// </summary>
    public class GameDb
    {
        /// <summary>
        ///  Thời gian tối đa sẽ lưu trữ thông tin về trang bị
        /// </summary>
        public const long MaxDBEquipStrongCmdSlot = (60 * 60 * 2 * 1000);

        /// <summary>
        /// Thời gian tối đa sẽ lưu trữ ROLE PRAM THEO DẠNG DICTIONARY
        /// </summary>
        private const long MaxDBRoleParamCmdSlot = (60 * 60 * 2 * 1000);


        /// <summary>
        /// LƯU LẠI DB [PHÚT] GẦN ĐÂY NHẤT
        /// </summary>
        private static int LastDBUpdateRoleStatMinute = TimeUtil.NowDateTime().Minute;

        /// <summary>
        /// LƯU LẠI DB [TIẾNG] GẦN ĐÂY NHẤT
        /// </summary>
        private static int LastDBUpdateRoleStatHour = TimeUtil.NowDateTime().Hour;

        /// <summary>
        /// THỜI GIAN UPDATE DB [NGÀY] GẦN ĐÂY NHẤT
        /// </summary>
        private static int LastDBUpdateRoleStatDay = TimeUtil.NowDateTime().DayOfYear;

        /// <summary>
        /// Khoảng thời gian tối đa thực hiện lưu trữ giữ mỗi lần
        /// </summary>
        private const long MaxDBCmdSlot = (60 * 60 * 1 * 1000);


        /// <summary>
        /// Hàm tự động lưu DB theo thời gian
        /// </summary>
        /// <param name="client"></param>
        /// <param name="force"></param>
        public static void ProcessDBCmdByTicks(KPlayer client, bool force = false)
        {
            long lastDbCmdTicks = 0;
            DateTime dateTime = TimeUtil.NowDateTime();
            long nowTicks = dateTime.Ticks / 10000;
            // Khởi tạo nhật ký lưu lại thời gian ghi vào DB
            bool instantUpdate = InstantDBUpdateRoleStat(dateTime);

            // LOẠI BỎ VIỆC GHI TIỀN ĐỊNH KỲ
            //lastDbCmdTicks = GetLastDBCmdTicks(client, (int)TCPGameServerCmds.CMD_DB_UPDATEMoney_CMD, nowTicks);
            //if (nowTicks - lastDbCmdTicks >= MaxDBCmdSlot || force)
            //{
            //    //Gửi lệnh tới GAMEDMMANAGER
            //    string strcmd = string.Format("{0}:{1}", client.RoleID, client.Money);
            //    GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_DB_UPDATEMoney_CMD,
            //        strcmd,
            //        null, client.ServerId);

            //    Global.SetLastDBCmdTicks(client, (int)TCPGameServerCmds.CMD_DB_UPDATEMoney_CMD, nowTicks);
            //}

            // 1 tiếng mới ghi GHI LẠI EXP VÀ LEVLE CỦA NHÂN VẬT 1 lần
            lastDbCmdTicks = GetLastDBCmdTicks(client, (int)TCPGameServerCmds.CMD_DB_UPDATE_EXPLEVEL, nowTicks);
            if (nowTicks - lastDbCmdTicks >= MaxDBCmdSlot || force || instantUpdate)
            {
                //Gửi lệnh tới GAMEDMMANAGER
                GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_DB_UPDATE_EXPLEVEL,
                    string.Format("{0}:{1}:{2}:{3}", client.RoleID, client.m_Level, client.m_Experience, client.Prestige),
                    null, client.ServerId);

                // Lưu lại các thông tin tới nhiệm vụ tuần hoàn
                // NHIỆM VỤ BẠO VĂN ĐỒNG
                Global.SaveRoleParamsInt32ValueToDB(client, RoleParamName.CurBVDTaskID, client.CurenQuestIDBVD, true);
                Global.SaveRoleParamsInt32ValueToDB(client, RoleParamName.QuestBVDTodayCount, client.QuestBVDTodayCount, true);
                Global.SaveRoleParamsInt32ValueToDB(client, RoleParamName.CanncelQuestBVD, client.CanncelQuestBVD, true);
                Global.SaveRoleParamsInt32ValueToDB(client, RoleParamName.QuestBVDStreakCount, client.QuestBVDStreakCount, true);
                // Nhiệm vụ hải tặc

                // Lưu lại tinh hoạt lực mỗi 5 phút 1 lần
                Global.SaveRoleParamsInt32ValueToDB(client, RoleParamName.GatherPoint, client.GetGatherPoint(), true);
                Global.SaveRoleParamsInt32ValueToDB(client, RoleParamName.MakePoint, client.GetMakePoint(), true);

                Global.SetLastDBCmdTicks(client, (int)TCPGameServerCmds.CMD_DB_UPDATE_EXPLEVEL, nowTicks);
            }

            // CHỈ THOÁT MỚI GHI LẠI AVATA
            if (force)
            {
                //Gửi lệnh tới GAMEDMMANAGER
                GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_DB_UPDATE_ROLE_AVARTA,
                    string.Format("{0}:{1}", client.RoleID, client.RolePic),
                    null, client.ServerId);

                Global.SetLastDBCmdTicks(client, (int)TCPGameServerCmds.CMD_DB_UPDATE_ROLE_AVARTA, nowTicks);
            }

            // CHỈ THOÁT MỚI GHI LẠI PK
            if (force)
            {
                //Gửi lệnh tới GAMEDMMANAGER
                GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_DB_UPDATEPKVAL_CMD,
                    string.Format("{0}:{1}:{2}", client.RoleID, client.PKValue, client.PKPoint),
                    null, client.ServerId);

                Global.SetLastDBCmdTicks(client, (int)TCPGameServerCmds.CMD_DB_UPDATEPKVAL_CMD, nowTicks);
            }

            // CHỈ THOÁT MỚI GHI LẠI THÔNG TIN TOP
            if (force)
            {
                GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_DB_UPDATEROLEDAILYDATA,
                    string.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}:{8}:{9}",
                    client.RoleID,
                    DataHelper.EncodeBase64(client.RoleName),
                    client.m_Level,
                    client.m_cPlayerFaction.GetFactionId(),
                    client.m_cPlayerFaction.GetRouteId(),
                    client.FactionHonor,
                    client.GetTotalValue() / 10000,
                    client.WorldHonor,
                    client.WorldMartial,
                    client.Prestige
                    ),
                    null, client.ServerId); ;

                Global.SetLastDBCmdTicks(client, (int)TCPGameServerCmds.CMD_DB_UPDATEROLEDAILYDATA, nowTicks);
            }
        }

        /// <summary>
        /// Khởi tạo lại nhất ký lữu trữ
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        private static bool InstantDBUpdateRoleStat(DateTime dateTime)
        {
            int day = dateTime.DayOfYear;
            int hour = dateTime.Hour;
            int minute = dateTime.Minute;

            if (day != LastDBUpdateRoleStatDay)
            {
                LastDBUpdateRoleStatDay = day;
                LastDBUpdateRoleStatHour = hour;
                LastDBUpdateRoleStatMinute = minute;
                return false;
            }

            //Nếu mà vừa lưu xong thì ko cần lưu tiếp
            if (hour == LastDBUpdateRoleStatHour && minute == LastDBUpdateRoleStatMinute)
            {
                return false;
            }

            int lastTime = LastDBUpdateRoleStatHour * 60 + LastDBUpdateRoleStatMinute;
            int nowTime = hour * 60 + minute;

            bool ret = false;

            int itemTime = 6 * 60 + 45; //Tính lại thời gian lưu
            if (itemTime > lastTime) //Nếu đúng thìluuw
            {
                if (nowTime >= itemTime)
                {
                    ret = true;
                }
            }

            LastDBUpdateRoleStatDay = day;
            LastDBUpdateRoleStatHour = hour;
            LastDBUpdateRoleStatMinute = minute;

            return ret;
        }

        /// <summary>
        /// Lấy ra thời gian lưu trữ gần đây nhất
        /// </summary>
        /// <param name="client"></param>
        /// <param name="dbCmdID"></param>
        /// <param name="nowTicks"></param>
        /// <returns></returns>
        private static long GetLastDBCmdTicks(KPlayer client, int dbCmdID, long nowTicks)
        {
            long lastDbCmdTicks = 0;
            lock (client.LastDBCmdTicksDict)
            {
                if (client.LastDBCmdTicksDict.TryGetValue(dbCmdID, out lastDbCmdTicks))
                {
                    return lastDbCmdTicks;
                }
            }

            return nowTicks;
        }

        /// <summary>
        /// THỰC HIỆN LƯU LẠI DỮ LIỆU VỀ SKILL
        /// </summary>
        /// <param name="client"></param>
        /// <param name="force"></param>
        public static void ProcessDBSkillCmdByTicks(KPlayer client, bool force = false)
        {
            if (null == client.SkillDataList)
            {
                return;
            }

            /// CHỈ THOÁT GAME MỚI GHI

            if (force)
            {
                lock (client.SkillDataList)
                {
                    for (int i = 0; i < client.SkillDataList.Count; i++)
                    {
                        SkillData skillData = client.SkillDataList[i];
                        if (skillData.DbID < 0)
                        {
                            continue;
                        }
                        GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_DB_UPSKILLINFO, string.Format("{0}:{1}:{2}:{3}:{4}:{5}", client.RoleID, skillData.SkillID, skillData.SkillLevel, skillData.LastUsedTick, skillData.CooldownTick, skillData.Exp), null, client.ServerId);
                    }
                }
            }
        }

        /// <summary>
        /// LƯu lại toàn bộ thông tin các thuộc tính theo dạng PRAMENTER đã đẩy vào DIC
        /// </summary>
        /// <param name="client"></param>
        /// <param name="force"></param>
        public static void ProcessDBRoleParamCmdByTicks(KPlayer client, bool force = false)
        {
            if (null == client.RoleParamsDict) return;

            long lastDbRoleParamCmdTicks = 0;
            long nowTicks = TimeUtil.NOW();

            lock (client.LastDBRoleParamCmdTicksDict)
            {
                String key = "";

                List<string> keysList = client.LastDBRoleParamCmdTicksDict.Keys.ToList<string>();
                int keysListCount = keysList.Count;

                for (int n = 0; n < keysListCount; n++)

                {
                    key = keysList[n];
                    lastDbRoleParamCmdTicks = client.LastDBRoleParamCmdTicksDict[key];

                    if (lastDbRoleParamCmdTicks > 0)
                    {
                        // 2 tiếng mới ghi 1 lần
                        if (nowTicks - lastDbRoleParamCmdTicks >= MaxDBRoleParamCmdSlot || force)
                        {
                            string paramValue = Global.GetRoleParamByName(client, key);
                            GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_DB_UPDATEROLEPARAM,
                                string.Format("{0}:{1}:{2}", client.RoleID, key, paramValue), null, client.ServerId);

                            LogManager.WriteLog(LogTypes.Analysis, "[WRITER TO DB]" + string.Format("{0}:{1}:{2}", client.RoleID, key, paramValue));

                            SetLastDBRoleParamCmdTicks(client, key, 0); //
                        }
                    }
                }
            }
        }

        /// <summary>
        /// LƯU LẠI TOÀN BỘ THÔNG TIN TRANG BỊ
        /// </summary>
        /// <param name="client"></param>
        /// <param name="force"></param>
        public static void ProcessDBEquipStrongCmdByTicks(KPlayer client, bool force = false)
        {
            if (null == client.GoodsDataList) return;

            GoodsData goodsData = null;
            long lastDbEquipStrongCmdTicks = 0;
            long nowTicks = TimeUtil.NOW();

            lock (client.GoodsDataList)
            {
                for (int i = 0; i < client.GoodsDataList.Count; i++)
                {
                    goodsData = client.GoodsDataList[i];

                    if (goodsData.Using < 0)
                    {
                        //NẾU MÀ VẬT PHẨM KO ĐƯỢC SỬ DỤNG THÌ BỎ QUA
                        continue;
                    }

                    lastDbEquipStrongCmdTicks = GetLastDBEquipStrongCmdTicks(client, goodsData.Id);
                    if (lastDbEquipStrongCmdTicks <= 0)
                    {
                        // NẾU VỪA MỚI ĐƯỢC LƯU XONG THÌ BRO QUA
                        continue;
                    }

                    if (nowTicks - lastDbEquipStrongCmdTicks >= MaxDBEquipStrongCmdSlot || force)
                    {
                        //UPDATE THÔNG TIN TRANG BỊ VÀO GAMEDB
                        UpdateEquipStrong(client, goodsData);
                    }
                }
            }
        }

        /// <summary>
        /// LẤY RA THỜI GIAN MỚI LƯU GẦN ĐÂY NHẤT
        /// </summary>
        /// <param name="client"></param>
        /// <param name="goodsDbID"></param>
        /// <returns></returns>
        private static long GetLastDBEquipStrongCmdTicks(KPlayer client, int goodsDbID)
        {
            long lastDbEquipStrongCmdTicks = 0;
            lock (client.LastDBEquipStrongCmdTicksDict)
            {
                if (client.LastDBEquipStrongCmdTicksDict.TryGetValue(goodsDbID, out lastDbEquipStrongCmdTicks))
                {
                    return lastDbEquipStrongCmdTicks;
                }
            }

            return 0;
        }

        /// <summary>
        /// LƯU LẠI THUỘC TÍNH CỦA TRANG BỊ
        /// </summary>
        /// <param name="client"></param>
        /// <param name="goodsData"></param>
        public static void UpdateEquipStrong(KPlayer client, GoodsData goodsData)
        {
            ModGoodsStrongDBCommand(Global._TCPManager.TcpOutPacketPool, client, goodsData);

            SetLastDBEquipStrongCmdTicks(client, goodsData.Id, 0, true);
        }

        /// <summary>
        /// LƯU LẠI ĐỘ BỀN TRANG BỊ
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="gd"></param>
        /// <returns></returns>
        public static int ModGoodsStrongDBCommand(TCPOutPacketPool pool, KPlayer client, GoodsData gd)
        {
            string strcmd = "";

            //Send CSLD VÀO DB
            string[] dbFields = null;
            strcmd = Global.FormatUpdateDBGoodsStr(client.RoleID, gd.Id, "*", "*", "*", "*", "*", "*", "*", "*", "*", "*", "*", "*", "*", "*", "*", "*", "*", gd.Strong, "*", "*", "*");
            TCPProcessCmdResults dbRequestResult = Global.RequestToDBServer(Global._TCPManager.tcpClientPool, pool, (int)TCPGameServerCmds.CMD_DB_UPDATEGOODS_CMD, strcmd, out dbFields, client.ServerId);
            if (dbRequestResult == TCPProcessCmdResults.RESULT_FAILED)
            {
                return -1;
            }

            if (dbFields.Length <= 0 || Convert.ToInt32(dbFields[1]) < 0)
            {
                return -2;
            }

            return 0;
        }

        /// <summary>
        /// Lưu lại thời gian gần đây nhất cập nhật thuộc tính trang bị
        /// </summary>
        /// <param name="client"></param>
        /// <param name="goodsDbID"></param>
        /// <param name="nowTicks"></param>
        /// <param name="forceAdd"></param>
        public static void SetLastDBEquipStrongCmdTicks(KPlayer client, int goodsDbID, long nowTicks, bool forceAdd)
        {
            lock (client.LastDBEquipStrongCmdTicksDict)
            {
                if (forceAdd)
                {
                    client.LastDBEquipStrongCmdTicksDict[goodsDbID] = nowTicks;
                }
                else
                {
                    long oldTicks;
                    if (!client.LastDBEquipStrongCmdTicksDict.TryGetValue(goodsDbID, out oldTicks) || oldTicks <= 0 || oldTicks > nowTicks)
                    {
                        client.LastDBEquipStrongCmdTicksDict[goodsDbID] = nowTicks;
                    }
                }
            }
        }

        /// <summary>
        /// THỰC HIỆN GỬI LỆNH LƯU TRỮ BUFF VÀO TRONG DB
        /// </summary>
        /// <param name="client"></param>
        /// <param name="bufferData"></param>
        public static void UpdateDBBufferData(KPlayer client, BufferData bufferData)
        {
            // NẾU KIỂU BUFF MÀ KO PHẢI KIỂU LƯU LẠI THỜI GIAN THÌ THÔI
            if (bufferData.BufferType >= 1)
            {
                return;
            }

            //GỬI LỆNH LƯU TRỮ VÀO DB
            GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_DB_UPDATEBUFFERITEM,
                string.Format("{0}:{1}:{2}:{3}:{4}:{5}", client.RoleID, bufferData.BufferID, bufferData.StartTime, bufferData.BufferSecs, bufferData.BufferVal, bufferData.CustomProperty),
                null, client.ServerId);
        }

        /// <summary>
        /// PACKET Thực hiện đăng ký mới 1 người chơi
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="state"></param>
        /// <param name="serverId"></param>
        /// <param name="logoutServerTicks"></param>
        /// <returns></returns>
        public static int RegisterUserIDToDBServer(string userID, int state, int serverId, ref long logoutServerTicks)
        {
            logoutServerTicks = 0;
            string[] fieldsData = null;
            long startTicks = TimeUtil.NOW();
            int count = 0;

            do
            {
                if (TCPProcessCmdResults.RESULT_FAILED == Global.RequestToDBServer(Global._TCPManager.tcpClientPool, Global._TCPManager.TcpOutPacketPool,
                    (int)TCPGameServerCmds.CMD_DB_REGUSERID, string.Format("{0}:{1}:{2}", userID, GameManager.ServerLineID, state), out fieldsData, serverId))
                {
                    if (state == 0)
                    {
                        //Thử lại ít nhất 5 lần mỗi lần cách nhau 1 phút
                        if (count++ < 5 || Math.Abs(TimeUtil.NOW() - startTicks) < 60000)
                        {
                            Thread.Sleep(100);
                            continue; //Không thử lại cho tới khi thành công nếu sẽ xảy ra sự cố nghiêm trọng
                        }
                    }

                    return -1;
                }

                if (null == fieldsData || fieldsData.Length <= 0) // NẾu dữ liệu sai có thể coi như ngoại tuyến
                {
                    return -2;
                }

                break;
            } while (true);

            if (fieldsData.Length >= 2)
            {
                logoutServerTicks = Convert.ToInt64(fieldsData[1]);
            }

            return Convert.ToInt32(fieldsData[0]);
        }

        /// <summary>
        /// Lưu lại thông tin các hoạt động trong ngày
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        public static void UpdateHuoDongDBCommand(TCPOutPacketPool pool, KPlayer client)
        {
            HuodongData huodongData = client.MyHuodongData;

            string StepTimeStr = (new DateTime(huodongData.StepTime * 10000)).ToString("yyyy-MM-dd HH$mm$ss");

            string strcmd = string.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}:{8}:{9}:{10}:{11}:{12}:{13}:{14}:{15}:{16}:{17}:{18}:{19}:{20}:{21}",
                client.RoleID,
                huodongData.LastWeekID,
                huodongData.LastDayID,
                huodongData.LoginNum,
                huodongData.NewStep,
                StepTimeStr,
                huodongData.LastMTime,
                huodongData.CurMID,
                huodongData.CurMTime,
                huodongData.SongLiID,
                huodongData.LoginGiftState,
                huodongData.OnlineGiftState,
                huodongData.LastLimitTimeHuoDongID,
                huodongData.LastLimitTimeDayID,
                huodongData.LimitTimeLoginNum,
                huodongData.LimitTimeGiftState,
                huodongData.EveryDayOnLineAwardStep,
                huodongData.GetEveryDayOnLineAwardDayID,
                huodongData.SeriesLoginGetAwardStep,
                huodongData.SeriesLoginAwardDayID,
                huodongData.SeriesLoginAwardGoodsID,
                huodongData.EveryDayOnLineAwardGoodsID
                );

            //Lưu lại toàn bộ thông tin hoạt động trong ngày
            GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_DB_UPDATHUODONGINFO, strcmd, null, client.ServerId);
        }

        /// <summary>
        /// Lưu giá trị biến toàn cục hệ thống vào DB
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        public static void SetSystemGlobalParameters(int id, string value)
        {
            GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_DB_QUERY_SYSPARAM, string.Format("{0}:{1}:{2}", 1, id, value), null, GameManager.LocalServerId);
        }

        /// <summary>
        /// Trả về giá trị biến toàn cục hệ thống tại vị trí tương ứng trong DB
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetSystemGlobalParameter(int id)
        {
            string[] paramValue = Global.SendToDB((int)TCPGameServerCmds.CMD_DB_QUERY_SYSPARAM, string.Format("{0}:{1}:{2}", 0, id, -1), GameManager.LocalServerId);
            return paramValue[1];
        }

        /// <summary>
        ///  LƯU LẠI THÔNG TIN NGƯỜI CHƠI THÔNG QUA PRAMENTER
        /// </summary>
        /// <param name="client"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="writeToDB"></param>
        public static void UpdateRoleParamByName(KPlayer client, string name, string value, bool writeToDB = false)
        {
            if (null == client.RoleParamsDict)
            {
                client.RoleParamsDict = new Dictionary<string, RoleParamsData>();
            }

            lock (client.RoleParamsDict)
            {
                RoleParamsData roleParamsData = null;
                if (!client.RoleParamsDict.TryGetValue(name, out roleParamsData))
                {
                    roleParamsData = new RoleParamsData()
                    {
                        ParamName = name,
                        ParamValue = value,
                    };

                    client.RoleParamsDict[name] = roleParamsData;
                }
                else
                {
                    if (roleParamsData.ParamValue == value && !string.IsNullOrEmpty(value))
                    {
                        return;
                    }
                    roleParamsData.ParamValue = value;
                }
            }

            //SEND LỆNH VỀ GAME DB
            if (writeToDB)
            {
                GameManager.DBCmdMgr.AddDBCmd((int)TCPGameServerCmds.CMD_DB_UPDATEROLEPARAM,
                    string.Format("{0}:{1}:{2}", client.RoleID, name, value), null, client.ServerId);

                return;
            }

            SetLastDBRoleParamCmdTicks(client, name, TimeUtil.NOW());
        }

        /// <summary>
        /// SET THỜI GIAN LƯU TRỮ GẦN ĐÂY NHẤT VÀO DB
        /// </summary>
        /// <param name="client"></param>
        /// <param name="paramName"></param>
        /// <param name="nowTicks"></param>
        public static void SetLastDBRoleParamCmdTicks(KPlayer client, string paramName, long nowTicks)
        {
            lock (client.LastDBRoleParamCmdTicksDict)
            {
                client.LastDBRoleParamCmdTicksDict[paramName] = nowTicks;
            }
        }
    }
}