using GameServer.KiemThe.Core.Item;
using GameServer.KiemThe.Logic;
using GameServer.KiemThe.LuaSystem.Logic;
using GameServer.Logic;
using GameServer.Server;
using Server.Data;
using Server.Protocol;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameServer.KiemThe.Core.Task
{
    /// <summary>
    /// Xử lý nhiệm vụ nghĩa quân
    /// </summary>
    public class TaskDailyArmyManager
    {
        private static TaskDailyArmyManager instance = new TaskDailyArmyManager();

        public Dictionary<int, Task> _TotalTaskData = new Dictionary<int, Task>();

        public static TaskDailyArmyManager getInstance()
        {
            return instance;
        }

        public bool IsHaveCompleteArmyQuest(int NpcID, KPlayer Client)
        {
            List<int> tasksList = null;

            if (!GameManager.NPCTasksMgr.DestNPCTasksDict.TryGetValue(NpcID, out tasksList))
                return false;   //Nếu thằng nPC này không có nhiệm vụ này thì chim cút

            if (0 == tasksList.Count)
                return false;   /// Nếu mà số lượng task trả về ==0 thì cũng chim cút

            // Nếu danh sách nhiệm vụ hiện có mà đéo có task nào thì chứng tỏ thằng này chưa làm nhiệm vụ nào==> Đéo có nhiệm vụ nào có thể hoàn tất
            if (Client.TaskDataList == null)
            {
                return false;
            }

            TCPOutPacketPool pool = Global._TCPManager.TcpOutPacketPool;
            TCPClientPool tcpClientPool = Global._TCPManager.tcpClientPool;

            // Nếu thằng này có nhiệm vụ có thể trả thì cứ check nếu có dạng nhiệm vụ dạng nói chuyện
            ProcessTask.Process(Global._TCPManager.MySocketListener, pool, Client, NpcID, NpcID, -1, TaskTypes.Talk);

            bool IsHaveComplteQuest = false;

            foreach (int TaskSelect in tasksList)
            {
                // Xuống đây check lại 1 lượt nữa phòng khi ở trên đã hoàn thành nhiệm vụ
                if (Client.TaskDataList == null)
                {
                    break;
                }
                if (Client.TaskDataList.Count == 0)
                {
                    break;
                }

                var find = Client.TaskDataList.Where(x => x.DoingTaskID == TaskSelect).FirstOrDefault();

                if (find != null)
                {
                    Task _Find = this.GetTaskTemplate(find.DoingTaskID);

                    if (_Find != null)
                    {
                        IsHaveComplteQuest = true;
                        break;
                    }

                    if (this.IsQuestComplete(find.DoingTaskID, find.DoingTaskVal1))
                    {
                        IsHaveComplteQuest = true;
                        break;
                    }
                }
            }

            return IsHaveComplteQuest;
        }

        public bool IsQuestComplete(int TaskID, int TaskValue)
        {
            bool IsComplete = false;

            Task _Task = this.GetTaskTemplate(TaskID);

            if (_Task.TargetType == (int)TaskTypes.Talk || _Task.TargetType == (int)TaskTypes.AnswerQuest || _Task.TargetType == (int)TaskTypes.JoinFaction)
            {
                if (TaskValue >= 1)
                {
                    IsComplete = true;
                }
            }

            if (_Task.TargetType == (int)TaskTypes.KillMonster || _Task.TargetType == (int)TaskTypes.MonsterSomething || _Task.TargetType == (int)TaskTypes.Crafting || _Task.TargetType == (int)TaskTypes.Enhance || _Task.TargetType == (int)TaskTypes.BuySomething || _Task.TargetType == (int)TaskTypes.UseSomething || _Task.TargetType == (int)TaskTypes.TransferSomething || _Task.TargetType == (int)TaskTypes.GetSomething || _Task.TargetType == (int)TaskTypes.Collect)
            {
                int NumberRequest = _Task.TargetNum;

                if (TaskValue >= NumberRequest)
                {
                    IsComplete = true;
                }
            }

            return IsComplete;
        }

        public bool IsHaveArmyQuest(int NpcID, KPlayer client)
        {
            List<int> tasksList = null;

            if (!GameManager.NPCTasksMgr.SourceNPCTasksDict.TryGetValue(NpcID, out tasksList))
                return false;

            if (0 == tasksList.Count)
                return false;

            bool IsHaveQuest = false;

            foreach (int TaskID in tasksList)
            {
                if (this.CanTakeNewTask(client, TaskID))
                {
                    IsHaveQuest = true;
                    break;
                }
            }

            return IsHaveQuest;
        }

        /// <summary>
        /// Set vào danh sách nhiệm vụ
        /// </summary>
        /// <param name="_InPut"></param>
        public void SetTask(Dictionary<int, Task> _InPut)
        {
            this._TotalTaskData = _InPut;
        }

        public bool CanTakeNewTask(KPlayer client, int taskID)
        {
            Task systemTask = this.GetTaskTemplate(taskID);

            if (systemTask == null)
            {
                return false;
            }

            // Nếu trên người đang có 1 task BVĐ đang làm dở thì méo cho nhận nữa
            if (client.TaskDataList != null)
            {
                if (client.TaskDataList.Count > 0)
                {
                    //Kiểm tra xem nó đang có nhiệm vụ bạo vặn đồng nào khác không
                    foreach (TaskData task in client.TaskDataList)
                    {
                        Task _Task = this.GetTaskTemplate(task.DoingTaskID);
                        {
                            if (_Task != null)
                            {
                                if ((int)TaskClasses.NghiaQuan == _Task.TaskClass)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            //Nếu ko đủ cấp thì không được nhận
            if (client.m_Level < 20)
            {
                return false;
            }
            //Nếu phái không hợp
            if (client.m_cPlayerFaction.GetFactionId() == 0)
            {
                return false;
            }
            // Nếu đã làm tổng 50 nhiệm vụ 1 ngày thì thôi
            if (client.QuestBVDTodayCount >= 50)
            {
                return false;
            }

            // Nếu giới tính không phù hợp
            int taskSex = systemTask.SexCondition;
            if (0 != taskSex)
            {
                if (client.RoleSex != taskSex)
                {
                    return false;
                }
            }

            // Nếu phái không phù hợp
            int taskOccupation = systemTask.OccupCondition;
            if (0 != taskOccupation)
            {
                int nOcc = client.m_cPlayerFaction.GetFactionId();

                if (nOcc != taskOccupation)
                {
                    return false;
                }
            }

            int taskClass = systemTask.TaskClass;

            // Lấy ra cấp độ

            if (client.m_Level == 0)
            {
                client.m_Level = 1;
            }

            int minLevel = systemTask.MinLevel;

            // Lấy ra MAX LEVEL
            int maxLevel = systemTask.MaxLevel;

            // Nếu không đủ cấp độ
            if (client.m_Level < minLevel || client.m_Level > maxLevel)
            {
                return false;
            }

            return true; //Return true
        }

       

        public Task GetTaskTemplate(int ID)
        {
            if (_TotalTaskData.ContainsKey(ID))
            {
                return _TotalTaskData[ID];
            }
            else
            {
                return null;
            }
        }

        public void GetNpcDataQuest(GameMap map, NPC npc, KPlayer client)
        {
            TCPOutPacketPool pool = Global._TCPManager.TcpOutPacketPool;
            TCPClientPool tcpClientPool = Global._TCPManager.tcpClientPool;
            KNPCDialog _NpcDialog = new KNPCDialog();

            Dictionary<int, string> Selections = new Dictionary<int, string>();

            Dictionary<int, string> OtherPram = new Dictionary<int, string>();

            if(client.CurenQuestIDBVD == -1)
            {
                this.GiveTaskArmyDaily(client, true);
            }    

            Task _TaskGet = this.GetTaskTemplate(client.CurenQuestIDBVD);

            if (client.TaskDataList != null)
            {
                // Đoạn này để tránh bug nếu mà nhiệm vụ hiện tại khác nhiệm vụ đã nhận trước đó
                foreach (TaskData TaskArmy in client.TaskDataList)
                {
                    Task _Task = this.GetTaskTemplate(TaskArmy.DoingTaskID);

                    //Tức là đang có nhiệm vụ BVD đang nhận
                    if (_Task != null)
                    {
                        if (_TaskGet.ID != _Task.ID)
                        {
                            // Gán nhiện vụ hiện tại = nhiệm vụ đang hoàn thành
                            _TaskGet = _Task;
                        }
                    }
                }
            }

            string Hisitory = "Hôm nay ngươi liên tục hoàn thành " + client.QuestBVDStreakCount + " nhiệm vụ<br><br>Tổng nhiệm vụ đã hoàn thành : " + client.QuestBVDTodayCount + "/50<br><br>Số lần hủy còn lại :" + client.CanncelQuestBVD + "/5";

            string PhanThuong = "Phần Thưởng  : Danh Vọng: " + KTGlobal.CreateStringByColor(_TaskGet.Point2 + "", ColorType.Done) + " | EXP: " + KTGlobal.CreateStringByColor(_TaskGet.Experienceaward + "", ColorType.Done) + " | Bạc Khóa :" + KTGlobal.CreateStringByColor(_TaskGet.BacKhoa + "", ColorType.Done);

            string ThuongMoc = "Thưởng Mốc : Danh Vọng: " + KTGlobal.CreateStringByColor("20", ColorType.Done) + " | EXP: " + KTGlobal.CreateStringByColor("100000", ColorType.Done) + " | Bạc Khóa :" + KTGlobal.CreateStringByColor("10000", ColorType.Done) + "| T/H Lực :" + KTGlobal.CreateStringByColor("20", ColorType.Done);

            _NpcDialog.Text = Hisitory + "<br><br>" + KTGlobal.CreateStringByColor("Nhiệm Vụ :", ColorType.Accpect) + _TaskGet.Title + "<br><br>" + KTGlobal.CreateStringByColor("Mục Tiêu :", ColorType.Importal) + "<br><br>" + _TaskGet.AcceptTalk + "<br><br>" + PhanThuong + "<br><br>" + ThuongMoc;

            if (client.TaskDataList == null)
            {
                client.TaskDataList = new List<TaskData>();
            }

            var findTaskClient = client.TaskDataList.Where(x => x.DoingTaskID == _TaskGet.ID).FirstOrDefault();

            if (findTaskClient != null)
            {
                if (IsQuestComplete(findTaskClient.DoingTaskID, findTaskClient.DoingTaskVal1))
                {
                    Selections.Add(2, KTGlobal.CreateStringByColor("Hoàn Thành", ColorType.Done));
                }
                else
                {
                    Selections.Add(31, KTGlobal.CreateStringByColor("Hoàn Thành Nhanh (300 đồng)", ColorType.Done));
					Selections.Add(3, KTGlobal.CreateStringByColor("Ta muốn hủy nhiệm vụ", ColorType.Importal));																																	  
                }
            }
            else
            {
                if (client.QuestBVDTodayCount < 50)
                {
                    Selections.Add(1, KTGlobal.CreateStringByColor("Tiếp nhận", ColorType.Accpect));
                }
                else
                {
                    Selections.Add(5, KTGlobal.CreateStringByColor("Ngày mai quay lại", ColorType.Normal));
                }
            }

            Action<TaskCallBack> ActionWork = (x) => QuestArmyProsecc(map, npc, client, x, _TaskGet.ID);

            Selections.Add(4, KTGlobal.CreateStringByColor("Thông tin", ColorType.Normal));

            _NpcDialog.OnSelect = ActionWork;

            _NpcDialog.Selections = Selections;

            _NpcDialog.OtherParams = OtherPram;

            _NpcDialog.Show(npc, client);
        }


        public void GetInfoArmyTask(GameMap map, NPC npc, KPlayer client)
        {
            TCPOutPacketPool pool = Global._TCPManager.TcpOutPacketPool;
            TCPClientPool tcpClientPool = Global._TCPManager.tcpClientPool;
            KNPCDialog _NpcDialog = new KNPCDialog();

            Dictionary<int, string> Selections = new Dictionary<int, string>();

            Dictionary<int, string> OtherPram = new Dictionary<int, string>();


            string Hisitory = "Hôm nay ngươi liên tục hoàn thành " + client.QuestBVDStreakCount + " nhiệm vụ<br><br>Tổng nhiệm vụ đã hoàn thành : " + client.QuestBVDTodayCount + "/50<br><br>Số lần hủy còn lại :" + client.CanncelQuestBVD + "/5";


            _NpcDialog.Text = Hisitory;
       
            _NpcDialog.Selections = Selections;

            _NpcDialog.OtherParams = OtherPram;

            _NpcDialog.Show(npc, client);
        }

        public bool IsHaveCompleteQuest(int NpcID, KPlayer Client)
        {
            List<int> tasksList = null;

            if (!GameManager.NPCTasksMgr.DestNPCTasksDict.TryGetValue(NpcID, out tasksList))
                return false;   //Nếu thằng nPC này không có nhiệm vụ này thì chim cút

            if (0 == tasksList.Count)
                return false;   /// Nếu mà số lượng task trả về ==0 thì cũng chim cút

            // Nếu danh sách nhiệm vụ hiện có mà đéo có task nào thì chứng tỏ thằng này chưa làm nhiệm vụ nào==> Đéo có nhiệm vụ nào có thể hoàn tất
            if (Client.TaskDataList == null)
            {
                return false;
            }

            TCPOutPacketPool pool = Global._TCPManager.TcpOutPacketPool;
            TCPClientPool tcpClientPool = Global._TCPManager.tcpClientPool;

            // Nếu thằng này có nhiệm vụ có thể trả thì cứ check nếu có dạng nhiệm vụ dạng nói chuyện
            ProcessTask.Process(Global._TCPManager.MySocketListener, pool, Client, NpcID, NpcID, -1, TaskTypes.Talk);

            bool IsHaveComplteQuest = false;

            foreach (int TaskSelect in tasksList)
            {
                // Xuống đây check lại 1 lượt nữa phòng khi ở trên đã hoàn thành nhiệm vụ
                if (Client.TaskDataList == null)
                {
                    break;
                }
                if (Client.TaskDataList.Count == 0)
                {
                    break;
                }

                var find = Client.TaskDataList.Where(x => x.DoingTaskID == TaskSelect).FirstOrDefault();
                if (find != null)
                {
                    Task _Find = this.GetTaskTemplate(find.DoingTaskID);
                    if (_Find != null)
                    {
                        // Nếu là nghĩa quân thì luôn cho thằng này ở trạng thái có QUEST
                        if (_Find.TaskClass == (int)TaskClasses.NghiaQuan)
                        {
                            IsHaveComplteQuest = true;
                            break;
                        }
                    }

                    if (this.IsQuestComplete(find.DoingTaskID, find.DoingTaskVal1))
                    {
                        IsHaveComplteQuest = true;
                        break;
                    }
                }
            }

            return IsHaveComplteQuest;
        }

        public bool CancelTask(KPlayer client, int dbID, int taskID)
        {
            string cmd2db = StringUtil.substitute("{0}:{1}:{2}", client.RoleID, dbID, taskID);
            string[] dbFields = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_SPR_ABANDONTASK, cmd2db, client.ServerId);
            if (null == dbFields || dbFields.Length < 1 || dbFields[0] == "-1")
                return false;

            //Hủy bỏ nhiệm vụ
            if (null != client.TaskDataList)
            {
                ProcessTask.ClearTaskGoods(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, taskID);

                lock (client.TaskDataList)
                {
                    for (int i = 0; i < client.TaskDataList.Count; i++)
                    {
                        if (client.TaskDataList[i].DoingTaskID == taskID)
                        {
                            client.TaskDataList.RemoveAt(i);
                            break;
                        }
                    }
                }
            }

            Task _TaskFind = this.GetTaskTemplate(taskID);

            if (_TaskFind != null)
            {
                int state = 0;
                int sourceNPC = _TaskFind.SourceNPC;
                if (-1 != sourceNPC)
                {
                    state = TaskManager.getInstance().ComputeNPCTaskState(client, client.TaskDataList, sourceNPC);
                    GameManager.ClientMgr.NotifyUpdateNPCTaskSate(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, (sourceNPC), state);
                }

                int destNPC = _TaskFind.DestNPC;
                if (-1 != destNPC && sourceNPC != destNPC)
                {
                    state = TaskManager.getInstance().ComputeNPCTaskState(client, client.TaskDataList, destNPC);
                    GameManager.ClientMgr.NotifyUpdateNPCTaskSate(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, (destNPC), state);
                }
            }

            return true;
        }

        public int CountAward(int TaskID)
        {
            int Count = 0;

            Task _Task = this.GetTaskTemplate(TaskID);

            if (_Task.Taskaward.Length > 0)
            {
                string[] TotalItem = _Task.Taskaward.Split('#');

                Count = TotalItem.Length;
            }

            return Count;
        }
/// Hoàn thành nhanh nhiệm vụ
        public void FastCompleteTask(GameMap map, NPC npc, KPlayer client, int TaskID)
        {
            TCPOutPacketPool pool = Global._TCPManager.TcpOutPacketPool;
            TCPClientPool tcpClientPool = Global._TCPManager.tcpClientPool;

            Task _TaskFind = this.GetTaskTemplate(TaskID);

            // Nếu nhiệm vụ không tồn tại thì send packet về là toang
            if (_TaskFind == null)
            {
                KT_TCPHandler.CloseDialog(client);

                PlayerManager.ShowNotification(client, "Có lỗi khi nhận nhiệm vụ! ErrorCode : " + TaskID);

                return;
            }

            TaskData taskData = TaskManager.getInstance().GetTaskData(TaskID, client);

            if (null == taskData || taskData.DoingTaskID != TaskID) // NẾu task ko tồn tại trọng người
            {
                KT_TCPHandler.CloseDialog(client);

                PlayerManager.ShowNotification(client, "Nhiệm vụ bạn muốn trả không tồn tại : " + TaskID);
                return;
            }

            /// Nếu nhiệm chưa hoàn thành thì thông báo về là task chauw xong
            /// if (!this.IsQuestComplete(TaskID, taskData.DoingTaskVal1))
            /// {
                /// KT_TCPHandler.CloseDialog(client);

                /// PlayerManager.ShowNotification(client, "Nhiệm vụ của bạn vẫn chưa hoàn thành.Hãy hoàn thành rồi quay lại");

                /// return;
            /// }
                    /// Nếu số đồng không đủ
                    if ( client.Token < 300)
                    {
                        PlayerManager.ShowNotification(client, "Số đồng mang theo không đủ 300 !");
                        return;
                    }

            /// Check phát nữa chống bug vật phẩm
            /// if ((_TaskFind.TargetType == (int)TaskTypes.MonsterSomething || _TaskFind.TargetType == (int)TaskTypes.Collect || _TaskFind.TargetType == (int)TaskTypes.TransferSomething) || (_TaskFind.TaskClass == (int)TaskClasses.NghiaQuan && _TaskFind.TargetType == (int)TaskTypes.Crafting) && "" != _TaskFind.PropsName)
            /// {
                ///Xóa vật phẩm
                /// int NumberRequest = _TaskFind.TargetNum;
                /// int GoodName = Int32.Parse(_TaskFind.PropsName);

                /// int CountItemInBag = ItemManager.GetItemCountInBag(client, GoodName);
                /// if (CountItemInBag < NumberRequest)
                /// {
                    /// PlayerManager.ShowNotification(client, "Vật phẩm trong người không đủ để trả nhiệm vụ");

                    /// return;
                /// }

            /// }

            int CountAward = this.CountAward(TaskID);

            // Kiểm tra số ô đồ trống xem có đủ để add thưởng không
            if (!KTGlobal.IsHaveSpace(CountAward, client))
            {
                KT_TCPHandler.CloseDialog(client);

                PlayerManager.ShowNotification(client, "Túi đồ của bạn không đủ để nhận thưởng");

                return;
            }

            int isMainTask = ((int)TaskClasses.MainTask == _TaskFind.TaskClass ? 1 : 0);

            string SendToDB = string.Format("{0}:{1}:{2}:{3}:{4}:{5}", client.RoleID, npc.ResID, _TaskFind.ID, taskData.DbID, isMainTask, _TaskFind.TaskClass);

           // LogManager.WriteLog(LogTypes.Quest, "SENDDB :" + SendToDB);

            // Đọc cái j đó từ DB ra
            byte[] sendBytesCmd = new UTF8Encoding().GetBytes(SendToDB);
            byte[] bytesData = null;

            if (TCPProcessCmdResults.RESULT_FAILED == Global.ReadDataFromDb((int)TCPGameServerCmds.CMD_SPR_COMPTASK, sendBytesCmd, sendBytesCmd.Length, out bytesData, client.ServerId))
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Không kết nối được với DBServer, CMD={0}", (int)TCPGameServerCmds.CMD_SPR_COMPTASK));

                KT_TCPHandler.CloseDialog(client);

                PlayerManager.ShowNotification(client, "Không kết nối được với DBServer");

                return;
            }

            Int32 length = BitConverter.ToInt32(bytesData, 0);
            string strData = new UTF8Encoding().GetString(bytesData, 6, length - 2);

            //Kết quả từ DB server trả về
            string[] fieldsData = strData.Split(':');

            // Nếu mà kết quả lấy từ DB Về mà toang thì set là toang
            if (fieldsData.Length < 3 || fieldsData[2] == "-1")
            {
                KT_TCPHandler.CloseDialog(client);

                PlayerManager.ShowNotification(client, "Có lỗi xảy ra khi trả nhiệm vụ ERROR CODE :" + fieldsData[2]);

                return;
            }
            else
            {
                //Nếu mà ổn thì ta sẽ
                if (ProcessTask.FastComplete(Global._TCPManager.MySocketListener, pool, client, npc.ResID, npc.ResID, _TaskFind.ID, taskData.DbID, true))
                {
                    // Nếu là main task
                    if (isMainTask > 0 && _TaskFind.ID > client.MainTaskID)
                    {
                        client.MainTaskID = _TaskFind.ID;
                    }

                    KT_TCPHandler.CloseDialog(client);
					/// Trừ đồng
                    KTGlobal.SubMoney(client, 300, Entities.MoneyType.Dong, "SpecialEvent");
                    PlayerManager.ShowNotification(client, "Trả nhiệm vụ :" + _TaskFind.Title + " thành công!");

                    this.GiveTaskArmyDaily(client, true);
                    // UPdate thêm số nhiệm vụ đã làm
                    client.QuestBVDStreakCount = client.QuestBVDStreakCount + 1;
                    client.QuestBVDTodayCount = client.QuestBVDTodayCount + 1;

                    return;
                }
                else
                {
                    KT_TCPHandler.CloseDialog(client);

                    PlayerManager.ShowNotification(client, "Trả nhiệm vụ :" + _TaskFind.Title + " thất bại!");

                    return;
                }
            }
        }
/// Hoàn thành nhanh nhiệm vụ
        public void CompleteTask(GameMap map, NPC npc, KPlayer client, int TaskID)
        {
            TCPOutPacketPool pool = Global._TCPManager.TcpOutPacketPool;
            TCPClientPool tcpClientPool = Global._TCPManager.tcpClientPool;

            Task _TaskFind = this.GetTaskTemplate(TaskID);

            // Nếu nhiệm vụ không tồn tại thì send packet về là toang
            if (_TaskFind == null)
            {
                KT_TCPHandler.CloseDialog(client);

                PlayerManager.ShowNotification(client, "Có lỗi khi nhận nhiệm vụ! ErrorCode : " + TaskID);

                return;
            }

            TaskData taskData = TaskManager.getInstance().GetTaskData(TaskID, client);

            if (null == taskData || taskData.DoingTaskID != TaskID) // NẾu task ko tồn tại trọng người
            {
                KT_TCPHandler.CloseDialog(client);

                PlayerManager.ShowNotification(client, "Nhiệm vụ bạn muốn trả không tồn tại : " + TaskID);
                return;
            }

            /// Nếu nhiệm chưa hoàn thành thì thông báo về là task chauw xong
            if (!this.IsQuestComplete(TaskID, taskData.DoingTaskVal1))
            {
                KT_TCPHandler.CloseDialog(client);

                PlayerManager.ShowNotification(client, "Nhiệm vụ của bạn vẫn chưa hoàn thành.Hãy hoàn thành rồi quay lại");

                return;
            }


            // Check phát nữa chống bug vật phẩm
            if ((_TaskFind.TargetType == (int)TaskTypes.MonsterSomething || _TaskFind.TargetType == (int)TaskTypes.Collect || _TaskFind.TargetType == (int)TaskTypes.TransferSomething) || (_TaskFind.TaskClass == (int)TaskClasses.NghiaQuan && _TaskFind.TargetType == (int)TaskTypes.Crafting) && "" != _TaskFind.PropsName)
            {
                //Xóa vật phẩm
                int NumberRequest = _TaskFind.TargetNum;
                int GoodName = Int32.Parse(_TaskFind.PropsName);

                int CountItemInBag = ItemManager.GetItemCountInBag(client, GoodName);
                if (CountItemInBag < NumberRequest)
                {
                    PlayerManager.ShowNotification(client, "Vật phẩm trong người không đủ để trả nhiệm vụ");

                    return;
                }

            }
            int CountAward = this.CountAward(TaskID);

            // Kiểm tra số ô đồ trống xem có đủ để add thưởng không
            if (!KTGlobal.IsHaveSpace(CountAward, client))
            {
                KT_TCPHandler.CloseDialog(client);

                PlayerManager.ShowNotification(client, "Túi đồ của bạn không đủ để nhận thưởng");

                return;
            }

            int isMainTask = ((int)TaskClasses.MainTask == _TaskFind.TaskClass ? 1 : 0);

            string SendToDB = string.Format("{0}:{1}:{2}:{3}:{4}:{5}", client.RoleID, npc.ResID, _TaskFind.ID, taskData.DbID, isMainTask, _TaskFind.TaskClass);

           // LogManager.WriteLog(LogTypes.Quest, "SENDDB :" + SendToDB);

            // Đọc cái j đó từ DB ra
            byte[] sendBytesCmd = new UTF8Encoding().GetBytes(SendToDB);
            byte[] bytesData = null;

            if (TCPProcessCmdResults.RESULT_FAILED == Global.ReadDataFromDb((int)TCPGameServerCmds.CMD_SPR_COMPTASK, sendBytesCmd, sendBytesCmd.Length, out bytesData, client.ServerId))
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Không kết nối được với DBServer, CMD={0}", (int)TCPGameServerCmds.CMD_SPR_COMPTASK));

                KT_TCPHandler.CloseDialog(client);

                PlayerManager.ShowNotification(client, "Không kết nối được với DBServer");

                return;
            }

            Int32 length = BitConverter.ToInt32(bytesData, 0);
            string strData = new UTF8Encoding().GetString(bytesData, 6, length - 2);

            //Kết quả từ DB server trả về
            string[] fieldsData = strData.Split(':');

            // Nếu mà kết quả lấy từ DB Về mà toang thì set là toang
            if (fieldsData.Length < 3 || fieldsData[2] == "-1")
            {
                KT_TCPHandler.CloseDialog(client);

                PlayerManager.ShowNotification(client, "Có lỗi xảy ra khi trả nhiệm vụ ERROR CODE :" + fieldsData[2]);

                return;
            }
            else
            {
                //Nếu mà ổn thì ta sẽ
                if (ProcessTask.Complete(Global._TCPManager.MySocketListener, pool, client, npc.ResID, npc.ResID, _TaskFind.ID, taskData.DbID, false))
                {
                    // Nếu là main task
                    if (isMainTask > 0 && _TaskFind.ID > client.MainTaskID)
                    {
                        client.MainTaskID = _TaskFind.ID;
                    }

                    KT_TCPHandler.CloseDialog(client);

                    PlayerManager.ShowNotification(client, "Trả nhiệm vụ :" + _TaskFind.Title + " thành công!");

                    this.GiveTaskArmyDaily(client, true);
                    // UPdate thêm số nhiệm vụ đã làm
                    client.QuestBVDStreakCount = client.QuestBVDStreakCount + 1;
                    client.QuestBVDTodayCount = client.QuestBVDTodayCount + 1;

                    return;
                }
                else
                {
                    KT_TCPHandler.CloseDialog(client);

                    PlayerManager.ShowNotification(client, "Trả nhiệm vụ :" + _TaskFind.Title + " thất bại!");

                    return;
                }
            }
        }

        public void QuestArmyProsecc(GameMap map, NPC npc, KPlayer client, TaskCallBack TaskData, int TaskID)
        {
            TCPOutPacketPool pool = Global._TCPManager.TcpOutPacketPool;

            if (TaskData != null)
            {
                int SelectID = TaskData.SelectID;

                var findTask = client.TaskDataList.Where(x => x.DoingTaskID == TaskID).FirstOrDefault();
                if (findTask != null)
                {
                    // Nếu là hủy nhiệm vụ
                    if (SelectID == 3)
                    {
                        if (this.CancelTask(client, findTask.DbID, TaskID))
                        {
                            KTLuaLib_Player.SendMSG("Hủy nhiệm vụ thành công", client, npc);

                            GameManager.ClientMgr.NotifyUpdateTask(Global._TCPManager.MySocketListener, pool, client, findTask.DbID, TaskID, -1, 0, 0);

                            if (client.CanncelQuestBVD < 5)
                            {
                                client.CanncelQuestBVD = client.CanncelQuestBVD + 1;
                            }
                            else
                            {
                                client.CanncelQuestBVD = 5;
                                client.QuestBVDStreakCount = 0;
                            }
                            //Trao 1 nhiệm vụ mới
                            this.GiveTaskArmyDaily(client, true);
                        }
                    }
                }

                if (SelectID == 2) // Nếu là hoàn thành nhiệm vụ
                {
                    this.CompleteTask(map, npc, client, TaskID);
                }
                if (SelectID == 31) // Nếu là hoàn thành nhanh nhiệm vụ
                {
                    this.FastCompleteTask(map, npc, client, TaskID);
                }
                else if (SelectID == 1) // Nếu là nhận nhiệm vụ
                {
                    this.AppcepTask(npc, client, TaskID);
                }
                else if (SelectID == 4) // Nếu là nhận nhiệm vụ
                {
                    KTLuaLib_Player.SendMSG("Mỗi ngày người chơi có thể làm " + KTGlobal.CreateStringByColor("50", ColorType.Done) + " nhiệm vụ Nghĩa Quân để nhận được điểm danh vọng.Điểm danh vọng có thể sử dụng để mua vật phẩm tại NPC " + KTGlobal.CreateStringByColor("Quan Quân Nhu", ColorType.Done) + ".Khi hoàn thành liên tiếp 10-20-30-40-50 nhiệm vụ người chơi sẽ được nhận thêm phần thưởng.", client, npc);
                }
                else if (SelectID == 5) // Nếu là nhận nhiệm vụ
                {
                    KT_TCPHandler.CloseDialog(client);
                }
            }
        }

        public void AppcepTask(NPC npc, KPlayer client, int TaskID, bool IsCloseATEnd = true)
        {
            Task _FindTask = this.GetTaskTemplate(TaskID);

            TCPOutPacketPool pool = Global._TCPManager.TcpOutPacketPool;
            TCPClientPool tcpClientPool = Global._TCPManager.tcpClientPool;

            if (_FindTask == null)
            {
                // Thực hiện đóng cửa sổ nhiệm vụ và thông báo tới người chơi
                KT_TCPHandler.CloseDialog(client);

                PlayerManager.ShowNotification(client, "Nhiệm vụ bạn vừa chọn không tồn tại!");
                return;
            }

            int taskClass = _FindTask.TaskClass;

            if (client.m_Level < 20)
            {
                PlayerManager.ShowNotification(client, "Cấp độ không đủ để nhận!");
                return;
            }
            if (client.m_cPlayerFaction.GetFactionId() == 0)
            {
                PlayerManager.ShowNotification(client, "Vào phái mới có thể nhận nhiệm vụ này");
                return;
            }
            if (client.QuestBVDTodayCount > 50)
            {
                PlayerManager.ShowNotification(client, "Nay người đã làm hết số lần rồi!");
                return;
            }

            if (_FindTask.TargetType == (int)TaskTypes.Crafting)
            {
                ProcessTask.Process(Global._TCPManager.MySocketListener, pool, client, -1, -1, Int32.Parse(_FindTask.PropsName), TaskTypes.Crafting);
            }

            //Kiểm tra xem có đủ điều kiện nhân nhiệm vụ mới không
            if (!this.CanTakeNewTask(client, TaskID))
            {
                KT_TCPHandler.CloseDialog(client);

                PlayerManager.ShowNotification(client, "Bạn không thể nhận nhiệm vụ này");

                return;
            }

            string strcmd = "";

            // Add Data
            TaskData taskData = new TaskData()
            {
                DbID = -1,
                DoingTaskID = TaskID,
            };

            if (_FindTask.PropsName != "")
            {
                bool IsHaveSpaceGetQuest = KTGlobal.IsHaveSpace(1, client);

                if (!IsHaveSpaceGetQuest)
                {
                    KT_TCPHandler.CloseDialog(client);

                    PlayerManager.ShowNotification(client, "Hành trang không đủ chỗ trống để nhận nhiệm vụ");

                    return;
                }
            }

            int focus = 1;

            if (Global.GetFocusTaskCount(client) >= KTGlobal.TaskMaxFocusCount)
            {
                focus = 0;
            }

            // Nếu là nhiệm vụ tuần hoàn
            int nStarLevel = 1;

            if (npc == null)
            {
                strcmd = string.Format("{0}:{1}:{2}:{3}:{4}", client.RoleID, -1, TaskID, focus, nStarLevel);
            }
            else
            {
                strcmd = string.Format("{0}:{1}:{2}:{3}:{4}", client.RoleID, npc.ResID, TaskID, focus, nStarLevel);
            }

            string[] fieldsData = null;

            if (TCPProcessCmdResults.RESULT_FAILED == Global.RequestToDBServer(tcpClientPool, pool, (int)TCPGameServerCmds.CMD_SPR_NEWTASK, strcmd, out fieldsData, client.ServerId))
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("Toang khi kết nối với CSDL, CMD={0}", (int)TCPGameServerCmds.CMD_SPR_NEWTASK));

                KT_TCPHandler.CloseDialog(client);

                PlayerManager.ShowNotification(client, "Có lỗi khi nhận nhiệm vụ! ErrorCode : 1");

                return;
            }

            strcmd = "";
            if (Convert.ToInt32(fieldsData[3]) < 0) //Nếu Pram gửi về mà lỗi thì toang
            {
                taskData.DbID = Convert.ToInt32(fieldsData[3]);

                KT_TCPHandler.CloseDialog(client);

                PlayerManager.ShowNotification(client, "Có lỗi khi nhận nhiệm vụ! ErrorCode : " + taskData.DbID);

                return;
            }

            //Nếu mà mọi thứ đều ổn
            // Check xem list task client có null không nếu null thì tạo mới
            if (null == client.TaskDataList)
            {
                client.TaskDataList = new List<TaskData>();
            }

            taskData.DbID = Convert.ToInt32(fieldsData[3]);
            taskData.DoingTaskVal1 = 0;
            taskData.DoingTaskVal2 = 0;
            taskData.DoingTaskFocus = focus;
            taskData.AddDateTime = Convert.ToInt64(fieldsData[2]);
            taskData.DoneCount = 0;
            taskData.StarLevel = nStarLevel;

            OldTaskData oldTaskData = Global.FindOldTaskByTaskID(client, TaskID);
            if (null != oldTaskData)
            {
                taskData.DoneCount = oldTaskData.DoCount;
            }

            //Trả về dữ liệu phần thưởng cho nhiệm vụ tuần hoàn
            Global.ProcessTaskData(client, taskData);

            lock (client.TaskDataList)
            {
                client.TaskDataList.Add(taskData);
            }

            // Thực hiện send DATA TASK VỀ CLLIENT
            byte[] DataSend = DataHelper.ObjectToBytes<TaskData>(taskData);
            GameManager.ClientMgr.SendToClient(client, DataSend, (int)TCPGameServerCmds.CMD_SPR_NEWTASK);

            int state = 0;
            int sourceNPC = _FindTask.SourceNPC;
            if (sourceNPC >= 0)
            {
                // Update STATE CỦA CLIENT
                state = TaskManager.getInstance().ComputeNPCTaskState(client, client.TaskDataList, sourceNPC);

                GameManager.ClientMgr.NotifyUpdateNPCTaskSate(Global._TCPManager.MySocketListener, pool, client, sourceNPC, state);
            }


            // NPC TRẢ NHIỆM VỤ
            int destNPC = _FindTask.DestNPC;

            // NẾU NHIỆM VỤ CÓ NPC TRẢ THÌ UPDATE TRẠNG THÁI CHO THẰNG NPC TRẢ NHIỆM VỤ
            if (-1 != destNPC)
            {
                state = TaskManager.getInstance().ComputeNPCTaskState(client, client.TaskDataList, destNPC);
                GameManager.ClientMgr.NotifyUpdateNPCTaskSate(Global._TCPManager.MySocketListener, pool, client, destNPC, state);
            }
            if (IsCloseATEnd)
            {
                PlayerManager.ShowNotification(client, "Nhận thành công nhiệm vụ [" + _FindTask.Title + "]");
                KT_TCPHandler.CloseDialog(client);
            }

            //Gọi hàm cập nhật tiến độ trước khi bắt đầu
            ProcessTask.ProseccTaskBeforeDoTask(Global._TCPManager.MySocketListener, pool, client);
        }

        /// <summary>
        /// Set nhiệm vụ bạo văn đồng cho người chơi
        /// </summary>
        /// <param name="client"></param>
        /// <param name="ForceChange"></param>
        /// <returns></returns>
        public bool GiveTaskArmyDaily(KPlayer client, bool ForceChange = false)
        {
            int TaskID = client.CurenQuestIDBVD;

            if (ForceChange)
            {
                int LevelHientai = client.m_Level;

                List<Task> _TotalTask = _TotalTaskData.Values.Where(x => x.MinLevel <= LevelHientai && x.MaxLevel >= LevelHientai).ToList();

                if (_TotalTask.Count == 0)
                {
                    return false;
                }

                int Random = new Random().Next(_TotalTask.Count);

                Task SelectTask = _TotalTask[Random];

                client.CurenQuestIDBVD = SelectTask.ID;

                return true;
            }
            else
            {
                // lấy ra danh sách nhiệm vụ trên người
                if (client.TaskDataList != null)
                {
                    lock (client.TaskDataList)
                    {
                        if (client.TaskDataList.Count > 0)
                        {
                            foreach (TaskData task in client.TaskDataList)
                            {
                                Task _Task = this.GetTaskTemplate(task.DoingTaskID);
                                {
                                    return false;
                                }
                            }
                        }
                    }

                    // nếu ko làm được thực hiện tìm 1 nhiệm vụ khác phù hợp
                    {
                        int LevelHientai = client.m_Level;

                        List<Task> _TotalTask = _TotalTaskData.Values.Where(x => x.MinLevel <= LevelHientai && x.MaxLevel >= LevelHientai).ToList();

                        if (_TotalTask.Count == 0)
                        {
                            return false;
                        }

                        int Random = new Random().Next(_TotalTask.Count);

                        Task SelectTask = _TotalTask[Random];

                        client.CurenQuestIDBVD = SelectTask.ID;

                        return true;
                    }
                }
                return false;
            }
        }
    }
}