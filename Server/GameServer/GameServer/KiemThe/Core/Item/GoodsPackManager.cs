using GameServer.Core.Executor;
using GameServer.Interface;
using GameServer.KiemThe;
using GameServer.KiemThe.Core.Item;
using GameServer.KiemThe.Logic;
using Server.Data;
using Server.Protocol;
using Server.TCP;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;

namespace GameServer.Logic
{
    /// <summary>
    /// Gói tin định nghĩa gói vật phẩm rơi dưới đất
    /// </summary>
    public class GoodsPackItem : IObject
    {
        /// <summary>
        /// ID tự Sinh
        /// </summary>
        public int AutoID
        {
            get;
            set;
        }

        /// <summary>
        /// ID GoodPacks
        /// </summary>
        public int GoodsPackID
        {
            get;
            set;
        }

        /// <summary>
        /// ID chủ sở hữu // Sẽ có trong trường hợp đánh boos
        /// </summary>
        public int OwnerRoleID
        {
            get;
            set;
        }

        /// <summary>
        /// Tên của chủ sở hữu
        /// </summary>
        public string OwnerRoleName
        {
            get;
            set;
        }

        /// <summary>
        /// 0 : Rơi ra từ quái  | 1 : Rơi ra từ người khác
        /// </summary>
        public int GoodsPackType
        {
            get;
            set;
        }

        /// <summary>
        /// Thời gian gói
        /// </summary>
        public long ProduceTicks
        {
            get;
            set;
        }

        /// <summary>
        /// Lock role ID vào thằng nhặt được
        /// </summary>
        public int LockedRoleID
        {
            get;
            set;
        }

        /// <summary>
        /// Danh sách vật phẩm
        /// </summary>
        public List<int> TeamRoleIDs = null;

        /// <summary>
        /// Team ID nào nhặt
        /// </summary>
        public int TeamID = -1;

        /// <summary>
        /// Thời gian để mở gói
        /// </summary>
        public long OpenPackTicks
        {
            get;
            set;
        }

        /// <summary>
        /// Ghi lại thời gian mwor gói
        /// </summary>
        private Dictionary<int, long> _RolesTicksDict = new Dictionary<int, long>();

        /// <summary>
        /// Ghi lại tích lũy mở gói
        /// </summary>
        public Dictionary<int, long> RolesTicksDict
        {
            get { return _RolesTicksDict; }
        }

        /// <summary>
        /// Vật phẩm tương ứng
        /// </summary>
        public GoodsData ItemInPacker { get; set; } = null;

        /// <summary>
        /// Có phải nhặt từ Boss không
        /// </summary>
        public bool DropFromBoss { get; set; } = false;

        /// <summary>
        /// Vật phẩm rơi ở bản đồ nào
        /// </summary>
        public int MapCode { get; set; } = -1;

        /// <summary>
        /// Tạo độ rơi của gói
        /// </summary>
        public Point FallPoint { get; set; }

        /// <summary>
        /// ID của phụ bản
        /// </summary>
        public int CopyMapID { get; set; } = -1;

        /// <summary>
        /// Thuộc về ai
        /// </summary>
        public int BelongTo { get; set; } = -1;

        /// <summary>
        /// Level đéo biết để làm j-> khả năng là chỉ để cho level nào đó có thể nhặt
        /// </summary>
        public int FallLevel { get; set; } = 0;

        /// <summary>
        /// Đối tượng
        /// </summary>
        public ObjectTypes ObjectType
        {
            get { return ObjectTypes.OT_GOODSPACK; }
        }

        /// <summary>
        /// Lần cuối cùng để bổ sung máu
        /// </summary>
        public long LastLifeMagicTick { get; set; }

