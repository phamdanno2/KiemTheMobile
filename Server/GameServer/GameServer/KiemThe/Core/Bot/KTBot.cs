using GameServer.Interface;
using GameServer.KiemThe.Core.Item;
using GameServer.KiemThe.Entities;
using Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Logic
{
    /// <summary>
    /// Đối tượng BOT
    /// </summary>
    public partial class KTBot : GameObject
    {
        /// <summary>
        /// BaseID của Bot
        /// </summary>
        private const int BaseID = 0x7F450000;

        /// <summary>
        /// ID tự động
        /// </summary>
        private static int AutoID = -1;

        /// <summary>
        /// Danh sách trang bị
        /// </summary>
        public List<GoodsData> Equips { get; set; }

        /// <summary>
        /// ID môn phái
        /// </summary>
        public int FactionID { get; set; }

        /// <summary>
        /// ID nhánh
        /// </summary>
        public int RouteID { get; set; }

        /// <summary>
        /// ID Avarta
        /// </summary>
        public int RolePic { get; set; }

        /// <summary>
        /// Giới tính
        /// </summary>
        public int RoleSex { get; set; }

        /// <summary>
        /// ID vũ khí
        /// </summary>
        public int WeaponID
        {
            get
            {
                if (this.Equips == null)
                {
                    return -1;
                }

                GoodsData weapon = this.Equips.Where((itemGD) => {
                    /// Vật phẩm tương ứng
                    if (ItemManager._TotalGameItem.TryGetValue(itemGD.GoodsID, out ItemData itemData))
                    {
                        return itemData.DetailType == (int) KE_ITEM_EQUIP_DETAILTYPE.equip_meleeweapon || itemData.DetailType == (int) KE_ITEM_EQUIP_DETAILTYPE.equip_rangeweapon;
                    }
                    /// Nếu không tồn tại
                    return false;
                }).FirstOrDefault();
                if (weapon == null)
                {
                    return -1;
                }
                else
                {
                    return weapon.GoodsID;
                }
            }
        }

        /// <summary>
        /// Ngũ hành vũ khí
        /// </summary>
        public int WeaponSeries
        {
            get
            {
                if (this.Equips == null)
                {
                    return -1;
                }

                GoodsData weapon = this.Equips.Where((itemGD) => {
                    /// Vật phẩm tương ứng
                    if (ItemManager._TotalGameItem.TryGetValue(itemGD.GoodsID, out ItemData itemData))
                    {
                        return itemData.DetailType == (int) KE_ITEM_EQUIP_DETAILTYPE.equip_meleeweapon || itemData.DetailType == (int) KE_ITEM_EQUIP_DETAILTYPE.equip_rangeweapon;
                    }
                    /// Nếu không tồn tại
                    return false;
                }).FirstOrDefault();
                if (weapon == null)
                {
                    return -1;
                }
                else
                {
                    return weapon.Series;
                }
            }
        }

        /// <summary>
        /// Cấp cường hóa vũ khí
        /// </summary>
        public int WeaponEnhanceLevel
        {
            get
            {
                if (this.Equips == null)
                {
                    return -1;
                }

                GoodsData weapon = this.Equips.Where((itemGD) => {
                    /// Vật phẩm tương ứng
                    if (ItemManager._TotalGameItem.TryGetValue(itemGD.GoodsID, out ItemData itemData))
                    {
                        return itemData.DetailType == (int) KE_ITEM_EQUIP_DETAILTYPE.equip_meleeweapon || itemData.DetailType == (int) KE_ITEM_EQUIP_DETAILTYPE.equip_rangeweapon;
                    }
                    /// Nếu không tồn tại
                    return false;
                }).FirstOrDefault();
                if (weapon == null)
                {
                    return -1;
                }
                else
                {
                    return weapon.Forge_level;
                }
            }
        }

        /// <summary>
        /// ID áo
        /// </summary>
        public int ArmorID
        {
            get
            {
                if (this.Equips == null)
                {
                    return -1;
                }

                GoodsData armor = this.Equips.Where((itemGD) => {
                    /// Vật phẩm tương ứng
                    if (ItemManager._TotalGameItem.TryGetValue(itemGD.GoodsID, out ItemData itemData))
                    {
                        return itemData.DetailType == (int) KE_ITEM_EQUIP_DETAILTYPE.equip_armor;
                    }
                    /// Nếu không tồn tại
                    return false;
                }).FirstOrDefault();
                if (armor == null)
                {
                    return -1;
                }
                else
                {
                    return armor.GoodsID;
                }
            }
        }

        /// <summary>
        /// ID mũ
        /// </summary>
        public int HelmID
        {
            get
            {
                if (this.Equips == null)
                {
                    return -1;
                }

                GoodsData helm = this.Equips.Where((itemGD) => {
                    /// Vật phẩm tương ứng
                    if (ItemManager._TotalGameItem.TryGetValue(itemGD.GoodsID, out ItemData itemData))
                    {
                        return itemData.DetailType == (int) KE_ITEM_EQUIP_DETAILTYPE.equip_helm;
                    }
                    /// Nếu không tồn tại
                    return false;
                }).FirstOrDefault();
                if (helm == null)
                {
                    return -1;
                }
                else
                {
                    return helm.GoodsID;
                }
            }
        }

        /// <summary>
        /// ID ngựa
        /// </summary>
        public int HorseID
        {
            get
            {
                if (this.Equips == null)
                {
                    return -1;
                }

                GoodsData horse = this.Equips.Where((itemGD) => {
                    /// Vật phẩm tương ứng
                    if (ItemManager._TotalGameItem.TryGetValue(itemGD.GoodsID, out ItemData itemData))
                    {
                        return itemData.DetailType == (int) KE_ITEM_EQUIP_DETAILTYPE.equip_horse;
                    }
                    /// Nếu không tồn tại
                    return false;
                }).FirstOrDefault();
                if (horse == null)
                {
                    return -1;
                }
                else
                {
                    return horse.GoodsID;
                }
            }
        }

        /// <summary>
        /// ID phi phong
        /// </summary>
        public int MantleID
        {
            get
            {
                if (this.Equips == null)
                {
                    return -1;
                }

                GoodsData mantle = this.Equips.Where((itemGD) => {
                    /// Vật phẩm tương ứng
                    if (ItemManager._TotalGameItem.TryGetValue(itemGD.GoodsID, out ItemData itemData))
                    {
                        return itemData.DetailType == (int) KE_ITEM_EQUIP_DETAILTYPE.equip_mantle;
                    }
                    /// Nếu không tồn tại
                    return false;
                }).FirstOrDefault();
                if (mantle == null)
                {
                    return -1;
                }
                else
                {
                    return mantle.GoodsID;
                }
            }
        }

        /// <summary>
        /// Có đang trong trạng thái cưỡi không
        /// </summary>
        public bool IsRiding { get; set; }

        /// <summary>
        /// ID Script Lua điều khiển
        /// </summary>
        public int ScriptID { get; set; } = -1;

        /// <summary>
        /// ID Script AI điều khiển
        /// </summary>
        public int AIID { get; set; } = -1;

        /// <summary>
        /// Phạm vi đuổi mục tiêu
        /// </summary>
        public int SeekRange { get; set; } = 1000;

        /// <summary>
        /// Loại đối tượng
        /// </summary>
        public override ObjectTypes ObjectType
        {
            get
            {
                return ObjectTypes.OT_BOT;
            }
        }

        private int _CurrentMapCode = -1;
        /// <summary>
        /// ID bản đồ hiện tại
        /// </summary>
        public new int CurrentMapCode
        {
            get
            {
                return this._CurrentMapCode;
            }
            set
            {
                this._CurrentMapCode = value;
            }
        }

        /// <summary>
        /// Tag của đối tượng
        /// </summary>
        public string Tag { get; set; } = "";

        /// <summary>
        /// Danh sách các biến cục bộ của đối tượng
        /// <para>Sử dụng ở Script Lua</para>
        /// </summary>
        private readonly Dictionary<int, long> LocalVariables = new Dictionary<int, long>();

        /// <summary>
        /// Trả về biến cục bộ tại vị trí tương ứng của đối tượng
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public long GetLocalParam(int idx)
        {
            lock (this.LocalVariables)
            {
                if (this.LocalVariables.TryGetValue(idx, out long value))
                {
                    return value;
                }
                else
                {
                    this.LocalVariables[idx] = 0;
                    return 0;
                }
            }
        }

        /// <summary>
        /// Thiết lập biến cục bộ tại vị trí tương ứng của đối tượng
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void SetLocalParam(int idx, long value)
        {
            lock (this.LocalVariables)
            {
                this.LocalVariables[idx] = value;
            }
        }

        /// <summary>
        /// Xóa rỗng toàn bộ danh sách biến cục bộ
        /// </summary>
        public void RemoveAllLocalParams()
        {
            lock (this.LocalVariables)
            {
                this.LocalVariables.Clear();
            }
        }

        /// <summary>
        /// Số lượng người chơi xung quanh
        /// </summary>
        public int VisibleClientsNum { get; set; }

        /// <summary>
        /// Tạo đối tượng BOT mới
        /// </summary>
        public KTBot() : base()
        {
            KTBot.AutoID = (KTBot.AutoID + 1) % 100000007;
            this.RoleID = KTBot.BaseID + KTBot.AutoID;

            /// Tạo mới Buff
            this.Buffs = new BuffTree(this);
            /// Thực hiện động tác đứng
            this.m_eDoing = KE_NPC_DOING.do_stand;
        }
    }
}
