using GameServer.KiemThe.Core.Item;
using GameServer.KiemThe.Logic;
using GameServer.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameServer.KiemThe.CopySceneEvents.MilitaryCampFuBen
{
    /// <summary>
    /// Nhiệm vụ giao vật phẩm cho NPC
    /// </summary>
    public partial class MilitaryCamp_Script_Main
    {
        /// <summary>
        /// Đánh dấu NPC đã nhận đủ vật phẩm yêu cầu chưa
        /// </summary>
        private bool isNPCTookEnoughGoods;

        /// <summary>
        /// Bắt đầu nhiệm vụ giao vật phẩm cho NPC
        /// </summary>
        /// <param name="task"></param>
        private void Begin_GiveGoodsToNPC(MilitaryCamp.EventInfo.StageInfo.TaskInfo task)
        {
            /// Tạo NPC
            this.CreateDynamicNPC(task.NPC, (npc) => {
                /// Sự kiện Click
                npc.Click = (player) => {
                    /// Hội thoại NPC
                    StringBuilder dialogMsgBuilder = new StringBuilder();
                    /// Tên nhiệm vụ
                    dialogMsgBuilder.AppendLine(string.Format("<color=yellow>{0}:</color>", task.Name));
                    /// Danh sách vật phẩm yêu cầu
                    List<string> requireItemsStrings = new List<string>();
                    /// Duyệt danh sách vật phẩm yêu cầu
                    foreach (KeyValuePair<int, int> pair in task.Target.Items)
                    {
                        requireItemsStrings.Add(string.Format("<color=yellow>[{0}]</color> - SL: <color=green>{1}</color>", KTGlobal.GetItemName(pair.Key), pair.Value));
                    }
                    /// Mục tiêu nhiệm vụ
                    dialogMsgBuilder.AppendLine(string.Format("<color=orange>{0}</color> thu thập đủ {1}", task.Target.RequireAllMembers ? "Mỗi thành viên" : "Toàn đội", string.Join(", ", requireItemsStrings)));


                    KNPCDialog dialog = new KNPCDialog();
                    dialog.Owner = player;
                    dialog.Text = dialogMsgBuilder.ToString();
                    dialog.Selections = new Dictionary<int, string>()
                    {
                        { -1, "Giao vật phẩm" },
                        { -1000, "Ta vẫn chưa thu thập đủ" },
                    };
                    dialog.OnSelect = (x) => {
                        /// Giao vật phẩm
                        if (x.SelectID == -1)
                        {
                            /// Đóng NPCDialog
							KT_TCPHandler.CloseDialog(player);

                            /// Nếu nhiệm vụ này yêu cầu toàn đội phải có đủ số lượng tương ứng
                            if (task.Target.RequireAllMembers)
                            {
                                /// Duyệt danh sách thành viên nhóm
                                foreach (KPlayer teammate in this.teamPlayers)
                                {
                                    /// Duyệt danh sách vật phẩm yêu cầu
                                    foreach (KeyValuePair<int, int> pair in task.Target.Items)
                                    {
                                        /// Nếu không đủ số lượng yêu cầu
                                        if (ItemManager.GetItemCountInBag(teammate, pair.Key) < pair.Value)
                                        {
                                            /// Thông báo thành viên không đủ số lượng yêu cầu
                                            PlayerManager.ShowNotification(player, string.Format("[{0}] số lượng vật phẩm yêu cầu chưa đủ.", teammate.RoleName));
                                            /// Thoát
                                            return;
                                        }
                                    }
                                }

                                /// Thông báo giao vật phẩm thành công
                                PlayerManager.ShowNotificationToAllTeammates(player, "Giao vật phẩm thành công!");
                                /// Đánh dấu NPC đã nhận đủ số lượng vật phẩm yêu cầu
                                this.isNPCTookEnoughGoods = true;
                                /// Thoát
                                return;
                            }
                            /// Nếu nhiệm vụ này chỉ yêu cầu 1 thành viên bất kỳ trong nhóm có đủ số lượng tương ứng
                            else
                            {
                                /// Duyệt danh sách thành viên nhóm
                                foreach (KPlayer teammate in this.teamPlayers)
                                {
                                    /// Đánh dấu người chơi có đủ số lượng tất cả không
                                    bool enoughMaterials = true;
                                    /// Duyệt danh sách vật phẩm yêu cầu
                                    foreach (KeyValuePair<int, int> pair in task.Target.Items)
                                    {
                                        /// Nếu không đủ số lượng yêu cầu
                                        if (ItemManager.GetItemCountInBag(teammate, pair.Key) < pair.Value)
                                        {
                                            /// Đánh dấu không đủ số lượng
                                            enoughMaterials = false;
                                            /// Thoát
                                            break;
                                        }
                                    }

                                    /// Nếu người chơi này đủ số lượng yêu cầu
                                    if (enoughMaterials)
                                    {
                                        /// Thông báo giao vật phẩm thành công
                                        PlayerManager.ShowNotificationToAllTeammates(player, "Giao vật phẩm thành công!");
                                        /// Đánh dấu NPC đã nhận đủ số lượng vật phẩm yêu cầu
                                        this.isNPCTookEnoughGoods = true;
                                        /// Thoát
                                        return;
                                    }
                                }

                                /// Thông báo không đủ số lượng yêu cầu
                                PlayerManager.ShowNotification(player, "Số lượng vật phẩm yêu cầu chưa đủ.");
                                /// Thoát
                                return;
                            }
                        }
                        /// Toác
                        else if (x.SelectID == -1000)
                        {
                            /// Đóng NPCDialog
							KT_TCPHandler.CloseDialog(player);
                        }
                    };
                    KTNPCDialogManager.AddNPCDialog(dialog);
                    dialog.Show(npc, player);
                };
			});
            /// Đánh dấu NPC chưa nhận đủ số lượng vật phẩm yêu cầu
            this.isNPCTookEnoughGoods = false;
        }

        /// <summary>
        /// Theo dõi nhiệm vụ giao vật phẩm cho NPC
        /// </summary>
        /// <param name="task"></param>
        private bool Track_GiveGoodsToNPC(MilitaryCamp.EventInfo.StageInfo.TaskInfo task)
        {
            /// Chỉ hoàn thành nhiệm vụ khi đã tiêu diệt Boss
            return this.isNPCTookEnoughGoods;
        }

        /// <summary>
        /// Thiết lập lại dữ liệu nhiệm vụ giao vật phẩm cho NPC
        /// </summary>
        private void Reset_GiveGoodsToNPC()
        {
            this.isNPCTookEnoughGoods = false;
        }
    }
}
