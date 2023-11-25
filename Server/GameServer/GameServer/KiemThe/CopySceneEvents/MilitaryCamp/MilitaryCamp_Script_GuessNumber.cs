using GameServer.KiemThe.Logic;
using GameServer.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameServer.KiemThe.CopySceneEvents.MilitaryCampFuBen
{
    /// <summary>
    /// Nhiệm vụ đoán số
    /// </summary>
    public partial class MilitaryCamp_Script_Main
    {
        /// <summary>
        /// Giá trị nhỏ nhất của số trong khoảng đã đoán
        /// </summary>
        private int guessNumberCurrentMinValue;

        /// <summary>
        /// Giá trị lớn nhất của số trong khoảng đã đoán
        /// </summary>
        private int guessNumberCurrentMaxValue;

        /// <summary>
        /// Số may mắn trong vòng đoán số hiện tại
        /// </summary>
        private int guessNumberLuckyNumber;

        /// <summary>
        /// Số lượt đã đoán số hiện tại
        /// </summary>
        private int guessNumberTotalTurns;

        /// <summary>
        /// Thành viên đoán số hiện tại
        /// </summary>
        private KPlayer guessNumberMember;

        /// <summary>
        /// Đánh dấu đã hoàn thành đoán số chưa
        /// </summary>
        private bool guessNumberCompleted;

        /// <summary>
        /// Bắt đầu nhiệm vụ đoán số
        /// </summary>
        /// <param name="task"></param>
        private void Begin_GuessNumber(MilitaryCamp.EventInfo.StageInfo.TaskInfo task)
        {
            /// <summary>
            /// Bắt đầu trò chơi
            /// </summary>
            void BeginGame()
            {
                /// Thiết lập lại khoảng Min-Max
                this.guessNumberCurrentMinValue = task.GuessNumberConfig.MinNumber;
                this.guessNumberCurrentMaxValue = task.GuessNumberConfig.MaxNumber;
                /// Chọn ngẫu nhiên số may mắn
                this.guessNumberLuckyNumber = KTGlobal.GetRandomNumber(task.GuessNumberConfig.MinNumber, task.GuessNumberConfig.MaxNumber);
                /// Thiết lập lại số lượt đã đoán
                this.guessNumberTotalTurns = 0;
                /// Chuyển lượt qua cho thằng khác
                this.guessNumberMember = this.teamPlayers.RandomRange(1).FirstOrDefault();
                /// Thông báo chuyển lượt đoán số
                this.NotifyAllPlayers(string.Format("Lượt đoán đầu tiên thuộc về [{0}].", this.guessNumberMember.RoleName));
            }

            /// Tạo NPC
            this.CreateDynamicNPC(task.NPC, (npc) => {
                /// Sự kiện Click
                npc.Click = (player) => {
                    /// Hội thoại NPC
                    StringBuilder dialogMsgBuilder = new StringBuilder();
                    /// Tên nhiệm vụ
                    dialogMsgBuilder.AppendLine(string.Format("<color=yellow>{0}:</color>", task.Name));
                    /// Mục tiêu nhiệm vụ
                    dialogMsgBuilder.AppendLine(string.Format("<color=yellow>Đoán chính xác</color> số hệ thống đưa ra trong khoảng <color=green>{0} - {1}</color>, hệ thống ngẫu nhiên hệ thống chọn người chơi bất kỳ trong đội mỗi lượt đoán. Nếu sau <color=yellow>{2} lượt</color> không ai <color=orange>đoán chính xác</color> thì trò chơi <color=yellow>bắt đầu lại</color>.", task.GuessNumberConfig.MinNumber, task.GuessNumberConfig.MaxNumber, task.GuessNumberConfig.MaxTurns));

                    /// Tạo NPC Dialog
                    KNPCDialog dialog = new KNPCDialog();
                    dialog.Owner = player;
                    dialog.Text = dialogMsgBuilder.ToString();
                    dialog.Selections = new Dictionary<int, string>()
                    {
                        { -1, "Ta muốn đoán số" },
                        { -1000, "Kết thúc đối thoại" },
                    };
                    dialog.OnSelect = (x) => {
                        /// Giao vật phẩm
                        if (x.SelectID == -1)
                        {
                            /// Đóng NPCDialog
							KT_TCPHandler.CloseDialog(player);

                            /// Nếu không phải lượt bản thân
                            if (this.guessNumberMember != player)
                            {
                                /// Thông báo không phải lượt đoán
                                PlayerManager.ShowNotification(player, string.Format("Hiện đang là lượt đoán số của [{0}].", this.guessNumberMember.RoleName));
                                /// Nếu thằng kia không còn trong nhóm
                                if (!this.teamPlayers.Contains(this.guessNumberMember))
                                {
                                    /// Tăng số lượt đã đoán lên
                                    this.guessNumberTotalTurns++;
                                    /// Chuyển lượt qua cho thằng khác
                                    this.guessNumberMember = this.teamPlayers.RandomRange(1).FirstOrDefault();
                                    /// Thông báo chuyển lượt đoán số
                                    this.NotifyAllPlayers(string.Format("Lượt đoán tiếp theo của [{0}].", this.guessNumberMember.RoleName));
                                }
                                /// Bỏ qua
                                return;
                            }
                            /// Nếu là lượt của bản thân
                            else
                            {
                                /// Hiện bảng nhập số
                                PlayerManager.ShowInputNumberBox(player, string.Format("Nhập số cần đoán trong khoảng <color=green>{0} - {1}</color>", this.guessNumberCurrentMinValue, this.guessNumberCurrentMaxValue), (number) => {
                                    /// Nếu không phải lượt bản thân
                                    if (this.guessNumberMember != player)
                                    {
                                        /// Thông báo không phải lượt đoán
                                        PlayerManager.ShowNotification(player, string.Format("Hiện đang là lượt đoán số của [{0}].", this.guessNumberMember.RoleName));
                                        /// Bỏ qua
                                        return;
                                    }
                                    
                                    /// Tăng số lượt đã đoán
                                    this.guessNumberTotalTurns++;
                                    
                                    /// Nếu đây là số cần đoán
                                    if (number == this.guessNumberLuckyNumber)
                                    {
                                        /// Thông báo chuyển lượt đoán số
                                        this.NotifyAllPlayers(string.Format("Chúc mừng [{0}] đã đoán chính xác số cần tìm là {1}.", this.guessNumberMember.RoleName, this.guessNumberLuckyNumber));
                                        /// Đã hoàn thành đoán số
                                        this.guessNumberCompleted = true;
                                        /// Bỏ qua
                                        return;
                                    }
                                    /// Nếu số cần tìm lớn hơn số đã đoán
                                    else if (this.guessNumberTotalTurns > number)
                                    {
                                        /// Đánh dấu lại khoảng Min
                                        this.guessNumberCurrentMinValue = number;
                                    }
                                    /// Nếu số cần tìm nhỏ hơn số đã đoán
                                    else
                                    {
                                        /// Đánh dấu lại khoảng Max
                                        this.guessNumberCurrentMaxValue = number;
                                    }

                                    /// Thông báo khoảng tương ứng
                                    this.NotifyAllPlayers(string.Format("[{0}] đoán không chính xác. Số cần đoán nằm trong khoảng {1} - {2}.", this.guessNumberMember.RoleName, this.guessNumberCurrentMinValue, this.guessNumberCurrentMaxValue));

                                    /// Nếu đã quá số lượt đoán
                                    if (this.guessNumberTotalTurns >= task.GuessNumberConfig.MaxTurns)
                                    {
                                        /// Thông báo quá số lượt đoán
                                        this.NotifyAllPlayers("Đã quá số lượt đoán vẫn chưa tìm ra số cần tìm, trò chơi bắt đầu lại.");
                                        /// Bắt đầu trò chơi
                                        BeginGame();
                                    }
                                    /// Nếu chưa quá số lượt đoán
                                    else
                                    {
                                        /// Chuyển lượt qua cho thằng khác
                                        this.guessNumberMember = this.teamPlayers.RandomRange(1).FirstOrDefault();
                                        /// Thông báo chuyển lượt đoán số
                                        this.NotifyAllPlayers(string.Format("Lượt đoán tiếp theo thuộc về [{0}].", this.guessNumberMember.RoleName));
                                    }
                                });
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

            /// Thông báo quá số lượt đoán
            this.NotifyAllPlayers(string.Format("Trò chơi đoán số bắt đầu. Hãy đối thoại với NPC {0} để tham gia.", task.NPC.Name));
            /// Bắt đầu trò chơi
            BeginGame();

            /// Chưa hoàn thành đoán số
            this.guessNumberCompleted = false;
        }

        /// <summary>
        /// Theo dõi nhiệm vụ đoán số
        /// </summary>
        /// <param name="task"></param>
        private bool Track_GuessNumber(MilitaryCamp.EventInfo.StageInfo.TaskInfo task)
        {
            /// Kết thúc khi hoàn thành đoán số
            return this.guessNumberCompleted;
        }

        /// <summary>
        /// Thiết lập lại dữ liệu nhiệm vụ mở cơ quan
        /// </summary>
        private void Reset_GuessNumber()
        {
            this.guessNumberCurrentMinValue = -1;
            this.guessNumberCurrentMaxValue = -1;
            this.guessNumberLuckyNumber = -1;
            this.guessNumberTotalTurns = -1;
            this.guessNumberMember = null;
            this.guessNumberCompleted = false;
        }
    }
}
