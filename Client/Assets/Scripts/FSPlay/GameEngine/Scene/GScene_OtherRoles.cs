using System.Collections.Generic;
using UnityEngine;
using FSPlay.GameEngine.Logic;
using FSPlay.GameEngine.Network;
using FSPlay.GameEngine.Sprite;
using Server.Data;
using FSPlay.KiemVu.Factory;
using FSPlay.KiemVu;
using FSPlay.GameFramework.Logic;

namespace FSPlay.GameEngine.Scene
{
	/// <summary>
	/// Quản lý đối tượng người chơi khác
	/// </summary>
	public partial class GScene
    {
        #region Khởi tạo
        /// <summary>
        /// Danh sách người chơi khác, con này sẽ cache từ từ load từng thằng một đảm bảo không bị giật
        /// </summary>
        private List<OtherRoleItem> waitToBeAddedRole = new List<OtherRoleItem>();

        /// <summary>
        /// Tải đối tượng người chơi vào bản đồ
        /// </summary>
        /// <param name="roleData"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="direction"></param>
        public void ToLoadOtherRole(RoleData roleData, int x, int y, int direction, int currentAction)
        {
            OtherRoleItem otherRoleItem = new OtherRoleItem()
            {
                RoleData = roleData,
                X = x,
                Y = y,
                Direction = direction,
                CurrentAction = currentAction,
            };

            this.waitToBeAddedRole.Add(otherRoleItem);
        }

        /// <summary>
        /// Thêm đối tượng vào danh sách
        /// <para>Con này gọi liên tục ở hàm Update</para>
        /// </summary>
        private void AddListRole()
        {
            if (waitToBeAddedRole.Count <= 0)
            {
                return;
            }

            OtherRoleItem otherRoleItem = this.waitToBeAddedRole[0];
            this.waitToBeAddedRole.RemoveAt(0);
            this.AddListRole(otherRoleItem);
        }

        /// <summary>
        /// Thêm đối tượng vào bản đồ
        /// </summary>
        private void AddListRole(OtherRoleItem otherRoleItem)
        {
            /// Nếu không có dữ liệu
            if (otherRoleItem == null)
            {
                //KTGlobal.AddNotification("Player DATA NULL, return => " + otherRoleItem.RoleData.RoleName);
                return;
            }

            /// Nếu đối tượng không tồn tại thì bỏ qua
            if (!Global.Data.OtherRoles.TryGetValue(otherRoleItem.RoleData.RoleID, out _))
			{
                //KTGlobal.AddNotification("Player not exist, return => " + otherRoleItem.RoleData.RoleName);
                return;
			}

            /// Tìm đối tượng cũ
            GSprite oldObject = KTGlobal.FindSpriteByID(otherRoleItem.RoleData.RoleID);
            /// Nếu tồn tại
            if (oldObject != null)
			{
                /// Xóa
                oldObject.Destroy();
			}

            /// Tải xuống đối tượng
            GSprite sprite = this.LoadRole(otherRoleItem.RoleData, otherRoleItem.X, otherRoleItem.Y, otherRoleItem.Direction, otherRoleItem.CurrentAction);
            /// Thực hiện gửi gói tin tải xuống hoàn tất
            GameInstance.Game.SpriteLoadAlready(sprite.RoleID);
        }

