using FSPlay.GameEngine.Logic;
using FSPlay.KiemVu.Entities.Config;
using FSPlay.KiemVu.Logic.Settings;
using FSPlay.KiemVu.UI.Main.ItemBox;
using FSPlay.KiemVu.Utilities.UnityUI;
using Server.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FSPlay.KiemVu.UI.Main.MainUI.RadarMap
{
    /// <summary>
    /// Khung sử dụng thuốc nhanh dưới RadarMap
    /// </summary>
    public class UIRadarMap_QuickMedicineBox : MonoBehaviour
    {
        #region Define
        /// <summary>
        /// Thuốc hồi sinh lực
        /// </summary>
        [SerializeField]
        private UIItemBox UIItemBox_HPMedicine;

        /// <summary>
        /// Thuốc hồi nội lực
        /// </summary>
        [SerializeField]
        private UIItemBox UIItemBox_MPMedicine;

        /// <summary>
        /// Khung chọn thuốc
        /// </summary>
        [SerializeField]
        private UISelectItem UISelectItem;
        #endregion

        #region Private fields
        /// <summary>
        /// UI Hover thuốc hồi sinh lực
        /// </summary>
        private UIHoverableObject uiHover_HPMedicine;

        /// <summary>
        /// UI Hover thuốc hồi nội lực
        /// </summary>
        private UIHoverableObject uiHover_MPMedicine;

        /// <summary>
        /// Luồng thực thi yêu cầu lưu thiết lập thuốc dùng nhanh vào hệ thống
        /// </summary>
        private Coroutine saveMedicineCoroutine = null;
        #endregion

        #region Properties
        /// <summary>
        /// Sự kiện dùng thuốc tương ứng
        /// </summary>
        public Action<GoodsData> UseMedicine { get; set; }

        /// <summary>
        /// Sự kiện thuốc được chọn
        /// </summary>
        public Action<GoodsData, GoodsData> MedicineSelected { get; set; }
        #endregion

        #region Core MonoBehaviour
        /// <summary>
        /// Hàm này gọi khi đối tượng được tạo ra
        /// </summary>
        private void Awake()
        {
            this.uiHover_HPMedicine = this.UIItemBox_HPMedicine.GetComponent<UIHoverableObject>();
            this.uiHover_MPMedicine = this.UIItemBox_MPMedicine.GetComponent<UIHoverableObject>();
        }

        /// <summary>
        /// Hàm này gọi ở Frame đầu tiên
        /// </summary>
        private void Start()
        {
            this.InitPrefabs();
            this.StartCoroutine(this.UpdateMedicineCountInBag());
        }
        #endregion

        #region Code UI
        /// <summary>
        /// Khởi tạo ban đầu
        /// </summary>
        private void InitPrefabs()
        {
            this.uiHover_HPMedicine.Hover = this.ButtonHPMedicine_Hovered;
            this.uiHover_MPMedicine.Hover = this.ButtonMPMedicine_Hovered;
            this.uiHover_HPMedicine.Click = this.ButtonHPMedicine_Clicked;
            this.uiHover_MPMedicine.Click = this.ButtonMPMedicine_Clicked;
            this.UIItemBox_HPMedicine.Click = null;
            this.UIItemBox_MPMedicine.Click = null;
        }

        /// <summary>
        /// Sự kiện khi bắt đầu Hover Button hồi sinh lực
        /// </summary>
        private void ButtonHPMedicine_Hovered()
        {
            this.ShowQuickUseItemSelectFrame_Left();
        }

        /// <summary>
        /// Sự kiện khi bắt đầu Hover Button hồi nội lực
        /// </summary>
        private void ButtonMPMedicine_Hovered()
        {
            this.ShowQuickUseItemSelectFrame_Right();
        }

        /// <summary>
        /// Sự kiện khi Button hồi sinh lực được ấn
        /// </summary>
        private void ButtonHPMedicine_Clicked()
        {
            /// Nếu chưa có thuốc nào được đặt vào
            if (this.UIItemBox_HPMedicine.Data == null)
            {
                this.ShowQuickUseItemSelectFrame_Left();
            }
            else
            {
                /// Nếu danh sách vật phẩm không tồn tại
                if (Global.Data.RoleData.GoodsDataList == null)
                {
                    return;
                }

                /// Tìm vị trí thuốc tương ứng trong túi
                GoodsData itemGD = Global.Data.RoleData.GoodsDataList.Where(x => x.GoodsID == this.UIItemBox_HPMedicine.Data.GoodsID).FirstOrDefault();
                /// Nếu không có thuốc
                if (itemGD == null)
                {
                    KTGlobal.AddNotification("Đã hết thuốc!");
                    return;
                }
                this.UseMedicine?.Invoke(itemGD);
            }
        }

        /// <summary>
        /// Sự kiện khi Button hồi nội lực được ấn
        /// </summary>
        private void ButtonMPMedicine_Clicked()
        {
            /// Nếu chưa có thuốc nào được đặt vào
            if (this.UIItemBox_MPMedicine.Data == null)
            {
                this.ShowQuickUseItemSelectFrame_Right();
            }
            else
            {
                /// Nếu danh sách vật phẩm không tồn tại
                if (Global.Data.RoleData.GoodsDataList == null)
                {
                    return;
                }

                /// Tìm vị trí thuốc tương ứng trong túi
                GoodsData itemGD = Global.Data.RoleData.GoodsDataList.Where(x => x.GoodsID == this.UIItemBox_MPMedicine.Data.GoodsID).FirstOrDefault();
                /// Nếu không có thuốc
                if (itemGD == null)
                {
                    KTGlobal.AddNotification("Đã hết thuốc!");
                    return;
                }
                this.UseMedicine?.Invoke(itemGD);
            }
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Thực thi sự kiện sau khoảng thời gian tương ứng
        /// </summary>
        /// <param name="sec"></param>
        /// <param name="callBack"></param>
        /// <returns></returns>
        private IEnumerator ExecuteLater(float sec, Action callBack)
        {
            yield return new WaitForSeconds(sec);
            callBack?.Invoke();
        }
        
        /// <summary>
        /// Hiện khung chọn vật phẩm có thể dùng
        /// </summary>
        private void ShowQuickUseItemSelectFrame_Left()
        {
            if (Global.Data.RoleData.GoodsDataList == null)
            {
                return;
            }

            List<GoodsData> hpMedicines = Global.Data.RoleData.GoodsDataList?.Where(x => KTGlobal.IsItemCanUse(x.GoodsID)).GroupBy(x => x.GoodsID).Select(x => x.First()).ToList();
            this.UISelectItem.Title = "Chọn vật phẩm sử dụng nhanh";
            this.UISelectItem.Items = hpMedicines;
            this.UISelectItem.ItemSelected = (itemGD) => {
                /// Nếu không có vật phẩm được chọn
                if (itemGD == null)
                {
                    return;
                }
                /// Nếu vật phẩm không tồn tại trong hệ thống
                if (!Loader.Loader.Items.TryGetValue(itemGD.GoodsID, out ItemData itemData))
                {
                    return;
                }

                /// Tạo mới đối tượng vật phẩm tương ứng
                GoodsData hpMedicine = new GoodsData()
                {
                    GoodsID = itemGD.GoodsID,
                    GCount = Global.Data.RoleData.GoodsDataList.Where(x => x.GoodsID == itemGD.GoodsID).Sum(x => x.GCount),
                    Binding = itemGD.Binding,
                };
                this.UIItemBox_HPMedicine.Data = hpMedicine;

                /// Thực thi sự kiện chọn thuốc
                KTAutoAttackSetting._AutoConfig._QickItemConfig.QuickKeySlot1 = itemGD.GoodsID;
                this.SaveAndExecuteMedicineSelected();
            };
            this.UISelectItem.Show();
        }
        
        /// <summary>
        /// Hiện khung chọn vật phẩm có thể dùng
        /// </summary>
        private void ShowQuickUseItemSelectFrame_Right()
        {
            if (Global.Data.RoleData.GoodsDataList == null)
            {
                return;
            }

            List<GoodsData> hpMedicines = Global.Data.RoleData.GoodsDataList?.Where(x => KTGlobal.IsItemCanUse(x.GoodsID)).GroupBy(x => x.GoodsID).Select(x => x.First()).ToList();
            this.UISelectItem.Title = "Chọn vật phẩm sử dụng nhanh";
            this.UISelectItem.Items = hpMedicines;
            this.UISelectItem.ItemSelected = (itemGD) => {
                /// Nếu không có vật phẩm được chọn
                if (itemGD == null)
                {
                    return;
                }
                /// Nếu vật phẩm không tồn tại trong hệ thống
                if (!Loader.Loader.Items.TryGetValue(itemGD.GoodsID, out ItemData itemData))
                {
                    return;
                }

                /// Tạo mới đối tượng vật phẩm tương ứng
                GoodsData mpMedicine = new GoodsData()
                {
                    GoodsID = itemGD.GoodsID,
                    GCount = Global.Data.RoleData.GoodsDataList.Where(x => x.GoodsID == itemGD.GoodsID).Sum(x => x.GCount),
                    Binding = itemGD.Binding,
                };
                this.UIItemBox_MPMedicine.Data = mpMedicine;

                /// Thực thi sự kiện chọn thuốc
                KTAutoAttackSetting._AutoConfig._QickItemConfig.QuickKeySlot2 = itemGD.GoodsID;
                this.SaveAndExecuteMedicineSelected();
            };
            this.UISelectItem.Show();
        }

        /// <summary>
        /// Cập nhật số lượng thuốc hiện có trong túi đồ
        /// </summary>
        /// <returns></returns>
        private IEnumerator UpdateMedicineCountInBag()
        {
            int lastHPMedicineID = -1;
            int lastHPMedicineCount = -1;
            int lastMPMedicineID = -1;
            int lastMPMedicineCount = -1;
            while (true)
            {
               
                /// Cập nhật từ AutoSetting vào
                if (KTAutoAttackSetting._AutoConfig._QickItemConfig.QuickKeySlot1 != -1 && (this.UIItemBox_HPMedicine.Data == null || this.UIItemBox_HPMedicine.Data.GoodsID != KTAutoAttackSetting._AutoConfig._QickItemConfig.QuickKeySlot1))
                {
                    GoodsData itemGD = new GoodsData()
                    {
                        GoodsID = KTAutoAttackSetting._AutoConfig._QickItemConfig.QuickKeySlot1,
                    };
                    this.UIItemBox_HPMedicine.Data = itemGD;
                    this.UIItemBox_HPMedicine.Refresh();
                }
                if (KTAutoAttackSetting._AutoConfig._QickItemConfig.QuickKeySlot2 != -1 && (this.UIItemBox_MPMedicine.Data == null || this.UIItemBox_MPMedicine.Data.GoodsID != KTAutoAttackSetting._AutoConfig._QickItemConfig.QuickKeySlot2))
                {
                    GoodsData itemGD = new GoodsData()
                    {
                        GoodsID = KTAutoAttackSetting._AutoConfig._QickItemConfig.QuickKeySlot2,
                    };
                    this.UIItemBox_MPMedicine.Data = itemGD;
                    this.UIItemBox_MPMedicine.Refresh();
                }

                /// Nếu có thuốc sinh lực
                if (this.UIItemBox_HPMedicine.Data != null && Global.Data.RoleData.GoodsDataList != null)
                {
                    int count = Global.Data.RoleData.GoodsDataList.Where(x => x.GoodsID == this.UIItemBox_HPMedicine.Data.GoodsID).Sum(x => x.GCount);
                    /// Nếu số lượng <= 0
                    if (count <= 0)
                    {
                        /// Xóa khỏi ô
                        this.UIItemBox_HPMedicine.Data = null;

                        KTAutoAttackSetting._AutoConfig._QickItemConfig.QuickKeySlot1 = -1;
                        /// Lưu thiết lập Auto
                        this.SaveAndExecuteMedicineSelected();
                    }
                    else
                    {
                        this.UIItemBox_HPMedicine.Data.GCount = count;
                        this.UIItemBox_HPMedicine.RefreshQuantity();
                    }

                    /// Nếu ID vật phẩm giống nhau và số lượng bị giảm xuống tức là có sử dụng
                    if (this.UIItemBox_HPMedicine.Data != null && lastHPMedicineID == this.UIItemBox_HPMedicine.Data.GoodsID && lastHPMedicineCount > count)
					{
                        this.UIItemBox_HPMedicine.PlayUseItemSuccessfullyEffect();
					}

                    /// Cập nhật ID vật phẩm và số lượng
                    lastHPMedicineID = this.UIItemBox_HPMedicine.Data == null ? -1 : this.UIItemBox_HPMedicine.Data.GoodsID;
                    lastHPMedicineCount = count;
                }
                /// Nếu có thuốc nội lực
                if (this.UIItemBox_MPMedicine.Data != null && Global.Data.RoleData.GoodsDataList != null)
                {
                    int count = Global.Data.RoleData.GoodsDataList.Where(x => x.GoodsID == this.UIItemBox_MPMedicine.Data.GoodsID).Sum(x => x.GCount);
                    /// Nếu số lượng <= 0
                    if (count <= 0)
                    {
                        /// Xóa khỏi ô
                        this.UIItemBox_MPMedicine.Data = null;
                        KTAutoAttackSetting._AutoConfig._QickItemConfig.QuickKeySlot2 = -1;
                        /// Lưu thiết lập Auto
                        this.SaveAndExecuteMedicineSelected();
                    }
                    else
                    {
                        this.UIItemBox_MPMedicine.Data.GCount = count;
                        this.UIItemBox_MPMedicine.RefreshQuantity();
                    }

                    /// Nếu ID vật phẩm giống nhau và số lượng bị giảm xuống tức là có sử dụng
                    if (this.UIItemBox_MPMedicine.Data != null && lastMPMedicineID == this.UIItemBox_MPMedicine.Data.GoodsID && lastMPMedicineCount > count)
                    {
                        this.UIItemBox_MPMedicine.PlayUseItemSuccessfullyEffect();
                    }

                    /// Cập nhật ID vật phẩm và số lượng
                    lastMPMedicineID = this.UIItemBox_MPMedicine.Data == null ? -1 : this.UIItemBox_MPMedicine.Data.GoodsID;
                    lastMPMedicineCount = count;
                }
                /// Nghỉ 1 giây
                yield return new WaitForSeconds(1f);
            }
        }

        /// <summary>
        /// Lưu thiết lập hệ thống và thực thi sự kiện chọn thuốc
        /// </summary>
        private void SaveAndExecuteMedicineSelected()
        {
            if (this.saveMedicineCoroutine != null)
            {
                this.StopCoroutine(this.saveMedicineCoroutine);
            }
            this.saveMedicineCoroutine = this.StartCoroutine(this.ExecuteLater(5f, () => {
                this.MedicineSelected?.Invoke(this.UIItemBox_HPMedicine.Data, this.UIItemBox_MPMedicine.Data);
            }));
        }
        #endregion

        #region Public methods

        #endregion
    }
}
