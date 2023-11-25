using FSPlay.GameEngine.Logic;
using FSPlay.GameEngine.Network;
using FSPlay.KiemVu;
using FSPlay.KiemVu.Entities.Config;
using FSPlay.KiemVu.Logic.Settings;
using Server.Data;
using System.Linq;

namespace FSPlay.KiemVu.Logic
{
    /// <summary>
    /// Tự dùng thức ăn
    /// </summary>
    public partial class KTAutoFightManager
    {
        /// <summary>
        /// Tự dung thức ăn
        /// </summary>
        private void AutoUseFood()
        {
            /// Nếu không có thiết lập tự dùng thức ăn thì bỏ qua
            if (!KTAutoAttackSetting._AutoConfig._AutoPKConfig.IsAutoEat)
            {
                return;
            }
            /// Nếu chưa đến thời gian kiểm tra thì bỏ qua
            else if (KTGlobal.GetCurrentTimeMilis() - this.AutoFightLastCheckUseFoodTick < this.AutoFight_AutoUseFoodEveryTick)
            {
                return;
            }

            /// Đánh dấu thời gian kiểm tra tự dùng thức ăn
            this.AutoFightLastCheckUseFoodTick = KTGlobal.GetCurrentTimeMilis();

            /// Nếu đang có Symbol thức ăn phục hồi thì bỏ qua
            if (Global.Data.RoleData.BufferDataList != null && Global.Data.RoleData.BufferDataList.Where(x => x.BufferID == KTGlobal.FoodBuffID).FirstOrDefault() != null)
            {
                return;
            }
            /// Nếu không có danh sách vật phẩm
            else if (Global.Data.RoleData.GoodsDataList == null)
			{
                return;
			}

            /// Thức ăn được chọn
            GoodsData foodSelected = null;
            string foodName = "";
            int nHPFastReplenish = 0;
            int nMPFastReplenish = 0;
            /// Xem trong túi người chơi có thức ăn gì
            foreach (GoodsData itemGD in Global.Data.RoleData.GoodsDataList)
            {
                /// Nếu tìm thấy thức ăn trong túi người chơi
                if (itemGD.GCount > 0 && KTGlobal.ListFoods.TryGetValue(itemGD.GoodsID, out ItemData itemData))
                {
                    int thisFoodHPFastReplenish = 0;
                    int thisFoodMPFastReplenish = 0;
                    /// Duyệt danh sách thuộc tính của thuốc
                    foreach (Medicine prop in itemData.MedicineProp)
                    {
                        if (prop.MagicName == "lifegrow_v")
                        {
                            thisFoodHPFastReplenish = prop.Value;
                        }
                        else if (prop.MagicName == "managrow_v")
                        {
                            thisFoodMPFastReplenish = prop.Value;
                        }
                    }

                    /// Nếu chưa thức ăn nào được chọn hoặc thức ăn mới có chỉ số hồi phục tốt hơn thức ăn cũ tìm thấy
                    if (foodSelected == null || (thisFoodHPFastReplenish > nHPFastReplenish && thisFoodMPFastReplenish > nMPFastReplenish))
                    {
                        foodSelected = itemGD;
                        foodName = itemData.Name;
                        nHPFastReplenish = thisFoodHPFastReplenish;
                        nMPFastReplenish = thisFoodMPFastReplenish;
                    }
                }
            }

            /// Nếu tìm thấy thức ăn
            if (foodSelected != null)
            {
                KTGlobal.AddNotification(string.Format("Tự dùng thức ăn - {0}", foodName));

                /// Nếu vật phẩm không thể sử dụng
                GameInstance.Game.SpriteUseGoods(foodSelected.Id);
            }
            else if (KTGlobal.GetCurrentTimeMilis() - this.AutoFightLastNotifyNoFoodTick >= this.AutoFight_NotifyNoFoodTick)
            {
                this.AutoFightLastNotifyNoFoodTick = KTGlobal.GetCurrentTimeMilis();
                KTGlobal.AddNotification("Không tìm thấy thức ăn!");
            }
        }
    }
}
