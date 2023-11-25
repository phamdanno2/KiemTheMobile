﻿using FSPlay.GameEngine.GoodsPack;
using FSPlay.GameEngine.Logic;
using FSPlay.KiemVu.Logic.Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSPlay.KiemVu.Logic
{
    /// <summary>
    /// Luồng thực thi tự động nhặt vật phẩm xung quanh
    /// </summary>
    public class KTAutoPickUpItemAround : TTMonoBehaviour
    {
        #region Private fields

        /// <summary>
        /// Tự động nhặt sau mỗi khoảng
        /// </summary>
        private const float AutoPickUpEverySec = 0.5f;

        #endregion Private fields

        #region Core MonoBehaviour

        /// <summary>
        /// Hàm này gọi ở Frame đầu tiên
        /// </summary>
        private void Start()
        {
            //this.StartCoroutine(this.ProcessAutoPickUpItemAround());
        }

        #endregion Core MonoBehaviour

        #region Private methods

        /// <summary>
        /// Luồng thực thi tự nhặt vật phẩm xung quanh khi đi qua
        /// </summary>
        /// <returns></returns>
        private IEnumerator ProcessAutoPickUpItemAround()
        {
            /// Lặp mãi mãi
            while (true)
            {
                /// Nếu mở tự nhặt xung quanh
                if (KTAutoAttackSetting._AutoConfig._PickItemConfig.IsAutoPickUp)
                {
                    /// Nếu không phải trong màn hình Game hoặc không có Leader
                    if (Global.Data == null || Global.Data.GameScene == null || Global.Data.Leader == null)
                    {
                        /// Tiếp tục vòng lặp
                        goto SKIP;
                    }
                    /// Nếu đang đợi tải xuống bản đồ
                    else if (Global.Data.WaitingForMapChange)
                    {
                        /// Tiếp tục vòng lặp
                        goto SKIP;
                    }

                    /// Vị trí của Leader
                    Vector2 leaderPos = Global.Data.Leader.PositionInVector2;
                    /// Tìm vật phẩm gần nhất xung quanh
                    List<GGoodsPack> goodsPacks = Global.Data.GameScene.FindGoodsPacks((gp) =>
                    {
                        /// Nếu  khoảng cách đủ gần với khoảng cách thiết lập thì nhặt
                        if (Vector2.Distance(gp.PositionInVector2, leaderPos) > KTAutoAttackSetting._AutoConfig._PickItemConfig.RadiusPick)
                        {
                            return false;
                        }

                        // Nếu auto chỉ thiết lập nhặt huyền tinh
                        if (KTAutoAttackSetting._AutoConfig._PickItemConfig.IsOnlyPickCrytal)
                        {
                            if (KTAutoAttackSetting.IsCanPickCrytalItem(gp.GoodsID, KTAutoAttackSetting._AutoConfig._PickItemConfig.CrytalLevel))
                            {
                                return true;
                            }
                            else // Nếu không thì đéo nhặt
                            {
                                return false;
                            }
                        }

                        // nếu auto thiết lập chỉ nhặt trang bị
                        if (KTAutoAttackSetting._AutoConfig._PickItemConfig.IsOnlyPickEquip)
                        {  /// Nếu số sao thỏa mãn
                            if (gp.Stars >= KTAutoAttackSetting._AutoConfig._PickItemConfig.StarPick)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }

                        // Nếu không thì bú tất
                        return true;
                    });
                    /// Duyệt danh sách và thực hiện nhặt
                    foreach (GGoodsPack gp in goodsPacks)
                    {
                        Global.Data.GameScene.DropItemClick(gp);
                    }
                }

            SKIP:
                /// Nghỉ
                yield return new WaitForSeconds(KTAutoPickUpItemAround.AutoPickUpEverySec);
            }
        }

        #endregion Private methods
    }
}