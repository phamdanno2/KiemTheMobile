using FSPlay.Drawing;
using FSPlay.GameEngine.Logic;
using FSPlay.GameEngine.Sprite;
using FSPlay.GameFramework.Logic;
using FSPlay.KiemVu;
using FSPlay.KiemVu.Control.Component;
using FSPlay.KiemVu.Factory;
using Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static FSPlay.KiemVu.Entities.Enum;

namespace FSPlay.GameEngine.Scene
{
    /// <summary>
    /// Quản lý điểm thu thập
    /// </summary>
    public partial class GScene
    {
        #region Hệ thống điểm thu thập
        /// <summary>
        /// Danh sách điểm thu thập
        /// </summary>
        private List<GrowPointObject> waitToBeAddedGrowPoint = new List<GrowPointObject>();

        /// <summary>
        /// Tải điểm thu thập vật
        /// </summary>
        /// <param name="growPointData"></param>
        public void ToLoadGrowPoint(GrowPointObject growPointData)
        {
            this.waitToBeAddedGrowPoint.Add(growPointData);
        }

        /// <summary>
        /// Thêm điểm thu thập vào bản đồ
        /// </summary>
        private void AddListGrowPoint()
        {
            if (this.waitToBeAddedGrowPoint.Count <= 0)
            {
                return;
            }

            GrowPointObject growPointData = this.waitToBeAddedGrowPoint[0];
            this.waitToBeAddedGrowPoint.RemoveAt(0);
            this.AddListGrowPoint(growPointData);
        }

        /// <summary>
        /// Tải danh sách điểm thu thập
        /// </summary>
        /// <param name="growPointData"></param>
        private void AddListGrowPoint(GrowPointObject growPointData)
        {
            /// Tên đối tượng
            string name = string.Format("GrowPoint_{0}", SpriteBaseIds.GrowPointBaseId + growPointData.ID);

            /// Đối tượng cũ
            GSprite sprite = this.FindSprite(name);
            /// Nếu đối tượng có tồn tại
            if (sprite != null)
            {
                /// Xóa đối tượng
                KTGlobal.RemoveObject(sprite, true);
            }

            /// Tải xuống đối tượng
            sprite = this.LoadGrowPoint(growPointData);
        }

        /// <summary>
        /// Tải xuống điểm thu thập
        /// </summary>
        public GSprite LoadGrowPoint(GrowPointObject growPointData)
        {
            string name = string.Format("GrowPoint_{0}", SpriteBaseIds.GrowPointBaseId + growPointData.ID);

            GSprite growPoint = new GSprite();
            growPoint.BaseID = SpriteBaseIds.GrowPointBaseId + growPointData.ID;
            growPoint.SpriteType = GSpriteTypes.GrowPoint;

            this.LoadSprite(
                growPoint,
                SpriteBaseIds.GrowPointBaseId + growPointData.ID,
                name,
                null,
                null,
                null,
                growPointData,
                null,
                Direction.DOWN,
                growPointData.PosX,
                growPointData.PosY
            );

            /// Bắt đầu
            growPoint.Start();


            /// Tìm đối tượng cũ
            GameObject oldObject = KTObjectPoolManager.Instance.FindSpawn(x => x.name == name);
            /// Nếu tồn tại
            if (oldObject != null)
            {
                /// Trả lại Pool
                KTObjectPoolManager.Instance.ReturnToPool(oldObject);
            }

            //Monster growP = Object2DFactory.MakeMonster();
            Monster growP = KTObjectPoolManager.Instance.Instantiate<Monster>("GrowPoint");
            growP.name = name;

            /// Gắn đối tượng tham chiếu
            growP.RefObject = growPoint;

            //growP.gameObject.name = name;
            ColorUtility.TryParseHtmlString("#8fff2e", out Color minimapNameColor);
            growP.MinimapNameColor = minimapNameColor;
            growP.ShowMinimapIcon = true;
            growP.ShowMinimapName = false;
            growP.MinimapIconSize = new Vector2(30, 30);
            ColorUtility.TryParseHtmlString("#8fff2e", out Color nameColor);
            growP.NameColor = nameColor;
            //growP.UIHeaderOffset = new Vector2(10, 100);

            growP.ShowHPBar = false;
            growP.ShowElemental = false;

            /// Res
            growP.StaticID = growPointData.ResID;
            growP.ResID = FSPlay.KiemVu.Loader.Loader.ListMonsters[growPointData.ResID].ResID;
            growP.Direction = Direction.DOWN;
            growP.UpdateData();


            GameObject role2D = growP.gameObject;
            growPoint.Role2D = role2D;
            role2D.transform.localPosition = new Vector2((float) growPointData.PosX, (float) growPointData.PosY);

            /// Thực hiện động tác đứng
            growPoint.DoStand();
            growP.ResumeCurrentAction();

            return growPoint;
        }

        /// <summary>
        /// Xóa điểm thu thập tương ứng khỏi hệ thống
        /// </summary>
        /// <param name="mapCode"></param>
        /// <param name="gpID"></param>
        /// <returns></returns>
        public bool DelGrowPoints(int mapCode, int gpID)
        {
            if (mapCode != Global.Data.RoleData.MapCode)
            {
                return false;
            }

            int roleID = SpriteBaseIds.GrowPointBaseId + gpID;
            GSprite growPoint = this.FindSprite(roleID);
            this.waitToBeAddedGrowPoint.RemoveAll(x => x.ID == gpID);

            if (null != growPoint)
            {
                KTGlobal.RemoveObject(growPoint, true);
            }

            return true;
        }
        #endregion
    }
}