        /// <summary>
        /// Lấy ra ô hiện tại của gói
        /// </summary>
        public Point CurrentGrid
        {
            get
            {
                GameMap gameMap = GameManager.MapMgr.DictMaps[this.MapCode];
                return new Point((int)(FallPoint.X / gameMap.MapGridWidth), (int)(FallPoint.Y / gameMap.MapGridHeight));
            }

            set
            {
                GameMap gameMap = GameManager.MapMgr.DictMaps[this.MapCode];
                this.FallPoint = new Point((int)(value.X * gameMap.MapGridWidth + gameMap.MapGridWidth / 2), (int)(value.Y * gameMap.MapGridHeight + gameMap.MapGridHeight / 2));
            }
        }

        /// <summary>
        /// Lấy ra tạo độ hiện tại
        /// </summary>
        public Point CurrentPos
        {
            get
            {
                return this.FallPoint;
            }

            set
            {
                this.FallPoint = value;
            }
        }

        /// <summary>
        /// Lấy ra bản đồ hiện tại
        /// </summary>
        public int CurrentMapCode
        {
            get
            {
                return this.MapCode;
            }
        }

        /// <summary>
        /// ID phụ bản
        /// </summary>
        public int CurrentCopyMapID
        {
            get
            {
                return this.CopyMapID;
            }
        }

        /// <summary>
        /// Hướng object
        /// </summary>
        public KiemThe.Entities.Direction CurrentDir
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Cllass quản lý toàn bộ đồ đạc rơi
    /// </summary>
    public class GoodsPackManager
    {
        /// <summary>
        /// ID khởi tạo ban đầu
        /// </summary>
        private long BaseAutoID = 0;

        /// <summary>
        /// Get ra ID tự động khi rơi ra
        /// </summary>
        /// <returns></returns>
        public int GetNextAutoID()
        {
            return (int)(Interlocked.Increment(ref BaseAutoID) & 0x7fffffff);
        }

        /// <summary>
        /// Danh sách toàn bộ vật phẩm rơi
        /// </summary>
        private Dictionary<int, GoodsPackItem> _GoodsPackDict = new Dictionary<int, GoodsPackItem>();

        public Dictionary<int, GoodsPackItem> GoodsPackDict
        {
            get { return _GoodsPackDict; }
            set { _GoodsPackDict = value; }
        }

        /// <summary>
        /// Chọn ra 1 vị trí trống để cho đồ rơi
        /// </summary>
        /// <param name="objType"></param>
        /// <param name="mapCode"></param>
        /// <param name="dict"></param>
        /// <param name="centerPoint"></param>
        /// <param name="copyMapID"></param>
        /// <param name="attacker"></param>
        /// <returns></returns>
        private Point FindABlankPointEx(ObjectTypes objType, int mapCode, Dictionary<string, bool> dict, Point centerPoint, int copyMapID, IObject attacker)
        {
            GameMap gameMap = GameManager.MapMgr.DictMaps[mapCode];
            MapGrid mapGrid = GameManager.MapGridMgr.DictGrids[mapCode];
            Point fallPoint = new Point(centerPoint.X * gameMap.MapGridWidth + gameMap.MapGridWidth / 2, centerPoint.Y * gameMap.MapGridHeight + gameMap.MapGridHeight / 2);

            int centerCridX = (int)centerPoint.X;
            int centerCridY = (int)centerPoint.Y;
            for (int k = 0; k < Global.GoodsPackFallGridArray_Length; k += 2)
            {
                int newGridX = Global.ClientViewGridArray[k] + centerCridX;
                int newGridY = Global.ClientViewGridArray[k + 1] + centerCridY;

                if (!Global.InOnlyObs(objType, mapCode, newGridX, newGridY))
                {
                    string key = string.Format("{0}_{1}", newGridX, newGridY);
                    if (dict.ContainsKey(key))
                    {
                        continue;
                    }

                    dict[key] = true;
                    return new Point(newGridX * gameMap.MapGridWidth + gameMap.MapGridWidth / 2, newGridY * gameMap.MapGridHeight + gameMap.MapGridHeight / 2);
                }
            }

            if (null != attacker)
            {
                fallPoint = attacker.CurrentPos;
                centerCridX = (int)fallPoint.X;
                centerCridY = (int)fallPoint.Y;
                for (int k = 0; k < Global.GoodsPackFallGridArray_Length; k += 2)
                {
                    int newGridX = Global.ClientViewGridArray[k] + centerCridX;
                    int newGridY = Global.ClientViewGridArray[k + 1] + centerCridY;

                    if (!Global.InOnlyObs(objType, mapCode, newGridX, newGridY))
                    {
                        string key = string.Format("{0}_{1}", newGridX, newGridY);
                        if (dict.ContainsKey(key))
                        {
                            continue;
                        }

                        dict[key] = true;
                        return new Point(newGridX * gameMap.MapGridWidth + gameMap.MapGridWidth / 2, newGridY * gameMap.MapGridHeight + gameMap.MapGridHeight / 2);
                    }
                }
            }

            return fallPoint;
        }

