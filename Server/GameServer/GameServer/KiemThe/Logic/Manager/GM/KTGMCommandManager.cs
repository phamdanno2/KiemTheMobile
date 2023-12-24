using GameServer.Interface;
using GameServer.KiemThe.CopySceneEvents;
using GameServer.KiemThe.GameEvents.TeamBattle;
using GameServer.KiemThe.CopySceneEvents.XiaoYaoGu;
using GameServer.KiemThe.Core;
using GameServer.KiemThe.Core.Activity.PlayerPray;
using GameServer.KiemThe.Core.Activity.SeashellCircle;
using GameServer.KiemThe.Core.Item;
using GameServer.KiemThe.Core.Task;
using GameServer.KiemThe.Entities;
using GameServer.KiemThe.GameEvents.FactionBattle;
using GameServer.KiemThe.GameEvents.GuildWarManager;
using GameServer.KiemThe.Logic.Manager.Battle;
using GameServer.KiemThe.LuaSystem;
using GameServer.KiemThe.Utilities;
using GameServer.Logic;
using GameServer.Server;
using KF.Client;
using Server.Data;
using Server.TCP;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;
using Tmsk.Contract;
using GameServer.KiemThe.GameEvents.EmperorTomb;
using GameServer.KiemThe.GameEvents.SpecialEvent;
using GameServer.KiemThe.Core.Activity.LuckyCircle;
using GameServer.KiemThe.Core.Activity.CardMonth;
using GameServer.KiemThe.Core.Activity.DownloadBouns;
using GameServer.KiemThe.Core.Activity.EveryDayOnlineEvent;
using GameServer.KiemThe.Core.Activity.LevelUpEvent;
using GameServer.KiemThe.Core.Activity.RechageEvent;

namespace GameServer.KiemThe.Logic
{
    /// <summary>
    /// Lớp thực hiện lệnh GM
    /// </summary>
    public static class KTGMCommandManager
    {
        #region Khởi tạo

        /// <summary>
        /// Thông tin GM
        /// </summary>
        private class GMInfo
        {
            /// <summary>
            /// ID nhân vật
            /// </summary>
            public int RoleID { get; set; }

            /// <summary>
            /// Địa chỉ IP
            /// </summary>
            public string IP { get; set; }
        }

        /// <summary>
        /// Đối tượng trung gian sử dụng khóa LOCK
        /// </summary>
        private static readonly object Mutex = new object();

        /// <summary>
        /// Danh sách GM trong hệ thống
        /// </summary>
        private static readonly Dictionary<int, GMInfo> GMList = new Dictionary<int, GMInfo>();

        /// <summary>
        /// Địa chỉ IP mà tất cả người chơi đều là GM
        /// </summary>
        private static readonly HashSet<string> EverybodyAtSpecificIPAddressIsGM = new HashSet<string>();

