using FSPlay.GameEngine.Logic;
using FSPlay.GameEngine.Network;
using FSPlay.KiemVu.Entities.Config;
using FSPlay.KiemVu.Logic.Settings;
using Server.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FSPlay.KiemVu.Logic
{
    public partial class KTAutoFightManager
    {
        public bool DoingAutoSell = false;

        /// <summary>
        /// Tự động bán đồ
        /// </summary>
        /// <returns></returns>
        private IEnumerator DoSellItem()
        {
            /// Nếu thiết lập không đốt lửa trại thì thôi
            if (!KTAutoAttackSetting._AutoConfig._PickItemConfig.IsAutoSellItem)
            {
                /// Bỏ đánh dấu bán
                this.DoingAutoSell = false;

                yield break;
            }

            long Tick = KTGlobal.GetCurrentTimeMilis() - this.AutoFightLastCheckAutoSell;

            /// Nếu chưa đến thời gian kiểm tra
            if (KTGlobal.GetCurrentTimeMilis() - this.AutoFightLastCheckAutoSell < this.AutoFight_CheckSellFullItemEveryTick)
            {
                /// Bỏ đánh dấu bán
                this.DoingAutoSell = false;

                yield break;
            }

            // Thời gian gầy đây nhất check túi đồ xem đầy hay chưa
            this.AutoFightLastCheckAutoSell = KTGlobal.GetCurrentTimeMilis();

            if (Global.Data.RoleData.GoodsDataList != null)
            {
                int Count = Global.Data.RoleData.GoodsDataList.Count(x => x.Using < 0);
                // NẾu túi đồ chưa đầy thì thôi
                if (Count < 100)
                {
                    /// Bỏ đánh dấu bán
                    this.DoingAutoSell = false;

                    yield break;
                }
            }
            else
            {
                this.DoingAutoSell = false;

                yield break;
            }

            /// Thiết lập thời gian kiểm tra

            /// Đánh dấu đang tự đốt lửa trại
            this.DoingAutoSell = true;

            int MapCodeBack = Global.Data.RoleData.MapCode;
            int XPostion = Global.Data.RoleData.PosX;
            int YPostion = Global.Data.RoleData.PosY;

            SELLSTATE = AUTOSELLSTATE.NONE;

            //Tạo ra 1 cái temp đồ sẽ bán để tránh bán quá nhanh trong LABADA
            List<GoodsData> TmpGoodWillBeSoul = new List<GoodsData>();

            while (this.DoingAutoSell)
            {

                KTGlobal.AddNotification("Đang thực hiện bán đồ");
                // Thực hiện CLICK này
                if (SELLSTATE == AUTOSELLSTATE.NONE)
                {
                    SELLSTATE = AUTOSELLSTATE.GOTONPC;

                    KTGlobal.QuestAutoFindPathToNPC(-1, 6781, () => {
                        AutoPathManager.Instance.StopAutoPath();

                        // Duyệt 1 vòng để lấy ra toàn bộ các vật phẩm có thể bán
                        foreach (GoodsData Data in Global.Data.RoleData.GoodsDataList)
                        {
                            // nếu vật phẩm đang sử dụng bỏ qua
                            if (Data.Using > 0)
                            {
                                continue;
                            }

                            // Nếu vật phẩm đã cường hóa bỏ qua
                            if (Data.Forge_level > 0)
                            {
                                continue;
                            }

                            // nếu vật phẩm là trang bị
                            if (!KTGlobal.IsEquip(Data.GoodsID))
                            {
                                continue;
                            }

                            // Nếu vật phẩm không thể bán thì bỏ qua
                            if (!KTGlobal.IsCanBeSold(Data))
                            {
                                continue;
                            }

                            StarLevelStruct starInfo = KTGlobal.StarCalculation(Data);
                            float starLevel = starInfo.StarLevel / 2f;

                            // nếu vật phẩm thỏa mãn điều kiện bán thì sẽ add vào danh sách bán
                            if (starLevel < KTAutoAttackSetting._AutoConfig._PickItemConfig.StarWillSell)
                            {
                                TmpGoodWillBeSoul.Add(Data);
                            }
                        }

                        SELLSTATE = AUTOSELLSTATE.CLICKSELL;
                    });
                }

                if (SELLSTATE == AUTOSELLSTATE.CLICKSELL)
                {
                    // Chuyển trạng thái sang start SELL
                    SELLSTATE = AUTOSELLSTATE.STARTSELL;

                    // Nếu có danh sách vật phẩm cần bán thì ta sẽ loop để bán lần lượt
                    if (TmpGoodWillBeSoul.Count > 0)
                    {
                        // Duyệt 1 vòng và thực hiện bán
                        for (int i = 0; i < TmpGoodWillBeSoul.Count; i++)
                        {
                            GoodsData _SelectItem = TmpGoodWillBeSoul[i];

                            // Check null phát nữa cho nó chắc
                            if (_SelectItem != null)
                            {
                                KTGlobal.AddNotification("[" + DateTime.Now.ToString() + "] Bán vật phẩm: " + KTGlobal.GetItemName(_SelectItem) + " thành công!");
                                //Thực hiện bán vật phẩm này
                                GameInstance.Game.SpriteBuyOutGoods(_SelectItem.Id);
                            }

                            // Nếu đang bán mà nhân vật di chuyển là cũng toác
                            if (!this.DoingAutoSell)
                            {
                                break;
                            }
                            // Mỗi vòng for nghỉ 1s mới bán cho nó đỡ cost gs
                            yield return new WaitForSeconds(1f);
                        }

                        // Nếu có config tự động sắp xếp lại túi đồ thì thực hiện sắp xếp
                        if (KTAutoAttackSetting._AutoConfig._PickItemConfig.IsAutoSort)
                        {
                            GameInstance.Game.SpriteSortBag();
                        }
                    }

                    // Nếu check 1 lần nữa mà đéo có vật phẩm nào có thể bán tức là trong túi đồ toàn đồ không thể bán thực hiện FORCE tắt nhặt đồ ở SETTINGS
                    if (Global.Data.RoleData.GoodsDataList.Count >= 100)
                    {
                        //  Force tắt chức năng bán vật phẩm
                        KTAutoAttackSetting._AutoConfig._PickItemConfig.IsAutoSellItem = false;
                    }

                    SELLSTATE = AUTOSELLSTATE.MOVEBACK;
                }

                if (SELLSTATE == AUTOSELLSTATE.MOVEBACK)
                {
                    SELLSTATE = AUTOSELLSTATE.END;

                    // Tự tìm đường về chỗ cũ
                    KTGlobal.QuestAutoFindPath(MapCodeBack, XPostion, YPostion, () => {
                        AutoPathManager.Instance.StopAutoPath();

                        // Set lại trạng thái đã bán xong
                        this.DoingAutoSell = false;

                        this.StopAutoFight();
                        this.StartAutoFarm();

                        startPos = new Vector2(XPostion, YPostion);
                    });
                }


                if (!this.DoingAutoSell)
                {
                    SELLSTATE = AUTOSELLSTATE.END;
                    /// Thoát lặp
                    break;
                }
                /// Chờ 1 giây mới thực hiện vòng lặp
                yield return new WaitForSeconds(1f);
            }
        }

        /// <summary>
        /// Thoát việc tự bán nếu có sự di chuyển của người chơi
        /// </summary>
        public void StopAutoSell()
        {
            this.DoingAutoSell = false;
        }
    }
}