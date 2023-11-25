using GameServer.KiemThe.GameEvents.BaiHuTang;
using GameServer.KiemThe.GameEvents.EmperorTomb;
using GameServer.KiemThe.GameEvents.KnowledgeChallenge;
using GameServer.KiemThe.GameEvents.Model;
using GameServer.KiemThe.GameEvents.TeamBattle;
using GameServer.KiemThe.Logic;
using System.Collections.Concurrent;

namespace GameServer.KiemThe.GameEvents
{
    /// <summary>
    /// Khai báo các hoạt động trong Game
    /// </summary>
    public static partial class GameMapEventsManager
    {
        /// <summary>
        /// Danh sách Script đang thực thi
        /// </summary>
        private static readonly ConcurrentDictionary<int, IActivityScript> activityScripts = new ConcurrentDictionary<int, IActivityScript>();

        /// <summary>
        /// Bắt đầu hoạt động tương ứng
        /// </summary>
        /// <param name="activity"></param>
        public static IActivityScript StartGameMapEvent(KTActivity activity)
        {
            /// Đánh dấu đã bắt đầu hoạt động
            activity.IsStarted = true;
            switch (activity.Data.CoreScript)
            {
                /// Nếu là Bạch Hổ Đường
                case "BaiHuTang":
                {
                    BaiHuTang_ActivityScript script = new BaiHuTang_ActivityScript();
                    script.Activity = activity;
                    /// Thêm sự kiện vào danh sách
                    GameMapEventsManager.activityScripts[activity.Data.ID] = script;
                    script.Begin();
                    return script;
                }
                /// Nếu là Võ Lâm Liên Đấu
                case "TeamBattle":
                {
                    TeamBattle_ActivityScript.NotifyActivity(activity.Data.ID);
                    break;
                }
                /// Nếu là Đoán hoa đăng
                case "KnowledgeChallenge":
                {
                    KnowledgeChallenge_ActivityScript script = new KnowledgeChallenge_ActivityScript();
                    script.Activity = activity;
                    /// Thêm sự kiện vào danh sách
                    GameMapEventsManager.activityScripts[activity.Data.ID] = script;
                    script.Begin();
                    return script;
                }
                /// Nếu là Tần Lăng
                case "EmperorTomb":
                {
                    EmperorTomb_ActivityScript script = new EmperorTomb_ActivityScript();
                    script.Activity = activity;
                    /// Thêm sự kiện vào danh sách
                    GameMapEventsManager.activityScripts[activity.Data.ID] = script;
                    script.Begin();
                    return script;
                }
            }

            return null;
        }

        /// <summary>
        /// Ngừng hoạt động tương ứng
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        public static void StopGameMapEvent(KTActivity activity)
		{
            /// Nếu không tồn tại thì thôi
            if (!GameMapEventsManager.activityScripts.TryGetValue(activity.Data.ID, out IActivityScript script))
            {
                return;
            }

            /// Thực hiện đóng sự kiện
            script.Close();
            /// Xóa khỏi danh sách
            GameMapEventsManager.activityScripts.TryRemove(activity.Data.ID, out _);
        }
    }
}
