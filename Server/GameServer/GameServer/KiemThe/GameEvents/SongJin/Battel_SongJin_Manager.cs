using GameServer.Logic;
using System.Collections.Generic;

namespace GameServer.KiemThe.Logic.Manager.Battle
{
    /// <summary>
    /// Class quản lý toàn bộ tống kim
    /// </summary>
    public class Battel_SonJin_Manager
    {
        public static Dictionary<int, Battle_SongJin> _TotalBattle = new Dictionary<int, Battle_SongJin>();

        public static void BattleStatup()
        {
            // Nếu là liên máy chủ
            if (GameManager.IsKuaFuServer)
            {
                Battle_SongJin _Battle_Hight = new Battle_SongJin();
                _Battle_Hight.initialize(4);
                // Call Statup()
                _Battle_Hight.startup();
                _TotalBattle.Add(4, _Battle_Hight);
				
            }
            else
            {
                // NẾu không phải liên máy chủ
                Battle_SongJin _Battle_Low = new Battle_SongJin();
                _Battle_Low.initialize(1);
                // Call Statup()
                _Battle_Low.startup();
                _TotalBattle.Add(1, _Battle_Low);

                // Khởi tọa chiến trường trung
                Battle_SongJin _Battle_Mid = new Battle_SongJin();
                _Battle_Mid.initialize(2);
                // Call Statup()
                _Battle_Mid.startup();
                _TotalBattle.Add(2, _Battle_Mid);
				
				// Khởi tọa chiến trường cao
                Battle_SongJin _Battle_Hight = new Battle_SongJin();
                _Battle_Hight.initialize(3);
                // Call Statup()
                _Battle_Hight.startup();
                _TotalBattle.Add(3, _Battle_Hight);
            }

        }

        /// <summary>
        /// Hàm này gọi khi người chơi rời chiến trường
        /// </summary>
        /// <param name="player"></param>
        public static void OnPlayerLeave(KPlayer player)
        {
            /// Chuyển Camp về -1
            player.Camp = -1;
            /// Chuyển trạng thái PK về hòa bình
            player.PKMode = (int)KiemThe.Entities.PKMode.Peace;
        }

