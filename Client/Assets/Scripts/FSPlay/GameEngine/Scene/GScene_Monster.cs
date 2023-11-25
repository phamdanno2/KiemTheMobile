using System.Collections.Generic;
using UnityEngine;
using FSPlay.GameEngine.Logic;
using FSPlay.GameEngine.Network;
using FSPlay.GameEngine.Sprite;
using Server.Data;
using FSPlay.KiemVu.Factory;
using FSPlay.KiemVu.Control.Component;
using FSPlay.KiemVu;

namespace FSPlay.GameEngine.Scene
{
	/// <summary>
	/// Quản lý quái
	/// </summary>
	public partial class GScene
    {
        #region Hệ thống quái
        /// <summary>
        /// Danh sách quái
        /// </summary>
        private List<BurstMonsterItem> waitToBeAddedMonster = new List<BurstMonsterItem>();

        /// <summary>
        /// Tải quái vật
        /// </summary>
        /// <param name="monsterData"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="direction"></param>
        public void ToLoadMonster(MonsterData monsterData, int x, int y, int direction)
        {
            BurstMonsterItem burstMonsterItem = new BurstMonsterItem()
            {
                MonsterData = monsterData,
                X = x,
                Y = y,
                Direction = direction,
            };

            this.waitToBeAddedMonster.Add(burstMonsterItem);
        }

        /// <summary>
        /// Thêm quái vào bản đồ
        /// </summary>
        private void AddListMonster()
        {
            if (this.waitToBeAddedMonster.Count <= 0)
            {
                return;
            }

            BurstMonsterItem burstMonsterItem = this.waitToBeAddedMonster[0];
            this.waitToBeAddedMonster.RemoveAt(0);
            this.AddListMonster(burstMonsterItem);
        }

        /// <summary>
        /// Tải danh sách quái
        /// </summary>
        /// <param name="burstMonsterItem"></param>
        /// <param name="addToCanvas"></param>
        private void AddListMonster(BurstMonsterItem burstMonsterItem)
        {
            /// Tên đối tượng
            string name = string.Format("Role_{0}", burstMonsterItem.MonsterData.RoleID);

            /// Nếu đối tượng không tồn tại thì bỏ qua
            if (!Global.Data.SystemMonsters.TryGetValue(burstMonsterItem.MonsterData.RoleID, out _))
            {
                return;
            }

            /// Đối tượng cũ
            GSprite sprite = this.FindSprite(name);
            /// Nếu đối tượng có tồn tại
            if (sprite != null)
            {
                /// Xóa đối tượng
                KTGlobal.RemoveObject(sprite, true);
            }

            /// Tải xuống đối tượng
            sprite = this.LoadMonster(burstMonsterItem.MonsterData, burstMonsterItem.X, burstMonsterItem.Y, burstMonsterItem.Direction);
            /// Thực hiện gửi gói tin tải xuống hoàn tất
            GameInstance.Game.SpriteLoadAlready(sprite.RoleID);
        }

