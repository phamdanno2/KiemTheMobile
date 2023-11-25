using GameServer.Core.Executor;
using GameServer.KiemThe.Entities;
using GameServer.KiemThe.Logic;
using GameServer.Logic;
using Server.Tools;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace GameServer.KiemThe.GameEvents.GuildWarManager
{

    /// <summary>
    /// Thực thể quản lý sự kiện trên 1 bản đồ
    /// </summary>
    public class GuildWarMap
    {
        /// <summary>
        /// Thông tin bản đồ ID
        /// </summary>
        public int MapId { get; set; }


        public bool IsFightMap { get; set; }

        /// <summary>
        /// Thông tin bản đồ
        /// </summary>
        public string MapName { get; set; }


        public GuildWardConfig _Config { get; set; }
        /// <summary>
        ///Tên bang hội
        /// </summary>
        public int BeLongGuild { get; set; } = -1;

        /// <summary>
        /// Tên bang hội
        /// </summary>
        public string BeLongGuildName { get; set; }


        //Danh sách limit lãnh thổ
        public ConcurrentDictionary<int, Monster> AllBudding = new ConcurrentDictionary<int, Monster>();
        /// <summary>
        /// Danh sách bang có thể đánh ở bản đồ này
        /// </summary>
        public List<int> TotalGuildCanJoinBattle { get; set; } = new List<int>();
        //Dict quản lý số điểm có trên bản đồ này để phân biết bang nào chiếm  ưu thế bang nào không chiếm ưu thế

        //MÁU QUÁI CHUẨN
        public double HPRate = 1;

        public GuildWarMap(GuildWardConfig Config)
        {
            LogManager.WriteLog(LogTypes.GuildWarManager, "START MAP WAR :" + MapName);
            this._Config = Config;

        }


        /// <summary>
        /// Đếm số long trụ hiện tại của bang
        /// </summary>
        /// <param name="GuildID"></param>
        /// <returns></returns>
        public int CountGuildObjective(int GuildID)
        {

            int COUNT = AllBudding.Values.Where(x => x.Camp == GuildID).Count();


            return COUNT;

        }

        /// <summary>
        /// Update số điểm của bang hội này
        /// </summary>
        /// <param name="GuildID"></param>
        /// <param name="GuildName"></param>
        /// <param name="Score"></param>
        public void UpdateGuildScore(int GuildID, string GuildName, int Score)
        {
            // Nếu bang này mà không nằm trong danh sách được cộng điểm thì thôi
            if (!TotalGuildCanJoinBattle.Contains(GuildID))
            {
                return;
            }


            GuidWarManager.getInstance().UpdateGuildPoint(GuildID, GuildName, Score, this.MapId, this.MapName);
        }



        /// <summary>
        /// Tạo trụ khi sự kiện bắt đầu
        /// </summary>
        /// <param name="GuildID"></param>
        /// <param name="GuildName"></param>
        public void CreateBuildingWhenStart()
        {

            LogManager.WriteLog(LogTypes.GuildWarManager, "Create CreateBuildingWhenStart:" + MapName);

            try
            {
                OpenConfig FindConfig = null;

                if (this.IsFightMap)
                {
                    FindConfig = _Config.WarMapConfigs.Where(x => x.FightMapID == this.MapId).FirstOrDefault();
                }
                else
                {
                    FindConfig = _Config.WarMapConfigs.Where(x => x.MapID == this.MapId).FirstOrDefault();
                }


                if (FindConfig != null)
                {
                    // Duyệt tất cả quái để tạo
                    foreach (ObjectivePostion _Monster in FindConfig.ObjectPostion.Where(x => x.IsMonster == false))
                    {

                        CreateLongTru(_Monster, this.BeLongGuild, this.BeLongGuildName);
                    }


                    // Nếu bản đồ này là lần đầu tiên tranh đoạt thì tạo quái ra
                    if (this.BeLongGuild == -1)
                    {
                        foreach (ObjectivePostion _Monster in FindConfig.ObjectPostion.Where(x => x.IsMonster == true))
                        {
                            // Set cho quái CAMP 1000 để nó ko đánh trụ
                            CreateMonster(_Monster, 1000);
                        }

                    }
                    else // NẾu trụ đã có bang thì có lệ sẽ xuất hiện quân chiếm đóng
                    {
                        int RANDOM = KTGlobal.GetRandomNumber(0, 100);

                        // Có 30% có tỉ lệ có quái tấn công
                        if (RANDOM < 30)
                        {
                            foreach (ObjectivePostion _Monster in FindConfig.ObjectPostion.Where(x => x.IsMonster == true))
                            {
                                // Set cho quái CAMP 1000 để nó ko đánh trụ
                                CreateMonster(_Monster, 1000);
                            }

                        }

                    }
                }

                LogManager.WriteLog(LogTypes.GuildWarManager, "Create CreateBuildingWhenStart:" + MapName + "==>END");
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.GuildWarManager, "BUG :" + ex.ToString());
            }

        }


        public void CreateMonster(ObjectivePostion _Monster, int CAMP)
        {

            try
            {
                MonsterAIType aiType = MonsterAIType.Special_Normal;

                /// Ngũ hành
                KE_SERIES_TYPE series = KE_SERIES_TYPE.series_none;

                int RandomSeri = KTGlobal.GetRandomNumber(1, 5);
                series = (KE_SERIES_TYPE)RandomSeri;

                /// Hướng quay
                KiemThe.Entities.Direction dir = KiemThe.Entities.Direction.NONE;

                int RandomDir = KTGlobal.GetRandomNumber(0, 7);

                dir = (KiemThe.Entities.Direction)RandomDir;


                string TITLE = "";


                int Random = new Random().Next(0, 100);

                // nếu đây là boss thì có tỉ lệ ra hoặc không ra
                if (_Monster.ID == 3408 || _Monster.ID == 3409 || _Monster.ID == 3410 || _Monster.ID == 3411 || _Monster.ID == 3412)
                {
                    // Nếu như tỉ lệ mà random mà nhỏ hơn 30 thì đéo ra boss
                    if (Random < 30)
                    {
                        return;
                    }
                    else
                    {
                        int RANODMTIMERESPAN = new Random().Next(600000, 1200000);

                        var Funtion = KTKTAsyncTask.Instance.ScheduleExecuteAsync(new DelayFuntionAsyncTask("CreateMonster", new Action(() => CreateMonsterByDelay(_Monster, CAMP))), RANODMTIMERESPAN);
                    }

                }
                else
                {

                    double BaseHP = 40385163 * HPRate;

                    Console.WriteLine("[" + this.MapId + "]CREATE MONSTER VOI CAMP :" + CAMP);
                    /// AIType
                    // Cứ 20s lại mọc lại 1 con
                    GameManager.MonsterZoneMgr.AddDynamicMonsters(this.MapId, _Monster.ID, -1, 1, _Monster.Posx, _Monster.PosY, _Monster.Name, TITLE, (int)BaseHP, 90, dir, series, aiType, 20000, -1, -1, "", null, CAMP, new Func<bool>(() =>
                   {

                       if (GuildWarManager.GuidWarManager.getInstance().BATTLESTATE == GUILDWARDSTATUS.STATUS_START)
                       {
                           return true;
                       }
                       else
                       {
                           return false;
                       }
                   }),
                    (_GameObject) =>
                     {
                         if (GuildWarManager.GuidWarManager.getInstance().BATTLESTATE == GUILDWARDSTATUS.STATUS_START || GuildWarManager.GuidWarManager.getInstance().BATTLESTATE == GUILDWARDSTATUS.STATUS_PREPAREEND)
                         {
                             if (_GameObject is KPlayer)
                             {
                                 KPlayer _Player = (KPlayer)_GameObject;

                                 // Cứ mỗi lần giết con NPC sẽ được 37 điểm
                                 GuidWarManager.getInstance().UpdateScore(_Player, 37, 0, 0, false);
                             }
                         }

                     });
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.GuildWarManager, "BUG :" + ex.ToString());
            }
        }


        public void CreateMonsterByDelay(ObjectivePostion _Monster, int CAMP)
        {

            try
            {
                MonsterAIType aiType = MonsterAIType.Special_Boss;

                /// Ngũ hành
                KE_SERIES_TYPE series = KE_SERIES_TYPE.series_none;


                int RandomSeri = KTGlobal.GetRandomNumber(1, 5);

                series = (KE_SERIES_TYPE)RandomSeri;

                /// Hướng quay
                KiemThe.Entities.Direction dir = KiemThe.Entities.Direction.NONE;

                int RandomDir = KTGlobal.GetRandomNumber(0, 7);

                dir = (KiemThe.Entities.Direction)RandomDir;


                string TITLE = "";


                if (BeLongGuild == -1)
                {
                    //  TITLE = "<b><color=#00ff2a>Hệ Thống</color></b>";


                }
                else
                {
                    CAMP = BeLongGuild;
                    // TITLE = "<b><color=#00ff2a>" + this.BeLongGuildName + "</color></b>";

                }


                string NotifyProtect = "Boss lãnh thổ <b>[" + _Monster.Name + "]</b> đã xuất hiện tại <color=yellow>" + _Monster.Name + "</color> đã xuất hiện hãy mau chóng tiêu diệt!";

                // Thông báo cho toàn bộ các bang tham chiến biết thôn tin về boss lãnh thổ
                foreach (int GUILD in TotalGuildCanJoinBattle)
                {
                    KTGlobal.SendGuildChat(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, GUILD, NotifyProtect, null, "");
                }



                double BaseHP = 40385163 * HPRate * 10;

                GameManager.MonsterZoneMgr.AddDynamicMonsters(this.MapId, _Monster.ID, -1, 1, _Monster.Posx, _Monster.PosY, _Monster.Name, TITLE, (int)BaseHP, 90, dir, series, aiType, -1, -1, -1, "", null, CAMP, null,
                 (_GameObject) =>
                 {
                     if (GuildWarManager.GuidWarManager.getInstance().BATTLESTATE == GUILDWARDSTATUS.STATUS_START || GuildWarManager.GuidWarManager.getInstance().BATTLESTATE == GUILDWARDSTATUS.STATUS_PREPAREEND)
                     {
                         if (_GameObject is KPlayer)
                         {
                             KPlayer _Player = (KPlayer)_GameObject;

                             // Cứ mỗi lần giết con NPC sẽ được 37 điểm
                             GuidWarManager.getInstance().UpdateScore(_Player, 37, 0, 0, false);
                         }
                     }

                 });
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.GuildWarManager, "BUG :" + ex.ToString());
            }
        }
        /// <summary>
        /// Thưởng EXP cho người chơi khi bảo vệ long trụ
        /// </summary>
        public void UpdateEXP()
        {

            try
            {
                foreach (Monster _Monster in AllBudding.Values.ToList())
                {

                    // Ông nào đứng gần trụ
                    List<KPlayer> friends = KTLogic.GetNearByPeacePlayersReductCost(_Monster, 200);

                    foreach (KPlayer _Player in friends)
                    {

                        if (_Player.Camp == _Monster.Camp)
                        {
                            string NotifyProtect = "Ngươi có công bảo vệ long trụ, nhận được <color=yellow>5</color> điểm tích lũy và 1600 EXP";

                            PlayerManager.ShowNotification(_Player, NotifyProtect);

                            GameManager.ClientMgr.ProcessRoleExperience(_Player, 16000, true, false, true, "TDLT");

                            GuidWarManager.getInstance().UpdateScore(_Player, 5, 0, 0, false);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.GuildWarManager, "BUG :" + ex.ToString());
            }
        }




        /// <summary>
        /// Tạo ra cột khi bắt đầu
        /// </summary>
        /// <param name="_Monster"></param>
        /// <param name="IsBoss"></param>
        public void CreateLongTru(ObjectivePostion _Monster, int _BelongGuid, string _GuildName)
        {
            try
            {
                MonsterAIType aiType = MonsterAIType.Special_Boss;

                /// Ngũ hành
                KE_SERIES_TYPE series = KE_SERIES_TYPE.series_none;

                int RandomSeri = KTGlobal.GetRandomNumber(1, 5);
                series = (KE_SERIES_TYPE)RandomSeri;

                /// Hướng quay
                KiemThe.Entities.Direction dir = KiemThe.Entities.Direction.NONE;

                int RandomDir = KTGlobal.GetRandomNumber(0, 7);

                dir = (KiemThe.Entities.Direction)RandomDir;

                string TITLE = "";

                int CAMP = 1000;

                // Nếu không phỉa
                if (_BelongGuid == -1)
                {
                    // Nếu ko thì set ttitle là hệ thống
                    TITLE = "<b><color=#00ff2a>Long Trụ Hệ Thống</color></b>";

                }
                else
                {
                    //Nếu trụ này thuộc về bang nào đó
                    CAMP = _BelongGuid;
                    TITLE = "<b><color=#00ff2a>" + _GuildName + "</color></b>";
                }

                // FIx máu trụ
                double BaseHP = (453755700 / 2) * HPRate;
                /// AIType

                // Console.Write(this);

                Console.WriteLine("[" + this.MapId + "]CREATE LONG TRU VOI CAMP :" + CAMP);

                GameManager.MonsterZoneMgr.AddDynamicMonsters(this.MapId, _Monster.ID, -1, 1, _Monster.Posx, _Monster.PosY, _Monster.Name, TITLE, (int)BaseHP, 90, dir, series, aiType, -1, -1, -1, "", (monster) =>
                {

                    int IDLOCALTION = _Monster.Posx * _Monster.PosY;

                    if (AllBudding.ContainsKey(IDLOCALTION))
                    {
                        AllBudding[IDLOCALTION] = monster;
                    }
                    else
                    {
                        AllBudding.TryAdd(IDLOCALTION, monster);
                    }

                    monster.IsStatic = true;
                    monster.LocalMapName = TITLE;
                    monster.UpdatePositionToLocalMapContinuously = true;
                    monster.m_IgnoreAllSeriesStates = true;

                }, CAMP, null, (_GameObject) =>
                {

                    if (GuildWarManager.GuidWarManager.getInstance().BATTLESTATE == GUILDWARDSTATUS.STATUS_START || GuildWarManager.GuidWarManager.getInstance().BATTLESTATE == GUILDWARDSTATUS.STATUS_PREPAREEND)
                    {
                        // Gọi sự kiện khi trụ này toác

                        // Nếu trụ này bị người chơi đập vỡ thì lại tạo 1 long trụ mới với CAMP là camp của bang này
                        if (_GameObject is KPlayer)
                        {
                            KPlayer _Client = (KPlayer)_GameObject;


                            this.CreateLongTru(_Monster, _Client.GuildID, _Client.GuildName);

                            if (this.BeLongGuild != -1)
                            {

                                if (_Client.GuildID != this.BeLongGuild)
                                {
                                    string MSG = KTGlobal.CreateStringByColor("Long trụ tại lãnh thổ [" + this.MapName + "] đang bị bang [" + _Client.GuildName + "] chiếm mất , hãy mau chóng cử ngưỡi canh giữ!", ColorType.Importal);

                                    KTGlobal.SendGuildChat(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, this.BeLongGuild, MSG, null, "");
                                }

                            }

                            if (_Client.TeamID != -1)
                            {

                                KPlayer _Lead = KTTeamManager.GetTeamLeader(_Client.TeamID);

                                List<KPlayer> TotalMember = _Client.Teammates;

                                foreach (KPlayer member in TotalMember)
                                {
                                    if (!Global.InCircle(new Point(member.PosX, member.PosY), _Client.CurrentPos, 1000))
                                    {
                                        continue;
                                    }
                                    // Cộng cho mỗi thằng 300 điểm
                                    GuidWarManager.getInstance().UpdateScore(member, 300, 1, 0, false);
                                }


                                string MSG = "Tổ đội của " + KTGlobal.CreateStringByColor(_Lead.RoleName, ColorType.Yellow) + " đã có công phá vỡ Long Trụ của địch";

                                KTGlobal.SendGuildChat(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, _Lead.GuildID, MSG, null, "");

                            }
                            else
                            {
                                string MSG = "Người chơi " + KTGlobal.CreateStringByColor(_Client.RoleName, ColorType.Yellow) + " đã có công phá vỡ Long Trụ của địch";

                                KTGlobal.SendGuildChat(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, _Client.GuildID, MSG, null, "");

                                // Cộng cho thằng latst hit trụ 500 điểm

                                GuidWarManager.getInstance().UpdateScore(_Client, 500, 1, 0, false);

                            }


                            // Cộng cho bang này 100 điểm tích lũy
                            this.UpdateGuildScore(_Client.GuildID, _Client.GuildName, 100);


                        }
                        else if (_GameObject is Monster)
                        {
                            // Nếu là quái đập vỡ long trụ thì tạo 1 cái long trụ rỗng
                            Monster _Client = (Monster)_GameObject;

                            this.CreateLongTru(_Monster, -1, "");

                            if (this.BeLongGuild != -1)
                            {
                                int COUNTLONGTRU = CountGuildObjective(this.BeLongGuild);

                                if (COUNTLONGTRU <= 3)
                                {
                                    string MSG = KTGlobal.CreateStringByColor("Lãnh thổ [" + this.MapName + "] đã bị quân Lưu vong nơi đó phản công, hãy mau quay về canh giữ!", ColorType.Importal);

                                    KTGlobal.SendGuildChat(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, this.BeLongGuild, MSG, null, "");
                                }

                            }


                        }

                    }
                });
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.GuildWarManager, "BUG :" + ex.ToString());
            }
        }

        public void ClearAllMonster()
        {
            try
            {
                foreach (Monster monster in AllBudding.Values.ToList())
                {
                    monster.MonsterZoneNode?.DestroyMonster(monster);
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.GuildWarManager, "BUG :" + ex.ToString());
            }
        }



        public void RemoveAllForbid()
        {

            try
            {
                List<KPlayer> objs = GameManager.ClientMgr.GetMapClients(this.MapId);

                if (objs != null)
                {
                    /// Duyệt danh sách
                    foreach (KPlayer obj in objs)
                    {
                        obj.ForbidUsingSkill = false;
                        obj.TempTitle = "";
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.GuildWarManager, "BUG :" + ex.ToString());
            }
        }


        public void ActiveAllStatusAndPKModelInMap()
        {
            try
            {
                List<KPlayer> objs = GameManager.ClientMgr.GetMapClients(this.MapId);
                LogManager.WriteLog(LogTypes.GuildWarManager, "[" + this.MapId + "][" + this.MapName + "] CALL RELOAD CAMP :" + objs.Count);

                if (objs != null)
                {
                    /// Duyệt danh sách
                    foreach (KPlayer obj in objs)
                    {
                        GuidWarManager.getInstance().OnEnterMap(obj);
                    }
                }

                LogManager.WriteLog(LogTypes.GuildWarManager, "[" + this.MapId + "][" + this.MapName + "] CALL RELOAD CAMP ENDDD :" + objs.Count);
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.GuildWarManager, "BUG :" + ex.ToString());
            }
        }



    }
}
