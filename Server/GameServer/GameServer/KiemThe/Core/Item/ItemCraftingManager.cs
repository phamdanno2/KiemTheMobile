using GameServer.Core.Executor;
using GameServer.KiemThe.Logic;
using GameServer.Logic;
using Server.Data;
using Server.Protocol;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace GameServer.KiemThe.Core.Item
{
    public class ItemCraftingManager
    {
        public static string LifeSkill_XML = "Config/KT_Item/LifeSkill.xml";

        public static LifeSkill _LifeSkill = new LifeSkill();

        /// <summary>
        /// Loading all Drop
        /// </summary>
        public static void Setup()
        {
            string Files = Global.GameResPath(LifeSkill_XML);
            using (var stream = System.IO.File.OpenRead(Files))
            {
                var serializer = new XmlSerializer(typeof(LifeSkill));
                _LifeSkill = serializer.Deserialize(stream) as LifeSkill;
            }
        }

        /// <summary>
        /// Truy vấn thông tin cấp độ và kinh nghiệm kỹ năng sống của người chơi tương ứng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="player"></param>
        /// <param name="level"></param>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static bool QueryLifeSkillLevelAndExp(int id, KPlayer player, out int level, out int exp)
        {
            level = 0;
            exp = 0;
            LifeSkillPram lifeSkillParam = player.GetLifeSkill(id);
            if (lifeSkillParam != null)
            {
                level = lifeSkillParam.LifeSkillLevel;
                exp = lifeSkillParam.LifeSkillExp;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Trả về cấp độ kỹ năng sống tương ứng
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        private static int GetLifeSkilLLevel(int id, KPlayer player)
        {
            LifeSkillPram lifeSkillParam = player.GetLifeSkill(id);
            if (lifeSkillParam != null)
            {
                return lifeSkillParam.LifeSkillLevel;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Thêm kinh nghiệm cho kỹ năng sống
        /// </summary>
        /// <param name="lifeSkillID"></param>
        /// <param name="player"></param>
        /// <param name="nExp"></param>
        public static void AddExp(int lifeSkillID, KPlayer player, int nExp)
        {
            LifeSkillPram lifeSkillParam = player.GetLifeSkill(lifeSkillID);
            if (lifeSkillParam != null)
            {
                /// Kinh nghiệm sau khi thêm vào
                int expAdd = nExp + lifeSkillParam.LifeSkillExp;
                do
                {
                    /// Cấp tiếp theo
                    LifeSkillExp nextLevelExp = ItemCraftingManager._LifeSkill.TotalExp.Where(x => x.Level == lifeSkillParam.LifeSkillLevel + 1).FirstOrDefault();

                    /// Nếu không tồn tại kinh nghiệm cấp hiện tại hoặc tiếp theo
                    if (nextLevelExp == null)
                    {
                        /// Thoát lặp
                        break;
                    }

                    /// Kinh nghiệm tối đa ở cấp hiện tại
                    int nMaxExp = nextLevelExp.Exp;

                    /// Nếu đủ để lên cấp
                    if (expAdd >= nMaxExp)
                    {
                        /// Trừ điểm kinh nghiệm còn lại
                        expAdd -= nMaxExp;
                        /// Tăng cấp
                        lifeSkillParam.LifeSkillLevel++;
                    }
                    else
                    {
                        /// Cập nhật kinh nghiệm ở cấp hiện tại
                        lifeSkillParam.LifeSkillExp = expAdd;
                        /// Thoát lặp
                        break;
                    }
                }
                while (true);
                /// Cập nhật kinh nghiệm ở cấp hiện tại
                lifeSkillParam.LifeSkillExp = expAdd;
            }

            LogManager.WriteLog(LogTypes.LifeSkill, "[" + player.RoleID + "][" + player.RoleName + "] Kỹ năng sống ID :" + lifeSkillID + " | LEVEL :" + lifeSkillParam.LifeSkillLevel + " | EXP :" + lifeSkillParam.LifeSkillExp);
            /// Gửi thông báo về Client
            KT_TCPHandler.NotifySelfLifeSkillLevelAndExpChanged(player, lifeSkillID, lifeSkillParam.LifeSkillLevel, lifeSkillParam.LifeSkillExp);
        }

        /// <summary>
        /// Thực hiện chế đồ
        /// </summary>
        /// <param name="recipeID"></param>
        /// <param name="player"></param>
        /// <param name="done"></param>
        /// <returns></returns>
        public static bool DoCrafting(int recipeID, KPlayer player, Action done)
        {
            /// Nếu đang chế đồ
            if (player.IsCrafting)
            {
                PlayerManager.ShowNotification(player, "Thao tác quá nhanh, xin hãy đợi giây lát!");
                return false;
            }

            bool IsCheThanhCong = false;

            var Find = _LifeSkill.TotalRecipe.Where(x => x.ID == recipeID).FirstOrDefault();
            if (Find != null)
            {
                int Belong = Find.Belong;

                int LevelSkill = GetLifeSkilLLevel(Belong, player);

                if (LevelSkill == 0)
                {
                    PlayerManager.ShowNotification(player, "Kỹ năng chưa được học!");
                    return false;
                }

                if (LevelSkill < Find.SkillLevel)
                {
                    PlayerManager.ShowNotification(player, "Kỹ năng chế tạo không đủ cấp độ để chế vật phẩm này!");
                    return false;
                }

                //if (player.m_eDoing != Entities.KE_NPC_DOING.do_stand)
                //{
                //    PlayerManager.ShowNotification(player, "Chỉ trạng thái đứng im mới được dùng kỹ năng sống");
                //    return false;
                //}

                int Cost = Find.Cost;

                var FindLIfeSkillData = _LifeSkill.TotalSkill.Where(x => x.Belong == Belong).FirstOrDefault();

                if (FindLIfeSkillData.Gene == 1)
                {
                    if (player.GetGatherPoint() < Cost)
                    {
                        PlayerManager.ShowNotification(player, "Hoạt lực không đủ, không thể hợp thành vật phẩm");
                        return false;
                    }
                }
                if (FindLIfeSkillData.Gene == 0)
                {
                    if (player.GetMakePoint() < Cost)
                    {
                        PlayerManager.ShowNotification(player, "Tinh lực không đủ, không thể hợp thành vật phẩm");
                        return false;
                    }
                }

                List<ItemStuff> ListStuffRequest = Find.ListStuffRequest;

                bool IsHaveStuff = true;

                /// Nếu là các công thức chế sò thì yêu cầu nguyên liệu không khóa
                bool isSeashellRecipe = false;
                if (recipeID >= 1557 && recipeID <= 1586)
                {
                    isSeashellRecipe = true;
                }

                foreach (ItemStuff _ItemRequest in ListStuffRequest)
                {
                    int NumberReqest = _ItemRequest.Number;

                    /// Tổng số vật phẩm có
                    int CountInBag = ItemManager.GetItemCountInBag(player, _ItemRequest.ItemTemplateID, isSeashellRecipe ? 0 : -1);
                    if (CountInBag < NumberReqest)
                    {
                        IsHaveStuff = false;
                        break;
                    }
                }

                if (!IsHaveStuff)
                {
                    PlayerManager.ShowNotification(player, "Nguyên liệu yêu cầu không đủ không thể hợp thành");
                    return false;
                }

                if (!KTGlobal.IsHaveSpace(1, player))
                {
                    PlayerManager.ShowNotification(player, "Túi trên người không đủ 1 ô trống không thể tiến hành chế tạo");
                    return false;
                }

                // Trừ hoạt lực
                if (FindLIfeSkillData.Gene == 1)
                {
                    player.ChangeCurGatherPoint(-Cost);
                }
                // Trừ tinh lực
                if (FindLIfeSkillData.Gene == 0)
                {
                    player.ChangeCurMakePoint(-Cost);
                }

                // Xóa nguyên liệu
                foreach (ItemStuff _ItemRequest in ListStuffRequest)
                {
                    int NumberReqest = _ItemRequest.Number;

                    if (!ItemManager.RemoveItemFromBag(player, _ItemRequest.ItemTemplateID, NumberReqest, isSeashellRecipe ? 0 : -1, "Chế đồ"))
                    {
                        PlayerManager.ShowNotification(player, "Có lỗi khi xóa vật phẩm trên người vui lòng liên hệ ADM để được giúp đỡ");
                        return false;
                    }
                }

                List<ItemCraf> TotalOutPut = Find.ListProduceOut;

                int Random = KTGlobal.GetRandomNumber(0, 100);

                ItemCraf _SelectItem = null;

                foreach (ItemCraf _Item in TotalOutPut)
                {
                    Random = Random - _Item.Rate;
                    if (Random <= 0)
                    {
                        _SelectItem = _Item;

                        break;
                    }
                }

                LifeSkillPram lifeSkillParam = player.GetLifeSkill(Belong);

                /// Đánh dấu đang chế đồ
                player.IsCrafting = true;

                /// Tạo DelayTask
                DelayAsyncTask asyncTask = new DelayAsyncTask()
                {
                    Player = player,
                    Name = "CraftingItem",
                    Tag = new Tuple<ItemCraf, Action>(_SelectItem, () => {
                        /// Thực hiện add EXP vào kỹ năng sống
                        AddExp(Belong, player, Find.ExpGain);
                        /// Thực hiện hàm Callback
                        done?.Invoke();
                    }),
                    Callback = ItemCraftingManager.TimerProc,
                };
                /// Thực thi Task tương ứng
                System.Threading.Tasks.Task executeTask = KTKTAsyncTask.Instance.ScheduleExecuteAsync(asyncTask, Find.MakeTime * 1000 / 18);

                return true;
            }
            else
            {
                PlayerManager.ShowNotification(player, "Vật phẩm muốn chế tạo không tồn tại!");
                return false;
            }
        }

        /// <summary>
        ///  Thực hiện tạo đồ sau khi chạy xong hàm DELAY
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void TimerProc(object sender, EventArgs e)
        {
            DelayAsyncTask task = (DelayAsyncTask) sender;

            /// Nếu đối tượng không hợp lệ
            if (!(task.Tag is Tuple<ItemCraf, Action> pair))
            {
                return;
            }

            ItemCraf _SelectItem = pair.Item1;
            Action done = pair.Item2;

            KPlayer player = task.Player;
            if (_SelectItem != null)
            {
                ItemData _FindOutPut = ItemManager.GetItemTemplate(_SelectItem.ItemTemplateID);

                int Series = _SelectItem.Series;

                if (_FindOutPut!=null)
                {
                   if(_FindOutPut.Series!=-1)
                    {
                        Series = _FindOutPut.Series;
                    }    
                }    
                // Thưc hiện add vật phẩm vào kỹ năng sôngs
                if (!ItemManager.CreateItem(Global._TCPManager.TcpOutPacketPool, task.Player, _SelectItem.ItemTemplateID, 1, 0, "ITEMCRAFTING", true, _SelectItem.Bind, false, Global.ConstGoodsEndTime, "", Series, player.RoleName))
                {
                    PlayerManager.ShowNotification(player, "Có lỗi khi nhận vật phẩm chế tạo");
                }

                TCPOutPacketPool pool = Global._TCPManager.TcpOutPacketPool;

              //  ProcessTask.Process(Global._TCPManager.MySocketListener, pool, player, -1, -1, _SelectItem.ItemTemplateID, TaskTypes.Crafting);

            }

            /// Thực hiện hàm Callback
            done?.Invoke();

            /// Hủy đánh dấu đang chế đồ
            player.IsCrafting = false;
        }
    }
}