using GameServer.KiemThe.Entities;
using GameServer.KiemThe.Utilities;
using GameServer.KiemThe.Utilities.Algorithms;
using GameServer.Logic;
using GameServer.Server;
using Server.Data;
using Server.Tools;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace GameServer.KiemThe.Logic
{
    /// <summary>
    /// Đối tượng thực thi dịch chuyển BOT theo dãy đường đi cho trước
    /// </summary>
    public class KTOtherObjectStoryBoard
    {
        #region Singleton - Instance
        /// <summary>
        /// Đối tượng thực thi dịch chuyển BOT theo dãy đường đi cho trước
        /// </summary>
        public static KTOtherObjectStoryBoard Instance { get; private set; }

        private KTOtherObjectStoryBoard() { }
        #endregion

        #region Initialize
        public static void Init()
        {
            KTOtherObjectStoryBoard.Instance = new KTOtherObjectStoryBoard();
        }
        #endregion

        #region Inheritance
        /// <summary>
        /// Luồng quản lý
        /// </summary>
        private class OtherObjectStoryBoardTimer : KTTimer
        {
            /// <summary>
            /// Chủ nhân
            /// </summary>
            public GameObject Owner { get; set; }

            /// <summary>
            /// Đường đi
            /// </summary>
            public Queue<UnityEngine.Vector2> Paths { get; set; }

            /// <summary>
            /// Thời gian thực thi lần trước
            /// </summary>
            public long LastTick { get; set; } = 0;

            /// <summary>
            /// Tọa độ lưới lần tước
            /// </summary>
            public UnityEngine.Vector2 LastGridPos { get; set; }

            /// <summary>
            /// Loại động tác di chuyển là chạy hay đi bộ
            /// </summary>
            public KE_NPC_DOING Action { get; set; }
        }

        /// <summary>
        /// Inner Timer
        /// </summary>
        private class InnerTimer : KTTimerManager<OtherObjectStoryBoardTimer>
        {
            /// <summary>
            /// Thời gian kích hoạt luồng kiểm tra
            /// </summary>
            protected override int PeriodTick
            {
                get
                {
                    return 100;
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
        /// Thời gian Tick kiểm tra
        /// </summary>
        private const float PeriodTick = 0.1f;

        /// <summary>
        /// Luồng quản lý
        /// </summary>
        private readonly InnerTimer timer = new InnerTimer();

        /// <summary>
        /// Danh sách các đối tượng đang quản lý
        /// </summary>
        private readonly ConcurrentDictionary<int, OtherObjectStoryBoardTimer> objectTimers = new ConcurrentDictionary<int, OtherObjectStoryBoardTimer>();

        #region Implements
        /// <summary>
        /// Thêm đối tượng cần dịch chuyển vào danh sách
        /// </summary>
        /// <param name="go"></param>
        /// <param name="fromPos"></param>
        /// <param name="toPos"></param>
        /// <param name="action"></param>
        public void Add(GameObject go, Point fromPos, Point toPos, KE_NPC_DOING action = KE_NPC_DOING.do_run, bool forceMoveBySpecialState = false)
        {
            if (go == null)
            {
                return;
            }
            else if (fromPos == default || toPos == default)
            {
                return;
            }
            /// Nếu dính trạng thái không thể di chuyển
            else if ((!forceMoveBySpecialState && !go.IsCanPositiveMove()) || (forceMoveBySpecialState && !go.IsCanMove()))
            {
                return;
            }

            /// Dừng thực thi StoryBoard trước đó
            this.Remove(go);

            /// Thực hiện tìm đường
            List<UnityEngine.Vector2> nodeList = KTGlobal.FindPath(go, new UnityEngine.Vector2((float) fromPos.X, (float) fromPos.Y), new UnityEngine.Vector2((float) toPos.X, (float) toPos.Y));

            /// Nếu không có đường đi thì bỏ qua
            if (nodeList.Count < 2)
            {
                return;
            }

            /// Hàng đợi chứa kết quả
            Queue<UnityEngine.Vector2> paths = new Queue<UnityEngine.Vector2>();
            /// Nạp tất cả các điểm vừa tìm được vào hàng đợi
            foreach (UnityEngine.Vector2 pos in nodeList)
            {
                paths.Enqueue(pos);
            }


            string pathStr = "";
            foreach (UnityEngine.Vector2 pos in paths)
            {
                pathStr += "|" + string.Format("{0}_{1}", (int) pos.x, (int) pos.y);
            }
            pathStr = pathStr.Substring(1);
            /// Gửi gói tin về Client thông báo
            this.SendObjectMoveToClient(go, pathStr, fromPos, toPos, go.CurrentDir, action);

            //Console.WriteLine("Move from {0} to {1} with paths = {2}", fromPos.ToString(), toPos.ToString(), pathStr);

            GameMap gameMap = GameManager.MapMgr.DictMaps[go.CurrentMapCode];

            /// Vị trí hiện tại của BOT
            UnityEngine.Vector2 currentPos = new UnityEngine.Vector2((int) go.CurrentPos.X, (int) go.CurrentPos.Y);
            /// Vị trí đích đến
            go.ToPos = toPos;

            OtherObjectStoryBoardTimer timer = new OtherObjectStoryBoardTimer()
            {
                Name = "Storyboard - Other Object " + go.RoleID,
                Alive = true,
                Interval = -1,
                PeriodActivation = KTOtherObjectStoryBoard.PeriodTick,
                Owner = go,
                Paths = paths,
                LastGridPos = KTGlobal.WorldPositionToGridPosition(gameMap, currentPos),
                LastTick = KTGlobal.GetCurrentTimeMilis(),
                Action = action,
            };
            timer.Start = () => {
                this.StartStoryBoard(timer);
            };
            timer.Tick = () => {
                this.TickStoryBoard(timer);
            };
            timer.Destroy = () => {
                this.FinishStoryBoard(timer);
            };
            this.timer.AddTimer(timer);

            this.objectTimers[go.RoleID] = timer;
        }

        /// <summary>
        /// Thêm đối tượng cần dịch chuyển vào danh sách
        /// </summary>
        /// <param name="monster"></param>
        /// <param name="pathString"></param>
        /// <param name="action"></param>
        public void Add(GameObject go, string pathString, KE_NPC_DOING action = KE_NPC_DOING.do_run, bool forceMoveBySpecialState = false)
        {
            if (go == null)
            {
                return;
            }
            /// Nếu dính trạng thái không thể di chuyển
            else if ((!forceMoveBySpecialState && !go.IsCanPositiveMove()) || (forceMoveBySpecialState && !go.IsCanMove()))
            {
                return;
            }

            /// Dừng thực thi StoryBoard trước đó
            this.Remove(go);

            Queue<UnityEngine.Vector2> paths = new Queue<UnityEngine.Vector2>();
            string[] points = pathString.Split('|');
            if (points.Length < 2)
            {
                return;
            }

            GameMap gameMap = GameManager.MapMgr.DictMaps[go.CurrentMapCode];

            Point toPos = new Point(-1, -1);
            /// Nạp tất cả các điểm vừa tìm được vào hàng đợi
            for (int i = 0; i < points.Length; i++)
            {
                string[] pos = points[i].Split('_');
                if (pos.Length < 2)
                {
                    throw new Exception();
                }

                int posX = int.Parse(pos[0]);
                int posY = int.Parse(pos[1]);

                /// Nếu vị trí hiện tại không thể đến được
                if (!GameManager.MapMgr.GetGameMap(go.CurrentMapCode).CanMove(new Point(posX, posY)))
                {
                    continue;
                }

                toPos = new Point(posX, posY);

                paths.Enqueue(new UnityEngine.Vector2(posX, posY));
            }

            /// Vị trí hiện tại của BOT
            UnityEngine.Vector2 currentPos = new UnityEngine.Vector2((int) go.CurrentPos.X, (int) go.CurrentPos.Y);
            /// Vị trí đích đến
            go.ToPos = toPos;

            OtherObjectStoryBoardTimer timer = new OtherObjectStoryBoardTimer()
            {
                Name = "Storyboard - Other Object " + go.RoleID,
                Alive = true,
                Interval = -1,
                PeriodActivation = KTOtherObjectStoryBoard.PeriodTick,
                Owner = go,
                Paths = paths,
                LastGridPos = KTGlobal.WorldPositionToGridPosition(gameMap, currentPos),
                LastTick = KTGlobal.GetCurrentTimeMilis(),
                Action = action,
            };
            timer.Start = () => {
                this.StartStoryBoard(timer);
            };
            timer.Tick = () => {
                this.TickStoryBoard(timer);
            };
            timer.Destroy = () => {
                this.FinishStoryBoard(timer);
            };
            this.timer.AddTimer(timer);

            this.objectTimers[go.RoleID] = timer;
        }

        /// <summary>
        /// Xóa đối tượng khỏi danh sách
        /// </summary>
        /// <param name="go"></param>
        /// <param name="synsClient"></param>
        /// <param name="includeElapse"></param>
        /// <param name="additionTick"></param>
        public void Remove(GameObject go, bool synsClient = true/*, bool includeElapse = true, long additionTick = 0*/)
        {
            if (this.objectTimers.TryGetValue(go.RoleID, out OtherObjectStoryBoardTimer objectTimer))
            {
                /*
                /// Thực hiện nốt thời gian deltaT từ lần cuối Tick cho đến hiện tại hủy, nếu khoảng deltaT chênh lệch đáng kể
                if (includeElapse && (KTGlobal.GetCurrentTimeMilis() - objectTimer.LastTick + additionTick) / 1000f >= 0.02f)
                {
                    this.TickStoryBoard(objectTimer, additionTick, false);
                }
                */

                this.timer.KillTimer(objectTimer);
                this.objectTimers.TryRemove(go.RoleID, out _);
                go.ToPos = go.CurrentPos;

                /// Đồng bộ về Client
                if (synsClient)
                {
                    this.SendObjectStopMoveToClient(go);
                }
            }
        }

        /// <summary>
        /// Kiểm tra có StoryBoard của BOT tương ứng đang thực thi không
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        public bool HasStoryBoard(GameObject go)
        {
            return this.objectTimers.TryGetValue(go.RoleID, out _);
        }

        /// <summary>
        /// Trả về danh sách đường đi hiện tại của đối tượng
        /// </summary>
        /// <returns></returns>
        public string GetCurrentPathString(GameObject go)
        {
            if (this.objectTimers.TryGetValue(go.RoleID, out OtherObjectStoryBoardTimer objectTimer))
            {
                Queue<UnityEngine.Vector2> paths = new Queue<UnityEngine.Vector2>(objectTimer.Paths);

                /// Vị trí hiện tại
                UnityEngine.Vector2 currentPos = new UnityEngine.Vector2((int) go.CurrentPos.X, (int) go.CurrentPos.Y);
                string str = string.Format("{0}_{1}", (int) currentPos.x, (int) currentPos.y) + "|";
                while (paths.Count > 0)
                {
                    UnityEngine.Vector2 pos = paths.Dequeue();
                    str += string.Format("{0}_{1}", (int) pos.x, (int) pos.y) + "|";
                }
                if (str.Length > 0)
                {
                    return str.Substring(0, str.Length - 1);
                }
                else
                {
                    return str;
                }
            }
            else
            {
                return "";
            }
        }
        #endregion

        #region Network
        /// <summary>
        /// Gửi gói tin BOT vật di chuyển tới người chơi xung quanh
        /// </summary>
        /// <param name="monster"></param>
        /// <param name="pathString"></param>
        /// <param name="fromPos"></param>
        /// <param name="toPos"></param>
        /// <param name="direction"></param>
        /// <param name="action"></param>
        private void SendObjectMoveToClient(GameObject go, string pathString, Point fromPos, Point toPos, KiemThe.Entities.Direction direction, KE_NPC_DOING action)
        {
            try
            {
                int fromPosX = (int) fromPos.X;
                int fromPosY = (int) fromPos.Y;
                int toPosX = (int) toPos.X;
                int toPosY = (int) toPos.Y;

                string zipPathString = pathString;

                int moveSpeed = go.GetCurrentRunSpeed();
                GameManager.ClientMgr.NotifyOthersToMoving(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, go, go.CurrentMapCode, go.CurrentCopyMapID, go.RoleID, KTGlobal.GetCurrentTimeMilis(), fromPosX, fromPosY, (int) action, toPosX, toPosY, (int) TCPGameServerCmds.CMD_SPR_MOVE, moveSpeed, zipPathString, null, direction);
            }
            catch (Exception ex)
            {
                LogManager.WriteException(ex.ToString());
            }
        }

        /// <summary>
        /// Thông báo cho người chơi khác đối tượng ngừng di chuyển
        /// </summary>
        /// <param name="obj"></param>
        private void SendObjectStopMoveToClient(GameObject go)
        {
            List<object> objsList = Global.GetAll9Clients(go);

            /// Tạo mới gói tin
            byte[] data = DataHelper.ObjectToBytes<SpriteStopMove>(new SpriteStopMove()
            {
                RoleID = go.RoleID,
                PosX = (int) go.CurrentPos.X,
                PosY = (int) go.CurrentPos.Y,
                MoveSpeed = go.GetCurrentRunSpeed(),
            });

            GameManager.ClientMgr.SendToClients(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, go, objsList, data, (int) TCPGameServerCmds.CMD_SPR_STOPMOVE);
        }
        #endregion

        #region Logic
        /// <summary>
        /// Bắt đầu thực hiện StoryBoard
        /// </summary>
        /// <param name="timer"></param>
        private void StartStoryBoard(OtherObjectStoryBoardTimer timer)
        {
            /// Nếu dính trạng thái không thể di chuyển
            if (!timer.Owner.IsCanMove())
            {
                this.Remove(timer.Owner);
                return;
            }

            ///// Nếu hàng đợi rỗng
            //if (timer.Paths.Count <= 0)
            //{
            //    return;
            //}

            /// Cập nhật động tác tương ứng
            timer.Owner.m_eDoing = timer.Action;

            /// Vị trí hiện tại
            UnityEngine.Vector2 currentPos = new UnityEngine.Vector2((int) timer.Owner.CurrentPos.X, (int) timer.Owner.CurrentPos.Y);
            /// Nếu vị trí hiện tại trùng với vị trí đầu tiên trong hàng đợi thì bỏ qua vị trí đầu
            if (currentPos == timer.Paths.Peek())
            {
                timer.Paths.Dequeue();
            }
        }

        /// <summary>
        /// Tick thực hiện StoryBoard
        /// </summary>
        /// <param name="timer"></param>
        private void TickStoryBoard(OtherObjectStoryBoardTimer timer, long additionTick = 0, bool autoRemoveUnusedStoryBoard = true)
        {
            ///// Nếu dính trạng thái không thể di chuyển
            //if (autoRemoveUnusedStoryBoard && !timer.Owner.IsCanMove())
            //{
            //    this.Remove(timer.Owner);
            //    return;
            //}

            ///// Thời gian Tick hiện tại
            //long currentTick = KTGlobal.GetCurrentTimeMilis();
            ///// Thời gian Tick lần trước
            //long lastTick = timer.LastTick;
            ///// Khoảng thời gian cần mô phỏng (giây)
            //float elapseTime = (currentTick - lastTick + additionTick) / 1000f;
            ///// Cập nhật thời gian Tick hiện tại
            //timer.LastTick = currentTick;

            ///// Thực hiện di chuyển đối tượng trong khoảng thời gian cần mô phỏng
            //bool needToRemove = KTStoryBoardLogic.StepMove(timer.Owner, timer.Paths, elapseTime, timer.Action);

            //GameMap gameMap = GameManager.MapMgr.GetGameMap(timer.Owner.CurrentMapCode);
            ///// Vị trí mới sau khi di chuyển
            //UnityEngine.Vector2 newPos = new UnityEngine.Vector2((int) timer.Owner.CurrentPos.X, (int) timer.Owner.CurrentPos.Y);
            ///// Tọa độ lưới
            //UnityEngine.Vector2 newGridPos = KTGlobal.WorldPositionToGridPosition(gameMap, newPos);

            ///// Nếu tọa độ lưới cũ khác tọa độ lưới mới
            //if (newGridPos != timer.LastGridPos)
            //{
            //    /// Cập nhật vị trí đối tượng vào Map
            //    GameManager.MapGridMgr.DictGrids.TryGetValue(timer.Owner.CurrentMapCode, out MapGrid mapGrid);
            //    if (mapGrid != null)
            //    {
            //        mapGrid.MoveObject(-1, -1, (int) timer.Owner.CurrentPos.X, (int) timer.Owner.CurrentPos.Y, timer.Owner);
            //    }

            //    /// Cập nhật tọa độ lưới cũ
            //    timer.LastGridPos = newGridPos;
            //}

            ///// Nếu cần thiết phải xóa StoryBoard
            //if (autoRemoveUnusedStoryBoard && needToRemove)
            //{
            //    this.Remove(timer.Owner);
            //    return;
            //}
        }

        /// <summary>
        /// Hoàn thành StoryBoard
        /// </summary>
        /// <param name="timer"></param>
        private void FinishStoryBoard(OtherObjectStoryBoardTimer timer)
        {
            /// Nếu không có StoryBoard cũ
            if (!this.HasStoryBoard(timer.Owner))
            {
                timer.Owner.m_eDoing = Entities.KE_NPC_DOING.do_stand;
            }
        }
        #endregion
    }
}
