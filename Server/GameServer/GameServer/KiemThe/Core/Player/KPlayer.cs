using GameServer.Core.Executor;
using GameServer.KiemThe;
using GameServer.KiemThe.Core;
using GameServer.KiemThe.Core.Item;
using GameServer.KiemThe.Entities;
using GameServer.KiemThe.Entities.Player;
using GameServer.KiemThe.Logic;
using GameServer.KiemThe.Utilities;
using GameServer.Logic.RefreshIconState;
using GameServer.Server;
using Server.Data;
using Server.Protocol;
using Server.TCP;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace GameServer.Logic
{
    /// <summary>
    /// Đối tượng người chơi
    /// </summary>
    public partial class KPlayer : GameObject
    {
        /// <summary>
        /// ID tự tăng
        /// </summary>
        private static int AutoID = -1;

        /// <summary>
        /// ID tĩnh (chỉ dùng cho BGWorker trong hệ thống)
        /// </summary>
        public int StaticID { get; private set; }

        /// <summary>
        /// ID người chơi
        /// </summary>
        public override int RoleID
        {
            get
            {
                return this._RoleDataEx.RoleID;
            }
        }

        /// <summary>
        /// Tên nhân vật
        /// </summary>
        public override string RoleName
        {
            get
            {
                string originName = this._RoleDataEx.RoleName;
                if (KTGMCommandManager.IsGM(this))
				{
                    return string.Format("<color=#0486dc>[GM]</color> {0}", originName);
                }
				else
				{
                    return originName;
				}
            }
        }

        /// <summary>
        /// Giới tính của ROLE
        /// </summary>
        public int RoleSex
        {
            get
            {
                return this._RoleDataEx.RoleSex;
            }
        }

        /// <summary>
        /// Kinh nghiệm hiện tại
        /// </summary>
        public long m_nExp { get; set; }

        /// <summary>
        /// Kinh nghiệm cấp tiếp theo
        /// </summary>
        public long m_nNextLevelExp
        {
            get
            {
                return KPlayerSetting.GetLevelExp(this.m_Level);
            }
        }

        /// <summary>
        /// Ngũ hành nhân vật
        /// </summary>
        public override KE_SERIES_TYPE m_Series
        {
            get
            {
                return KFaction.GetSeries(this.m_cPlayerFaction.GetFactionId());
            }
        }

        /// <summary>
        /// Cây kỹ năng
        /// </summary>
        public SkillTree Skills { get; private set; }

        private long _LastTickToggleHorseState = 0;
        /// <summary>
        /// Thời gian lần trước thay đổi trạng thái cưới ngựa
        /// </summary>
        public long LastTickToggleHorseState
        {
            get
            {
                lock (this)
				{
                    return this._LastTickToggleHorseState;
                }
            }
            set
            {
                lock (this)
				{
                    this._LastTickToggleHorseState = value;
                }
            }
        }

        /// <summary>
        /// Có đang trong trạng thái cưỡi không
        /// </summary>
        public bool IsRiding { get; set; }

        /// <summary>
        /// Mục tiêu đang tương tác hiện tại
        /// </summary>
        public GameObject CurrentTarget { get; set; }

        private KPlayer_Progress _CurrentProgress = null;
        /// <summary>
        /// Thao tác đang thực thi
        /// </summary>
        public KPlayer_Progress CurrentProgress
        {
            get
            {
                return this._CurrentProgress;
            }
            set
            {
                /// Nếu có thao tác trước đó nhưng chưa thực thi hoàn tất
                if (this._CurrentProgress != null && !this._CurrentProgress.Completed)
                {
                    /// Thực thi sự kiện hủy bỏ
                    this._CurrentProgress.Cancel?.Invoke();
                    /// Gửi thông báo ngắt Progress về Client
                    KT_TCPHandler.SendStopProgressBar(this);
                }

                /// Đánh dấu thao tác cũ có tồn tại hay không
                bool oldProgressExist = this._CurrentProgress != null;

                /// Thiết lập giá trị mới
                this._CurrentProgress = value;

                /// Nếu là thao tác ngắt
                if (value == null)
                {
                    if (oldProgressExist)
                    {
                        /// Gửi thông báo ngắt Progress về Client
                        KT_TCPHandler.SendStopProgressBar(this);
                    }
                }
                /// Nếu là thao tác thêm mới
                else
                {
                    /// Gửi thông báo chạy Progress về Client
                    KT_TCPHandler.SendStartProgressBar(this, value.Hint, value.DurationTick, 0);
                }
            }
        }

        private KPlayer_Captcha _CurrentCaptcha = null;
        /// <summary>
        /// Captcha hiện tại của nhân vật
        /// </summary>
        public KPlayer_Captcha CurrentCaptcha
        {
            get
            {
                return this._CurrentCaptcha;
            }
            set
            {
                /// Đánh dấu lại
                this._CurrentCaptcha = value;
            }
        }


        /// <summary>
        /// Danh sách vật phẩm đang bán trong cửa hàng của người chơi
        /// </summary>
        private readonly List<GoodsData> OwnShop = new List<GoodsData>();

        /// <summary>
        /// Tạo Captcha tương ứng
        /// </summary>
        public void GenerateCaptcha()
        {
            KTCaptchaManager.Instance.Generate(this);
        }

        /// <summary>
        /// Hủy Captcha tương ứng
        /// </summary>
        public void RemoveCaptcha()
        {
            this._CurrentCaptcha = null;
        }

        /// <summary>
        /// Làm rỗng danh sách vật phẩm đang bán trong cửa hàng của người chơi
        /// </summary>
        public void ClearOwnShop()
        {
            lock (this.OwnShop)
            {
                this.OwnShop.Clear();
            }
        }

        /// <summary>
        /// Thêm vật phẩm vào danh sách bán
        /// </summary>
        /// <param name="Data"></param>
        public void AddItemSellGoodToDic(GoodsData Data)
        {
            lock (this.OwnShop)
            {
                this.OwnShop.Add(Data);
            }
        }

        /// <summary>
        /// Xóa vật phẩm khỏi danh sách bán
        /// </summary>
        /// <param name="Data"></param>
        public void RemoveItemSell(GoodsData Data)
        {
            lock (this.OwnShop)
            {
                this.OwnShop.Remove(Data);
            }
        }

        /// <summary>
        /// Danh sách vật phẩm đã bán trước đó
        /// </summary>
        /// <returns></returns>
        public List<GoodsData> GetGoodAreadySellBefore()
        {
            lock (this.OwnShop)
            {
                return this.OwnShop;
            }
        }

        /// <summary>
        /// Khởi tạo đối tượng
        /// </summary>
        public KPlayer()
        {
            /// Tăng ID tự động
            KPlayer.AutoID = (KPlayer.AutoID + 1) % 10000007;
            /// Thiết lập ID tĩnh
            this.StaticID = KPlayer.AutoID;

            this.m_Kind = NPCKIND.kind_player;

            /// Khởi tạo thực thể quản lý mặc đồ đạc cho nhân vật
            this._KPlayerEquipBody = new KPlayerEquipBody(this);

            /// Đánh dấu thời điểm ra Captcha
            this.NextCaptchaTicks = KTGlobal.GetCurrentTimeMilis() + KTGlobal.GetRandomNumber(ServerConfig.Instance.CaptchaAppearMinPeriod, ServerConfig.Instance.CaptchaAppearMaxPeriod);

            /// Khởi tạo Cheat Detectors
            this.InitCheatDetectors();
        }

        /// <summary>
        /// Socket của người chơi
        /// </summary>
        public TMSKSocket ClientSocket
        {
            get;
            set;
        }

        /// <summary>
        /// Thế hệ của thiết bị
        /// </summary>
        public string DeviceGeneration { get; set; } = "";

        /// <summary>
        /// Mẫu thiết bị
        /// </summary>
        public string DeviceModel { get; set; } = "";

        /// <summary>
        /// UserID (dùng để đồng bộ tài khoản)
        /// </summary>
        public string strUserID
        {
            get;
            set;
        }

        /// <summary>
        /// ID thiết bị (UnityEngine.SystemInfo.deviceUniqueIdentifier)
        /// </summary>
        public string deviceID { get; set; }

        /// <summary>
        /// UserName (dùng để đồng bộ tài khoản)
        /// </summary>
        public string strUserName;

        /// <summary>
        /// Quản lý trang bị
        /// </summary>
        public KPlayerEquipBody _KPlayerEquipBody { get; set; }

        /// <summary>
        /// Trả về đối tượng quản lý trang bị
        /// </summary>
        /// <returns></returns>
        public KPlayerEquipBody GetPlayEquipBody()
        {
            return _KPlayerEquipBody;
        }


        /// <summary>
        /// Quản lý trạng thái Icon
        /// </summary>
        public IconStateManager _IconStateMgr { get; set; } = new IconStateManager();


        /// <summary>
        /// Đã Logout chưa
        /// </summary>
        public bool LogoutState { get; set; } = false;

        /// <summary>
        /// ID Server
        /// </summary>
        public int ServerId
        {
            get { return ClientSocket.ServerId; }
        }

        /// <summary>
        /// Đánh dấu lần đầu thực hiện CMD_PLAY_GAME
        /// </summary>
        public bool FirstEnterPlayGameCmd { get; set; } = true;


        #region Thông tin phiên bản
        /// <summary>
        /// Phiên bản App
        /// </summary>
        public int MainExeVer { get; set; } = 0;

        /// <summary>
        /// Phiên bản tài nguyên
        /// </summary>
        public int ResVer { get; set; } = 0;
        #endregion

        #region 活动上下文数据

        /// <summary>
        /// 跨服上下文数据
        /// </summary>
        public object KuaFuContextData = null;

        /// <summary>
        /// 活动场景上下文数据
        /// </summary>
        public object SceneContextData = null;

        /// <summary>
        /// 活动场景上下文数据2
        /// </summary>
        public object SceneContextData2 = null;

        /// <summary>
        /// 场景类型
        /// </summary>
        public int SceneType;

        /// <summary>
        /// 场景对象
        /// </summary>
        public object SceneObject;

        /// <summary>
        /// 场景配置对象
        /// </summary>
        public object SceneInfoObject;

        /// <summary>
        /// 唯一场次ID
        /// </summary>
        public long SceneGameId;

        #endregion 活动上下文数据

        #region 防外挂校验

        public CheckCheat CheckCheatData = new CheckCheat();


        public InterestingData InterestingData = new InterestingData();

        #endregion 防外挂校验


        #region IObject

        /// <summary>
        /// Loại đối tượng
        /// </summary>
        public override ObjectTypes ObjectType
        {
            get { return ObjectTypes.OT_CLIENT; }
        }


        /// <summary>
        /// 最后一次检查gmail的时间
        /// </summary>
        private long _LastCheckGMailTick = TimeUtil.NOW();

        /// <summary>
        /// 最后一次检查gmail的时间
        /// </summary>
        public long LastCheckGMailTick
        {
            get { return _LastCheckGMailTick; }
            set { _LastCheckGMailTick = value; }
        }

        /// <summary>
        /// Tọa độ lưới
        /// </summary>
        public override Point CurrentGrid
        {
            get
            {
                GameMap gameMap = null;
                if (!GameManager.MapMgr.DictMaps.TryGetValue(this.MapCode, out gameMap))
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("KPlayer CurrentGrid Error MapCode={0}", this.MapCode));
                    return new Point(0, 0);
                }

                return new Point((int)(this.PosX / gameMap.MapGridWidth), (int)(this.PosY / gameMap.MapGridHeight));
            }

            set
            {
                GameMap gameMap = GameManager.MapMgr.DictMaps[this.MapCode];
                this.PosX = (int)(value.X * gameMap.MapGridWidth + gameMap.MapGridWidth / 2);
                this.PosY = (int)(value.Y * gameMap.MapGridHeight + gameMap.MapGridHeight / 2);
            }
        }

        /// <summary>
        /// Tọa độ thực
        /// </summary>
        public override Point CurrentPos
        {
            get
            {
                return new Point(this.PosX, this.PosY);
            }

            set
            {
                //if (value.X == 0 || value.Y == 0)
                //{
                //    Console.WriteLine(new System.Diagnostics.StackTrace());
                //}

                this.PosX = (int)value.X;
                this.PosY = (int)value.Y;
            }
        }

        /// <summary>
        /// Trả vê vũ khí mà nhân vật đang sử dụng
        /// </summary>
        /// <returns></returns>
        public KE_EQUIP_WEAPON_CATEGORY GetWeaponCategory()
        {
            /// Nếu danh sách vật phẩm rỗng
            if (this.GoodsDataList == null)
            {
                return KE_EQUIP_WEAPON_CATEGORY.emKEQUIP_WEAPON_CATEGORY_HAND;
            }

            /// Vũ khí đang sử dụng
            GoodsData weaponGD = this.GoodsDataList.Where(x => x.Using == (int) KE_EQUIP_POSITION.emEQUIPPOS_WEAPON).FirstOrDefault();
            /// Nếu không có vũ khí
            if (weaponGD == null)
            {
                return KE_EQUIP_WEAPON_CATEGORY.emKEQUIP_WEAPON_CATEGORY_HAND;
            }
            /// Nếu có vũ khí
            else
            {
                /// Thông tin trang bị tương ứng
                if (ItemManager._TotalGameItem.TryGetValue(weaponGD.GoodsID, out ItemData itemData))
                {
                    return (KE_EQUIP_WEAPON_CATEGORY) itemData.Category;
                }
                /// Nếu không có thông tin
                else
                {
                    return KE_EQUIP_WEAPON_CATEGORY.emKEQUIP_WEAPON_CATEGORY_HAND;
                }
            }
        }

        /// <summary>
        /// ID bản đồ hiện tại
        /// </summary>
        public override int CurrentMapCode
        {
            get
            {
                return this.MapCode;
            }
        }

        /// <summary>
        /// ID phụ bản hiện tại
        /// </summary>
        public override int CurrentCopyMapID
        {
            get
            {
                return this.CopyMapID;
            }
            set
            {
                this.CopyMapID = value;
            }
        }

        /// <summary>
        /// Hướng quay hiện tại
        /// </summary>
        public override KiemThe.Entities.Direction CurrentDir
        {
            get
            {
                return (KiemThe.Entities.Direction)this.RoleDirection;
            }

            set
            {
                this.RoleDirection = (int)value;
            }
        }

        #endregion IObject

        #region Cập nhật đối tượng xung quanh
        /// <summary>
        /// Thời điểm lần trước cập nhật tầm nhìn
        /// </summary>
        public long LastUpdateGridTicks { get; set; }

        /// <summary>
        /// Làm rỗng danh sách đối tượng xung quanh
        /// </summary>
        /// <param name="recalcMonsterVisibleNum"></param>
        public void ClearVisibleObjects(bool recalcMonsterVisibleNum)
        {
            if (recalcMonsterVisibleNum)
            {
                List<Object> keysList = this.VisibleGrid9Objects.Keys.ToList<Object>();
                for (int i = 0; i < keysList.Count; i++)
                {
                    Object key = keysList[i];
                    if (key is Monster)
                    {
                        if ((key as Monster).CurrentCopyMapID == this.CopyMapID)
                        {
                            (key as Monster).VisibleClientsNum--;
                        }
                    }
                    if (key is KTBot)
                    {
                        if ((key as KTBot).CurrentCopyMapID == this.CopyMapID)
                        {
                            (key as KTBot).VisibleClientsNum--;
                        }
                    }
                    if (key is KDynamicArea)
                    {
                        if ((key as KDynamicArea).CurrentCopyMapID == this.CopyMapID)
                        {
                            (key as KDynamicArea).VisibleClientsNum--;
                        }
                    }
                }
            }

            this.VisibleGrid9Objects.Clear();
        }
        #endregion

        #region Gửi gói tin

        /// <summary>
        /// Gửi gói tin về cho bản thân
        /// </summary>
        public void SendPacket(int cmdId, string cmdData)
        {
            TCPManager.getInstance().MySocketListener.SendData(ClientSocket, TCPOutPacket.MakeTCPOutPacket(TCPOutPacketPool.getInstance(), cmdData, cmdId));
        }


        /// <summary>
        /// Gửi gói tin về cho bản thân
        /// </summary>
        public void SendPacket<T>(int cmdId, T cmdData)
        {
            TCPManager.getInstance().MySocketListener.SendData(ClientSocket, DataHelper.ObjectToTCPOutPacket<T>(cmdData, TCPOutPacketPool.getInstance(), cmdId));
        }

        /// <summary>
        /// Gửi gói tin về cho bản thân
        /// </summary>
        /// <param name="cmdData"></param>
        /// <param name="pushBack"></param>
        public void SendPacket(TCPOutPacket cmdData, bool pushBack = true)
        {
            TCPManager.getInstance().MySocketListener.SendData(ClientSocket, cmdData, pushBack);
        }

        /// <summary>
        /// Gửi thông tin phiên bản về cho bản thân
        /// </summary>
        public void PushVersion()
        {
            SendPacket((int)TCPGameServerCmds.CMD_SPR_PUSH_VERSION, string.Format("{0}:{1}", this.MainExeVer, this.ResVer));
        }

        #endregion
    }
}