        /// <summary>
        /// Chọn Drop Postion cho gói
        /// </summary>
        /// <param name="index"></param>
        /// <param name="centerPoint"></param>
        /// <returns></returns>
        public Point GetFallGoodsPosition(ObjectTypes objType, int mapCode, Dictionary<string, bool> dict, Point centerPoint, int copyMapID, IObject attacker)
        {
            return FindABlankPointEx(objType, mapCode, dict, centerPoint, copyMapID, attacker);
        }

        /// <summary>
        /// Lấy ra các thành viên trong đội có thể nhặt vật phẩm
        /// </summary>
        /// <param name="goodsPackItem"></param>
        /// <returns></returns>
        public string FormatTeamRoleIDs(GoodsPackItem goodsPackItem)
        {
            string teamRoleIDs = "";
            if (null == goodsPackItem)
            {
                return teamRoleIDs;
            }

            if (null != goodsPackItem.TeamRoleIDs && goodsPackItem.TeamRoleIDs.Count > 0)
            {
                for (int i = 0; i < goodsPackItem.TeamRoleIDs.Count; i++)
                {
                    if (teamRoleIDs.Length > 0)
                    {
                        teamRoleIDs += ",";
                    }

                    teamRoleIDs += goodsPackItem.TeamRoleIDs[i].ToString();
                }
            }

            return teamRoleIDs;
        }

        public void ProcessGoodsPackItem(IObject attacker, IObject obj, GoodsPackItem goodsPackItem, int forceBinding)
        {
            if (null == goodsPackItem) //获取掉路项失败
            {
                return;
            }

            LogManager.WriteLog(LogTypes.Chat, "[" + goodsPackItem.OwnerRoleID + "]DROP ITEM :" + goodsPackItem.ItemInPacker.ItemName);
            GameManager.MapGridMgr.DictGrids[goodsPackItem.MapCode].MoveObject(-1, -1, (int)goodsPackItem.FallPoint.X, (int)goodsPackItem.FallPoint.Y, goodsPackItem);
        }

        /// <summary>
        /// Xóa gói khi chạy quá ranger
        /// </summary>
        /// <param name="client"></param>
        public void SendMySelfGoodsPackItems(SocketListener sl, TCPOutPacketPool pool, KPlayer client, List<Object> objsList)
        {
            if (null == objsList) return;

            GoodsPackItem goodsPackItem = null;

            for (int i = 0; i < objsList.Count && i < 30; i++)
            {
                if (!(objsList[i] is GoodsPackItem))
                {
                    continue;
                }

                goodsPackItem = objsList[i] as GoodsPackItem;

                if (goodsPackItem.ItemInPacker == null)
                {
                    continue;
                }

                GoodsData goodsData = goodsPackItem.ItemInPacker;
                if (null == goodsData)
                {
                    continue;
                }

                // LogManager.WriteLog(LogTypes.Chat, "[" + client.RoleName + "]=============> SEND NOTIFY PACKNAME :" + goodsPackItem.ItemInPacker.ItemName);

                GameManager.ClientMgr.NotifyMySelfNewGoodsPack(sl, pool, client,
                    goodsPackItem.BelongTo <= 0 ? -1 : goodsPackItem.OwnerRoleID,
                    goodsPackItem.AutoID,
                    goodsData,
                    (int)goodsPackItem.FallPoint.X,
                    (int)goodsPackItem.FallPoint.Y,
                    goodsPackItem.ProduceTicks,
                    goodsPackItem.TeamID
                );
            }
        }

