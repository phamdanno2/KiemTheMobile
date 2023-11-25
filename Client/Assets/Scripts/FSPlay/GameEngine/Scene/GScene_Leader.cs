using System.Collections.Generic;
using UnityEngine;
using FSPlay.GameEngine.Logic;
using FSPlay.GameEngine.Network;
using FSPlay.GameEngine.Sprite;
using Server.Data;
using FSPlay.KiemVu.Factory;
using FSPlay.GameFramework.Logic;
using FSPlay.KiemVu.Utilities.UnityComponent;
using FSPlay.KiemVu;

namespace FSPlay.GameEngine.Scene
{
	/// <summary>
	/// Quản lý đối tượng Leader
	/// </summary>
	public partial class GScene
    {
        #region Khởi tạo ban đầu
        /// <summary>
        /// Tải xuống đối tượng Leader
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="direction"></param>
        /// <param name="newLifeDeco"></param>
        private void LoadLeader(int x, int y, int direction)
        {
            /// Xóa Leader cũ
            this.Leader?.Destroy();

            /// Hướng quay
            int oldDirection = direction;

            /// Khởi tạo đối tượng
            this.Leader = new GSprite();
            this.Leader.BaseID = SpriteBaseIds.RoleBaseId + Global.Data.RoleData.RoleID;
            this.Leader.SpriteType = GSpriteTypes.Leader;

            this.Leader.CoordinateChanged += (s) =>
            {
                this.UpdateLeaderEvent(s);
            };

            /// Tải đối tượng
            this.LoadSprite(
                this.Leader,
                Global.Data.RoleData.RoleID,
                "Leader",
                Global.Data.RoleData,
                null,
                null,
                null,
                null,
                (KiemVu.Entities.Enum.Direction) oldDirection,
                x,
                y
            );

            /// Bắt đầu đối tượng
            this.Leader.Start();

            /// Tìm đối tượng cũ
            GameObject oldObject = KTObjectPoolManager.Instance.FindSpawn(x => x.name == "Leader");
            /// Nếu tồn tại
            if (oldObject != null)
            {
                /// Trả lại Pool
                KTObjectPoolManager.Instance.ReturnToPool(oldObject);
            }

            /// Khởi tạo Res 2D
            //KiemVu.Control.Component.Character role = Object2DFactory.MakeLeader();
            KiemVu.Control.Component.Character role = KTObjectPoolManager.Instance.Instantiate<KiemVu.Control.Component.Character>("Leader");
            role.name = "Leader";

            GameObject role2D = role.gameObject;
            this.Leader.Role2D = role2D;
            role2D.transform.localPosition = new Vector2(x, y);
            Global.MainCamera.transform.localPosition = new Vector3(x, y, -10);
            Global.RadarMapCamera.transform.localPosition = new Vector3(x, y, -10);
            Global.MinimapCanvas.transform.localPosition = new Vector3(x, y, -10);

            /// Gắn các sự kiện Camera theo dõi
            Global.MainCamera.GetComponent<SmoothCamera2D>().Target = role2D.transform;
            Global.RadarMapCamera.GetComponent<FollowTarget>().Target = role2D.transform;
            Global.MinimapCanvas.GetComponent<FollowTarget>().Target = role2D.transform;

            /// Gắn đối tượng tham chiếu
            role.RefObject = this.Leader;
            ColorUtility.TryParseHtmlString("#2effb2", out Color nameColor);
            role.NameColor = nameColor;
            ColorUtility.TryParseHtmlString("#2eff5f", out Color hpBarColor);
            role.HPBarColor = hpBarColor;
            role.ShowMPBar = true;
            ColorUtility.TryParseHtmlString("#5fff2e", out Color minimapNameColỏ);
            role.MinimapNameColor = minimapNameColỏ;
            role.ShowMinimapIcon = true;
            role.ShowMinimapName = false;
            role.MinimapIconSize = new Vector2(50, 50);

            GoodsData weapon = null;
            GoodsData helm = null;
            GoodsData armor = null;
            GoodsData mantle = null;
            GoodsData horse = null;

            Dictionary<KiemVu.Entities.Enum.KE_EQUIP_POSITION, GoodsData> equips = KTGlobal.GetEquips(Global.Data.RoleData);
            equips.TryGetValue(KiemVu.Entities.Enum.KE_EQUIP_POSITION.emEQUIPPOS_WEAPON, out weapon);
            equips.TryGetValue(KiemVu.Entities.Enum.KE_EQUIP_POSITION.emEQUIPPOS_HEAD, out helm);
            equips.TryGetValue(KiemVu.Entities.Enum.KE_EQUIP_POSITION.emEQUIPPOS_BODY, out armor);
            equips.TryGetValue(KiemVu.Entities.Enum.KE_EQUIP_POSITION.emEQUIPPOS_MANTLE, out mantle);
            equips.TryGetValue(KiemVu.Entities.Enum.KE_EQUIP_POSITION.emEQUIPPOS_HORSE, out horse);

            /// Đổ dữ liệu vào
            role.Data = new RoleDataMini()
            {
                RoleSex = Global.Data.RoleData.RoleSex,

                IsRiding = Global.Data.RoleData.IsRiding,
                ArmorID = armor == null ? -1 : armor.GoodsID,
                HelmID = helm == null ? -1 : helm.GoodsID,
                WeaponID = weapon == null ? -1 : weapon.GoodsID,
                WeaponEnhanceLevel = weapon == null ? 0 : weapon.Forge_level,
                WeaponSeries = weapon == null ? 0 : weapon.Series,
                MantleID = mantle == null ? -1 : mantle.GoodsID,
                HorseID = horse == null ? -1 : horse.GoodsID,
            };
            role.UpdateRoleData();

            /// Thiết lập hướng
            role.Direction = this.Leader.Direction;
            /// Thực hiện động tác đứng
            role.Stand();

            /// Thực hiện Recover lại các hiệu ứng có trên người
            if (Global.Data.RoleData.BufferDataList != null)
            {
                foreach (BufferData buff in Global.Data.RoleData.BufferDataList)
                {
                    if (KiemVu.Loader.Loader.Skills.TryGetValue(buff.BufferID, out KiemVu.Entities.Config.SkillDataEx skillData))
                    {
                        this.Leader.AddBuff(skillData.StateEffectID, buff.BufferSecs == -1 ? -1 : buff.BufferSecs - (KTGlobal.GetServerTime() - buff.StartTime));
                    }
                }

                if (PlayZone.GlobalPlayZone != null && PlayZone.GlobalPlayZone.UIRolePart != null)
                {
                    PlayZone.GlobalPlayZone.UIRolePart.UIBuffList.RefreshDataList();
                }
            }
            else
            {
                Global.Data.RoleData.BufferDataList = new List<BufferData>();
            }
            /// End

            /// Cập nhật hiển thị thanh máu và khí
            this.RefreshSpriteLife(this.Leader);
            this.RefreshLeaderMagic();
        }