        /// <summary>
        /// Tải xuống đối tượng
        /// </summary>
        private GSprite LoadRole(RoleData roleData, int x, int y, int direction, int currentAction)
        {
            //KTDebug.LogError("Begin load PLAYER => " + roleData.RoleName);

            string name =  string.Format("Role_{0}", roleData.RoleID);

            GSprite sprite = new GSprite();


            /// Tạo mới đối tượng
            sprite.BaseID = SpriteBaseIds.RoleBaseId + roleData.RoleID;
            sprite.OldGridX = roleData.PosX / CurrentMapData.GridSizeX;
            sprite.OldGridY = roleData.PosY / CurrentMapData.GridSizeX;
			sprite.SpriteType = GSpriteTypes.Other;
			sprite.CoordinateChanged += (sender) =>
			{
				this.UpdateRoleEvent(sender);
			};

            /// Tải đối tượng
			this.LoadSprite(
				sprite, 
				roleData.RoleID,
				name,
				roleData,
                null,
				null,
				null,
				null,
				(KiemVu.Entities.Enum.Direction) direction, 
				x,
                y
            );

            /// Bắt đầu
            sprite.Start();

            /// Tìm đối tượng cũ
            GameObject oldObject = KTObjectPoolManager.Instance.FindSpawn(x => x.name == name);
            /// Nếu tồn tại
            if (oldObject != null)
            {
                /// Trả lại Pool
                KTObjectPoolManager.Instance.ReturnToPool(oldObject);
                //KTGlobal.AddNotification("Duplicate PLAYER obj => " + roleData.RoleName);
            }

            //KiemVu.Control.Component.Character role = Object2DFactory.MakeOtherRole();
            KiemVu.Control.Component.Character role = KTObjectPoolManager.Instance.Instantiate<KiemVu.Control.Component.Character>("OtherRole");
            role.name = name;

            /// Gắn đối tượng tham chiếu
            role.RefObject = sprite;
            /// TODO check state with Leader
            ColorUtility.TryParseHtmlString("#2effd9", out Color nameColor);
            role.NameColor = nameColor;
            ColorUtility.TryParseHtmlString("#2eff5f", out Color hpBarColor);
            role.HPBarColor = hpBarColor;

            /// Không hiển thị thanh nội lực
            role.ShowMPBar = false;

            /// TODO check type of OtherRole vs Leader
            ColorUtility.TryParseHtmlString("#2ebdff", out Color minimapNameColor);
            role.MinimapNameColor = minimapNameColor;
            role.ShowMinimapIcon = true;
            role.ShowMinimapName = false;
            role.MinimapIconSize = new Vector2(50, 50);

            GoodsData weapon = null;
            GoodsData helm = null;
            GoodsData armor = null;
            GoodsData mantle = null;
            GoodsData horse = null;

            Dictionary<KiemVu.Entities.Enum.KE_EQUIP_POSITION, GoodsData> equips = KTGlobal.GetEquips(roleData);
            equips.TryGetValue(KiemVu.Entities.Enum.KE_EQUIP_POSITION.emEQUIPPOS_WEAPON, out weapon);
            equips.TryGetValue(KiemVu.Entities.Enum.KE_EQUIP_POSITION.emEQUIPPOS_HEAD, out helm);
            equips.TryGetValue(KiemVu.Entities.Enum.KE_EQUIP_POSITION.emEQUIPPOS_BODY, out armor);
            equips.TryGetValue(KiemVu.Entities.Enum.KE_EQUIP_POSITION.emEQUIPPOS_MANTLE, out mantle);
            equips.TryGetValue(KiemVu.Entities.Enum.KE_EQUIP_POSITION.emEQUIPPOS_HORSE, out horse);

            /// Đổ dữ liệu vào
            role.Data = new RoleDataMini()
            {
                RoleSex = roleData.RoleSex,

                IsRiding = roleData.IsRiding,
                ArmorID = armor == null ? -1 : armor.GoodsID,
                HelmID = helm == null ? -1 : helm.GoodsID,
                WeaponID = weapon == null ? -1 : weapon.GoodsID,
                WeaponEnhanceLevel = weapon == null ? 0 : weapon.Forge_level,
                WeaponSeries = weapon == null ? 0 : weapon.Series,
                MantleID = mantle == null ? -1 : mantle.GoodsID,
                HorseID = horse == null ? -1 : horse.GoodsID,
            };
            role.UpdateRoleData();

            GameObject role2D = role.gameObject;
            sprite.Role2D = role2D;
            role2D.transform.localPosition = new Vector2((float)x, (float)y);

            /// Tải xuống các hiệu ứng
            if (roleData.BufferDataList != null)
            {
                foreach (BufferData buff in roleData.BufferDataList)
                {
                    if (KiemVu.Loader.Loader.Skills.TryGetValue(buff.BufferID, out KiemVu.Entities.Config.SkillDataEx skillData))
                    {
                        sprite.AddBuff(skillData.StateEffectID, buff.BufferSecs == -1 ? -1 : buff.BufferSecs - (KTGlobal.GetServerTime() - buff.StartTime));
                    }
                }
            }
            /// End
            
            /// Thực hiện động tác đứng 
            sprite.DoStand(true);

            /// Làm mới giá trị máu của đối tượng
            this.RefreshSpriteLife(sprite);

            //KTDebug.LogError("Load PLAYER done => " + roleData.RoleName);

            return sprite;
        }

        /// <summary>
        /// Cập nhật sự kiện của đối tượng người chơi khác
        /// </summary>
        /// <param name="sprite"></param>
        private void UpdateRoleEvent(GSprite sprite)
        {
            if (!EnableChangMap)
            {
                return;
            }
			
			int gridX =  (int)((sprite.PosX / CurrentMapData.GridSizeX));
            int gridY = (int)((sprite.PosY / CurrentMapData.GridSizeY));
			if (gridX != sprite.OldGridX || gridY != sprite.OldGridY)
			{				
				sprite.OldGridX = gridX;
                sprite.OldGridY = gridY;
			}
        }
        #endregion
    }
}