        /// <summary>
        /// Tải xuống quái
        /// </summary>
        public GSprite LoadMonster(MonsterData monsterData, int x, int y, int direction)
        {
            string name = string.Format("Role_{0}", monsterData.RoleID);

            GSprite monster = new GSprite();
            monster.BaseID = monsterData.RoleID;
            monster.SpriteType = GSpriteTypes.Monster;

            monster.CoordinateChanged += (sender) =>
            {
                this.UpdateMonsterEvent(sender);
            };

            this.LoadSprite(
                monster,
                monsterData.RoleID,
                name,
                null,
                monsterData,
                null,
                null,
                null,
                (KiemVu.Entities.Enum.Direction) direction,
                x,
                y
            );

            /// Bắt đầu
            monster.Start();


            /// Tìm đối tượng cũ
            GameObject oldObject = KTObjectPoolManager.Instance.FindSpawn(x => x.name == name);
            /// Nếu tồn tại
            if (oldObject != null)
            {
                /// Trả lại Pool
                KTObjectPoolManager.Instance.ReturnToPool(oldObject);
            }

            //Monster mons = Object2DFactory.MakeMonster();
            Monster mons = KTObjectPoolManager.Instance.Instantiate<Monster>("Monster");
            mons.name = name;

            /// Gắn đối tượng tham chiếu
            mons.RefObject = monster;

            //mons.gameObject.name = name;
            Color minimapNameColor;
            /// Nếu là NPC di động
            if (monsterData.MonsterType == (int) MonsterTypes.DynamicNPC)
            {
                ColorUtility.TryParseHtmlString("#f5ff2e", out minimapNameColor);
                mons.ShowMinimapName = true;
            }
            /// Nếu là quái hoặc Boss
            else
            {
                ColorUtility.TryParseHtmlString("#ee2eff", out minimapNameColor);
                mons.ShowMinimapName = false;
            }
            mons.MinimapNameColor = minimapNameColor;
            mons.ShowMinimapIcon = true;
            Color nameColor;
            /// Nếu là NPC di động
            if (monsterData.MonsterType == (int) MonsterTypes.DynamicNPC)
            {
                ColorUtility.TryParseHtmlString("#f5ff2e", out nameColor);
                mons.ShowHPBar = false;
                mons.ShowElemental = false;
                mons.MinimapIconSize = new Vector2(100, 100);
            }
            /// Nếu là quái thường
            else
            {
                ColorUtility.TryParseHtmlString(monsterData.MonsterType == (int) MonsterTypes.Normal || monsterData.MonsterType == (int) MonsterTypes.Hater || monsterData.MonsterType == (int) MonsterTypes.Special_Normal || monsterData.MonsterType == (int) MonsterTypes.Static || monsterData.MonsterType == (int) MonsterTypes.Static_ImmuneAll ? "#ffffff" : "#ae52ff", out nameColor);
                mons.ShowHPBar = true;
                mons.ShowElemental = true;
                mons.MinimapIconSize = new Vector2(30, 30);
            }
            mons.NameColor = nameColor;
            //mons.UIHeaderOffset = new Vector2(10, 0);


            /// Res
            mons.StaticID = monsterData.ExtensionID;
            mons.ResID = FSPlay.KiemVu.Loader.Loader.ListMonsters[monsterData.ExtensionID].ResID;
            mons.Direction = (KiemVu.Entities.Enum.Direction)  direction;
            mons.UpdateData();

            GameObject role2D = mons.gameObject;
            monster.Role2D = role2D;
            role2D.transform.localPosition = new Vector2((float)x, (float)y);

            /// Thực hiện Recover lại các hiệu ứng có trên người
            if (monsterData.ListBuffs != null)
            {
                foreach (BufferData buff in monsterData.ListBuffs)
                {
                    if (KiemVu.Loader.Loader.Skills.TryGetValue(buff.BufferID, out KiemVu.Entities.Config.SkillDataEx skillData))
                    {
                        monster.AddBuff(skillData.StateEffectID, buff.BufferSecs == -1 ? -1 : buff.BufferSecs - (KTGlobal.GetServerTime() - buff.StartTime));
                    }
                }
            }
            /// End

            /// Cập nhật hiển thị thanh máu
            this.RefreshSpriteLife(monster);

            /// Thực hiện động tác đứng
            monster.DoStand(true);
            mons.ResumeCurrentAction();

            /// Thiết lập các hiệu ứng đặc biệt
            KTGlobal.PlayMonsterSpecialEffect(monster, monsterData.MonsterType);

            return monster;
        }

        /// <summary>
        /// Cập nhật sự kiện quái
        /// </summary>
        /// <param name="sprite"></param>
        private void UpdateMonsterEvent(GSprite sprite)
        {
            if (!this.EnableChangMap)
            {
                return;
            }

            int gridX = (int)((sprite.PosX / CurrentMapData.GridSizeX));
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