        /// <summary>
        /// Notify về client xóa gói vật phẩm
        /// </summary>
        /// <param name="client"></param>
        public void DelMySelfGoodsPackItems(SocketListener sl, TCPOutPacketPool pool, KPlayer client, List<Object> objsList)
        {
            if (null == objsList) return;
            GoodsPackItem goodsPackItem = null;
            for (int i = 0; i < objsList.Count; i++)
            {
                if (!(objsList[i] is GoodsPackItem))
                {
                    continue;
                }

                goodsPackItem = objsList[i] as GoodsPackItem;

                GoodsData _Item = goodsPackItem.ItemInPacker;

                if (!Global.CanAddGoods(client, _Item.GoodsID, _Item.GCount, _Item.Binding))
                {
                    return;
                }

                GameManager.ClientMgr.NotifyMySelfDelGoodsPack(sl, pool, client, goodsPackItem.AutoID);
            }
        }

        /// <summary>
        /// Hàm tick theo thời gian | Để xóa vật phẩm các kiểu khi hết thời gian
        /// </summary>
        /// <param name="client"></param>
        public void ProcessAllGoodsPackItems(SocketListener sl, TCPOutPacketPool pool)
        {
            List<GoodsPackItem> goodsPackItemList = new List<GoodsPackItem>();

            lock (_GoodsPackDict)
            {
                foreach (var val in _GoodsPackDict.Values)
                {
                    goodsPackItemList.Add(val);
                }
            }

            long nowTicks = TimeUtil.NOW();

            GoodsPackItem goodsPackItem = null;

            for (int i = 0; i < goodsPackItemList.Count; i++)
            {
                goodsPackItem = goodsPackItemList[i];

                //Lấy thời gian hiện tại - thời gian tạo ra gói nếu mà còn nhỏ thời gian xóa gói thì tiếp tục
                if (nowTicks - goodsPackItem.ProduceTicks < (KTGlobal.PackDestroyTimeTick * 1000))
                {
                    continue;
                }

                //LOCK LẠI PACK
                lock (_GoodsPackDict) //XÓA ĐI NÀY
                {
                    // Console.WriteLine("AUTO REMOVE PACKER AFTER 60S :" + goodsPackItem.AutoID);
                    _GoodsPackDict.Remove(goodsPackItem.AutoID);

                    GameManager.MapGridMgr.DictGrids[goodsPackItem.MapCode].RemoveObject(goodsPackItem);
                }
            }
        }