        #endregion

        #region Các sự kiện tương tác của Leader

        protected int LastEventTicks = 0;
        protected int LastTeleportKey = -1;

        /// <summary>
        /// Cập nhật sự kiện cho Leader
        /// </summary>
        /// <param name="sprite"></param>
        private void UpdateLeaderEvent(GSprite sprite)
        {
            if (!EnableChangMap)
            {
                return;
            }

            int ticks = Global.GetMyTimer();
            LastEventTicks = ticks;

            int leaderNowX = (int)((Leader.PosX / CurrentMapData.GridSizeX));
            int leaderNowY = (int)((Leader.PosY / CurrentMapData.GridSizeY));
            if (leaderNowX != Leader.OldGridX || leaderNowY != Leader.OldGridY)
            {
                Leader.OldGridX = leaderNowX;
                Leader.OldGridY = leaderNowY;
            }

            #region Đồng bộ tọa độ lên UIRadarMap và UILocalMap
            if (PlayZone.GlobalPlayZone != null && PlayZone.GlobalPlayZone.UIRadarMap != null)
            {
                PlayZone.GlobalPlayZone.UIRadarMap.LeaderPosition = new Vector2(leaderNowX, leaderNowY);
            }

            if (PlayZone.GlobalPlayZone != null && PlayZone.GlobalPlayZone.UILocalMap != null)
            {
                PlayZone.GlobalPlayZone.UILocalMap.LeaderPosition = new Vector2(this.Leader.PosX, this.Leader.PosY);
            }
            #endregion
        }

        #endregion

        #region Syns nhân vật đang Online lần trước với GS
        /// <summary>
        /// Thời gian gói tin syns nhân vật đang Online lần trước
        /// </summary>
        private long LastClientHearTicks = 0;

        /// <summary>
        /// Truyền tin tới Server syns nhân vật đang Online
        /// </summary>
        private void SendClientHeart()
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }

            long nowTicks = KTGlobal.GetCurrentTimeMilis();
            if (nowTicks - LastClientHearTicks >= (60 * 1000))
            {
                LastClientHearTicks = nowTicks;
                GameInstance.Game.SpriteHeart();
            }
        }

        #endregion


        #region Tương tác với đối tượng Leader
        /// <summary>
        /// Trả ra đối tượng Leader
        /// </summary>
        public GSprite GetLeader()
        {
            return this.Leader;
        }
        #endregion
    }
}