        public static bool IsInBattle(KPlayer client)
        {
            if (client.MapCode == 1635 || client.MapCode == 1638 || client.MapCode == 1639 || client.MapCode == 1641 || client.MapCode == 1642)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void ShutDown()
        {
            foreach (KeyValuePair<int, Battle_SongJin> entry in _TotalBattle)
            {
                Battle_SongJin PlayerBattle = entry.Value;
                PlayerBattle.showdown();
            }
        }

        /// <summary>
        /// </summary>
        /// Trạng thái trận đấu
        public static int StateBattle(KPlayer player, int Level)
        {
            int BattleLevel = GetBattleLevel(player.m_Level);

            if (BattleLevel != Level)
            {
                return -1000;
            }
            else
            {
                if (BattleLevel != -1)
                {
                    Battle_SongJin _Battle = _TotalBattle[BattleLevel];

                    return _Battle.StateBattle(player);
                }
                else
                {
                    return -10;
                }
            }
        }

        /// <summary>
        /// </summary>
        /// Số lượng 2 bên lúc đang ký báo danh
        public static int CountBattle(KPlayer player, int Level, int Camp)
        {
            int BattleLevel = GetBattleLevel(player.m_Level);

            if (BattleLevel != Level)
            {
                return -1000;
            }
            else
            {
                if (BattleLevel != -1)
                {
                    Battle_SongJin _Battle = _TotalBattle[BattleLevel];

                    return _Battle.CountBattle(player, Camp);
                }
                else
                {
                    return -10;
                }
            }
        }

        /// <summary>
        /// Kiểm tra đăng ký
        /// </summary>
        /// <param name="player"></param>
        /// <param name="Camp"></param>
        /// <param name="Level"></param>
        /// <returns></returns>
        public static int CheckBaoDanhPhe(KPlayer player, int Level)
        {
            int BattleLevel = GetBattleLevel(player.m_Level);
            if (BattleLevel != Level)
            {
                return 0;
            }
            else
            {
                if (BattleLevel != -1)
                {
                    Battle_SongJin _Battle = _TotalBattle[BattleLevel];
                    if (_Battle.BATTLESTATE == BattelStatus.STATUS_PREPARE || _Battle.BATTLESTATE == BattelStatus.STATUS_START || _Battle.BATTLESTATE == BattelStatus.STATUS_PREPAREEND || _Battle.BATTLESTATE == BattelStatus.STATUS_END)
                    {
                        //Kiểm tra đăng ký phe tống
                        if (_Battle.SongCampRegister.ContainsKey(player.RoleID))
                        {
                            return 1;
                        }
                        //Kiểm tra đăng ký phe Kim
                        if (_Battle.JinCampRegister.ContainsKey(player.RoleID))
                        {
                            return 2;
                        }
                        return 0;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
        }

        public static int BattleRegister(KPlayer player, int Camp, int Level)
        {
            int BattleLevel = GetBattleLevel(player.m_Level);

            if (BattleLevel != Level)
            {
                return -1000;
            }
            else
            {
                if (BattleLevel != -1)
                {
                    Battle_SongJin _Battle = _TotalBattle[BattleLevel];

                    return _Battle.Register(player, Camp);
                }
                else
                {
                    return -10;
                }
            }
        }

        public static int GetBattleLevelBK(int InputLevel)
        {
            if (GameManager.IsKuaFuServer)
            {
                return 4;
            }
            else
            {
                //if (InputLevel < 90)
                //{
                //    return 1;
                //}
                //else if (InputLevel >= 90 && InputLevel < 120)
                //{
                //    return 2;
                //}
                //else if (InputLevel >= 120)
                //{
                //    return 3;
                //}
                //else
                //{
                //    return -1;
                //}
                if (InputLevel >= 60 && InputLevel <= 200)
                {
                    return 2;
                }
                else
                {
                    return -1;
                }
            }
        }

        public static int GetBattleLevel(int InputLevel)
        {
            if (GameManager.IsKuaFuServer)
            {
                return 4;
            }
            else
            {
                foreach (KeyValuePair<int, Battle_SongJin> Battle in _TotalBattle)
                {
                    if (Battle.Value.GetMinLevelJoin() <= InputLevel && InputLevel <= Battle.Value.GetMaxLevelJoin())
                    {
                        return Battle.Key;
                    }
                }
                return -1;
            }    
        }

        public static SongJinBattleRankingInfo GetRanking(KPlayer _Player)
        {
            SongJinBattleRankingInfo _Ranking = new SongJinBattleRankingInfo();

            int BattleLevel = GetBattleLevel(_Player.m_Level);

            if (BattleLevel != -1)
            {
                Battle_SongJin _Battle = _TotalBattle[BattleLevel];

                return _Battle.RankingBuilder(_Player);
            }

            return _Ranking;
        }

        /// <summary>
        ///  Thực hiện hồi sinh cho player
        /// </summary>
        /// <param name="client"></param>
        public static void Revice(KPlayer client)
        {
            int BattleLevel = GetBattleLevel(client.m_Level);

            if (BattleLevel != -1)
            {
                Battle_SongJin _Battle = _TotalBattle[BattleLevel];
                _Battle.Revice(client);
            }
        }


        #region BattleControler
        public static void ForceStartBattle(int BattleLevel)
        {
            if (BattleLevel != -1)
            {
                Battle_SongJin _Battle = _TotalBattle[BattleLevel];

                _Battle.ForceStartBattle();
            }
        }

        public static void ForceEndBattle(int BattleLevel)
        {
            if (BattleLevel != -1)
            {
                Battle_SongJin _Battle = _TotalBattle[BattleLevel];

                _Battle.ForceEndBattle();
            }
        }

        #endregion

        public static string GetNameCamp(int Camp)
        {
            if (Camp == 10)
            {
                return "Tống";
            }
            else if (Camp == 20)
            {
                return "Kim";
            }
            return "";
        }

        public static bool CanUsingTeleport(KPlayer client)
        {
            int BattleLevel = GetBattleLevel(client.m_Level);

            if (BattleLevel != -1)
            {
                Battle_SongJin _Battle = _TotalBattle[BattleLevel];

                return _Battle.CanUsingTeleport();
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Random các vị trí dịch chuyển ra ngoài của Tống
        /// </summary>
        private static readonly List<UnityEngine.Vector2> RandomTeleportPos_Song = new List<UnityEngine.Vector2>()
        {
            new UnityEngine.Vector2(3380, 2392),
            new UnityEngine.Vector2(3989, 2154),
            new UnityEngine.Vector2(4831, 2000),
            new UnityEngine.Vector2(5449, 2376),
            new UnityEngine.Vector2(5639, 2685),
            new UnityEngine.Vector2(5063, 2849),
            new UnityEngine.Vector2(4443, 2996),
            new UnityEngine.Vector2(3690, 2789),
        };

        /// <summary>
        /// Random các vị trí dịch ra ngoài của Kim
        /// </summary>
        private static readonly List<UnityEngine.Vector2> RandomTeleportPos_Jin = new List<UnityEngine.Vector2>()
        {
            new UnityEngine.Vector2(14111, 7410),
            new UnityEngine.Vector2(14715, 7139),
            new UnityEngine.Vector2(14387, 6818),
            new UnityEngine.Vector2(13572, 6627),
            new UnityEngine.Vector2(12855, 6836),
            new UnityEngine.Vector2(12546, 7206),
            new UnityEngine.Vector2(12779, 7520),
            new UnityEngine.Vector2(13503, 7677),
        };

        /// <summary>
        /// Sử dụng cổng dịch chuyển
        /// </summary>
        /// <param name="player"></param>
        public static void UseTeleport(KPlayer player)
        {
            UnityEngine.Vector2 randomPos;

            if (player.Camp == 10)
            {
                randomPos = Battel_SonJin_Manager.RandomTeleportPos_Song[KTGlobal.GetRandomNumber(0, Battel_SonJin_Manager.RandomTeleportPos_Song.Count - 1)];
            }
            else
            {
                randomPos = Battel_SonJin_Manager.RandomTeleportPos_Jin[KTGlobal.GetRandomNumber(0, Battel_SonJin_Manager.RandomTeleportPos_Jin.Count - 1)];
            }

            /// Dịch chuyển người chơi đến các vị trí ngẫu nhiên
            GameManager.ClientMgr.NotifyOthersGoBack(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, player, (int)randomPos.x, (int)randomPos.y);

            /// Bất tử
            player.SendChangeMapProtectionBuff();
        }

        public static BattelStatus GetBattleStatus(KPlayer client)
        {
            int BattleLevel = GetBattleLevel(client.m_Level);

            if (BattleLevel != -1)
            {
                Battle_SongJin _Battle = _TotalBattle[BattleLevel];

                return _Battle.GetBattelState();
            }
            else
            {
                return BattelStatus.STATUS_NULL;
            }
        }

        public static void NpcClick(GameMap map, NPC npc, KPlayer client)
        {
            int BattleLevel = GetBattleLevel(client.m_Level);

            if (BattleLevel != -1)
            {
                Battle_SongJin _Battle = _TotalBattle[BattleLevel];

                _Battle.CheckAward(map, npc, client);
            }
        }
    }
}