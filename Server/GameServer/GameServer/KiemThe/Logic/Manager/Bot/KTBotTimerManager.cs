using GameServer.KiemThe.CopySceneEvents;
using GameServer.KiemThe.Entities;
using GameServer.KiemThe.LuaSystem;
using GameServer.KiemThe.Utilities;
using GameServer.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace GameServer.KiemThe.Logic
{
    /// <summary>
    /// Quản lý luồng thực thi Buff của nhân vật
    /// </summary>
    public class KTBotTimerManager
    {
        #region Singleton - Instance

        /// <summary>
        /// Đối tượng quản lý Timer của đối tượng
        /// </summary>
        public static KTBotTimerManager Instance { get; private set; }

        /// <summary>
        /// Private constructor
        /// </summary>
        private KTBotTimerManager() : base()
        {
            this.InitLocal();
        }

        #endregion Singleton - Instance

        #region Initialize

        /// <summary>
        /// Hàm này gọi đến khởi tạo đối tượng
        /// </summary>
        public static void Init()
        {
            KTBotTimerManager.Instance = new KTBotTimerManager();
        }

        #endregion Initialize

        #region Inheritance
        /// <summary>
        /// Luồng quản lý
        /// </summary>
        private class BotTimer : KTTimer
        {
            /// <summary>
            /// Chủ nhân
            /// </summary>
            public KTBot Owner { get; set; }

            /// <summary>
            /// Thời gian tồn tại
            /// </summary>
            public float LifeTime { get; set; }

            /// <summary>
            /// Thời gian thực hiện random dịch chuyển AI
            /// </summary>
            public float AIRandomMoveTick { get; set; }
        }

        /// <summary>
        /// Inner Timer
        /// </summary>
        private class InnerTimer : KTTimerManager<BotTimer>
        {
            /// <summary>
            /// Thời gian kích hoạt luồng kiểm tra
            /// </summary>
            protected override int PeriodTick
            {
                get
                {
                    return 500;
                }
            }

            /// <summary>
            /// Inner Timer
            /// </summary>
            public InnerTimer() : base()
            {

            }
        }
        #endregion

        /// <summary>
        /// Thời gian mỗi lần tự tìm vị trí ngẫu nhiên xung quanh để chạy đến
        /// </summary>
        private const float AIRandomMoveTickMin = 5f;

        /// <summary>
        /// Thời gian mỗi lần tự tìm vị trí ngẫu nhiên xung quanh để chạy đến
        /// </summary>
        private const float AIRandomMoveTickMax = 10f;

        /// <summary>
        /// Thời gian chạy Tick
        /// </summary>
        private const float PeriodTick = 0.5f;

        /// <summary>
        /// Luồng quản lý
        /// </summary>
        private readonly InnerTimer timer = new InnerTimer();

        /// <summary>
        /// Danh sách Timer đang thực thi
        /// </summary>
        private readonly List<BotTimer> botTimers = new List<BotTimer>();

        /// <summary>
        /// Đối tượng Random
        /// </summary>
        private readonly Random random = new Random();

        #region Core
        /// <summary>
        /// Khởi tạo ban đầu nội bộ
        /// </summary>
        private void InitLocal()
        {

        }
        #endregion

        /// <summary>
        /// Thêm đối tượng vào danh sách quản lý
        /// </summary>
        /// <param name="obj"></param>
        public void Add(KTBot obj)
        {
            if (obj == null)
            {
                return;
            }

            lock (this.botTimers)
            {
                /// Timer cũ
                BotTimer objectTimer = this.botTimers.Where(x => x.Owner == obj).FirstOrDefault();
                /// Nếu đã tồn tại thì bỏ qua
                if (objectTimer != null)
                {
                    return;
                }
            }

            BotTimer timer = new BotTimer()
            {
                Name = "Monster " + obj.RoleID,
                Alive = true,
                Interval = -1,
                PeriodActivation = KTBotTimerManager.PeriodTick,
                Owner = obj,
                Start = () =>
                {
                    this.ProcessStart(obj);
                },
            };
            timer.AIRandomMoveTick = random.Next((int) KTBotTimerManager.AIRandomMoveTickMin, (int) KTBotTimerManager.AIRandomMoveTickMax);
            timer.Tick = () => {
                this.ProcessTick(obj);

                timer.LifeTime += KTBotTimerManager.PeriodTick;
                if (timer.LifeTime > 0 && timer.LifeTime % timer.AIRandomMoveTick == 0)
                {
                    this.ProcessAIMoveTick(obj);
                }
            };
            this.timer.AddTimer(timer);

            lock (this.botTimers)
            {
                this.botTimers.Add(timer);
            }
        }

        /// <summary>
        /// Dừng và xóa đối tượng khỏi luồng thực thi
        /// </summary>
        /// <param name="obj"></param>
        public void Remove(KTBot obj)
        {
            lock (this.botTimers)
            {
                BotTimer objectTimer = this.botTimers.Where(x => x.Owner == obj).FirstOrDefault();
                if (objectTimer != null)
                {
                    this.timer.KillTimer(objectTimer);
                    this.botTimers.Remove(objectTimer);
                }

                /// Làm rỗng danh sách biến cục bộ
                obj.RemoveAllLocalParams();
                /// Làm mới AI
                obj.ResetAI();
            }
        }

        #region Logic
        /// <summary>
        /// Đánh dấu có cần thiết phải xóa quái vật này ra khỏi Timer không
        /// <para>- Nếu chết</para>
        /// <para>- Nếu không có người chơi xung quanh</para>
        /// <param name="checkPlayersAround"></param>
        /// </summary>
        /// <returns></returns>
        private bool NeedToBeRemoved(KTBot bot, bool checkPlayersAround = false)
        {
            /// Nếu không có tham chiếu
            if (bot == null)
            {
                return true;
            }

            /// Nếu không tìm thấy bản đồ hiện tại
            if (!GameManager.MapMgr.DictMaps.TryGetValue(bot.CurrentMapCode, out GameMap map))
            {
                return true;
            }
            /// Nếu có phụ bản nhưng không tìm thấy thông tin
            if (bot.CurrentCopyMapID != -1 && !CopySceneEventManager.IsCopySceneExist(bot.CurrentCopyMapID, bot.CurrentMapCode))
            {
                return true;
            }

            /// Nếu kiểm tra người chơi xung quanh
            if (checkPlayersAround)
            {
                /// Nếu không có người chơi xung quanh
                if (bot.VisibleClientsNum <= 0)
                {
                    return true;
                }
            }

            /// Nếu thỏa mãn tất cả điều kiện
            return false;
        }

        /// <summary>
        /// Thực hiện hàm Start
        /// </summary>
        /// <param name="monster"></param>
        private void ProcessStart(KTBot bot)
        {
            /// Nếu vi phạm điều kiện, cần xóa khỏi Timer
            if (this.NeedToBeRemoved(bot))
            {
                this.Remove(bot);
                return;
            }

            /// Nếu Script có tồn tại trong hệ thống
            if (KTLuaScript.Instance.GetScriptByID(bot.ScriptID) != null)
            {
                /// ID bản đồ hiện tại
                GameManager.MapMgr.DictMaps.TryGetValue(bot.CurrentMapCode, out GameMap map);

                /// Thực hiện ScriptLua tương ứng
                KTLuaEnvironment.ExecuteBotScript_Start(map, bot, bot.ScriptID);
            }
        }

        /// <summary>
        /// Thực hiện hàm Tick
        /// </summary>
        /// <param name="monster"></param>
        private void ProcessTick(KTBot bot)
        {
            /// Nếu vi phạm điều kiện, cần xóa khỏi Timer
            if (this.NeedToBeRemoved(bot))
            {
                this.Remove(bot);
                return;
            }

            /// Nếu quái đã chết
            if (bot.IsDead())
            {
                return;
            }

            ///// Thực hiện hàm Start của đối tượng
            //bot.Tick();

            ///// Thực hiện hàm AI_Tick
            //bot.AI_Tick();

            ///// Nếu Script có tồn tại trong hệ thống
            //if (KTLuaScript.Instance.GetScriptByID(bot.ScriptID) != null)
            //{
            //    /// ID bản đồ hiện tại
            //    GameManager.MapMgr.DictMaps.TryGetValue(bot.CurrentMapCode, out GameMap map);

            //    /// Thực hiện ScriptLua tương ứng
            //    KTLuaEnvironment.ExecuteBotScript_Tick(map, bot, bot.ScriptID);
            //}
        }

        /// <summary>
        /// Thực hiện hàm AITick
        /// </summary>
        /// <param name="bot"></param>
        private void ProcessAIMoveTick(KTBot bot)
        {
            /// Nếu vi phạm điều kiện, cần xóa khỏi Timer
            if (this.NeedToBeRemoved(bot, true))
            {
                this.Remove(bot);
                return;
            }

            bot.RandomMoveAround();
        }
        #endregion
    }
}