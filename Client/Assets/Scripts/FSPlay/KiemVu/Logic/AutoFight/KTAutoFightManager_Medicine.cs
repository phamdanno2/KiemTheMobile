using FSPlay.GameEngine.Logic;
using FSPlay.GameEngine.Network;
using FSPlay.GameEngine.Sprite;
using FSPlay.KiemVu.Entities.Config;
using FSPlay.KiemVu.Logic.Settings;
using Server.Data;
using System.Collections.Generic;
using System.Linq;

namespace FSPlay.KiemVu.Logic
{
    /// <summary>
    /// Tự dùng thuốc
    /// </summary>
    public partial class KTAutoFightManager
    {
        /// <summary>
        /// Tự dùng thuốc
        /// </summary>
        private void AutoUseMedicine()
        {
            /// Nếu không có thiết lập tự dùng thuốc thì bỏ qua
            if (!KTAutoAttackSetting._AutoConfig._AutoPKConfig.IsAutoHp)
            {
                return;
            }
            /// Nếu chưa đến thời gian kiểm tra thì bỏ qua
            else if (KTGlobal.GetCurrentTimeMilis() - this.AutoFightLastCheckUseMedicineTick < this.AutoFight_AutoUseMedicineEveryTick)
            {
                return;
            }
            /// Nếu không có danh sách vật phẩm
            else if (Global.Data.RoleData.GoodsDataList == null)
			{
                return;
			}

            /// Đánh dấu thời gian kiểm tra tự dùng thuốc
            this.AutoFightLastCheckUseMedicineTick = KTGlobal.GetCurrentTimeMilis();

            /// Đối tượng người chơi
            GSprite leader = Global.Data.Leader;
            /// Nếu Loader NULL
            if (leader == null)
			{
                return;
			}

            /// % sinh nội lực
            int hpPercent = leader.HP * 100 / leader.HPMax;
            int mpPercent = leader.MP * 100 / leader.MPMax;

            /// Thuốc sinh lực
            Loader.Loader.Items.TryGetValue(KTAutoAttackSetting._AutoConfig._AutoPKConfig.HPMedicine, out ItemData hpMedicine);
            /// Thuốc nội lực
            Loader.Loader.Items.TryGetValue(KTAutoAttackSetting._AutoConfig._AutoPKConfig.MPMedicine, out ItemData mpMedicine);

            /// Nếu không có thiết lập thuốc phục hồi sinh lực, nội lực
            if (hpMedicine == null && mpMedicine == null)
            {
                if (KTGlobal.GetCurrentTimeMilis() - this.AutoFightLastNotifyNoMedicineTick >= this.AutoFight_NotifyNoMedicineTick)
                {
                    this.AutoFightLastNotifyNoMedicineTick = KTGlobal.GetCurrentTimeMilis();
                   // KTGlobal.AddNotification("Chưa thiết lập thuốc phục hồi sinh nội lực!");
                }
                return;
            }

            /// Đánh dấu có thuốc trong túi không
            bool hpMedicineFound = true;
            bool mpMedicineFound = true;

            GoodsData hpMedicineGD = null;
            GoodsData mpMedicineGD = null;

            /// Nếu lượng sinh lực dưới mức thiết lập tự dùng thuốc
            if (hpPercent <= KTAutoAttackSetting._AutoConfig._AutoPKConfig.HpPercent)
            {
                /// Nếu tồn tại thuốc hồi sinh lực
                if (hpMedicine != null)
                {
                    /// Thuốc
                    GoodsData itemGD = Global.Data.RoleData.GoodsDataList.Where(x => x.GoodsID == hpMedicine.ItemID && x.GCount > 0).FirstOrDefault();
                    /// Nếu có thuốc trong túi người chơi
                    if (itemGD != null)
                    {
                        /// Sử dụng vật phẩm
                        //GameInstance.Game.SpriteUseGoods(itemGD.Id, itemGD.GoodsID);
                        hpMedicineGD = itemGD;
                    }
                    else
                    {
                        hpMedicineFound = false;
                    }
                }
            }

            /// Nếu lượng nội lực dưới mức thiết lập tự dùng thuốc
            if (mpPercent <= KTAutoAttackSetting._AutoConfig._AutoPKConfig.MpPercent)
            {
                /// Nếu tồn tại thuốc hồi sinh lực
                if (mpMedicine != null)
                {
                    /// Thuốc
                    GoodsData itemGD = Global.Data.RoleData.GoodsDataList.Where(x => x.GoodsID == mpMedicine.ItemID && x.GCount > 0).FirstOrDefault();
                    /// Nếu có thuốc trong túi người chơi
                    if (itemGD != null)
                    {
                        /// Sử dụng vật phẩm
                        //GameInstance.Game.SpriteUseGoods(itemGD.Id, itemGD.GoodsID);
                        mpMedicineGD = itemGD;
                    }
                    else
                    {
                        mpMedicineFound = false;
                    }
                }
            }

            /// Nếu có thuốc hồi sinh lực
            if (hpMedicineGD != null)
			{
                /// Nếu có thuốc hồi nội lực
                if (mpMedicineGD != null)
				{
                    /// Sử dụng thuốc
                    GameInstance.Game.SpriteUseGoods(hpMedicineGD.Id, mpMedicineGD.Id);
				}
				/// Nếu không có thuốc hồi nội lực
				else
				{
                    /// Sử dụng thuốc
                    GameInstance.Game.SpriteUseGoods(hpMedicineGD.Id);
                }
			}
            /// Nếu có thuốc hồi nội lực
            else if (mpMedicineGD != null)
			{
                /// Sử dụng thuốc
                GameInstance.Game.SpriteUseGoods(mpMedicineGD.Id);
            }

            /// Nếu không tìm thấy thuốc
            if (!hpMedicineFound || !mpMedicineFound)
            {
                /// Nếu đã đến thời gian thông báo
                if (KTGlobal.GetCurrentTimeMilis() - this.AutoFightLastNotifyNoMedicineTick >= this.AutoFight_NotifyNoMedicineTick)
                {
                    this.AutoFightLastNotifyNoMedicineTick = KTGlobal.GetCurrentTimeMilis();

                    List<string> medicineNames = new List<string>();
                    if (!hpMedicineFound)
                    {
                        medicineNames.Add(hpMedicine.Name);
                    }
                    if (!mpMedicineFound)
                    {
                        medicineNames.Add(mpMedicine.Name);
                    }
                    KTGlobal.AddNotification(string.Format("Không tìm thấy {0}!", string.Join(", ", medicineNames)));
                }
            }
        }
    }
}
