using System.Linq;
using FSPlay.Drawing;
using FSPlay.GameEngine.Sprite;
using FSPlay.GameEngine.Interface;
using Server.Data;
using FSPlay.KiemVu.Factory;

namespace FSPlay.GameEngine.Scene
{
	/// <summary>
	/// Quản lý các đối tượng trong bản đồ
	/// </summary>
	public partial class GScene
    {
        #region Quản lý tìm kiếm đối tượng

        /// <summary>
        /// Tìm đối tượng theo tên
        /// </summary>
        public IObject FindName(string name)
        {
            return KTObjectsManager.Instance.FindObjects<IObject>(x => x.Name == name).FirstOrDefault();
        }

        /// <summary>
        /// Tìm đối tượng GSprite theo tên
        /// </summary>
        public GSprite FindSprite(string name)
        {
            GSprite sprite = this.FindName(name) as GSprite;
            return sprite;
        }

        /// <summary>
        /// Tìm đối tượng GSprite theo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public GSprite FindSprite(int id)
        {
            GSprite sprite = KTObjectsManager.Instance.FindObject<GSprite>(id);
            return sprite;
        }

        /// <summary>
        /// Thêm đối tượng vào danh sách
        /// </summary>
        private void AddSprite(GSprite sprite)
        {
            this.Add(sprite);
        }

        /// <summary>
        /// Thêm đối tượng vào danh sách
        /// </summary>
        private void Add(IObject obj)
        {
            KTObjectsManager.Instance.AddObject(obj);
        }

        /// <summary>
        /// Xóa đối tượng khỏi danh sách
        /// </summary>
        public static void Remove(IObject obj)
        {
            KTObjectsManager.Instance.RemoveObject(obj);
        }

        #endregion

        #region Quản lý tải
        /// <summary>
        /// Khởi tạo đối tượng
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="roleID"></param>
        /// <param name="systemName"></param>
        /// <param name="rd"></param>
        /// <param name="md"></param>
        /// <param name="npc"></param>
        /// <param name="direction"></param>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        private void LoadSprite(GSprite sprite, int roleID, string systemName, RoleData rd, MonsterData md, NPCRole npc, GrowPointObject gp, DynamicArea dynArea, FSPlay.KiemVu.Entities.Enum.Direction direction, int posX, int posY)
        {
            sprite.RoleID = roleID;
            sprite.Name = systemName;
            sprite.RoleData = rd;
            sprite.MonsterData = md;
            sprite.NPCData = npc;
            sprite.GPData = gp;
            sprite.DynAreaData = dynArea;
            sprite.Direction = direction;
            sprite.Coordinate = new Point(posX, posY);
            this.Add(sprite);
        }

        #endregion
    }
}