        /// <summary>
        /// Tải danh sách GM mới nhất trong hệ thống
        /// </summary>
        public static void LoadGMList()
        {
            Console.WriteLine("Load GMList.xml");
            try
            {
                lock (KTGMCommandManager.Mutex)
                {
                    KTGMCommandManager.GMList.Clear();
                    KTGMCommandManager.EverybodyAtSpecificIPAddressIsGM.Clear();
                    string xmlText = File.ReadAllText("GMList.xml");
                    XElement xmlNode = XElement.Parse(xmlText);
                    foreach (XElement node in xmlNode.Elements("GM"))
                    {
                        string strRoleID = node.Attribute("RoleID").Value;
                        string strIPAddress = node.Attribute("IP").Value;
                        /// Nếu ID nhân vật là bất kỳ *
                        if (strRoleID == "*")
                        {
                            /// Nếu danh sách địa chỉ IP chưa chứa
                            if (!KTGMCommandManager.EverybodyAtSpecificIPAddressIsGM.Contains(strIPAddress))
                            {
                                KTGMCommandManager.EverybodyAtSpecificIPAddressIsGM.Add(strIPAddress);
                            }
                        }
                        else
                        {
                            GMInfo gmInfo = new GMInfo()
                            {
                                RoleID = int.Parse(strRoleID),
                                IP = strIPAddress,
                            };
                            KTGMCommandManager.GMList[gmInfo.RoleID] = gmInfo;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Load GMList.xml error\nException: {0}", ex.ToString()));
            }
        }

        #endregion Khởi tạo

        #region Kiểm tra

        /// <summary>
        /// Kiểm tra người chơi có ID tương ứng có phải GM không
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public static bool IsGM(TMSKSocket socket, int roleID)
        {
            lock (KTGMCommandManager.Mutex)
            {
                try
                {
                    /// Địa chỉ IP của người chơi
                    IPEndPoint ipEndPoint = socket.RemoteEndPoint as IPEndPoint;
                    string ipAddress = ipEndPoint.Address.ToString();

                    /// Nếu trong danh sách toàn bộ GM thuộc IP có dấu * hoặc IP tương ứng của người chơi này thì là GM
                    if (KTGMCommandManager.EverybodyAtSpecificIPAddressIsGM.Contains("*") || KTGMCommandManager.EverybodyAtSpecificIPAddressIsGM.Contains(ipAddress))
                    {
                        return true;
                    }

                    /// Nếu không có tên trong danh sách GM
                    if (!KTGMCommandManager.GMList.TryGetValue(roleID, out GMInfo gmInfo))
                    {
                        return false;
                    }

                    /// Nếu IP GM config là bất kỳ hoặc IP GM config trùng với IP người chơi thì là GM
                    return gmInfo.IP == "*" || gmInfo.IP == ipAddress;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public static bool IsGMByRoleID(int roleID)
        {
            /// Nếu không có tên trong danh sách GM
            if (!KTGMCommandManager.GMList.TryGetValue(roleID, out GMInfo gmInfo))
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        /// <summary>
        /// Kiểm tra người chơi có phải GM không
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static bool IsGM(KPlayer player)
        {
            return KTGMCommandManager.IsGM(player.ClientSocket, player.RoleID);
        }

        #endregion Kiểm tra

        #region Thực thi lệnh GM

        /// <summary>
        /// Thực hiện lệnh GM
        /// </summary>
        /// <param name="player">Đối tượng người chơi</param>
        /// <param name="command">Chuỗi biểu diễn lệnh</param>
        public static void Process(KPlayer player, string command)
        {
            /// Nếu dữ liệu không chính xác
            if (player == null || string.IsNullOrEmpty(command))
            {
                return;
            }
            /// Nếu không phải GM
            else if (!KTGMCommandManager.IsGM(player))
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Process GM Command for {0}({1}) => ACCESS DENIED", player.RoleName, player.RoleID));
                return;
            }

            try
            {
                /// Chuẩn hóa
                command = command.Trim();
                while (command.IndexOf("  ") != -1)
                {
                    command = command.Replace("  ", " ");
                }

                /// Phân tích dữ liệu
                string[] para = command.Split(' ');

                /// Tên hàm
                string functionName = para[0];
                /// Tổng số tham biến
                int paramsCount = para.Length - 1;

                /// Kiểm tra và thực thi
                switch (functionName)
                {
                    #region Vòng quay may mắn
                    case "ReloadLuckyCircle":
                    {
                        if (paramsCount == 0)
                        {
                            KTLuckyCircleManager.Init();
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'ReloadLuckyCircle' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }

                    case "SetLuckyCircleTotalTurn":
                    {
                        if (paramsCount == 1)
                        {
                            int totalTurn = int.Parse(para[1]);
                            KTGMCommandManager.SetLuckyCircleTotalTurn(player, totalTurn);
                        }
                        else if (paramsCount == 2)
                        {
                            int targetID = int.Parse(para[1]);
                            int totalTurn = int.Parse(para[2]);
                            KTGMCommandManager.SetLuckyCircleTotalTurn(targetID, totalTurn);
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'SetLuckyCircleTotalTurn' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }

                    case "GetLuckyCircleTotalTurn":
                    {
                        if (paramsCount == 0)
                        {
                            KTGMCommandManager.GetLuckyCircleTotalTurn(player, player);
                        }
                        else if (paramsCount == 1)
                        {
                            int targetID = int.Parse(para[1]);
                            KTGMCommandManager.GetLuckyCircleTotalTurn(player, targetID);
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'GetLuckyCircleTotalTurn' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }
                    #endregion

                    #region Bách Bảo Rương
                    case "ReloadSeashellCircle":
                    {
                        if (paramsCount == 0)
                        {
                            KTSeashellCircleManager.Init();
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'ReloadSeashellCircle' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }

                    case "SetSeashellTreasureNextTurn":
                    {
                        if (paramsCount == 0)
                        {
                            KTGMCommandManager.SetSeashellTreasureNextTurn(player, -1);
                        }
                        else if (paramsCount == 1)
                        {
                            int targetID = int.Parse(para[1]);
                            KTGMCommandManager.SetSeashellTreasureNextTurn(targetID, -1);
                        }
                        else if (paramsCount == 2)
                        {
                            int targetID = int.Parse(para[1]);
                            int bet = int.Parse(para[2]);
                            KTGMCommandManager.SetSeashellTreasureNextTurn(targetID, bet);
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'SetSeashellTreasureNextTurn' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }
                    #endregion

                    #region Tải lại ServerConfig

                    case "ReloadServerConfig":
                    {
                        if (paramsCount == 0)
                        {
                            ServerConfig.Init();
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'ReloadServerConfig' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }

                    #endregion Tải lại ServerConfig

                    #region Script-Lua

                    case "LoadLua":
                    {
                        if (paramsCount == 1)
                        {
                            int scriptID = int.Parse(para[1]);
                            KTGMCommandManager.ReloadScriptLua(scriptID);
                            PlayerManager.ShowNotification(player, string.Format("Tải mới Script ID {0} thành công!", scriptID));
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'LoadLua' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }

                    #endregion Script-Lua

                    #region Bất tử GM

                    case "Invisiblity":
                    {
                        if (paramsCount == 1)
                        {
                            int state = int.Parse(para[1]);
                            player.GM_Invisiblity = state == 1;
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'Invisiblity' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }

                    case "Immortality":
                    {
                        if (paramsCount == 1)
                        {
                            int state = int.Parse(para[1]);
                            player.GM_Immortality = state == 1;
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'Immortality' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }

                    case "BandAcc":
                    {
                        string otherRoleName = para[1];
                        int minutes = Global.SafeConvertToInt32(para[2]);
                        int roleID = RoleName2IDs.FindRoleIDByName(otherRoleName);
                        if (-1 != roleID)
                        {
                            KPlayer otherClient = GameManager.ClientMgr.FindClient(roleID);
                            if (null != otherClient)
                            {
                                Global.ForceCloseClient(otherClient, "GMKICK");
                            }
                            else
                            {
                                string gmCmdData = string.Format("-kick {0}", para[1]);

                                GameManager.DBCmdMgr.AddDBCmd((int) TCPGameServerCmds.CMD_SPR_CHAT,
                                    string.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}:{8}", roleID, "", 0, "", 0, gmCmdData, 0, 0, GameManager.ServerLineID),
                                    null, GameManager.LocalServerId);
                            }
                        }

                        Global.BanRoleNameToDBServer(otherRoleName, minutes);

                        PlayerManager.ShowNotification(player, "Ban người chơi thành công");

                        break;
                    }

                    case "BandAccByRoleID":
                    {

                        int minutes = Global.SafeConvertToInt32(para[2]);
                        int roleID = Int32.Parse(para[1]);
                        if (-1 != roleID)
                        {
                            KPlayer otherClient = GameManager.ClientMgr.FindClient(roleID);
                            if (null != otherClient)
                            {
                                Global.BanRoleNameToDBServer(otherClient.RoleName, minutes);

                                Global.ForceCloseClient(otherClient, "GMKICK");
                            }
                            else
                            {
                                string gmCmdData = string.Format("-kick {0}", para[1]);

                                GameManager.DBCmdMgr.AddDBCmd((int) TCPGameServerCmds.CMD_SPR_CHAT,
                                    string.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}:{8}", roleID, "", 0, "", 0, gmCmdData, 0, 0, GameManager.ServerLineID),
                                    null, GameManager.LocalServerId);
                            }
                        }


                        PlayerManager.ShowNotification(player, "Ban người chơi thành công");

                        break;
                    }

                    case "UnBandAcc":
                    {
                        string otherRoleName = para[1];
                        Global.BanRoleNameToDBServer(otherRoleName, 0);
                        PlayerManager.ShowNotification(player, "Ban người chơi thành công");

                        break;
                    }

                    case "BandChat":
                    {
                        string otherRoleName = para[1];
                        int Hours = Global.SafeConvertToInt32(para[2]);

                        if (Hours > 0)
                        {
                            Global.BanRoleChatToDBServer(otherRoleName, Hours);

                            BanChatManager.AddBanRoleName(otherRoleName, Hours);
                        }

                        PlayerManager.ShowNotification(player, "Ban người chơi thành công");

                        break;
                    }

                    #endregion Bất tử GM

                    #region Tạo Captcha
                    case "GenerateCaptcha":
                    {
                        if (paramsCount == 0)
                        {
                            KTGMCommandManager.GenerateCaptcha(player);
                        }
                        else if (paramsCount == 1)
                        {
                            int targetID = int.Parse(para[1]);
                            KTGMCommandManager.GenerateCaptcha(targetID);
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'GenerateCaptcha' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }
                    #endregion

                    #region Trạng thái ngũ hành

                    case "AddSeriesState":
                    {
                        if (paramsCount == 2)
                        {
                            string stateID = para[1];
                            float time = float.Parse(para[2]);
                            KTGMCommandManager.AddSeriesState(player, stateID, time);
                        }
                        else if (paramsCount == 3)
                        {
                            int targetID = int.Parse(para[1]);
                            string stateID = para[2];
                            float time = float.Parse(para[3]);
                            KTGMCommandManager.AddSeriesState(targetID, stateID, time);
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'AddSeriesState' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }
                    case "RemoveSeriesState":
                    {
                        if (paramsCount == 1)
                        {
                            string stateID = para[1];
                            KTGMCommandManager.RemoveSeriesState(player, stateID);
                        }
                        else if (paramsCount == 2)
                        {
                            int targetID = int.Parse(para[1]);
                            string stateID = para[2];
                            KTGMCommandManager.RemoveSeriesState(targetID, stateID);
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'RemoveSeriesState' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }

                    #endregion Trạng thái ngũ hành

                    #region Trị liệu

                    case "Heal":
                    {
                        if (paramsCount == 0)
                        {
                            KTGMCommandManager.Heal(player);
                        }
                        else if (paramsCount == 1)
                        {
                            int targetID = int.Parse(para[1]);
                            KTGMCommandManager.Heal(targetID);
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'Heal' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }

                    #endregion Trị liệu

                    #region Tải lại danh sách GM

                    case "ReloadGMList":
                    {
                        if (paramsCount == 0)
                        {
                            KTGMCommandManager.LoadGMList();
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'ReloadGMList' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }

                    #endregion Tải lại danh sách GM

                    #region ID người chơi

                    case "PlayerInfo":
                    {
                        if (paramsCount == 0)
                        {
                            KTGMCommandManager.GetPlayerInfo(player, player.RoleName);
                        }
                        else if (paramsCount == 1)
                        {
                            string targetName = para[1];
                            KTGMCommandManager.GetPlayerInfo(player, targetName);
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'PlayerInfo' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }

                    #endregion ID người chơi

                    #region Dịch chuyển

                    case "GoTo":
                    {
                        if (paramsCount == 2)
                        {
                            int posX = int.Parse(para[1]);
                            int posY = int.Parse(para[2]);
                            KTGMCommandManager.GoTo(player, player.CurrentMapCode, posX, posY);
                        }
                        else if (paramsCount == 3)
                        {
                            int mapCode = int.Parse(para[1]);
                            int posX = int.Parse(para[2]);
                            int posY = int.Parse(para[3]);
                            KTGMCommandManager.GoTo(player, mapCode, posX, posY);
                        }
                        else if (paramsCount == 4)
                        {
                            int targetID = int.Parse(para[1]);
                            int mapCode = int.Parse(para[2]);
                            int posX = int.Parse(para[3]);
                            int posY = int.Parse(para[4]);
                            KTGMCommandManager.GoTo(targetID, mapCode, posX, posY);
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'GoTo' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }

                    #endregion Dịch chuyển

                    #region Kỹ năng và Buff

                    case "AddSkill":
                    {
                        if (paramsCount == 2)
                        {
                            int skillID = int.Parse(para[1]);
                            int level = int.Parse(para[2]);
                            KTGMCommandManager.AddSkill(player, skillID, level);
                        }
                        else if (paramsCount == 3)
                        {
                            int targetID = int.Parse(para[1]);
                            int skillID = int.Parse(para[2]);
                            int level = int.Parse(para[3]);
                            KTGMCommandManager.AddSkill(targetID, skillID, level);
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'AddSkill' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }
                    case "RemoveSkill":
                    {
                        if (paramsCount == 1)
                        {
                            int skillID = int.Parse(para[1]);
                            KTGMCommandManager.RemoveSkill(player, skillID);
                        }
                        else if (paramsCount == 2)
                        {
                            int targetID = int.Parse(para[1]);
                            int skillID = int.Parse(para[2]);
                            KTGMCommandManager.RemoveSkill(targetID, skillID);
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'RemoveSkill' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }
                    case "AddBuff":
                    {
                        if (paramsCount == 2)
                        {
                            int skillID = int.Parse(para[1]);
                            int level = int.Parse(para[2]);
                            KTGMCommandManager.AddBuff(player, skillID, level);
                        }
                        else if (paramsCount == 3)
                        {
                            int targetID = int.Parse(para[1]);
                            int skillID = int.Parse(para[2]);
                            int level = int.Parse(para[3]);
                            KTGMCommandManager.AddBuff(targetID, skillID, level);
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'AddBuff' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }
                    case "RemoveBuff":
                    {
                        if (paramsCount == 1)
                        {
                            int skillID = int.Parse(para[1]);
                            KTGMCommandManager.RemoveBuff(player, skillID);
                        }
                        else if (paramsCount == 2)
                        {
                            int targetID = int.Parse(para[1]);
                            int skillID = int.Parse(para[2]);
                            KTGMCommandManager.RemoveBuff(targetID, skillID);
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'RemoveBuff' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }
                    case "ResetAllSkillCooldown":
                    {
                        if (paramsCount == 0)
                        {
                            KTGMCommandManager.ResetAllSkillCooldown(player);
                        }
                        else if (paramsCount == 1)
                        {
                            int targetID = int.Parse(para[1]);
                            KTGMCommandManager.ResetAllSkillCooldown(targetID);
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'ResetAllSkillCooldown' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }

                    #endregion Kỹ năng và Buff

                    #region System chat

                    case "SysChat":
                    {
                        string message = command.Substring(functionName.Length + 1);
                        KTGMCommandManager.SendSystemChat(message);
                        break;
                    }
                    case "SysNotify":
                    {
                        string message = command.Substring(functionName.Length + 1);
                        KTGMCommandManager.SendSystemEventNotification(message);
                        break;
                    }

                    #endregion System chat

                    #region Tạo vật phẩm

                    case "CreateItem":
                    {
                        if (paramsCount == 2)
                        {
                            int itemID = int.Parse(para[1]);
                            int quantity = int.Parse(para[2]);
                            KTGMCommandManager.CreateItem(player, itemID, quantity, -1);
                        }
                        else if (paramsCount == 3)
                        {
                            int targetID = int.Parse(para[1]);
                            int itemID = int.Parse(para[2]);
                            int quantity = int.Parse(para[3]);
                            KTGMCommandManager.CreateItem(targetID, itemID, quantity);
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'CreateItem' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }

                    case "CreateItemExp":
                    {
                        if (paramsCount == 3)
                        {
                            int itemID = int.Parse(para[1]);
                            int quantity = int.Parse(para[2]);
                            int EXP = int.Parse(para[3]);
                            KTGMCommandManager.CreateItem(player, itemID, quantity, EXP);
                        }
                        else if (paramsCount == 4)
                        {
                            int targetID = int.Parse(para[1]);
                            int itemID = int.Parse(para[2]);
                            int quantity = int.Parse(para[3]);
                            int EXP = int.Parse(para[4]);
                            KTGMCommandManager.CreateItem(targetID, itemID, quantity);
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'CreateItem' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }

                    #endregion Tạo vật phẩm

                    #region Tạo quái

                    case "CreateMonster":
                    {
                        if (paramsCount == 2)
                        {
                            int monsterID = int.Parse(para[1]);
                            int monsterType = int.Parse(para[2]);
                            GameManager.MonsterZoneMgr.AddDynamicMonsters(player.MapCode, monsterID, player.CopyMapID, 1, player.PosX, player.PosY, "", "", -1, -1, Entities.Direction.DOWN, KE_SERIES_TYPE.series_none, (MonsterAIType) monsterType, -1, -1, -1, "", null);
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'CreateMonster' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }

                    #endregion Tạo quái

                    #region Xóa toàn bộ quái trong bản đồ
                    case "RemoveAllMonsters":
                    {
                        if (paramsCount == 0)
                        {
                            /// Danh sách đối tượng trong bản đồ
                            List<Monster> objs = GameManager.MonsterMgr.GetObjectsByMap(player.CurrentMapCode, player.CurrentCopyMapID);
                            /// Duyệt danh sách
                            foreach (Monster obj in objs)
                            {
                                if (!obj.IsDead())
                                {
                                    obj.MonsterZoneNode?.DestroyMonster(obj);
                                }
                            }
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'RemoveAllMonsters' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }
                    #endregion

                    #region Thiết lập cấp cường hóa cho trang bị

                    case "EquipEnhance":
                    {
                        if (paramsCount == 2)
                        {
                            int slot = int.Parse(para[1]);
                            int level = int.Parse(para[2]);
                            KTGMCommandManager.EquipEnhance(player, slot, level);
                        }
                        else if (paramsCount == 3)
                        {
                            int targetID = int.Parse(para[1]);
                            int slot = int.Parse(para[2]);
                            int level = int.Parse(para[3]);
                            KTGMCommandManager.EquipEnhance(targetID, slot, level);
                        }
                        break;
                    }

                    #endregion Thiết lập cấp cường hóa cho trang bị

                    #region Thêm tiền

                    case "AddMoney":
                    {
                        if (paramsCount == 2)
                        {
                            int type = int.Parse(para[1]);
                            int amount = int.Parse(para[2]);
                            KTGMCommandManager.AddMoney(player, type, amount);
                        }
                        else if (paramsCount == 3)
                        {
                            int targetID = int.Parse(para[1]);
                            int type = int.Parse(para[2]);
                            int amount = int.Parse(para[3]);
                            KTGMCommandManager.AddMoney(targetID, type, amount);
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'AddMoney' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }

                    case "ResetGuildRecoreMoney":
                    {
                        Global.SaveRoleParamsInt32ValueToDB(player, RoleParamName.TotalGuildMoneyAdd, 0, true);
                        Global.SaveRoleParamsInt32ValueToDB(player, RoleParamName.TotalGuildMoneyWithDraw, 0, true);

                        break;
                    }

                    #endregion Thêm tiền

                    #region Thêm vật phẩm rơi ở MAP

                    case "AddDropItem":
                    {
                        if (paramsCount == 1)
                        {
                            int monsterID = int.Parse(para[1]);
                            KTGMCommandManager.AddDropItem(player, monsterID);
                        }
                        else if (paramsCount == 2)
                        {
                            int targetID = int.Parse(para[1]);
                            int monsterID = int.Parse(para[2]);
                            KTGMCommandManager.AddDropItem(targetID, monsterID);
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'AddDropItem' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }

                    #endregion Thêm vật phẩm rơi ở MAP

                    #region Hoạt động

                    case "StartActivity":
                    {
                        if (paramsCount == 1)
                        {
                            int activityID = int.Parse(para[1]);
                            KTGMCommandManager.StartActivity(activityID);
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'StartActivity' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }
                    case "StopActivity":
                    {
                        if (paramsCount == 1)
                        {
                            int activityID = int.Parse(para[1]);
                            KTGMCommandManager.StopActivity(activityID);
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'StopActivity' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }

                    #endregion Hoạt động

                    #region Sát khí

                    case "SetPKValue":
                    {
                        if (paramsCount == 1)
                        {
                            int value = int.Parse(para[1]);
                            KTGMCommandManager.SetPKValue(player, value);
                        }
                        else if (paramsCount == 2)
                        {
                            int targetID = int.Parse(para[1]);
                            int value = int.Parse(para[2]);
                            KTGMCommandManager.SetPKValue(targetID, value);
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'SetPKValue' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }

                    #endregion Sát khí

                    #region Kỹ năng sống

                    case "ResetLifeSkillLevelAndExp":
                    {
                        if (paramsCount == 0)
                        {
                            KTGMCommandManager.ResetLifeSkillLevelAndExp(player);
                        }
                        else if (paramsCount == 1)
                        {
                            int targetID = int.Parse(para[1]);
                            KTGMCommandManager.ResetLifeSkillLevelAndExp(targetID);
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'ResetLifeSkillLevelAndExp' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }

                    case "SetLifeSkillLevelAndExp":
                    {
                        if (paramsCount == 3)
                        {
                            int lifeSkillID = int.Parse(para[1]);
                            int level = int.Parse(para[2]);
                            int exp = int.Parse(para[3]);
                            KTGMCommandManager.SetLifeSkillLevelAndExp(player, lifeSkillID, level, exp);
                        }
                        else if (paramsCount == 4)
                        {
                            int targetID = int.Parse(para[1]);
                            int lifeSkillID = int.Parse(para[2]);
                            int level = int.Parse(para[3]);
                            int exp = int.Parse(para[4]);
                            KTGMCommandManager.SetLifeSkillLevelAndExp(targetID, lifeSkillID, level, exp);
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'SetLifeSkillLevelAndExp' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }
                    case "AddGatherMakePoint":
                    {
                        if (paramsCount == 2)
                        {
                            int gatherPoint = int.Parse(para[1]);
                            int makePoint = int.Parse(para[2]);
                            KTGMCommandManager.AddGatherMakePoint(player, gatherPoint, makePoint);
                        }
                        else if (paramsCount == 3)
                        {
                            int targetID = int.Parse(para[1]);
                            int gatherPoint = int.Parse(para[2]);
                            int makePoint = int.Parse(para[3]);
                            KTGMCommandManager.AddGatherMakePoint(targetID, gatherPoint, makePoint);
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'AddGatherMakePoint' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }

                    #endregion Kỹ năng sống

                    #region Danh hiệu

                    case "SetTempTitle":
                    {
                        if (paramsCount == 1)
                        {
                            string title = para[1];
                            KTGMCommandManager.SetTempTitle(player, title.Replace("_", " "));
                        }
                        else if (paramsCount == 2)
                        {
                            int targetID = int.Parse(para[1]);
                            string title = para[2];
                            KTGMCommandManager.SetTempTitle(targetID, title.Replace("_", " "));
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'SetTempTitle' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }

                    #endregion Danh hiệu

                    #region Thư

                    case "SendSystemMail":
                    {
                        if (paramsCount == 3)
                        {
                            int targetID = int.Parse(para[1]);
                            string title = para[2].Replace('_', ' ');
                            string content = para[3].Replace('_', ' ');
                            /// Nếu có mục tiêu
                            if (targetID != -1)
                            {
                                KTGMCommandManager.SendSystemMail(targetID, title, content, "", 0, 0);
                            }
                            else
                            {
                                KTGMCommandManager.SendSystemMail(player, title, content, "", 0, 0);
                            }
                        }
                        else if (paramsCount == 4)
                        {
                            int targetID = int.Parse(para[1]);
                            string title = para[2].Replace('_', ' ');
                            string content = para[3].Replace('_', ' ');
                            string items = para[4];
                            /// Nếu có mục tiêu
                            if (targetID != -1)
                            {
                                KTGMCommandManager.SendSystemMail(targetID, title, content, items, 0, 0);
                            }
                            else
                            {
                                KTGMCommandManager.SendSystemMail(player, title, content, items, 0, 0);
                            }
                        }
                        else if (paramsCount == 5)
                        {
                            int targetID = int.Parse(para[1]);
                            string title = para[2].Replace('_', ' ');
                            string content = para[3].Replace('_', ' ');
                            int boundMoney = int.Parse(para[4]);
                            int boundToken = int.Parse(para[5]);
                            /// Nếu có mục tiêu
                            if (targetID != -1)
                            {
                                KTGMCommandManager.SendSystemMail(targetID, title, content, "", boundMoney, boundToken);
                            }
                            else
                            {
                                KTGMCommandManager.SendSystemMail(player, title, content, "", boundMoney, boundToken);
                            }
                        }
                        else if (paramsCount == 6)
                        {
                            int targetID = int.Parse(para[1]);
                            string title = para[2].Replace('_', ' ');
                            string content = para[3].Replace('_', ' ');
                            string items = para[4];
                            int boundMoney = int.Parse(para[5]);
                            int boundToken = int.Parse(para[6]);
                            /// Nếu có mục tiêu
                            if (targetID != -1)
                            {
                                KTGMCommandManager.SendSystemMail(targetID, title, content, items, boundMoney, boundToken);
                            }
                            else
                            {
                                KTGMCommandManager.SendSystemMail(player, title, content, items, boundMoney, boundToken);
                            }
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'SendSystemMail' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }

                    #endregion Thư

                    #region Kinh nghiệm và cấp độ

                    case "AddExp":
                    {
                        if (paramsCount == 1)
                        {
                            int exp = int.Parse(para[1]);
                            KTGMCommandManager.AddExp(player, exp);
                        }
                        else if (paramsCount == 2)
                        {
                            int targetID = int.Parse(para[1]);
                            int exp = int.Parse(para[2]);
                            KTGMCommandManager.AddExp(targetID, exp);
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'AddExp' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }

                    case "SetLevel":
                    {
                        if (paramsCount == 1)
                        {
                            int level = int.Parse(para[1]);
                            KTGMCommandManager.SetLevel(player, level);
                        }
                        else if (paramsCount == 2)
                        {
                            int targetID = int.Parse(para[1]);
                            int level = int.Parse(para[2]);
                            KTGMCommandManager.SetLevel(targetID, level);
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'SetLevel' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }

                    #endregion Kinh nghiệm

                    #region Vào phái

                    case "JoinFaction":
                    {
                        if (paramsCount == 1)
                        {
                            int factionID = int.Parse(para[1]);
                            KTGMCommandManager.JoinFaction(player, factionID);
                        }
                        else if (paramsCount == 2)
                        {
                            int targetID = int.Parse(para[1]);
                            int factionID = int.Parse(para[2]);
                            KTGMCommandManager.JoinFaction(targetID, factionID);
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'JoinFaction' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }

                    #endregion Vào phái

                    #region Battle

                    case "StopServer":
                    {
                        Program.Exit();
                    }
                    break;

                    case "CreateFireCamp":
                    {
                        GameMap Map = GameManager.MapMgr.DictMaps[player.MapCode];

                        MonsterDeadHelper.CreateFireCampStep1(Map, player.PosX, player.PosY, player, player.CopyMapID);
                    }
                    break;

                    case "SetMainTask":
                    {
                        if (paramsCount == 1)
                        {
                            int State = int.Parse(para[1]);

                            ProcessTask.GMSetMainTaskID(player, State);
                        }
                        else if (paramsCount == 2)
                        {
                            string otherRoleName = para[1];

                            int roleID = RoleName2IDs.FindRoleIDByName(otherRoleName);

                            KPlayer otherClient = GameManager.ClientMgr.FindClient(roleID);

                            if (null != otherClient)
                            {
                                int State = int.Parse(para[2]);

                                ProcessTask.GMSetMainTaskID(otherClient, State);
                            }
                        }
                    }
                    break;

                    case "ResetBVD":
                    {
                        if (paramsCount == 1)
                        {
                            string otherRoleName = para[1];

                            int roleID = RoleName2IDs.FindRoleIDByName(otherRoleName);

                            KPlayer otherClient = GameManager.ClientMgr.FindClient(roleID);

                            if (otherClient != null)
                            {
                                otherClient.CurenQuestIDBVD = -1;
                                otherClient.CanncelQuestBVD = 0;
                                otherClient.QuestBVDStreakCount = 0;
                                otherClient.QuestBVDTodayCount = 0;

                                if (otherClient.TaskDataList != null)
                                {
                                    // Đoạn này để tránh bug nếu mà nhiệm vụ hiện tại khác nhiệm vụ đã nhận trước đó
                                    foreach (TaskData TaskArmy in otherClient.TaskDataList)
                                    {
                                        Task _Task = TaskDailyArmyManager.getInstance().GetTaskTemplate(TaskArmy.DoingTaskID);

                                        //Tức là đang có nhiệm vụ BVD đang nhận
                                        if (_Task != null && _Task.TaskClass == (int) TaskClasses.NghiaQuan)
                                        {
                                            TaskDailyArmyManager.getInstance().CancelTask(otherClient, TaskArmy.DbID, TaskArmy.DoingTaskID);
                                        }
                                    }
                                }

                                Global.ForceCloseClient(otherClient, "GMKICK");
                            }
                        }
                    }
                    break;

                    case "ResetHaiTac":
                    {
                        if (paramsCount == 1)
                        {
                            string otherRoleName = para[1];

                            int roleID = RoleName2IDs.FindRoleIDByName(otherRoleName);

                            KPlayer otherClient = GameManager.ClientMgr.FindClient(roleID);

                            if (otherClient != null)
                            {
                                PirateTaskManager.getInstance().SetQuestIDDay(otherClient, -1);
                                PirateTaskManager.getInstance().SetNumQuestThisDay(otherClient, 0);

                                if (otherClient.TaskDataList != null)
                                {
                                    // Đoạn này để tránh bug nếu mà nhiệm vụ hiện tại khác nhiệm vụ đã nhận trước đó
                                    foreach (TaskData TaskArmy in otherClient.TaskDataList)
                                    {
                                        Task _Task = PirateTaskManager.getInstance().GetTaskTemplate(TaskArmy.DoingTaskID);

                                        //Tức là đang có nhiệm vụ BVD đang nhận
                                        if (_Task != null && _Task.TaskClass == (int) TaskClasses.HaiTac)
                                        {
                                            PirateTaskManager.getInstance().CancelTask(otherClient, TaskArmy.DbID, TaskArmy.DoingTaskID);
                                        }
                                    }
                                }
                                Global.ForceCloseClient(otherClient, "GMKICK");
                            }
                        }
                    }
                    break;

                    case "ResetThuongHoi":
                    {
                        if (paramsCount == 1)
                        {
                            string otherRoleName = para[1];

                            int roleID = RoleName2IDs.FindRoleIDByName(otherRoleName);

                            KPlayer otherClient = GameManager.ClientMgr.FindClient(roleID);

                            FirmTaskManager.getInstance().SetNumQuestThisWeek(otherClient, 0);
                        }
                    }
                    break;

                    case "BattleStateTest":
                    {
                        int State = int.Parse(para[1]);

                        G2C_EventState _State = new G2C_EventState();
                        _State.EventID = 50;
                        _State.State = State;

                        player.SendPacket<G2C_EventState>((int) TCPGameServerCmds.CMD_KT_EVENT_STATE, _State);

                        break;
                    }

                    //case "GuildWarStart":
                    //    {
                    //        GuidWarManager.getInstance().ChangeState(player);

                    //        break;
                    //    }

                    case "StartBattle":
                    {
                        int Level = int.Parse(para[1]);

                        if (Level < 1 || Level > 3)
                        {
                            PlayerManager.ShowNotification(player, "Cấp chiến trương không hợp lệ");
                        }
                        else
                        {
                            Battel_SonJin_Manager.ForceStartBattle(Level);
                        }

                        break;
                    }

                    case "StartFactionBattle":
                    {
                        FactionBattleManager.ForceStartBattle();
                    }
                    break;

                    case "EndFactionBattle":
                    {
                        FactionBattleManager.ForceEndBattle();
                    }
                    break;


                    case "GuildWarStart":
                    {
                        GuidWarManager.getInstance().BattleForceStart();
                    }
                    break;

                    case "GuildWarEnd":
                    {
                        GuidWarManager.getInstance().BattleForceEnd();
                    }
                    break;

                    case "DoneTask":
                    {
                        int TaskID = int.Parse(para[1]);

                        var findtask = player.TaskDataList.Where(x => x.DoingTaskID == TaskID).FirstOrDefault();
                        if (findtask != null)
                        {
                            findtask.DoingTaskVal1 = 100;

                            if (!GameManager.MapMgr.DictMaps.TryGetValue(player.MapCode, out GameMap map))
                            {
                                PlayerManager.ShowNotification(player, "Không lấy được bản đồ đang đứng");
                            }

                            Task FindTask = TaskManager.getInstance().FindTaskById(TaskID);
                            if (FindTask != null)
                            {
                                NPC npc = NPCGeneralManager.FindNPCBYRES(player.MapCode, player.CopyMapID, FindTask.DestNPC);

                                if (npc != null)
                                {
                                    if (FindTask.TaskClass == (int) TaskClasses.MainTask)
                                    {
                                        MainTaskManager.getInstance().CompleteTask(map, npc, player, TaskID);
                                    }
                                    else if (FindTask.TaskClass == (int) TaskClasses.HaiTac)
                                    {
                                        PirateTaskManager.getInstance().CompleteTask(map, npc, player, TaskID);
                                    }
                                    else if (FindTask.TaskClass == (int) TaskClasses.NghiaQuan)
                                    {
                                        TaskDailyArmyManager.getInstance().CompleteTask(map, npc, player, TaskID);
                                    }
                                }
                                else
                                {
                                    npc = NPCGeneralManager.FindNPCBYRES(8, player.CopyMapID, 6786);

                                    if (FindTask.TaskClass == (int) TaskClasses.MainTask)
                                    {
                                        MainTaskManager.getInstance().CompleteTask(map, npc, player, TaskID);
                                    }
                                    else if (FindTask.TaskClass == (int) TaskClasses.HaiTac)
                                    {
                                        PirateTaskManager.getInstance().CompleteTask(map, npc, player, TaskID);
                                    }
                                    else if (FindTask.TaskClass == (int) TaskClasses.NghiaQuan)
                                    {
                                        TaskDailyArmyManager.getInstance().CompleteTask(map, npc, player, TaskID);
                                    }
                                }
                            }
                        }

                        break;
                    }

                    case "EndBattle":
                    {
                        int Level = int.Parse(para[1]);

                        if (Level < 1 || Level > 3)
                        {
                            PlayerManager.ShowNotification(player, "Cấp chiến trương không hợp lệ");
                        }
                        else
                        {
                            Battel_SonJin_Manager.ForceEndBattle(Level);
                        }
                        break;
                    }

                    case "KillEffectTest":
                    {
                        int COUNT = int.Parse(para[1]);

                        G2C_KillStreak _State = new G2C_KillStreak();

                        _State.KillNumber = COUNT;

                        player.SendPacket<G2C_KillStreak>((int) TCPGameServerCmds.CMD_KT_KILLSTREAK, _State);

                        break;
                    }

                    case "NotifyTest":
                    {
                        G2C_EventNotification _Notify = new G2C_EventNotification();
                        _Notify.EventName = "Tống Kim Công Báo";
                        _Notify.ShortDetail = "TIME|500";
                        _Notify.TotalInfo = new List<string>();

                        _Notify.TotalInfo.Add("Giết Địch : 100");
                        _Notify.TotalInfo.Add("Bị Giết : 100");

                        _Notify.TotalInfo.Add("Tích Lũy : 10550");

                        _Notify.TotalInfo.Add("Hạng Hiện Tại  : 1");

                        player.SendPacket<G2C_EventNotification>((int) TCPGameServerCmds.CMD_KT_EVENT_NOTIFICATION, _Notify);

                        break;
                    }

                    case "CheckSigNet":
                    {
                        int TotalBS = int.Parse(para[1]);

                        int LevelStart = int.Parse(para[2]);

                        int ExpStart = int.Parse(para[3]);

                        ItemEnhance.Caclulation(TotalBS, LevelStart, ExpStart);

                        break;
                    }

                    #endregion Battle

                    #region LIENSV

                    case "GetLine":
                    {
                        int MapCode = int.Parse(para[1]);

                        try
                        {
                            List<KuaFuLineData> list = YongZheZhanChangClient.getInstance().GetKuaFuLineDataList(MapCode) as List<KuaFuLineData>;

                            Console.WriteLine(list);

                            player.SendPacket((int) (TCPGameServerCmds.CMD_SPR_KUAFU_MAP_INFO), list);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }

                        break;
                    }

                    case "TryEnter":
                    {
                        int MapCode = int.Parse(para[1]);
                        int Lines = int.Parse(para[2]);

                        try
                        {
                            string[] cmdParams = new string[2];
                            cmdParams[0] = MapCode + "";
                            cmdParams[1] = Lines + "";
                            KuaFuMapManager.getInstance().ProcessKuaFuMapEnterCmd(player, (int) (TCPGameServerCmds.CMD_SPR_KUAFU_MAP_ENTER), null, cmdParams);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }

                        break;
                    }

                    #endregion LIENSV

                    #region Kiểm tra thông tin hệ thống

                    case "CheckCcu":
                    {
                        try
                        {
                            PlayerManager.ShowNotification(player, "CCU ONLINE: " + GameManager.ClientMgr.GetClientCount());
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }

                        break;
                    }
                    case "CheckCcu2":
                    {
                        try
                        {
                            string[] dbFields = Global.ExecuteDBCmd((int) TCPGameServerCmds.CMD_DB_GETTOTALONLINENUM, string.Format("{0}", player.RoleID), GameManager.LocalServerId);
                            if (null == dbFields || dbFields.Length < 1)
                            {
                            }
                            else
                            {
                                int totalOnlineNum = Global.SafeConvertToInt32(dbFields[0]);
                                PlayerManager.ShowNotification(player, "CCU ONLINE 2: " + totalOnlineNum);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }

                        break;
                    }

                    #endregion Kiểm tra thông tin hệ thống

                    #region Tần Lăng
                    case "GetEmperorTombData":
                    {
                        if (paramsCount == 0)
                        {
                            KTGMCommandManager.GetEmperorTombData(player, player);
                        }
                        else if (paramsCount == 1)
                        {
                            int targetID = int.Parse(para[1]);
                            KTGMCommandManager.GetEmperorTombData(player, targetID);
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'GetEmperorTombData' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }

                    case "ResetEmperorTombTimeLeft":
                    {
                        if (paramsCount == 0)
                        {
                            KTGMCommandManager.ResetEmperorTombTimeLeft(player.RoleID);
                        }
                        else if (paramsCount == 1)
                        {
                            int targetID = int.Parse(para[1]);
                            KTGMCommandManager.ResetEmperorTombTimeLeft(targetID);
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'ResetEmperorTombTimeLeft' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }

                    case "EmperorTomb_CreateBossTest":
                    {
                        if (paramsCount == 0)
                        {
                            /// Hướng quay
                            KiemThe.Entities.Direction dir = KiemThe.Entities.Direction.NONE;
                            /// Mức máu
                            int hp = 1000000;
                            /// Ngũ hành
                            KE_SERIES_TYPE series = KE_SERIES_TYPE.series_none;

                            /// Tạo Boss
                            GameManager.MonsterZoneMgr.AddDynamicMonsters(player.MapCode, 6918, -1, 1, player.PosX, player.PosY, "", "", hp, 100, dir, series, MonsterAIType.Boss, -1, -1, 100000, null, (boss) => {
                                /// Thực thi sự kiện Tick
                                boss.OnTick = () => {
                                    /// % máu hiện tại
                                    int nHPPercent = (int) (boss.m_CurrentLife / (float) boss.m_CurrentLifeMax * 100);
                                    /// Nếu còn trên 50% máu
                                    if (nHPPercent >= 50)
                                    {
                                        /// Miễn dịch sát thương ngoại
                                        boss.m_ImmuneToPhysicDamage = true;
                                        boss.m_ImmuneToMagicDamage = false;

                                        PlayerManager.ShowNotification(player, "Immune to physic");
                                    }
                                    /// Nếu còn dưới 50% máu
                                    else
                                    {
                                        /// Miễn dịch sát thương nội
                                        boss.m_ImmuneToMagicDamage = true;
                                        boss.m_ImmuneToPhysicDamage = false;

                                        PlayerManager.ShowNotification(player, "Immune to magic");
                                    }
                                };
                            }, 65535, null, null);
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'EmperorTomb_CreateBossTest' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }
                    #endregion

                    #region Đoán Hoa Đăng

                    case "ResetKnowledgeChallengeQuestions":
                    {
                        if (paramsCount == 0)
                        {
                            KTGMCommandManager.ResetKnowledgeChallengeQuestions(player);
                        }
                        else if (paramsCount == 1)
                        {
                            int targetID = int.Parse(para[1]);
                            KTGMCommandManager.ResetKnowledgeChallengeQuestions(targetID);
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'ResetKnowledgeChallengeQuestions' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }

                    #endregion Bí cảnh

                    #region Tiêu Dao Cốc

                    case "ResetXoYo":
                    {
                        if (paramsCount == 0)
                        {
                            KTGMCommandManager.ResetCopySceneEnterTimes(player, AcitvityRecore.XoYo);
                        }
                        else if (paramsCount == 1)
                        {
                            int targetID = int.Parse(para[1]);
                            KTGMCommandManager.ResetCopySceneEnterTimes(targetID, AcitvityRecore.XoYo);
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'ResetXoYo' Faild...\n" + "Invalid param.");
                        }

                        break;
                    }

                    case "AddXoYoMonthStoragePoint":
                    {
                        if (paramsCount == 1)
                        {
                            int nPoint = int.Parse(para[1]);
                            KTGMCommandManager.AddXoYoMonthStoragePoint(player, nPoint);
                        }
                        else if (paramsCount == 2)
                        {
                            int targetID = int.Parse(para[1]);
                            int nPoint = int.Parse(para[2]);
                            KTGMCommandManager.AddXoYoMonthStoragePoint(targetID, nPoint);
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'AddXoYoMonthStoragePoint' Faild...\n" + "Invalid param.");
                        }

                        break;
                    }

                    case "GetXoYoMonthStoragePoint":
                    {
                        if (paramsCount == 0)
                        {
                            KTGMCommandManager.GetXoYoMonthStoragePoint(player);
                        }
                        else if (paramsCount == 2)
                        {
                            int targetID = int.Parse(para[1]);
                            KTGMCommandManager.GetXoYoMonthStoragePoint(targetID);
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'GetXoYoMonthStoragePoint' Faild...\n" + "Invalid param.");
                        }

                        break;
                    }

                    #endregion Tiêu Dao Cốc

                    #region Bí cảnh

                    case "ResetMiJing":
                    {
                        if (paramsCount == 0)
                        {
                            KTGMCommandManager.ResetCopySceneEnterTimes(player, AcitvityRecore.MiJing);
                        }
                        else if (paramsCount == 1)
                        {
                            int targetID = int.Parse(para[1]);
                            KTGMCommandManager.ResetCopySceneEnterTimes(targetID, AcitvityRecore.MiJing);
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'ResetMiJing' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }

                    #endregion Bí cảnh

                    #region Du Long Các

                    case "ResetYouLong":
                    {
                        if (paramsCount == 0)
                        {
                            KTGMCommandManager.ResetCopySceneEnterTimes(player, AcitvityRecore.YouLong);
                        }
                        else if (paramsCount == 1)
                        {
                            int targetID = int.Parse(para[1]);
                            KTGMCommandManager.ResetCopySceneEnterTimes(targetID, AcitvityRecore.YouLong);
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'ResetYouLong' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }

                    #endregion Du Long Các

                    #region Võ lâm liên đấu
                    case "StartTeamBattle":
                    {
                        if (paramsCount == 0)
                        {
                            TeamBattle_ActivityScript.BeginBattle();
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'StartTeamBattle' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }

                    case "ArrangeTeamBattleFinalRound":
                    {
                        if (paramsCount == 0)
                        {
                            TeamBattle_ActivityScript.ArrangeAndIncreaseStageToTopTeamToTheFinalRound();
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'ArrangeTeamBattleFinalRound' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }

                    case "ArrangeTeamBattlePlayersRank":
                    {
                        if (paramsCount == 0)
                        {
                            TeamBattle_ActivityScript.ArrangePlayersRank();
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'ArrangeTeamBattlePlayersRank' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }

                    case "ArrangeTeamBattlePlayersRankAndUpdateAwardsStateToAllTeams":
                    {
                        if (paramsCount == 1)
                        {
                            int state = int.Parse(para[1]);
                            TeamBattle_ActivityScript.ArrangePlayersRankAndUpdateAllTeamsAwardState(state == 1);
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'ArrangeTeamBattlePlayersRankAndUpdateAwardsStateToAllTeams' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }

                    case "UpdateTeamBattleAwardState":
                    {
                        if (paramsCount == 1)
                        {
                            int state = int.Parse(para[1]);
                            KTGMCommandManager.SetTeamBattleAwardState(player, state);
                        }
                        else if (paramsCount == 2)
                        {
                            int targetID = int.Parse(para[1]);
                            int state = int.Parse(para[2]);
                            KTGMCommandManager.SetTeamBattleAwardState(targetID, state);
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'UpdateTeamBattleAwardState' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }

                    case "ResetTeamBattleTeamData":
                    {
                        if (paramsCount == 0)
                        {
                            TeamBattle_ActivityScript.ClearTeamsData();
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'ResetTeamBattleTeamData' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }

                    case "ReloadTeamBattle":
                    {
                        if (paramsCount == 0)
                        {
                            TeamBattle.Init();
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'ReloadTeamBattle' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }
                    #endregion

                    #region Uy danh

                    case "AddPrestige":
                    {
                        if (paramsCount == 1)
                        {
                            int value = int.Parse(para[1]);
                            KTGMCommandManager.AddPrestige(player, value);
                        }
                        else if (paramsCount == 2)
                        {
                            int targetID = int.Parse(para[1]);
                            int value = int.Parse(para[2]);
                            KTGMCommandManager.AddPrestige(targetID, value);
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'AddPrestige' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }

                    #endregion Uy danh

                    #region Danh vọng

                    case "AddRepute":
                    {
                        if (paramsCount == 2)
                        {
                            int reputeID = int.Parse(para[1]);
                            int value = int.Parse(para[2]);
                            KTGMCommandManager.AddRepute(player, reputeID, value);
                        }
                        else if (paramsCount == 3)
                        {
                            int targetID = int.Parse(para[1]);
                            int reputeID = int.Parse(para[2]);
                            int value = int.Parse(para[3]);
                            KTGMCommandManager.AddRepute(targetID, reputeID, value);
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'AddRepute' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }

                    case "SetRetupe":
                    {
                        if (paramsCount == 4)
                        {
                            int targetID = int.Parse(para[1]);
                            int reputeID = int.Parse(para[2]);
                            int value = int.Parse(para[3]);
                            int exp = int.Parse(para[4]);


                            KPlayer target = KTGMCommandManager.FindPlayer(targetID);

                            KTGlobal.SetRepute(target, reputeID, value, exp);
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'AddRepute' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }

                    #endregion Danh vọng

                    #region Xóa túi đồ bản thân

                    case "ClearBag":
                    {
                        List<GoodsData> itemGDs;
                        lock (player.GoodsDataList)
                        {
                            itemGDs = player.GoodsDataList.Where(x => x.Using < 0).ToList();
                        }
                        PlayerManager.ResolveRemoveItems(player, itemGDs);
                    }
                    break;

                    #endregion Xóa túi đồ bản thân

                    #region Thay đổi danh hiệu cá nhân

                    case "ModRoleTitle":
                    {
                        if (paramsCount == 2)
                        {
                            int method = int.Parse(para[1]);
                            int titleID = int.Parse(para[2]);
                            KTGMCommandManager.ModRoleTitle(player, method, titleID);
                        }
                        else if (paramsCount == 3)
                        {
                            int targetID = int.Parse(para[1]);
                            int method = int.Parse(para[2]);
                            int titleID = int.Parse(para[3]);
                            KTGMCommandManager.ModRoleTitle(targetID, method, titleID);
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'ModRoleTitle' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }

                    #endregion Thay đổi danh hiệu cá nhân

                    #region Tu Luyện Châu

                    case "SetXiuLianZhu_TimeLeft":
                    {
                        if (paramsCount == 1)
                        {
                            int hour10 = int.Parse(para[1]);
                            KTGMCommandManager.SetXiuLianZhu_TimeLeft(player, hour10);
                        }
                        else if (paramsCount == 2)
                        {
                            int targetID = int.Parse(para[1]);
                            int hour10 = int.Parse(para[2]);
                            KTGMCommandManager.SetXiuLianZhu_TimeLeft(targetID, hour10);
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'SetXiuLianZhu_TimeLeft' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }

                    case "AddXiuLianZhu_Exp":
                    {
                        if (paramsCount == 1)
                        {
                            int exp = int.Parse(para[1]);
                            KTGMCommandManager.AddXiuLianZhu_Exp(player, exp);
                        }
                        else if (paramsCount == 2)
                        {
                            int targetID = int.Parse(para[1]);
                            int exp = int.Parse(para[2]);
                            KTGMCommandManager.AddXiuLianZhu_Exp(targetID, exp);
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'AddXiuLianZhu_Exp' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }

                    case "SetXiuLianZhu_Exp":
                    {
                        if (paramsCount == 1)
                        {
                            int exp = int.Parse(para[1]);
                            KTGMCommandManager.SetXiuLianZhu_Exp(player, exp);
                        }
                        else if (paramsCount == 2)
                        {
                            int targetID = int.Parse(para[1]);
                            int exp = int.Parse(para[2]);
                            KTGMCommandManager.SetXiuLianZhu_Exp(targetID, exp);
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'SetXiuLianZhu_Exp' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }

                    #endregion Chúc phúc

                    #region Chúc phúc

                    case "ResetPray":
                    {
                        player.LastPrayResult.Clear();
                        player.LastPrayResult = null;
                        player.SavePrayDataToDB();
                        break;
                    }
                    case "SetPrayTimes":
                    {
                        if (paramsCount == 1)
                        {
                            int value = int.Parse(para[1]);
                            KTGMCommandManager.SetPrayTimes(player, value);
                        }
                        else if (paramsCount == 2)
                        {
                            int targetID = int.Parse(para[1]);
                            int value = int.Parse(para[2]);
                            KTGMCommandManager.SetPrayTimes(targetID, value);
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'SetPrayTimes' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }

                    #endregion Chúc phúc

                    #region Sự kiện đặc biệt
                    case "ReloadSpecialEvent":
                    {
                        if (paramsCount == 0)
                        {
                            SpecialEvent.Init();
                        }
                        else
                        {
                            LogManager.WriteLog(LogTypes.Error, "Process GM Command 'ReloadSpecialEvent' Faild...\n" + "Invalid param.");
                        }
                        break;
                    }
                    #endregion


                    #region Set lại chức vụ

                    case "SetRankGiaToc":
                        {
                            int targetID = int.Parse(para[1]);
                            int Rank = int.Parse(para[2]);
                            KPlayer client = GameManager.ClientMgr.FindClient(targetID);

                            if (client != null)
                            {
                                string CMDBUILD = client.RoleID + ":" + client.FamilyID + ":" + Rank + ":" + -1;

                                string[] result = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_KT_FAMILY_CHANGE_RANK, CMDBUILD, client.ServerId);

                                int Status = Int32.Parse(result[0]);

                                if (Status > 0)
                                {
                                    client.FamilyRank = Rank;

                                    KT_TCPHandler.NotifyOthersMyTitleChanged(client);

                                    KT_TCPHandler.NotifyOtherMyGuildAndFamilyRankChanged(client);


                                   // PlayerManager.ShowMessageBox(client, "Thông báo", "Chuyển giao chức vụ thành công");

                                    client.SendPacket((int)TCPGameServerCmds.CMD_KT_FAMILY_CHANGE_RANK, string.Format("{0}:{1}", client.RoleID, client.FamilyRank));
                                }
                            }

                            break;
                        }

                    case "SetRankBangHoi":
                        {
                            int targetID = int.Parse(para[1]);
                            int Rank = int.Parse(para[2]);
                            KPlayer client = GameManager.ClientMgr.FindClient(targetID);

                            if (client != null)
                            {
                                string CMDBUILD = client.RoleID + ":" + client.GuildID + ":" + Rank;

                                byte[] ByteSendToDB = Encoding.ASCII.GetBytes(CMDBUILD);

                                string[] result = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_KT_GUILD_CHANGERANK, CMDBUILD, client.ServerId);

                                int Status = Int32.Parse(result[0]);

                                if (Status > 0)
                                {
                                    client.GuildRank = Rank;

                                    /// Thông báo danh hiệu thay đổi
                                    KT_TCPHandler.NotifyOthersMyTitleChanged(client);

                                    /// Thông báo cập nhật thông tin gia tộc và bang hội
                                    KT_TCPHandler.NotifyOtherMyGuildAndFamilyRankChanged(client);

                                    string responseData = string.Format("{0}", client.GuildRank);
                                    client.SendPacket((int)TCPGameServerCmds.CMD_KT_GUILD_CHANGERANK, responseData);
                                }
                            }
                            break;
                        }


                    #endregion

                    #region Reload XML GS
                    case "ReloadWefare":
                        {
                            CardMonthManager.Setup();
                            //CheckPointManager.Setup();
                            DownloadBounsManager.Setup();
                            EveryDayOnlineManager.Setup();
                            LevelUpEventManager.Setup();
                            RechageManager.Setup();
                            break;
                        }
                    case "ReloadSkillData":
                        {
                            KSkill.LoadSkillData();
                            break;
                        }
                    #endregion

                }
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, "Process GM Command Error...\n" + ex.ToString(), false);
            }
        }

        #region Helper

        /// <summary>
        /// Tìm người chơi có tên tương ứng
        /// </summary>
        /// <param name="playerName"></param>
        /// <returns></returns>
        private static KPlayer FindPlayer(string playerName)
        {
            KPlayer player = GameManager.ClientMgr.FindClients(x => x.RoleName == playerName).FirstOrDefault();
            return player;
        }

        /// <summary>
        /// Tìm người chơi có ID tương ứng
        /// </summary>
        /// <param name="playerID"></param>
        /// <returns></returns>
        private static KPlayer FindPlayer(int playerID)
        {
            return GameManager.ClientMgr.FindClient(playerID);
        }

        #endregion Helper

        #region Script-Lua

        /// <summary>
        /// Tải lại Script Lua mới nhất
        /// </summary>
        /// <param name="scriptID">ID Script Lua</param>
        private static void ReloadScriptLua(int scriptID)
        {
            KTLuaScript.Instance.ReloadScript(KTLuaEnvironment.LuaEnv, scriptID);
        }

        #endregion Script-Lua

        #region Trạng thái ngũ hành

        /// <summary>
        /// Thêm trạng thái ngũ hành cho đối tượng có ID tương ứng
        /// </summary>
        /// <param name="targetID"></param>
        /// <param name="stateID"></param>
        /// <param name="time"></param>
        private static void AddSeriesState(int targetID, string stateID, float time)
        {
            GameObject target = KTGMCommandManager.FindPlayer(targetID);
            if (target == null)
            {
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'AddSeriesState' Error...\n" + "Target ID '" + targetID + "' not found.");
                return;
            }

            KTGMCommandManager.AddSeriesState(target, stateID, time);
        }

        /// <summary>
        /// Thêm trạng thái ngũ hành
        /// </summary>
        /// <param name="target">Đối tượng</param>
        /// <param name="stateID">Trạng thái</param>
        /// <param name="time">Thời gian (giây)</param>
        private static void AddSeriesState(GameObject target, string stateID, float time)
        {
            /// Kiểm tra trạng thái
            if (!Utils.TryParseEnum<KE_STATE>(stateID, out KE_STATE state))
            {
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'AddSeriesState' Error...\n" + "State ID '" + stateID + "' not found.");
                return;
            }

            /// Kiểm tra thời gian
            if (time < 0 || time > 60)
            {
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'AddSeriesState' Error...\n" + "Duration is INVALID, must be between 0 and 60s.");
                return;
            }

            /// Thêm trạng thái vào đối tượng
            target.AddSpecialState(target, new UnityEngine.Vector2((int) target.CurrentPos.X, (int) target.CurrentPos.Y), state, (int) (time * 18), (int) (time * 18), true);
        }

        /// <summary>
        /// Xóa trạng thái ngũ hành khỏi đối tượng có ID tương ứng
        /// </summary>
        /// <param name="targetID"></param>
        /// <param name="stateID"></param>
        private static void RemoveSeriesState(int targetID, string stateID)
        {
            GameObject target = KTGMCommandManager.FindPlayer(targetID);
            if (target == null)
            {
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'RemoveSeriesState' Error...\n" + "Target ID '" + targetID + "' not found.");
                return;
            }

            KTGMCommandManager.RemoveSeriesState(target, stateID);
        }

        /// <summary>
        /// Xóa trạng thái ngũ hành
        /// </summary>
        /// <param name="target">Đối tượng</param>
        /// <param name="stateID">ID trạng thái</param>
        private static void RemoveSeriesState(GameObject target, string stateID)
        {
            /// Kiểm tra trạng thái
            if (!Utils.TryParseEnum<KE_STATE>(stateID, out KE_STATE state))
            {
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'RemoveSeriesState' Error...\n" + "State ID '" + stateID + "' not found.");
                return;
            }

            /// Xóa trạng thái khỏi đối tượng
            target.RemoveSpecialState(state, true);
        }

        #endregion Trạng thái ngũ hành

        #region Gia nhập phái

        /// <summary>
        /// Thiết lập môn phái cho đối tượng tương ứng
        /// </summary>
        /// <param name="targetID">ID đối tượng</param>
        /// <param name="factionID">ID phái</param>
        private static void JoinFaction(int targetID, int factionID)
        {
            KPlayer target = KTGMCommandManager.FindPlayer(targetID);
            /// Kiểm tra đối tượng
            if (target == null)
            {
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'Heal' Error...\n" + "Target not found.");
                return;
            }

            /// Thực hiện phục hồi cho mục tiêu
            KTGMCommandManager.JoinFaction(target, factionID);
        }

        /// <summary>
        /// Thiết lập môn phái cho đối tượng tương ứng
        /// </summary>
        /// <param name="player">Người chơi</param>
        /// <param name="factionID">ID phái</param>
        private static void JoinFaction(KPlayer player, int factionID)
        {
            /// Gia nhập môn phái
            PlayerManager.JoinFaction(player, factionID);
        }

        #endregion Gia nhập phái

        #region Trị liệu

        /// <summary>
        /// Trị liệu phục hồi sinh lực, nội lực, thể lực cho đối tượng
        /// </summary>
        /// <param name="targetID">ID đối tượng</param>
        private static void Heal(int targetID)
        {
            KPlayer target = KTGMCommandManager.FindPlayer(targetID);
            /// Kiểm tra đối tượng
            if (target == null)
            {
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'Heal' Error...\n" + "Target not found.");
                return;
            }

            /// Thực hiện phục hồi cho mục tiêu
            KTGMCommandManager.Heal(target);
        }

        /// <summary>
        /// Trị liệu phục hồi sinh lực, nội lực, thể lực
        /// </summary>
        /// <param name="player">Người chơi</param>
        private static void Heal(KPlayer player)
        {
            /// Phục hồi sinh lực
            player.m_CurrentLife = player.m_CurrentLifeMax;
            /// Phục hồi nội lực
            player.m_CurrentMana = player.m_CurrentManaMax;
            /// Phục hồi thể lực
            player.m_CurrentStamina = player.m_CurrentStaminaMax;
        }

        #endregion Trị liệu

        #region Thông tin người chơi

        /// <summary>
        /// Trả ra thông tin người chơi tương ứng
        /// </summary>
        /// <param name="player">Thông tin hiển thị cho người chơi</param>
        /// <param name="playerName">Tên người chơi cần tìm</param>
        private static void GetPlayerInfo(KPlayer player, string playerName)
        {
            KPlayer target = KTGMCommandManager.FindPlayer(playerName);
            /// Kiểm tra đối tượng
            if (target == null)
            {
                PlayerManager.ShowNotification(player, string.Format("Không tìm thấy thông tin người chơi {0}!", playerName));
                return;
            }

            /// Bản đồ tương ứng
            GameMap gameMap = GameManager.MapMgr.GetGameMap(target.CurrentMapCode);

            string msg = string.Format("Thông tin người chơi: {0}({1}), vị trí: {4} ({2}, {3})", target.RoleName, target.RoleID, (int) target.CurrentPos.X, (int) target.CurrentPos.Y, gameMap == null ? "Chưa rõ" : gameMap.MapName);
            ///PlayerManager.ShowNotification(player, msg);
        }

        #endregion Thông tin người chơi

        #region Dịch chuyển

        /// <summary>
        /// Dịch chuyển đối tượng có ID tương ứng
        /// </summary>
        /// <param name="targetID">ID đối tượng</param>
        /// <param name="mapCode">ID bản đồ</param>
        /// <param name="posX">Vị trí X</param>
        /// <param name="posY">Vị trí Y</param>
        private static void GoTo(int targetID, int mapCode, int posX, int posY)
        {
            KPlayer target = KTGMCommandManager.FindPlayer(targetID);
            /// Kiểm tra đối tượng
            if (target == null)
            {
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'GoTo' Error...\n" + "Target not found.");
                return;
            }

            /// Thực hiện dịch chuyển đối tượng tương ứng
            KTGMCommandManager.GoTo(target, mapCode, posX, posY);
        }

        /// <summary>
        /// Dịch chuyển người chơi
        /// </summary>
        /// <param name="player">Người chơi</param>
        /// <param name="mapCode">ID bản đồ</param>
        /// <param name="posX">Vị trí X</param>
        /// <param name="posY">Vị trí Y</param>
        private static void GoTo(KPlayer player, int mapCode, int posX, int posY)
        {
            /// Lấy dữ liệu bản đồ đích đến
            if (GameManager.MapMgr.DictMaps.TryGetValue(mapCode, out GameMap gameMap))
            {
                /// Nếu bản đồ đích khác bản đồ hiện tại
                if (player.CurrentMapCode != mapCode)
                {
                    GameManager.ClientMgr.NotifyChangeMap(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, player, mapCode, posX, posY, (int) player.CurrentDir);
                }
                else
                {
                    GameManager.ClientMgr.NotifyOthersGoBack(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, player, posX, posY, (int) player.CurrentDir);
                }
            }
            else
            {
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'GoTo' Error...\n" + "Map not exist.");
            }
        }

        #endregion Dịch chuyển

        #region Kỹ năng và Buff

        /// <summary>
        /// Thêm kỹ năng cho người chơi tương ứng
        /// </summary>
        /// <param name="targetID">ID đối tượng</param>
        /// <param name="skillID">ID kỹ năng</param>
        /// <param name="level">Cấp độ</param>
        private static void AddSkill(int targetID, int skillID, int level)
        {
            KPlayer target = KTGMCommandManager.FindPlayer(targetID);
            /// Kiểm tra đối tượng
            if (target == null)
            {
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'AddSkill' Error...\n" + "Target not found.");
                return;
            }

            KTGMCommandManager.AddSkill(target, skillID, level);
        }

        /// <summary>
        /// Thêm kỹ năng cho người chơi tương ứng
        /// </summary>
        /// <param name="player">Người chơi</param>
        /// <param name="skillID">ID kỹ năng</param>
        /// <param name="level">Cấp độ</param>
        private static void AddSkill(KPlayer player, int skillID, int level)
        {
            player.Skills.AddSkill(skillID);
            player.Skills.AddSkillLevel(skillID, level);
        }

        /// <summary>
        /// Xóa kỹ năng khỏi đối tượng có ID tương ứng
        /// </summary>
        /// <param name="targetID">ID đối tượng</param>
        /// <param name="skillID">ID kỹ năng</param>
        private static void RemoveSkill(int targetID, int skillID)
        {
            KPlayer target = KTGMCommandManager.FindPlayer(targetID);
            /// Kiểm tra đối tượng
            if (target == null)
            {
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'RemoveSkill' Error...\n" + "Target not found.");
                return;
            }

            KTGMCommandManager.RemoveSkill(target, skillID);
        }

        /// <summary>
        /// Xóa kỹ năng khỏi người chơi tương ứng
        /// </summary>
        /// <param name="player">Người chơi</param>
        /// <param name="skillID">ID kỹ năng</param>
        private static void RemoveSkill(KPlayer player, int skillID)
        {
            player.Skills.RemoveSkill(skillID);
        }

        /// <summary>
        /// Thêm Buff cho đối tượng có ID tương ứng
        /// </summary>
        /// <param name="targetID">ID đối tượng</param>
        /// <param name="skillID">ID kỹ năng</param>
        /// <param name="level">Cấp độ</param>
        private static void AddBuff(int targetID, int skillID, int level)
        {
            KPlayer target = KTGMCommandManager.FindPlayer(targetID);
            /// Kiểm tra đối tượng
            if (target == null)
            {
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'AddSkill' Error...\n" + "Target not found.");
                return;
            }

            KTGMCommandManager.AddBuff(target, skillID, level);
        }

        /// <summary>
        /// Thêm Buff tương ứng cho đối tượng
        /// </summary>
        /// <param name="target">Đối tượng</param>
        /// <param name="skillID">ID kỹ năng</param>
        /// <param name="level">Cấp độ</param>
        private static void AddBuff(GameObject target, int skillID, int level)
        {
            SkillDataEx skillData = KSkill.GetSkillData(skillID);
            if (skillData == null)
            {
                return;
            }
            SkillLevelRef skill = new SkillLevelRef()
            {
                Data = skillData,
                AddedLevel = level,
                BonusLevel = 0,
                CanStudy = false,
            };

            PropertyDictionary skillPd = skill.Properties;
            int duration = -1;
            if (skillPd.ContainsKey((int) MAGIC_ATTRIB.magic_skill_statetime))
            {
                if (skillPd.Get<KMagicAttrib>((int) MAGIC_ATTRIB.magic_skill_statetime).nValue[0] == -1)
                {
                    duration = -1;
                }
                else
                {
                    duration = skillPd.Get<KMagicAttrib>((int) MAGIC_ATTRIB.magic_skill_statetime).nValue[0] * 1000 / 18;
                }
            }

            target.Buffs.AddBuff(new BuffDataEx()
            {
                Skill = skill,
                Duration = duration,
                LoseWhenUsingSkill = false,
                SaveToDB = false,
                StartTick = KTGlobal.GetCurrentTimeMilis(),
            });
        }

        /// <summary>
        /// Xóa Buff khỏi đối tượng có ID tương ứng
        /// </summary>
        /// <param name="targetID">ID đối tượng</param>
        /// <param name="skillID">ID kỹ năng</param>
        private static void RemoveBuff(int targetID, int skillID)
        {
            KPlayer target = KTGMCommandManager.FindPlayer(targetID);
            /// Kiểm tra đối tượng
            if (target == null)
            {
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'RemoveBuff' Error...\n" + "Target not found.");
                return;
            }

            KTGMCommandManager.RemoveBuff(target, skillID);
        }

        /// <summary>
        /// Xóa Buff khỏi đối tượng tương ứng
        /// </summary>
        /// <param name="target">Đối tượng</param>
        /// <param name="skillID">ID kỹ năng</param>
        private static void RemoveBuff(GameObject target, int skillID)
        {
            target.Buffs.RemoveBuff(skillID);
        }

        /// <summary>
        /// Làm mới toàn bộ dữ liệu phục hồi kỹ năng của đối tượng có ID tương ứng
        /// </summary>
        /// <param name="targetID">ID đối tượng</param>
        /// <param name="skillID">ID kỹ năng</param>
        private static void ResetAllSkillCooldown(int targetID)
        {
            KPlayer target = KTGMCommandManager.FindPlayer(targetID);
            /// Kiểm tra đối tượng
            if (target == null)
            {
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'ResetAllSkillCooldown' Error...\n" + "Target not found.");
                return;
            }

            KTGMCommandManager.ResetAllSkillCooldown(target);
        }

        /// <summary>
        /// Làm mới toàn bộ dữ liệu phục hồi kỹ năng của đối tượng tương ứng
        /// </summary>
        /// <param name="target">Đối tượng</param>
        /// <param name="skillID">ID kỹ năng</param>
        private static void ResetAllSkillCooldown(KPlayer target)
        {
            target.Skills.ClearSkillCooldownList();
        }

        #endregion Kỹ năng và Buff

        #region System Chat

        /// <summary>
        /// Gửi tin nhắn dưới kênh hệ thống
        /// </summary>
        /// <param name="message">Tin nhắn</param>
        private static void SendSystemChat(string message)
        {
            KTGlobal.SendSystemChat(message);
        }

        /// <summary>
        /// Gửi tin nhắn dưới kênh hệ thống đồng thời hiển thị ở dòng chữ chạy ngang trêu đầu
        /// </summary>
        /// <param name="message"></param>
        public static void SendSystemEventNotification(string message)
        {
            KTGlobal.SendSystemEventNotification(message);
        }

        #endregion System Chat

        #region Tạo vật phẩm

        /// <summary>
        /// Tạo vật phẩm số lượng tương ứng
        /// </summary>
        /// <param name="targetID">ID đối tượng</param>
        /// <param name="itemID">ID vật phẩm</param>
        /// <param name="quantity">Số lượng</param>
        private static void CreateItem(int targetID, int itemID, int quantity)
        {
            KPlayer target = KTGMCommandManager.FindPlayer(targetID);
            /// Kiểm tra đối tượng
            if (target == null)
            {
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'CreateItem' Error...\n" + "Target not found.");
                return;
            }

            KTGMCommandManager.CreateItem(target, itemID, quantity, -1);
        }

        /// <summary>
        /// Tạo vật phẩm số lượng tương ứng
        /// </summary>
        /// <param name="player">Người chơi</param>
        /// <param name="itemID">ID vật phẩm</param>
        /// <param name="quantity">Số lượng</param>
        private static void CreateItem(KPlayer player, int itemID, int quantity, int EXP)
        {
            /// Nếu vật phẩm tồn tại
            if (ItemManager._TotalGameItem.ContainsKey(itemID))
            {
                string TimeUsing = Global.ConstGoodsEndTime;

                if (EXP > 0)
                {
                    DateTime dt = DateTime.Now.AddMinutes(EXP);

                    // "1900-01-01 12:00:00";
                    TimeUsing = dt.ToString("yyyy-MM-dd HH:mm:ss");
                }
                ItemManager.CreateItem(Global._TCPManager.TcpOutPacketPool, player, itemID, quantity, 0, "GMCOMMAND", true, 0, false, TimeUsing);
            }
            else
            {
                PlayerManager.ShowNotification(player, "Vật phẩm không tồn tại!");
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'CreateItem' Error...\n" + "Item ID = " + itemID + " not found.");
            }
        }

        #endregion Tạo vật phẩm

        #region Thiết lập cấp cường hóa cho trang bị

        /// <summary>
        /// Tạo vật phẩm số lượng tương ứng
        /// </summary>
        /// <param name="targetID">ID đối tượng</param>
        /// <param name="slot">Vị trí trang bị trong túi</param>
        /// <param name="level">Cấp cường hóa</param>
        private static void EquipEnhance(int targetID, int slot, int level)
        {
            KPlayer target = KTGMCommandManager.FindPlayer(targetID);
            /// Kiểm tra đối tượng
            if (target == null)
            {
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'CreateItem' Error...\n" + "Target not found.");
                return;
            }

            KTGMCommandManager.EquipEnhance(target, slot, level);
        }

        /// <summary>
        /// Tạo vật phẩm số lượng tương ứng
        /// </summary>
        /// <param name="player">Người chơi</param>
        /// <param name="slot">Vị trí trang bị trong túi</param>
        /// <param name="level">Cấp cường hóa</param>
        private static void EquipEnhance(KPlayer player, int slot, int level)
        {
            /// Vật phẩm tại vị trí tương ứng trong túi
            GoodsData itemGD = player.GoodsDataList?.Where(x => x.Using == -1 && x.BagIndex == slot).FirstOrDefault();
            /// Nếu vật phẩm tại vị trí tương ứng không tồn tại
            if (itemGD == null)
            {
                PlayerManager.ShowNotification(player, "Không tồn tại vật phẩm tại vị trí " + slot + " trong túi đồ!");
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'EquipEnhance' Error...\n" + "BagPos = " + slot + " has no item.");
                return;
            }
            /// Nếu vật phẩm không tồn tại trong hệ thống
            if (!ItemManager._TotalGameItem.TryGetValue(itemGD.GoodsID, out ItemData itemData))
            {
                PlayerManager.ShowNotification(player, "Không tồn tại vật phẩm tại vị trí " + slot + " trong túi đồ!");
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'EquipEnhance' Error...\n" + "BagPos = " + slot + ", Item ID = " + itemGD.GoodsID + " not found.");
                return;
            }

            /// Nếu không phải trang bị
            if (!ItemManager.KD_ISEQUIP(itemData.Genre))
            {
                PlayerManager.ShowNotification(player, "Vật phẩm tại vị trí " + slot + " trong túi đồ không phải trang bị, không thể cường hóa được!");
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'EquipEnhance' Error...\n" + "BagPos = " + slot + ", Item ID = " + itemGD.GoodsID + " is not equip.");
                return;
            }

            ItemManager.SetEquipForgeLevel(itemGD, player, level);
        }

        #endregion Thiết lập cấp cường hóa cho trang bị

        #region Thêm tiền

        /// <summary>
        /// Thêm tiền số lượng tương ứng
        /// </summary>
        /// <param name="targetID">ID đối tượng</param>
        /// <param name="type">Loại tiền (1: Bạc thường, 2: Bạc khóa, 3: Đồng thường, 4: Đồng khóa)</param>
        /// <param name="amount">Số lượng</param>
        private static void AddMoney(int targetID, int type, int amount)
        {
            KPlayer target = KTGMCommandManager.FindPlayer(targetID);
            /// Kiểm tra đối tượng
            if (target == null)
            {
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'AddMoney' Error...\n" + "Target not found.");
                return;
            }

            KTGMCommandManager.AddMoney(target, type, amount);
        }

        /// <summary>
        /// Thêm tiền số lượng tương ứng
        /// </summary>
        /// <param name="player">Người chơi</param>
        /// <param name="type">Loại tiền (1: Bạc thường, 2: Bạc khóa, 3: Đồng thường, 4: Đồng khóa)</param>
        /// <param name="amount">Số lượng</param>
        private static void AddMoney(KPlayer player, int type, int amount)
        {
            switch (type)
            {
                case 1:
                {
                    GameManager.ClientMgr.AddMoney(player, amount, "GMCommand");
                    break;
                }
                case 2:
                {
                    GameManager.ClientMgr.AddUserBoundMoney(player, amount, "GMCommand");
                    break;
                }
                case 3:
                {
                    GameManager.ClientMgr.AddToken(Global._TCPManager.MySocketListener, Global._TCPManager.tcpClientPool, Global._TCPManager.TcpOutPacketPool, player, amount, "GMCommand");
                    break;
                }
                case 4:
                {
                    GameManager.ClientMgr.AddBoundToken(Global._TCPManager.MySocketListener, Global._TCPManager.tcpClientPool, Global._TCPManager.TcpOutPacketPool, player, amount, "GMCommand");
                    break;
                }
                case 5:
                {
                    // Add Tích lũy cá nhân ở bang hội
                    GameManager.ClientMgr.AddGuildMoney(Global._TCPManager.MySocketListener, Global._TCPManager.tcpClientPool, Global._TCPManager.TcpOutPacketPool, player, amount, "GMCommand");
                    break;
                }
                case 6:
                {
                    // Add Tích lũy cá nhân ở bang hội
                    KTGlobal.UpdateGuildMoney(500, player.GuildID, player);
                    break;
                }
            }
        }

        #endregion Thêm tiền

        #region Thêm vật phẩm rơi ở MAP

        /// <summary>
        /// Thêm vật phẩm rơi ở MAP theo ID quái tương ứng
        /// </summary>
        /// <param name="targetID">ID đối tượng</param>
        /// <param name="monsterID">ID cấu hình quái tương ứng bọc</param>
        private static void AddDropItem(int targetID, int monsterID)
        {
            KPlayer target = KTGMCommandManager.FindPlayer(targetID);
            /// Kiểm tra đối tượng
            if (target == null)
            {
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'AddDropItem' Error...\n" + "Target not found.");
                return;
            }

            KTGMCommandManager.AddDropItem(target, monsterID);
        }

        /// <summary>
        /// Thêm vật phẩm rơi ở MAP theo ID quái tương ứng
        /// </summary>
        /// <param name="player">Người chơi</param>
        /// <param name="monsterID">ID cấu hình quái tương ứng bọc</param>
        private static void AddDropItem(KPlayer player, int monsterID)
        {
            // ItemDropManager.GetDropMonsterDie(monsterID, player);
        }

        #endregion Thêm vật phẩm rơi ở MAP

        #region Hoạt động

        /// <summary>
        /// Chủ động bắt đầu sự kiện có ID tương ứng
        /// </summary>
        /// <param name="activityID"></param>
        private static void StartActivity(int activityID)
        {
            KTActivityManager.StartActivity(activityID);
        }

        /// <summary>
        /// Chủ động kết thúc sự kiện có ID tương ứng
        /// </summary>
        /// <param name="activityID"></param>
        private static void StopActivity(int activityID)
        {
            KTActivityManager.StopActivity(activityID);
        }

        #endregion Hoạt động

        #region Thiết lập sát khí

        /// <summary>
        /// Thiết lập số điểm sát khí tương ứng cho đối tượng
        /// </summary>
        /// <param name="targetID">ID đối tượng</param>
        /// <param name="value">Giá trị</param>
        private static void SetPKValue(int targetID, int value)
        {
            KPlayer target = KTGMCommandManager.FindPlayer(targetID);
            /// Kiểm tra đối tượng
            if (target == null)
            {
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'SetPKValue' Error...\n" + "Target not found.");
                return;
            }

            KTGMCommandManager.SetPKValue(target, value);
        }

        /// <summary>
        /// Thiết lập số điểm sát khí tương ứng cho đối tượng
        /// </summary>
        /// <param name="player">Người chơi</param>
        /// <param name="value">Giá trị</param>
        private static void SetPKValue(KPlayer player, int value)
        {
            if (value < 0 || value > 10)
            {
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'SetPKValue' Error...\n" + "Value must be between [0-10].");
                return;
            }

            player.PKValue = value;
        }

        #endregion Thiết lập sát khí

        #region Thiết lập cấp độ và kinh nghiệm kỹ năng sống

        /// <summary>
        /// Thiết lập cấp độ và kinh nghiệm kỹ năng sống cho đối tượng
        /// </summary>
        /// <param name="targetID">ID đối tượng</param>
        /// <param name="lifeSkillID">ID kỹ năng sống</param>
        /// <param name="level">Cấp độ</param>
        /// <param name="exp">Kinh nghiệm</param>
        private static void SetLifeSkillLevelAndExp(int targetID, int lifeSkillID, int level, int exp)
        {
            KPlayer target = KTGMCommandManager.FindPlayer(targetID);
            /// Kiểm tra đối tượng
            if (target == null)
            {
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'SetLifeSkillLevelAndExp' Error...\n" + "Target not found.");
                return;
            }

            KTGMCommandManager.SetLifeSkillLevelAndExp(target, lifeSkillID, level, exp);
        }

        /// <summary>
        /// Thiết lập cấp độ và kinh nghiệm kỹ năng sống cho đối tượng
        /// </summary>
        /// <param name="player">Người chơi</param>
        /// <param name="lifeSkillID">ID kỹ năng sống</param>
        /// <param name="level">Cấp độ</param>
        /// <param name="exp">Kinh nghiệm</param>
        private static void SetLifeSkillLevelAndExp(KPlayer player, int lifeSkillID, int level, int exp)
        {
            if (level < 0 || level > 120)
            {
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'SetLifeSkillLevelAndExp' Error...\n" + "Value must be between [0-120].");
                return;
            }

            LifeSkillPram lifeSkillParam = player.GetLifeSkill(lifeSkillID);
            if (lifeSkillParam == null)
            {
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'SetLifeSkillLevelAndExp' Error...\n" + "Can not find life skill ID = " + lifeSkillID + ".");
                return;
            }

            LifeSkillExp lifeSkillExp = ItemCraftingManager._LifeSkill.TotalExp.Where(x => x.Level == level).FirstOrDefault();
            if (lifeSkillExp == null)
            {
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'SetLifeSkillLevelAndExp' Error...\n" + "Can not find max exp for life skill ID = " + lifeSkillID + ".");
                return;
            }

            if (exp < 0)
            {
                exp = 0;
            }
            if (exp > lifeSkillExp.Exp - 1)
            {
                exp = lifeSkillExp.Exp - 1;
            }

            player.SetLifeSkillParam(lifeSkillID, level, exp);
        }

        #endregion Thiết lập cấp độ và kinh nghiệm kỹ năng sống

        #region Reset cấp độ và kinh nghiệm kỹ năng sống

        /// <summary>
        /// Reset cấp độ và kinh nghiệm kỹ năng sống cho đối tượng
        /// </summary>
        /// <param name="targetID">ID đối tượng</param>
        private static void ResetLifeSkillLevelAndExp(int targetID)
        {
            KPlayer target = KTGMCommandManager.FindPlayer(targetID);
            /// Kiểm tra đối tượng
            if (target == null)
            {
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'ResetLifeSkillLevelAndExp' Error...\n" + "Target not found.");
                return;
            }

            KTGMCommandManager.ResetLifeSkillLevelAndExp(target);
        }

        /// <summary>
        /// Reset cấp độ và kinh nghiệm kỹ năng sống cho đối tượng
        /// </summary>
        /// <param name="player">Người chơi</param>
        private static void ResetLifeSkillLevelAndExp(KPlayer player)
        {
            Dictionary<int, LifeSkillPram> lifeSkills = new Dictionary<int, LifeSkillPram>();
            for (int i = 1; i < 12; i++)
            {
                LifeSkillPram param = new LifeSkillPram();
                param.LifeSkillID = i;
                param.LifeSkillLevel = 1;
                param.LifeSkillExp = 0;
                lifeSkills[i] = param;
            }
            player.SetLifeSkills(lifeSkills);
        }

        #endregion Reset cấp độ và kinh nghiệm kỹ năng sống

        #region Thiết lập điểm tinh lực, hoạt lực

        /// <summary>
        /// Thiết lập cấp độ và kinh nghiệm kỹ năng sống cho đối tượng
        /// </summary>
        /// <param name="targetID">ID đối tượng</param>
        /// <param name="gatherPoint">Tinh lực</param>
        /// <param name="makePoint">Hoạt lực</param>
        private static void AddGatherMakePoint(int targetID, int gatherPoint, int makePoint)
        {
            KPlayer target = KTGMCommandManager.FindPlayer(targetID);
            /// Kiểm tra đối tượng
            if (target == null)
            {
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'AddGatherMakePoint' Error...\n" + "Target not found.");
                return;
            }

            KTGMCommandManager.AddGatherMakePoint(target, gatherPoint, makePoint);
        }

        /// <summary>
        /// Thiết lập cấp độ và kinh nghiệm kỹ năng sống cho đối tượng
        /// </summary>
        /// <param name="player">Người chơi</param>
        /// <param name="gatherPoint">Tinh lực</param>
        /// <param name="makePoint">Hoạt lực</param>
        private static void AddGatherMakePoint(KPlayer player, int gatherPoint, int makePoint)
        {
            player.ChangeCurMakePoint(makePoint);
            player.ChangeCurGatherPoint(gatherPoint);
        }

        #endregion Thiết lập điểm tinh lực, hoạt lực

        #region Thiết lập danh hiệu tạm thời

        /// <summary>
        /// Thiết lập danh hiệu tạm thời cho đối tượng
        /// </summary>
        /// <param name="targetID">ID đối tượng</param>
        /// <param name="title">Danh hiệu</param>
        private static void SetTempTitle(int targetID, string title)
        {
            KPlayer target = KTGMCommandManager.FindPlayer(targetID);
            /// Kiểm tra đối tượng
            if (target == null)
            {
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'SetTempTitle' Error...\n" + "Target not found.");
                return;
            }

            KTGMCommandManager.SetTempTitle(target, title);
        }

        /// <summary>
        /// Thiết lập cấp độ và kinh nghiệm kỹ năng sống cho đối tượng
        /// </summary>
        /// <param name="player">Người chơi</param>
        /// <param name="title">Danh hiệu</param>
        private static void SetTempTitle(KPlayer player, string title)
        {
            player.TempTitle = title;
        }

        #endregion Thiết lập danh hiệu tạm thời

        #region Gửi thư

        /// <summary>
        /// Gửi thư cho đối tượng tương ứng
        /// </summary>
        /// <param name="targetID">ID đối tượng</param>
        /// <param name="title">Tiêu đề</param>
        /// <param name="content">Nội dung</param>
        /// <param name="items">Danh sách vật phẩm đính kèm</param>
        /// <param name="boundMoney">Bạc khóa đính kèm</param>
        /// <param name="boundToken">Đồng khóa đính kèm</param>
        private static void SendSystemMail(int targetID, string title, string content, string items, int boundMoney, int boundToken)
        {
            KPlayer target = KTGMCommandManager.FindPlayer(targetID);
            /// Kiểm tra đối tượng
            if (target == null)
            {
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'SendMail' Error...\n" + "Target not found.");
                return;
            }

            KTGMCommandManager.SendSystemMail(target, title, content, items, boundMoney, boundToken);
        }

        /// <summary>
        /// Thiết lập cấp độ và kinh nghiệm kỹ năng sống cho đối tượng
        /// </summary>
        /// <param name="player">Người chơi</param>
        /// <param name="title">Tiêu đề</param>
        /// <param name="content">Nội dung</param>
        /// <param name="items">Danh sách vật phẩm đính kèm</param>
        /// <param name="boundMoney">Bạc khóa đính kèm</param>
        /// <param name="boundToken">Đồng khóa đính kèm</param>
        public static void SendSystemMail(KPlayer player, string title, string content, string items, int boundMoney, int boundToken)
        {
            /// Nếu có vật phẩm đính kèm
            if (!string.IsNullOrEmpty(items))
            {
                /// Danh sách vật phẩm đính kèm
                List<Tuple<int, int, int, int>> stickItems = new List<Tuple<int, int, int, int>>();

                foreach (string itemInfo in items.Split('|'))
                {
                    string[] para = itemInfo.Split('_');

                    int itemID, count, binding, enhanceLevel;
                    if (para.Length != 4)
                    {
                        LogManager.WriteLog(LogTypes.Error, "Process GM Command 'SendMail' Error...\n" + "Incorrect item parameters.");
                        return;
                    }
                    else if (!int.TryParse(para[0], out itemID) || !int.TryParse(para[1], out count) || !int.TryParse(para[2], out binding) || !int.TryParse(para[3], out enhanceLevel))
                    {
                        LogManager.WriteLog(LogTypes.Error, "Process GM Command 'SendMail' Error...\n" + "Incorrect item parameters.");
                        return;
                    }

                    stickItems.Add(new Tuple<int, int, int, int>(itemID, count, binding, enhanceLevel));
                }

                /// Thực hiện thêm vật phẩm
                KTMailManager.SendSystemMailToPlayerWithItemIDs(player, stickItems, title, content, boundMoney, boundToken);
            }
            /// Nếu không có vật phẩm đính kèm
            else
            {
                /// Thực hiện thêm vật phẩm
                KTMailManager.SendSystemMailToPlayer(player, title, content, boundMoney, boundToken);
            }
        }

        #endregion Gửi thư

        #region Thêm kinh nghiệm

        /// <summary>
        /// Thêm kinh nghiệm cho người chơi tương ứng
        /// </summary>
        /// <param name="targetID">ID đối tượng</param>
        /// <param name="exp">Kinh nghiệm</param>
        private static void AddExp(int targetID, int exp)
        {
            KPlayer target = KTGMCommandManager.FindPlayer(targetID);
            /// Kiểm tra đối tượng
            if (target == null)
            {
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'AddExp' Error...\n" + "Target not found.");
                return;
            }

            KTGMCommandManager.AddExp(target, exp);
        }

        /// <summary>
        /// Thêm kinh nghiệm cho người chơi tương ứng
        /// </summary>
        /// <param name="player">Người chơi</param>
        /// <param name="exp">Kinh nghiệm</param>
        private static void AddExp(KPlayer player, int exp)
        {
            GameManager.ClientMgr.ProcessRoleExperience(player, exp, true, false, true, "GMCommand");
        }

        #endregion Thêm kinh nghiệm

        #region Thiết lập cấp độ

        /// <summary>
        /// Thiết lập cấp độ cho người chơi tương ứng
        /// </summary>
        /// <param name="targetID">ID đối tượng</param>
        /// <param name="level">Cấp độ</param>
        private static void SetLevel(int targetID, int level)
        {
            KPlayer target = KTGMCommandManager.FindPlayer(targetID);
            /// Kiểm tra đối tượng
            if (target == null)
            {
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'SetLevel' Error...\n" + "Target not found.");
                return;
            }

            KTGMCommandManager.SetLevel(target, level);
        }

        /// <summary>
        /// Thiết lập cấp độ cho người chơi tương ứng
        /// </summary>
        /// <param name="player">Người chơi</param>
        /// <param name="level">Cấp độ</param>
        private static void SetLevel(KPlayer player, int level)
        {
            GameManager.ClientMgr.SetRoleLevel(player, level);
            Global.ForceCloseClient(player, "GMCommand");
        }

        #endregion Thêm kinh nghiệm

        #region Reset số lượt Đoán hoa đăng

        /// <summary>
        /// Reset số lượt Đoán hoa đăng trong ngày
        /// </summary>
        /// <param name="targetID">ID đối tượng</param>
        /// <param name="eventID">Loại phụ bản</param>
        private static void ResetKnowledgeChallengeQuestions(int targetID)
        {
            KPlayer target = KTGMCommandManager.FindPlayer(targetID);
            /// Kiểm tra đối tượng
            if (target == null)
            {
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'ResetKnowledgeChallengeQuestions' Error...\n" + "Target not found.");
                return;
            }

            KTGMCommandManager.ResetKnowledgeChallengeQuestions(target);
        }

        /// <summary>
        /// Reset số lượt Đoán hoa đăng trong ngày của đối tượng
        /// </summary>
        /// <param name="player">Người chơi</param>
        /// <param name="eventID">Loại phụ bản</param>
        private static void ResetKnowledgeChallengeQuestions(KPlayer player)
        {
            player.SetValueOfDailyRecore((int) AcitvityRecore.KnowledgeChallenge_TotalQuestions, 0);
        }

        #endregion

        #region Thiết lập lại số lượt phụ bản

        /// <summary>
        /// Reset giá trị số lượt tham gia phụ bản tương ứng trong ngày
        /// </summary>
        /// <param name="targetID">ID đối tượng</param>
        /// <param name="eventID">Loại phụ bản</param>
        private static void ResetCopySceneEnterTimes(int targetID, AcitvityRecore eventID)
        {
            KPlayer target = KTGMCommandManager.FindPlayer(targetID);
            /// Kiểm tra đối tượng
            if (target == null)
            {
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'ResetCopySceneEnterTimes' Error...\n" + "Target not found.");
                return;
            }

            KTGMCommandManager.ResetCopySceneEnterTimes(target, eventID);
        }

        /// <summary>
        /// Reset giá trị số lượt tham gia phụ bản tương ứng trong ngày của đối tượng
        /// </summary>
        /// <param name="player">Người chơi</param>
        /// <param name="eventID">Loại phụ bản</param>
        private static void ResetCopySceneEnterTimes(KPlayer player, AcitvityRecore eventID)
        {
            CopySceneEventManager.SetCopySceneTotalEnterTimesToday(player, eventID, 0);
        }

        #endregion Thiết lập lại số lượt phụ bản

        #region Thêm uy danh

        /// <summary>
        /// Thêm uy danh cho người chơi tương ứng
        /// </summary>
        /// <param name="targetID">ID đối tượng</param>
        /// <param name="value">Số uy danh thêm vào</param>
        private static void AddPrestige(int targetID, int value)
        {
            KPlayer target = KTGMCommandManager.FindPlayer(targetID);
            /// Kiểm tra đối tượng
            if (target == null)
            {
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'AddPrestige' Error...\n" + "Target not found.");
                return;
            }

            KTGMCommandManager.AddPrestige(target, value);
        }

        /// <summary>
        /// Thêm uy danh cho người chơi tương ứng
        /// </summary>
        /// <param name="player">Người chơi</param>
        /// <param name="value">Số uy danh thêm vào</param>
        private static void AddPrestige(KPlayer player, int value)
        {
            player.Prestige += value;
        }

        #endregion Thêm uy danh

        #region Thêm danh vọng

        /// <summary>
        /// Thêm danh vọng cho người chơi tương ứng
        /// </summary>
        /// <param name="targetID">ID đối tượng</param>
        /// <param name="reputeID">Danh vọng</param>
        /// <param name="value">Giá trị</param>
        private static void AddRepute(int targetID, int reputeID, int value)
        {
            KPlayer target = KTGMCommandManager.FindPlayer(targetID);
            /// Kiểm tra đối tượng
            if (target == null)
            {
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'AddRepute' Error...\n" + "Target not found.");
                return;
            }

            KTGMCommandManager.AddRepute(target, reputeID, value);
        }

        /// <summary>
        /// Thêm danh vọng cho người chơi tương ứng
        /// </summary>
        /// <param name="player">Người chơi</param>
        /// <param name="reputeID">Danh vọng</param>
        /// <param name="value">Số uy danh thêm vào</param>
        private static void AddRepute(KPlayer player, int reputeID, int value)
        {
            KTGlobal.AddRepute(player, reputeID, value);
        }

        #endregion Thêm danh vọng

        #region Thêm/xóa danh hiệu nhân vật

        /// <summary>
        /// Thêm/xóa danh hiệu nhân vật cho người chơi tương ứng
        /// </summary>
        /// <param name="targetID">ID đối tượng</param>
        /// <param name="method">Loại thao tác (-1: Xóa, 1: Thêm)</param>
        /// <param name="titleID">ID danh hiệu</param>
        private static void ModRoleTitle(int targetID, int method, int titleID)
        {
            KPlayer target = KTGMCommandManager.FindPlayer(targetID);
            /// Kiểm tra đối tượng
            if (target == null)
            {
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'ModRoleTitle' Error...\n" + "Target not found.");
                return;
            }

            KTGMCommandManager.ModRoleTitle(target, method, titleID);
        }

        /// <summary>
        /// Thêm/xóa danh hiệu nhân vật cho người chơi tương ứng
        /// </summary>
        /// <param name="player">Người chơi</param>
        /// <param name="method">Loại thao tác (-1: Xóa, 1: Thêm)</param>
        /// <param name="titleID">ID danh hiệu</param>
        private static void ModRoleTitle(KPlayer player, int method, int titleID)
        {
            /// Nếu là thao tác thêm
            if (method == 1)
            {
                player.AddRoleTitle(titleID);
            }
            /// Nếu là thao tác xóa
            else if (method == -1)
            {
                player.RemoveRoleTitle(titleID);
            }
        }

        #endregion Thêm/xóa danh hiệu nhân vật

        #region Thiết lập số lượt chúc phúc

        /// <summary>
        /// Thiết lập số lượt chúc phúc cho người chơi tương ứng
        /// </summary>
        /// <param name="targetID">ID đối tượng</param>
        /// <param name="value">Giá trị</param>
        private static void SetPrayTimes(int targetID, int value)
        {
            KPlayer target = KTGMCommandManager.FindPlayer(targetID);
            /// Kiểm tra đối tượng
            if (target == null)
            {
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'SetPrayTimes' Error...\n" + "Target not found.");
                return;
            }

            KTGMCommandManager.SetPrayTimes(target, value);
        }

        /// <summary>
        /// Thiết lập số lượt chúc phúc cho người chơi tương ứng
        /// </summary>
        /// <param name="player">Người chơi</param>
        /// <param name="value">Giá trị</param>
        private static void SetPrayTimes(KPlayer player, int value)
        {
            KTPlayerPrayManager.SetTotalTurnLeft(player, value);
        }

        #endregion Thiết lập số lượt chúc phúc

        #region Thêm điểm tích lũy tháng Tiêu Dao Cốc
        /// <summary>
        /// Thêm điểm tích lũy tháng Tiêu Dao Cốc cho người chơi tương ứng
        /// </summary>
        /// <param name="targetID">ID đối tượng</param>
        /// <param name="value">Giá trị</param>
        private static void AddXoYoMonthStoragePoint(int targetID, int value)
        {
            KPlayer target = KTGMCommandManager.FindPlayer(targetID);
            /// Kiểm tra đối tượng
            if (target == null)
            {
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'AddXoYoMonthStoragePoint' Error...\n" + "Target not found.");
                return;
            }

            KTGMCommandManager.AddXoYoMonthStoragePoint(target, value);
        }

        /// <summary>
        /// Thêm điểm tích lũy tháng Tiêu Dao Cốc cho người chơi tương ứng
        /// </summary>
        /// <param name="player">Người chơi</param>
        /// <param name="value">Giá trị</param>
        private static void AddXoYoMonthStoragePoint(KPlayer player, int value)
        {
            XoYo_EventScript.AddCurrentMonthStoragePoint(player, value);
        }
        #endregion

        #region Thiết lập sẽ quay vào rương xấu xí ở Bách Bảo Rương lượt tiếp theo
        /// <summary>
        /// Thiết lập sẽ quay vào rương xấu xí ở Bách Bảo Rương lượt tiếp theo cho người chơi tương ứng
        /// </summary>
        /// <param name="targetID">ID đối tượng</param>
        /// <param name="bet">Số sò cược tương ứng</param>
        private static void SetSeashellTreasureNextTurn(int targetID, int bet)
        {
            KPlayer target = KTGMCommandManager.FindPlayer(targetID);
            /// Kiểm tra đối tượng
            if (target == null)
            {
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'SetSeashellTreasureNextTurn' Error...\n" + "Target not found.");
                return;
            }

            KTGMCommandManager.SetSeashellTreasureNextTurn(target, bet);
        }

        /// <summary>
        /// Thiết lập sẽ quay vào rương xấu xí ở Bách Bảo Rương lượt tiếp theo cho người chơi tương ứng
        /// </summary>
        /// <param name="player">Người chơi</param>
        /// <param name="bet">Số sò cược tương ứng</param>
        private static void SetSeashellTreasureNextTurn(KPlayer player, int bet)
        {
            player.GM_SetWillGetTreasureNextTurn = true;
            player.GM_SetWillGetTreasureNextTurnWithBet = bet;
        }
        #endregion

        #region Thêm kinh nghiệm Tu Luyện Châu
        /// <summary>
        /// Thêm kinh nghiệm Tu Luyện Châu cho người chơi tương ứng
        /// </summary>
        /// <param name="targetID">ID đối tượng</param>
        /// <param name="exp">Kinh nghiệm</param>
        private static void AddXiuLianZhu_Exp(int targetID, int exp)
        {
            KPlayer target = KTGMCommandManager.FindPlayer(targetID);
            /// Kiểm tra đối tượng
            if (target == null)
            {
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'AddXiuLianZhu_Exp' Error...\n" + "Target not found.");
                return;
            }

            KTGMCommandManager.AddXiuLianZhu_Exp(target, exp);
        }

        /// <summary>
        /// Thêm kinh nghiệm Tu Luyện Châu cho người chơi tương ứng
        /// </summary>
        /// <param name="player">Người chơi</param>
        /// <param name="exp">Kinh nghiệm</param>
        private static void AddXiuLianZhu_Exp(KPlayer player, int exp)
        {
            player.XiuLianZhu_Exp += exp;
        }
        #endregion

        #region Thiết lập kinh nghiệm Tu Luyện Châu
        /// <summary>
        /// Thiết lập kinh nghiệm Tu Luyện Châu cho người chơi tương ứng
        /// </summary>
        /// <param name="targetID">ID đối tượng</param>
        /// <param name="exp">Kinh nghiệm</param>
        private static void SetXiuLianZhu_Exp(int targetID, int exp)
        {
            KPlayer target = KTGMCommandManager.FindPlayer(targetID);
            /// Kiểm tra đối tượng
            if (target == null)
            {
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'SetXiuLianZhu_Exp' Error...\n" + "Target not found.");
                return;
            }

            KTGMCommandManager.SetXiuLianZhu_Exp(target, exp);
        }

        /// <summary>
        /// Thiết lập kinh nghiệm Tu Luyện Châu cho người chơi tương ứng
        /// </summary>
        /// <param name="player">Người chơi</param>
        /// <param name="exp">Kinh nghiệm</param>
        private static void SetXiuLianZhu_Exp(KPlayer player, int exp)
        {
            player.XiuLianZhu_Exp = exp;
        }
        #endregion

        #region Thiết lập số giờ Tu Luyện Châu
        /// <summary>
        /// Thiết lập số giờ Tu Luyện Châu cho người chơi tương ứng
        /// </summary>
        /// <param name="targetID">ID đối tượng</param>
        /// <param name="exp">Số giờ (* 10)</param>
        private static void SetXiuLianZhu_TimeLeft(int targetID, int hour10)
        {
            KPlayer target = KTGMCommandManager.FindPlayer(targetID);
            /// Kiểm tra đối tượng
            if (target == null)
            {
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'AddXiuLianZhu_Exp' Error...\n" + "Target not found.");
                return;
            }

            KTGMCommandManager.SetXiuLianZhu_TimeLeft(target, hour10);
        }

        /// <summary>
        /// Thiết lập số giờ Tu Luyện Châu cho người chơi tương ứng
        /// </summary>
        /// <param name="player">Người chơi</param>
        /// <param name="exp">Số giờ (* 10)</param>
        private static void SetXiuLianZhu_TimeLeft(KPlayer player, int hour10)
        {
            player.XiuLianZhu_TotalTime = hour10;
        }
        #endregion

        #region Trả về thông tin điểm tích lũy Tiêu Dao tháng trước và tháng này
        /// <summary>
        /// Trả về thông tin điểm tích lũy Tiêu Dao tháng trước và tháng này của người chơi tương ứng
        /// </summary>
        /// <param name="targetID">ID đối tượng</param>
        private static void GetXoYoMonthStoragePoint(int targetID)
        {
            KPlayer target = KTGMCommandManager.FindPlayer(targetID);
            /// Kiểm tra đối tượng
            if (target == null)
            {
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'GetXoYoMonthStoragePoint' Error...\n" + "Target not found.");
                return;
            }

            KTGMCommandManager.GetXoYoMonthStoragePoint(target);
        }

        /// <summary>
        /// Trả về thông tin điểm tích lũy Tiêu Dao tháng trước và tháng này của người chơi tương ứng
        /// </summary>
        /// <param name="player">Người chơi</param>
        private static void GetXoYoMonthStoragePoint(KPlayer player)
        {
            int lastMonthStoragePoint = XoYo_EventScript.GetLastMonthStoragePoint(player);
            bool isReceivedLastMonthStorageAward = XoYo_EventScript.HasAlreadyGottenLastMonthAward(player);
            int currentMonthStoragePoint = XoYo_EventScript.GetCurrentMonthStoragePoint(player);

            PlayerManager.ShowNotification(player, string.Format("Tháng trước: {0} ({2}), tháng này: {1}", lastMonthStoragePoint, currentMonthStoragePoint, isReceivedLastMonthStorageAward ? "Đã nhận thưởng" : "Chưa nhận thưởng"));
        }
        #endregion

        #region Thiết lập trạng thái nhận thưởng liên đấu
        /// <summary>
        /// Thiết lập trạng thái nhận thưởng liên đấu cho chiến đội của người chơi tương ứng
        /// </summary>
        /// <param name="targetID">ID đối tượng</param>
        /// <param name="state">Trạng thái (0: chưa nhận, 1: đã nhận)</param>
        private static void SetTeamBattleAwardState(int targetID, int state)
        {
            KPlayer target = KTGMCommandManager.FindPlayer(targetID);
            /// Kiểm tra đối tượng
            if (target == null)
            {
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'UpdateTeamBattleAwardState' Error...\n" + "Target not found.");
                return;
            }

            KTGMCommandManager.SetTeamBattleAwardState(target, state);
        }

        /// <summary>
        /// Thiết lập trạng thái nhận thưởng liên đấu cho chiến đội của người chơi tương ứng
        /// </summary>
        /// <param name="player">Người chơi</param>
        /// <param name="state">Trạng thái (0: chưa nhận, 1: đã nhận)</param>
        private static void SetTeamBattleAwardState(KPlayer player, int state)
        {
            TeamBattle_ActivityScript.UpdateTeamAwardState(player, state == 1);
        }
        #endregion

        #region Mở Captcha
        /// <summary>
        /// Mở Captcha cho người chơi tương ứng
        /// </summary>
        /// <param name="targetID">ID đối tượng</param>
        private static void GenerateCaptcha(int targetID)
        {
            KPlayer target = KTGMCommandManager.FindPlayer(targetID);
            /// Kiểm tra đối tượng
            if (target == null)
            {
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'GenerateCaptcha' Error...\n" + "Target not found.");
                return;
            }

            KTGMCommandManager.GenerateCaptcha(target);
        }

        /// <summary>
        /// Mở Captcha cho người chơi tương ứng
        /// </summary>
        /// <param name="player">Người chơi</param>
        private static void GenerateCaptcha(KPlayer player)
        {
            player.GenerateCaptcha();
        }
        #endregion

        #region Thông tin Tần Lăng
        /// <summary>
        /// Trả về thông tin Tần Lăng trong ngày của người chơi tương ứng
        /// </summary>
        /// <param name="player">Đối tượng theo dõi</param>
        /// <param name="targetID">ID người chơi cần</param>
        private static void GetEmperorTombData(KPlayer player, int targetID)
        {
            KPlayer target = KTGMCommandManager.FindPlayer(targetID);
            /// Kiểm tra đối tượng
            if (target == null)
            {
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'GetEmperorTombData' Error...\n" + "Target not found.");
                return;
            }

            KTGMCommandManager.GetEmperorTombData(player, target);
        }

        /// <summary>
        /// Trả về thông tin Tần Lăng trong ngày của người chơi tương ứng
        /// </summary>
        /// <param name="player">Đối tượng theo dõi</param>
        /// <param name="target">Người chơi</param>
        private static void GetEmperorTombData(KPlayer player, KPlayer target)
        {
            /// Tổng số giờ còn lại
            int timeLeft = target.GetValueOfDailyRecore((int) AcitvityRecore.EmperorTomb_TodaySecLeft);
            if (timeLeft == -1)
            {
                timeLeft = EmperorTomb.Config.DurationPerDay;
            }
            else if (timeLeft < 0)
            {
                timeLeft = 0;
            }
            /// Dữ liệu tầng
            int stageData = target.GetValueOfDailyRecore((int) AcitvityRecore.EmperorTomb_TodayData);
            string stagePassedString;
            if (stageData == -1)
            {
                stagePassedString = "Chưa có";
            }
            else
            {
                List<char> str = new List<char>();
                foreach (char c in stageData.ToString())
                {
                    str.Add(c);
                }
                stagePassedString = string.Join(", ", str);
            }
            /// Thông tin
            string infoText = string.Format("Thời gian còn: {0}, Tầng đã qua: {1}", timeLeft / 1000, stagePassedString);
            /// Hiện thông tin
            PlayerManager.ShowNotification(player, infoText);
        }
        #endregion

        #region Reset số giờ Tần Lăng
        /// <summary>
        /// Reset số giờ còn lại trong Tần Lăng của người chơi tương ứng
        /// </summary>
        /// <param name="targetID"></param>
        public static void ResetEmperorTombTimeLeft(int targetID)
        {
            KPlayer target = KTGMCommandManager.FindPlayer(targetID);
            /// Kiểm tra đối tượng
            if (target == null)
            {
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'ResetEmperorTombTimeLeft' Error...\n" + "Target not found.");
                return;
            }

            KTGMCommandManager.ResetEmperorTombTimeLeft(target);
        }

        /// <summary>
        /// Reset số giờ còn lại trong Tần Lăng của người chơi tương ứng
        /// </summary>
        /// <param name="player"></param>
        public static void ResetEmperorTombTimeLeft(KPlayer player)
        {
            player.SetValueOfDailyRecore((int) AcitvityRecore.EmperorTomb_TodaySecLeft, -1);
        }
        #endregion

        #region Truy vấn Vòng quay may mắn
        /// <summary>
        /// Thiết lập số lượt đã quay Vòng quay may mắn của người chơi tương ứng
        /// </summary>
        /// <param name="targetID"></param>
        /// <param name="totalTurn"></param>
        public static void SetLuckyCircleTotalTurn(int targetID, int totalTurn)
        {
            KPlayer target = KTGMCommandManager.FindPlayer(targetID);
            /// Kiểm tra đối tượng
            if (target == null)
            {
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'SetLuckyCircleTotalTurn' Error...\n" + "Target not found.");
                return;
            }

            KTGMCommandManager.SetLuckyCircleTotalTurn(target, totalTurn);
        }

        /// <summary>
        /// Thiết lập số lượt đã quay Vòng quay may mắn của người chơi tương ứng
        /// </summary>
        /// <param name="player"></param>
        /// <param name="totalTurn"></param>
        public static void SetLuckyCircleTotalTurn(KPlayer player, int totalTurn)
        {
            player.LuckyCircle_TotalTurn = totalTurn;
        }

        /// <summary>
        /// Thiết lập số lượt đã quay Vòng quay may mắn của người chơi tương ứng
        /// </summary>
        /// <param name="player"></param>
        /// <param name="targetID"></param>
        public static void GetLuckyCircleTotalTurn(KPlayer player, int targetID)
        {
            KPlayer target = KTGMCommandManager.FindPlayer(targetID);
            /// Kiểm tra đối tượng
            if (target == null)
            {
                LogManager.WriteLog(LogTypes.Error, "Process GM Command 'GetLuckyCircleTotalTurn' Error...\n" + "Target not found.");
                return;
            }

            KTGMCommandManager.GetLuckyCircleTotalTurn(player, target);
        }

        /// <summary>
        /// Thiết lập số lượt đã quay Vòng quay may mắn của người chơi tương ứng
        /// </summary>
        /// <param name="player"></param>
        public static void GetLuckyCircleTotalTurn(KPlayer player, KPlayer target)
        {
            PlayerManager.ShowNotification(player, string.Format("Tổng số lượt đã quay: {0}", target.LuckyCircle_TotalTurn));
        }
        #endregion
        #endregion Thực thi lệnh GM
    }
}