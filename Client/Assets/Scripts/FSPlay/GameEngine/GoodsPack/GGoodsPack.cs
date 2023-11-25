using FSPlay.Drawing;
using UnityEngine;
using FSPlay.GameEngine.Logic;
using FSPlay.GameEngine.Interface;
using FSPlay.KiemVu.Control.Component;
using FSPlay.KiemVu;
using static FSPlay.KiemVu.Entities.Enum;
using FSPlay.KiemVu.Factory;

namespace FSPlay.GameEngine.GoodsPack
{
    /// <summary>
    /// Đối tượng vật phẩm rơi ở Map
    /// </summary>
    public class GGoodsPack : IObject
    {
        #region Khởi tạo đối tượng

        /// <summary>
        /// Khởi tạo đối tượng
        /// </summary>
        public GGoodsPack()
        {
        }

        #endregion


        #region 2D Objects
        /// <summary>
        /// Component Item
        /// </summary>
        public Item ComponentItem { get; private set; } = null;
        #endregion

        #region Kế thừa IObject
        /// <summary>
        /// BaseID
        /// </summary>
        public int BaseID { get; set; }

        /// <summary>
        /// Tên
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Trạng thái ban đầu
        /// </summary>
        private bool _InitStatus = false;
        /// <summary>
        /// Trạng thái ban đầu
        /// </summary>
        public bool InitStatus
        {
            get { return _InitStatus; }
        }

        /// <summary>
        /// Đối tượng GameObject 2D
        /// </summary>
        public GameObject Role2D { get; set; }

        /// <summary>
        /// Vị trí hiện tại (tọa độ thực)
        /// </summary>
        public Point Coordinate
        {
            get { return new Point(this.PosX, this.PosY); }
            set
            {
                this.PosX = value.X;
                this.PosY = value.Y;

                this.ApplyXYPos();
            }
        }

        private int _PosX = 0;

        /// <summary>
        /// Tọa độ thực X
        /// </summary>
        public int PosX
        {
            get { return this._PosX; }
            set
            {
                this._PosX = value;
                ApplyXYPos();
            }
        }

        private int _PosY = 0;

        /// <summary>
        /// Tọa độ thực Y
        /// </summary>
        public int PosY
        {
            get { return this._PosY; }
            set
            {
                this._PosY = value;
                this.ApplyXYPos();
            }
        }

        /// <summary>
        /// Cập nhật tọa độ XY
        /// </summary>
        private void ApplyXYPos()
        {
            if (null != this.Role2D)
            {
                this.Role2D.transform.localPosition = new Vector3(this._PosX, this._PosY);
            }
        }

        /// <summary>
        /// Trả về tọa độ của đối tượng dưới dạng UnityEngine.Vector2
        /// </summary>
        public Vector2 PositionInVector2
        {
            get
            {
                return new Vector2(this.PosX, this.PosY);
            }
        }

        #endregion

        #region Kế thừa ISprite

        /// <summary>
        /// Loại đối tượng
        /// </summary>
        public GSpriteTypes SpriteType
        {
            get;
            set;
        }

        /// <summary>
        /// Động tác
        /// </summary>
        public KE_NPC_DOING CurrentAction { get; set; } 

        /// <summary>
        /// Hướng quay (8 hướng)
        /// </summary>
        public Direction Direction { get; set; }

        /// <summary>
        /// Tốc chạy
        /// </summary>
        public int MoveSpeed { get; set; }

        #endregion


        #region Kế thừa IObject - Hiển thị

        /// <summary>
        /// Đã bắt đầu chưa
        /// </summary>
        private bool _Started = false;

        /// <summary>
        /// Bắt đầu
        /// </summary>
        public void Start()
        {
            if (this._Started)
            {
                return;
            }

            this._Started = true;
            this._InitStatus = true;

            this.ComponentItem = this.Role2D.GetComponent<Item>();
            this.StartTick = KTGlobal.GetCurrentTimeMilis();

            KTObjectsManager.Instance.AddObject(this);
        }

        /// <summary>
        /// Hủy đối tượng
        /// </summary>
        public void Destroy()
        {
            KTObjectsManager.Instance.RemoveObject(this);

            /// Nếu tồn tại đối tượng
            if (null != Role2D)
            {
                this.ComponentItem.Destroy();
                this.Role2D = null;
            }
        }

        #endregion 

        #region Kế thừa IObject - Render

        /// <summary>
        /// Hàm này gọi liên tục mỗi Frame, tương tự hàm Update
        /// </summary>
        /// <param name="time"></param>
        public void OnFrameRender()
        {
            if (!this._Started)
            {
                return;
            }

            /// Nếu hết thời gian thì tự hủy vật phẩm
            if (this.StartTick > 0 && (KTGlobal.GetServerTime() - this.StartTick) >= GGoodsPack.AutoRemoveTick)
            {
                KTGlobal.RemoveObject(this, true);
                return;
            }
        }

        #endregion

        #region Thuộc tính
        /// <summary>
        /// Tự động xóa sau khoảng
        /// </summary>
        public const long AutoRemoveTick = 60000;

        /// <summary>
        /// Thời điểm được tạo ra
        /// </summary>
        private long StartTick = 0;

        /// <summary>
        /// ID chủ nhân
        /// </summary>
        public int OwnerID { get; set; }

        /// <summary>
        /// ID nhóm chủ nhân
        /// </summary>
        public int TeamID { get; set; }

        /// <summary>
        /// ID tự động
        /// </summary>
        public int AutoID { get; set; }

        /// <summary>
        /// ID vật phẩm tương ứng
        /// </summary>
        public int GoodsID { get; set; }

        /// <summary>
        /// Số sao
        /// </summary>
        public int Stars { get; set; }

        /// <summary>
        /// Cấp cường hóa
        /// </summary>
        public int EnhanceLevel { get; set; }
        #endregion
    }
}
