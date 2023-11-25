using GameDBServer.Data;
using GameDBServer.DB;
using Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameDBServer.Logic.Ten
{
    /// <summary>
    /// TenManager发奖
    /// </summary>
    class TenManager
    {
        /// <summary>
        /// 状态
        /// </summary>
        public enum TenResultType
        {
            Default = 0,        //默认
            Success = 1,        //成功
            EnoPara = -1,       //缺少参数
            EnoRole = -3,       //角色ID不存在
            EIp = -4,           //IP不正确
            ECountMax = -5,     //超过领取次数(每人可领取道具数量限制由运营同事协商)
            EAware = -6,        //礼包错误
            EBag = -7,          //背包已满，不能领取
            Fail = -8,          //领取失败（其他原有）
            ETimeOut = -9,       //活动时间已过
            ELevelLimit = -10,   //活动时间已过
        };

        /// <summary>
        ///基本信息
        /// </summary>
        private static Dictionary<int, TenAwardData> _tenAwards = new Dictionary<int, TenAwardData>();

        private static bool _isInitTen = false;

        /// <summary>
        /// 加载奖励配置
        /// </summary>
        public static void initTen(string[] fields)
        {
            _tenAwards = new Dictionary<int, TenAwardData>();
            if (fields == null || fields.Length <= 0)
                return;

            foreach (var item in fields)
            {
                if (item == null) continue;

                string[] arr = item.Split(':');
                TenAwardData config = new TenAwardData();
                config.AwardID = Convert.ToInt32(arr[0]);
                config.DbKey = arr[1];
                config.OnlyNum = Convert.ToInt32(arr[2]);
                config.DayMaxNum = Convert.ToInt32(arr[3]);
                config.MailTitle = arr[5];
                config.MailContent = arr[6];
                config.MailUser = arr[7];
                config.BeginTime = DateTime.ParseExact(arr[8], "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                config.EndTime = DateTime.ParseExact(arr[9], "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                config.RoleLevel = Convert.ToInt32(arr[10]);

                string awards = arr[4];
                if (awards.Length > 0)
                {
                    config.AwardGoods = new List<GoodsData>();

                    string[] awardsArr = awards.Split('|');
                    foreach (string award in awardsArr)
                    {
                        string[] oneArr = award.Split(',');

                        GoodsData d = new GoodsData();
                        d.GoodsID = Convert.ToInt32(oneArr[0]);
                        d.GCount = Convert.ToInt32(oneArr[1]);
                        d.Binding = Convert.ToInt32(oneArr[2]);
                        config.AwardGoods.Add(d);
                    }
                }

                _tenAwards.Add(config.AwardID, config);
            }

            _isInitTen = true;
        }

        private static TenAwardData getTenAward(int awardID)
        {
            if (_tenAwards.ContainsKey(awardID))
                return _tenAwards[awardID];

            return null;
        }

        /// <summary>
        /// 上次扫描的时间
        /// </summary>
        private static long LastScanTicks = DateTime.Now.Ticks / 10000;

        /// <summary>
        /// 扫描新发奖信息
        /// </summary>
        public static void ScanLastGroup(DBManager dbMgr)
        {
            long nowTicks = DateTime.Now.Ticks / 10000;
            if (nowTicks - LastScanTicks < (30 * 1000) || !_isInitTen)
                return;

            LastScanTicks = nowTicks;

            // 扫描新邮件
            List<TenAwardData> groupList = DBQuery.ScanNewGroupTenFromTable(dbMgr);
            if (null != groupList && groupList.Count > 0 && _tenAwards.Count > 0 && _isInitTen)
            {
                foreach (var item in groupList)
                {
                    bool isSucc = DBWriter.UpdateTenState(dbMgr, item.DbID, 1);
                    if (isSucc)
                    {
                        int result = SendAward(dbMgr, item.UserID, item.RoleID, item.AwardID);
                        DBWriter.UpdateTenState(dbMgr, item.DbID, result);
                    }
                }
            }
        }

        /// <summary>
        /// 发送礼包
        /// </summary>
        /// <param name="dbMgr"></param>
        /// <param name="roleID"></param>
        /// <param name="awardID"></param>
        /// <returns></returns>
        public static int SendAward(DBManager dbMgr, string userID, int roleID, int awardID)
        {
            //礼包配置
            TenAwardData awardData = getTenAward(awardID);
            if (awardData == null)
                return (int)TenResultType.EAware;

            DateTime now = DateTime.Now;
            if (now < awardData.BeginTime || now > awardData.EndTime)
                return (int)TenResultType.ETimeOut;

            //角色等级
            DBRoleInfo roleData = DBManager.getInstance().GetDBRoleInfo(roleID);
            if (roleData == null) return (int)TenResultType.EnoRole;

        

            if (awardData.OnlyNum > 0)
            {
                int totalNum = DBQuery.TenOnlyNum(dbMgr, userID, awardID);
                if (totalNum > 0) return (int)TenResultType.ECountMax;
            }

            if (awardData.DayMaxNum > 0)
            {
                int totalNum = DBQuery.TenDayNum(dbMgr, userID, awardID);
                if (totalNum >= awardData.DayMaxNum) return (int)TenResultType.ECountMax;
            }

            string mailGoodsString = "";
            if (null != awardData.AwardGoods)
            {
                foreach (var goods in awardData.AwardGoods)
                {
                    int useCount = goods.GCount;
                    mailGoodsString += string.Format("{0}_{1}_{2}_{3}_{4}_{5}_{6}_{7}_{8}_{9}_{10}_{11}_{12}_{13}_{14}_{15}",
                        goods.GoodsID, goods.Forge_level,1, goods.Props, useCount,
                        0, 0, "",0, goods.Binding,
                       0, 0, goods.Strong,0, 0,0);

                    if (mailGoodsString.Length > 0)
                        mailGoodsString += "|";
                }
            }

            string[] fields = { "-1", awardData.MailUser, roleID.ToString(), "", awardData.MailTitle.ToString(), awardData.MailContent.ToString(), "0", "0", "0", mailGoodsString };
            int addGoodsCount = 0;

            int mailID = Global.AddMail(dbMgr, fields, out addGoodsCount);
            if (mailID > 0)
            {
                //添加GM命令消息
                string gmCmd = String.Format("{0}|{1}", roleID.ToString(), mailID);
                string gmCmdData = string.Format("-notifymail {0}", gmCmd);
                ChatMsgManager.AddGMCmdChatMsg(-1, gmCmdData);

                return mailID;
            }

            return (int)TenResultType.Fail;
        }

        //
    }


}