        /// <summary>
        /// Kiểm tra xem có thể nhặt được vật phẩm này không
        /// </summary>
        /// <param name="goodsPackItem"></param>
        /// <param name="roleID"></param>
        /// <returns></returns>
        private bool CanOpenGoodsPack(GoodsPackItem goodsPackItem, int roleID)
        {
            if (goodsPackItem.BelongTo <= 0) //Nếu thời gian thuộc về thằng chủ mà âm thì được nhặt ngay
            {
                return true;
            }

            //Nếu hết thời gian nhận chủ thì cho nhặt luôn
            long nowTicks = TimeUtil.NOW();
            if (nowTicks - goodsPackItem.ProduceTicks >= (KTGlobal.GoodsPackOvertimeTick * 1000))
            {
                return true;
            }

            if (goodsPackItem.TeamID != -1)
            {
                KPlayer gc = GameManager.ClientMgr.FindClient(roleID);

                // nếu gói rơi ra thuộc về đội này thì cho nhặt
                if (gc.TeamID == goodsPackItem.TeamID)
                {
                    return true;
                }
            }

            if (goodsPackItem.OwnerRoleID < 0 || goodsPackItem.OwnerRoleID == roleID) //Nếu mà sở hữu thuộc bản thân thì nhặt ngay
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Thực hiện mở khóa gói rơi nếu nó thuộc về ai đó
        /// </summary>
        /// <param name="goodsPackItem"></param>
        /// <param name="client"></param>
        public void UnLockGoodsPackItem(KPlayer client)
        {
            if (null == client.LockedGoodsPackItem)
            {
                return;
            }

            lock (_GoodsPackDict)
            {
                if (_GoodsPackDict.ContainsKey(client.LockedGoodsPackItem.AutoID))
                {
                    client.LockedGoodsPackItem.LockedRoleID = -1;
                }
            }

            client.LockedGoodsPackItem = null;
        }

        /// <summary>
        /// Lấy ra vật phẩm ở trong gói
        /// </summary>
        /// <param name="goodsPackItem"></param>
        public List<GoodsData> GetLeftGoodsDataList(GoodsPackItem goodsPackItem)
        {
            if (goodsPackItem.ItemInPacker == null) return null;

            List<GoodsData> goodsDataList = new List<GoodsData>();

            goodsDataList.Add(goodsPackItem.ItemInPacker);

            return goodsDataList;
        }

        /// <summary>
        /// Xử lý khi click vào PACK
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="autoID"></param>
        public GoodsPackListData ProcessClickOnGoodsPack(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int autoID, out TCPOutPacket tcpOutPacket, int nID, int openState, bool tcpPacketData)
        {
            tcpOutPacket = null;
            int retError = 0;
            long leftTicks = 0;
            long packTicks = -1;

            List<GoodsData> leftGoodsDataList = null;
            GoodsPackItem goodsPackItem = null;
            lock (_GoodsPackDict)
            {
                if (_GoodsPackDict.TryGetValue(autoID, out goodsPackItem))
                {
                    if (openState > 0) //Nếu mà gói này có thể mở
                    {
                        // Nếu có gói này
                        if (goodsPackItem != null)
                        {
                            List<GoodsData> GoodsDataList = null;
                            GoodsDataList = GetLeftGoodsDataList(goodsPackItem);

                            if (GoodsDataList != null)
                            {
                                for (int n = 0; n < GoodsDataList.Count; ++n)
                                {
                                    if (!Global.CanAddGoods(client, GoodsDataList[n].GoodsID, GoodsDataList[n].GCount, GoodsDataList[n].Binding))
                                    {
                                        return null;
                                    }
                                }
                            }
                        }

                        /// Kiểm tra xem có nhặt được vật này không
                        if (CanOpenGoodsPack(goodsPackItem, client.RoleID))
                        {
                            if (-1 == goodsPackItem.LockedRoleID || goodsPackItem.LockedRoleID == client.RoleID)
                            {
                                goodsPackItem.LockedRoleID = client.RoleID;
                                client.LockedGoodsPackItem = goodsPackItem;
                                leftGoodsDataList = GetLeftGoodsDataList(goodsPackItem);

                                goodsPackItem.OpenPackTicks = TimeUtil.NOW();

                                //Keiemr tra xem gói nhặt có thuộc team nào ko
                                if (null != goodsPackItem.TeamRoleIDs)
                                {
                                    long lastOpenPackTicks = 0;
                                    goodsPackItem.RolesTicksDict.TryGetValue(client.RoleID, out lastOpenPackTicks);
                                    packTicks = (15 * 1000) - lastOpenPackTicks;
                                }
                            }
                            else
                            {
                                retError = -3;
                                goodsPackItem = null;
                            }
                        }
                        else
                        {
                            long nowTicks = TimeUtil.NOW();
                            leftTicks = (KTGlobal.GoodsPackOvertimeTick * 1000) - (nowTicks - goodsPackItem.ProduceTicks);

                            if (null != goodsPackItem.TeamRoleIDs && -1 != goodsPackItem.TeamRoleIDs.IndexOf(client.RoleID))
                            {
                                packTicks = -2;
                            }

                            retError = -2;
                            goodsPackItem = null;
                        }
                    }
                    else
                    {
                        long lastOpenPackTicks = 0;
                        goodsPackItem.RolesTicksDict.TryGetValue(client.RoleID, out lastOpenPackTicks);
                        goodsPackItem.RolesTicksDict[client.RoleID] = lastOpenPackTicks + (TimeUtil.NOW() - goodsPackItem.OpenPackTicks);

                        goodsPackItem.LockedRoleID = -1;
                        client.LockedGoodsPackItem = null;
                        goodsPackItem = null;
                    }
                }
                else
                {
                    retError = -1;
                }
            }

            List<GoodsData> goodsDataList = null;
            if (goodsPackItem != null)
            {
                goodsDataList = leftGoodsDataList;
            }

            GoodsPackListData goodsPackListData = new GoodsPackListData()
            {
                AutoID = autoID,
                GoodsDataList = goodsDataList,
                OpenState = openState,
                RetError = retError,
                LeftTicks = leftTicks,
                PackTicks = packTicks,
            };

            if (tcpPacketData)
            {
                tcpOutPacket = DataHelper.ObjectToTCPOutPacket<GoodsPackListData>(goodsPackListData, pool, nID);
            }

            return goodsPackListData;
        }

        /// <summary>
        /// Xử lý sự kiện khi nhặt vật phẩm
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="pool"></param>
        /// <param name="client"></param>
        /// <param name="autoID"></param>
        public void ProcessGetThing(SocketListener sl, TCPOutPacketPool pool, KPlayer client, int autoID, int goodsDbID, out bool bRet)
        {
            bRet = false;

            GoodsPackItem goodsPackItem = null;

            lock (GameManager.GoodsPackMgr.GoodsPackDict)
            {
                if (!_GoodsPackDict.TryGetValue(autoID, out goodsPackItem))
                {
                    PlayerManager.ShowNotification(client, "Vật phẩm không tồn tại");
                    return;
                }
            }

            if (goodsPackItem != null)
            {
                /// Vị trí hiện tại của vật phẩm
                UnityEngine.Vector2 gpPos = new UnityEngine.Vector2((int)goodsPackItem.CurrentPos.X, (int)goodsPackItem.CurrentPos.Y);
                /// Vị trí của người chơi
                UnityEngine.Vector2 playerPos = new UnityEngine.Vector2((int)client.CurrentPos.X, (int)client.CurrentPos.Y);
                /// Khoảng cách
                float distance = UnityEngine.Vector2.Distance(gpPos, playerPos);
                /// Nếu quá xa
                if (distance > 100)
                {
                    PlayerManager.ShowNotification(client, "Khoảng cách quá xa, không thể nhặt vật phẩm!");
                    return;
                }

                if (!CanOpenGoodsPack(goodsPackItem, client.RoleID))
                {
                    PlayerManager.ShowNotification(client, "Vật phẩm này thuộc về người khác");
                    return;
                }

                // Nếu mà có vật phẩm trong gói rơi
                if (goodsPackItem.ItemInPacker != null)
                {
                    if (KTGlobal.IsHaveSpace(1, client))
                    {
                        lock (_GoodsPackDict) // Xóa bỏ khỏi DICT
                        {
                            _GoodsPackDict.Remove(autoID);
                        }

                        bool WriterLogs = false;

                        if (goodsPackItem.DropFromBoss)
                        {
                            WriterLogs = true;
                        }

                        if (ItemManager.CreateItem(pool, client, goodsPackItem.ItemInPacker.GoodsID, goodsPackItem.ItemInPacker.GCount, 0, "DROPMAP", true, goodsPackItem.ItemInPacker.Binding, true, Global.ConstGoodsEndTime, goodsPackItem.ItemInPacker.Props, goodsPackItem.ItemInPacker.Series, goodsPackItem.ItemInPacker.Creator, goodsPackItem.ItemInPacker.Forge_level, 1, WriterLogs))
                        {

                            if (goodsPackItem.GoodsPackType == 1)
                            {
                                ItemData _Template = ItemManager.GetItemTemplate(goodsPackItem.ItemInPacker.GoodsID);
                                if (_Template != null)
                                {
                                    LogManager.WriteLog(LogTypes.Item, "[" + client.strUserID + "][" + client.RoleID + "][" + client.RoleName + "][NHẶT VẬT PHẨM][" + goodsPackItem.ItemInPacker.GoodsID + "][" + _Template.Name + "] từ [" + goodsPackItem.OwnerRoleID + "] [" + goodsPackItem.OwnerRoleName + "]");
                                }
                            }
                            // Xóa bỏ vật phẩm khỏi dữ liệu map rơi
                            GameManager.MapGridMgr.DictGrids[goodsPackItem.MapCode].RemoveObject(goodsPackItem);

                            // Thông báo cho bọn xung quanh xóa vật phẩm này khỏi bản đồ
                            List<Object> objsList = Global.GetAll9Clients(goodsPackItem);

                            GameManager.ClientMgr.NotifyOthersDelGoodsPack(sl, pool, objsList, client.MapCode, autoID, client.RoleID);

                            // Notify về client là nhặt được cái gì
                            GameManager.ClientMgr.NotifySelfGetThing(sl, pool, client, goodsDbID);

                            /// Thực thi hàm Callback khi nhặt vật phẩm
                            client.OnPickUpItem(goodsPackItem);
                        }
                        else
                        {
                            PlayerManager.ShowNotification(client, "Có lỗi khi nhặt vật phẩm");
                        }
                    }
                    else
                    {
                        PlayerManager.ShowNotification(client, "Túi đồ đầy không thể nhặt");
                    }
                }
                else
                {
                    PlayerManager.ShowNotification(client, "Gói rơi không có gì bên trong");
                }
            }
        }

        /// <summary>
        /// Thực hiện xóa BoundMoneypack
        /// </summary>
        /// <param name="goodsPackItem"></param>
        public void ExternalRemoveGoodsPack(GoodsPackItem goodsPackItem)
        {
            GameManager.MapGridMgr.DictGrids[goodsPackItem.MapCode].RemoveObject(goodsPackItem);

            // lấy ra toàn bộ OBJECT trong tầm nhìn
            List<Object> objsList = Global.GetAll9Clients(goodsPackItem);

            //Thực hiện xóa các PACK
            GameManager.ClientMgr.NotifyOthersDelGoodsPack(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, objsList, goodsPackItem.MapCode, goodsPackItem.AutoID, -1);
        }

        /// <summary>
        /// Tìm vật phẩm ở 1 vị trí
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        private GoodsPackItem FindGoodsPackItemByPos(Point grid, KPlayer gameClient)
        {
            MapGrid mapGrid = null;
            if (!GameManager.MapGridMgr.DictGrids.TryGetValue(gameClient.MapCode, out mapGrid))
            {
                return null;
            }

            if (null == mapGrid)
            {
                return null;
            }

            /// 获取指定格子中的对象列表
            List<Object> objsList = mapGrid.FindObjects((int)grid.X, (int)grid.Y);
            if (null != objsList)
            {
                for (int objIndex = 0; objIndex < objsList.Count; objIndex++)
                {
                    if (objsList[objIndex] is GoodsPackItem)
                    {
                        if (gameClient.CopyMapID > 0)
                        {
                            if ((objsList[objIndex] as GoodsPackItem).CopyMapID == gameClient.CopyMapID)
                            {
                                return objsList[objIndex] as GoodsPackItem;
                            }
                        }
                        else
                        {
                            return objsList[objIndex] as GoodsPackItem;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Lấy ra số vật phẩm có thể nhặt được
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        private List<GoodsPackItem> FindGoodsPackItemListByPos(Point grid, int girdNum, KPlayer gameClient)
        {
            MapGrid mapGrid = null;
            if (!GameManager.MapGridMgr.DictGrids.TryGetValue(gameClient.MapCode, out mapGrid))
            {
                return null;
            }

            if (null == mapGrid)
            {
                return null;
            }

            int startGridX = (int)grid.X - girdNum;
            int endGridX = (int)grid.X + girdNum;
            int startGridY = (int)grid.Y - girdNum;
            int endGridY = (int)grid.Y + girdNum;

            startGridX = Global.GMax(startGridX, 0);
            startGridY = Global.GMax(startGridY, 0);
            endGridX = Global.GMin(endGridX, mapGrid.MapGridXNum - 1);
            endGridY = Global.GMin(endGridY, mapGrid.MapGridYNum - 1);

            List<GoodsPackItem> GoodsPackItemList = new List<GoodsPackItem>();

            for (int gridX = startGridX; gridX <= endGridX; gridX++)
            {
                for (int gridY = startGridY; gridY <= endGridY; gridY++)
                {
                    /// Lấy ra toàn bộ số vật phẩm rơi ở ô hiện tại
                    List<Object> objsList = mapGrid.FindGoodsPackItems((int)gridX, (int)gridY);
                    if (null != objsList)
                    {
                        for (int objIndex = 0; objIndex < objsList.Count; objIndex++)
                        {
                            if (objsList[objIndex] is GoodsPackItem)
                            {
                                /// Kiểm tra xem có nhặt được ko
                                if (!CanOpenGoodsPack(objsList[objIndex] as GoodsPackItem, gameClient.RoleID))
                                {
                                    continue; // Nếu ko nhặt được htif thôi
                                }

                                if (gameClient.CopyMapID > 0)
                                {
                                    if ((objsList[objIndex] as GoodsPackItem).CopyMapID == gameClient.CopyMapID)
                                    {
                                        GoodsPackItemList.Add(objsList[objIndex] as GoodsPackItem);
                                    }
                                }
                                else
                                {
                                    GoodsPackItemList.Add(objsList[objIndex] as GoodsPackItem);
                                }
                            }
                        }
                    }
                }
            }

            return GoodsPackItemList;
        }

        /// <summary>
        /// Xử lý sự kiện nhặt khi đi qua ô
        /// </summary>
        /// <param name="client"></param>
        public void ProcessClickGoodsPackWhenMovingToOtherGrid(KPlayer client, int gridNum = 1)
        {
            //Lấy ra danh sách vật phẩm nẳm trên ô
            List<GoodsPackItem> goodsPackItemList = FindGoodsPackItemListByPos(client.CurrentGrid, gridNum, client);

            if (null == goodsPackItemList || goodsPackItemList.Count <= 0)
            {
                return;
            }

            lock (client.PickUpGoodsPackMutex)
            {
                for (int i = 0; i < goodsPackItemList.Count; i++)
                {
                    GoodsPackItem goodsPackItem = goodsPackItemList[i];

                    TCPOutPacket tcpOutPacket = null;

                    try
                    {
                        bool bRet = true;

                        GameManager.GoodsPackMgr.ProcessGetThing(Global._TCPManager.MySocketListener, Global._TCPManager.TcpOutPacketPool, client, goodsPackItem.AutoID, -1, out bRet);

                        if (!bRet)
                            return;

                        UnLockGoodsPackItem(client);
                    }
                    finally
                    {
                    }
                }
            }
        }
    }
}