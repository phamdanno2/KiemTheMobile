﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSPlay.GameEngine.Logic
{
    /// <summary>
    /// Loại đối tượng
    /// </summary>
    public enum GSpriteTypes
    {
        /// <summary>
        /// Không có
        /// </summary>
        None = -1,
        /// <summary>
        /// Leader
        /// </summary>
        Leader = 0,
        /// <summary>
        /// Người chơi khác
        /// </summary>
        Other,
        /// <summary>
        /// Quái
        /// </summary>
        Monster,
        /// <summary>
        /// NPC
        /// </summary>
        NPC,
        /// <summary>
        /// Bot
        /// </summary>
        FakeRole,
        /// <summary>
        /// Điểm thu thập
        /// </summary>
        GrowPoint,
        /// <summary>
        /// Khu vực động
        /// </summary>
        DynamicArea,
        /// <summary>
        /// Cổng dịch chuyển
        /// </summary>
        Teleport,
        /// <summary>
        /// Vật phẩm rơi
        /// </summary>
        GoodsPack,
        /// <summary>
        /// Bot
        /// </summary>
        Bot,
    }


    /// <summary>
    /// Loại thao tác với vật phẩm
    /// </summary>
    public enum ModGoodsTypes
    {
        /// <summary>
        /// Vứt bỏ
        /// </summary>
        Abandon = 0,
        /// <summary>
        /// Trang bị lên người
        /// </summary>
        EquipLoad = 1,
        /// <summary>
        /// Tháo trang bị
        /// </summary>
        EquipUnload = 2,
        /// <summary>
        /// Thay đổi số lượng
        /// </summary>
        ModValue = 3,
        /// <summary>
        /// Xóa vật phẩm
        /// </summary>
        Destroy = 4,
        /// <summary>
        /// Bán cho NPC
        /// </summary>
        SaleToNpc = 5,
        /// <summary>
        /// Tách
        /// </summary>
        SplitItem = 6,
    }

    /// <summary>
    /// Kiểu Nhiệm Vụ
    /// </summary>
    public enum TaskTypes
    {
        /// <summary>
        /// Không có
        /// </summary>
        None = -1,
        /// <summary>
        /// Nói chuyện với NPC
        /// </summary>
        Talk = 0,
        /// <summary>
        /// Giết quái
        /// </summary>
        KillMonster = 1,
        /// <summary>
        /// Giết quái và nhặt vật phẩm
        /// </summary>
        MonsterSomething = 2,
        /// <summary>
        /// Mua vật phẩm từ SHOP
        /// </summary>
        BuySomething = 3,
        /// <summary>
        /// Sử dụng vật phẩm
        /// </summary>
        UseSomething = 4,
        /// <summary>
        /// Giao vật phẩm cho NPC
        /// </summary>
        TransferSomething = 5,
        /// <summary>
        /// Nhận vật phẩm từ NPC
        /// </summary>
        GetSomething = 6,
        /// <summary>
        /// Nhận tiền từ NPC
        /// </summary>
        Collect = 7,
        /// <summary>
        /// Trả lời câu hỏi
        /// </summary>
        AnswerQuest = 8,
        /// <summary>
        /// Vào phái
        /// </summary>
        JoinFaction = 9,
        /// <summary>
        /// Chế đồ
        /// </summary>
        Crafting = 10,
        /// <summary>
        /// Cường hóa
        /// </summary>
        Enhance = 11,
        /// <summary>
        /// Tham gia hoạt động
        /// </summary>
        JoinEvent = 12,
    };

    /// <summary>
    /// Trạng thái nhiệm vụ của NPC
    /// </summary>
    public enum NPCTaskStates
    {
        /// <summary>
        /// Không có trạng thái
        /// </summary>
        None,
        /// <summary>
        /// Có nhiệm vụ chính tuyến để nhận
        /// </summary>
        ToReceive_MainQuest,
        /// <summary>
        /// Có nhiệm vụ chính tuyến để trả
        /// </summary>
        ToReturn_MainQuest,
        /// <summary>
        /// Có nhiệm vụ phụ để nhận
        /// </summary>
        ToReceive_SubQuest,
        /// <summary>
        /// Có nhiệm vụ phụ để trả
        /// </summary>
        ToReturn_SubQuest,
        /// <summary>
        /// Có nhiệm vụ tuần hoàn để nhận
        /// </summary>
        ToReceive_DailyQuest,
        /// <summary>
        /// Có nhiệm vụ tuần hoàn để trả
        /// </summary>
        ToReturn_DailyQuest,
    };

    /// <summary>
    /// Giao dịch
    /// </summary>
    public enum GoodsExchangeCmds
    {
        /// <summary>
        /// Không có
        /// </summary>
        None = 0,
        /// <summary>
        /// Yêu cầu giao dịch
        /// </summary>
        Request,
        /// <summary>
        /// Từ chối
        /// </summary>
        Refuse,
        /// <summary>
        /// Đồng ý
        /// </summary>
        Agree,
        /// <summary>
        /// Hủy giao dịch
        /// </summary>
        Cancel,
        /// <summary>
        /// Thêm vật phẩm lên phiên
        /// </summary>
        AddGoods,
        /// <summary>
        /// Gỡ bỏ vật phẩm khỏi phiên
        /// </summary>
        RemoveGoods,
        /// <summary>
        /// Thêm bạc vào
        /// </summary>
        UpdateMoney,
        /// <summary>
        /// Thêm đồng vào
        /// </summary>
        UpdateYuanBao,
        /// <summary>
        /// Khóa
        /// </summary>
        Lock,
        /// <summary>
        /// Bỏ Khóa
        /// </summary>
        Unlock,
        /// <summary>
        /// Hoàn Tất Giao dịch
        /// </summary>
        Done,
    }

    /// <summary>
    /// Thực thể mua hàng
    /// </summary>
    public enum GoodsStallCmds
    {
        /// <summary>
        /// Không có
        /// </summary>
        None = 0,
        /// <summary>
        /// Yêu cầu mở cửa hàng
        /// </summary>
        Request,
        /// <summary>
        /// Bắt đầu ngồi bán
        /// </summary>
        Start,
        /// <summary>
        /// Hủy Bán
        /// </summary>
        Cancel,
        /// <summary>
        /// Thêm vật phẩm lên sập hàng
        /// </summary>
        AddGoods,
        /// <summary>
        /// Gỡ bỏ vật phẩm khỏi sập hàng
        /// </summary>
        RemoveGoods,
        /// <summary>
        /// Update lại tên cửa hàng
        /// </summary>
        UpdateMessage,
        /// <summary>
        /// Xem cửa hàng của ai đó
        /// </summary>
        ShowStall,
        /// <summary>
        /// Mua vật phẩm của cửa hàng nào đó
        /// </summary>
        BuyGoods,
    }

    /// <summary>
    /// Loại quái vật
    /// </summary>
    public enum MonsterTypes
    {
        /// <summary>
        /// Quái thường, chỉ khi bị sát thương mới đuổi theo
        /// </summary>
        Normal = 0,

        /// <summary>
        /// Quái tinh anh, Boss, cứ gặp người là đuổi theo
        /// </summary>
        Elite = 1,

        /// <summary>
        /// Thủ lĩnh
        /// </summary>
        Leader = 2,

        /// <summary>
        /// Boss
        /// </summary>
        Boss = 3,

        /// <summary>
        /// Hải tặc
        /// </summary>
        Pirate = 4,

        /// <summary>
        /// Quái chữ đỏ, tự đuổi người
        /// </summary>
        Hater = 5,

        /// <summary>
        /// Quái đặc biệt, tự đuổi người, AI chạy liên tục kể cả khi không có người xung quanh
        /// </summary>
        Special_Normal = 6,

        /// <summary>
        /// Boss đặc biệt, AI chạy liên tục kể cả khi không có người xung quanh
        /// </summary>
        Special_Boss = 7,

        /// <summary>
        /// Quái tĩnh, không có AI
        /// </summary>
        Static = 8,

        /// <summary>
        /// Quái tĩnh, miễn dịch toàn bộ, không có AI
        /// </summary>
        Static_ImmuneAll = 9,

        /// <summary>
        /// NPC di chuyển
        /// </summary>
        DynamicNPC = 10,

        /// <summary>
        /// Tổng số
        /// </summary>
        Total,
    }


    /// <summary>
    /// Loại phúc lợi
    /// </summary>
    public enum ActivityTypes
    {
        /// <summary>
        /// Không có
        /// </summary>
        None = 0,

        /// <summary>
        /// Nạp đầu
        /// </summary>
        InputFirst = 1,

        /// <summary>
        /// Nạp mỗi ngày
        /// </summary>
        MeiRiChongZhiHaoLi = 27,

        /// <summary>
        /// Tích nạp
        /// </summary>
        TotalCharge = 38,
        /// <summary>
        /// Tích tiêu
        /// </summary>
        TotalConsume = 39,

        /// <summary>
        /// Tổng số
        /// </summary>
        MaxVal,
    }

    /// <summary>
    /// Loại đối tượng
    /// </summary>
    public enum ObjectTypes
    {
        /// <summary>
        /// Người chơi
        /// </summary>
        OT_CLIENT = 0,
        /// <summary>
        /// Quái vật
        /// </summary>
        OT_MONSTER = 1,
        /// <summary>
        /// Vật phẩm rơi ở MAP
        /// </summary>
        OT_GOODSPACK = 2,
        /// <summary>
        /// NPC
        /// </summary>
        OT_NPC = 5,
    }

    /// <summary>
    /// Loại kết nối lại
    /// </summary>
    public enum ReConnectType
    {
        /// <summary>
        /// Từ màn hình đăng nhập
        /// </summary>
        LoginWindow = 0,
        /// <summary>
        /// Từ màn hình chọn nhân vật
        /// </summary>
        SelectRoleWindow = 1,
        /// <summary>
        /// Kết nối lại với nhân vật hiện tại
        /// </summary>
        ReconnectCurrentRole = 3,
        /// <summary>
        /// Từ màn hình chọn Server
        /// </summary>
        ServerListWindow = 4,
        /// <summary>
        /// Đổi Server
        /// </summary>
        ChangeServer = 5,
    }
}
