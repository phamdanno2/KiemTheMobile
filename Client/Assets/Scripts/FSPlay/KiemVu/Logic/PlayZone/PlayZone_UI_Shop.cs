using FSPlay.GameEngine.Logic;
using FSPlay.GameEngine.Network;
using FSPlay.KiemVu;
#if !UNITY_IOS
using FSPlay.KiemVu.Factory.InAppPurchase;
#endif
using FSPlay.KiemVu.UI;
using FSPlay.KiemVu.UI.Main;
using Server.Data;
using UnityEngine;

/// <summary>
/// Quản lý các khung giao diện trong màn chơi
/// </summary>
public partial class PlayZone
{
#region UI Shop
    /// <summary>
    /// Của hàng từ NPC
    /// </summary>
    protected UIShop UIShop { get; set; } = null;

    /// <summary>
    /// Hiển thị khung cửa hàng từ NPC
    /// </summary>
    /// <param name="shopTab"></param>
    public void OpenUIShop(ShopTab shopTab)
    {
        /// Nếu đã tồn tại khung thì thôi
        if (this.UIShop != null)
        {
            return;
        }

        CanvasManager canvas = Global.MainCanvas.GetComponent<CanvasManager>();
        this.UIShop = canvas.LoadUIPrefab<UIShop>("MainGame/UIShop");
        canvas.AddUI(this.UIShop);

        this.UIShop.Data = shopTab;
        this.UIShop.Buy = (shopItemData, quantity) => {
            GameInstance.Game.SpriteBuyGoods(shopItemData.ID, quantity, shopTab.ShopID, -1);
        };
        this.UIShop.BuyBack = (itemGD) => {
            GameInstance.Game.SpriteBuyGoods(itemGD.Id, itemGD.GCount, 9999, -1);
        };
        this.UIShop.Sell = (itemGD) => {
            GameInstance.Game.SpriteBuyOutGoods(itemGD.Id);
            KTGlobal.CloseItemInfo();
        };
        this.UIShop.Close = this.CloseUIShop;
    }

    /// <summary>
    /// Đóng khung cửa hàng từ NPC
    /// </summary>
    public void CloseUIShop()
    {
        /// Nếu khung tồn tại
        if (this.UIShop != null)
        {
            GameObject.Destroy(this.UIShop.gameObject);
            this.UIShop = null;
        }
    }
#endregion

#region Kỳ Trân Các
    /// <summary>
    /// Khung Kỳ Trân Các
    /// </summary>
    public UITokenShop UITokenShop { get; protected set; }

    /// <summary>
    /// Mở khung Kỳ Trân Các
    /// </summary>
    /// <param name="data"></param>
    public void OpenTokenShop(TokenShop data)
    {
        /// Nếu đã tồn tại khung thì thôi
        if (this.UITokenShop != null)
        {
            return;
        }

        CanvasManager canvas = Global.MainCanvas.GetComponent<CanvasManager>();
        this.UITokenShop = canvas.LoadUIPrefab<UITokenShop>("MainGame/UITokenShop");
        canvas.AddUI(this.UITokenShop);

        /// Tham chiếu shop tương ứng từ vật phẩm
        foreach (ShopTab shopTab in data.Token)
        {
            foreach (ShopItem shopItem in shopTab.Items)
            {
                shopItem.ShopTab = shopTab;
            }
        }
        foreach (ShopTab shopTab in data.BoundToken)
        {
            foreach (ShopItem shopItem in shopTab.Items)
            {
                shopItem.ShopTab = shopTab;
            }
        }

        this.UITokenShop.Data = data;
        this.UITokenShop.Buy = (shopItemData, quantity, shopID, couponID) => {
            GameInstance.Game.SpriteBuyGoods(shopItemData.ID, quantity, shopID, couponID);
        };
        this.UITokenShop.StoreBuyItem = (storeItemData) => {
#if !UNITY_IOS
            /// Thực hiện mua hàng -  Test
            IAPManager.Instance.BuyProductID(storeItemData.ID);
#endif
        };
        this.UITokenShop.Close = this.CloseTokenShop;
    }

    /// <summary>
    /// Đóng khung Kỳ Trân Các
    /// </summary>
    public void CloseTokenShop()
    {
        /// Nếu khung tồn tại
        if (this.UITokenShop != null)
        {
            GameObject.Destroy(this.UITokenShop.gameObject);
            this.UITokenShop = null;
        }
    }
#endregion

#region Cửa hàng của người chơi
#region Yêu cầu mở gian hàng
    /// <summary>
    /// Yêu cầu mở gian hàng của người chơi tương ứng
    /// </summary>
    /// <param name="roleID"></param>
    public void RequestOpenTargetShop(int roleID)
    {
        /// Nếu không tìm thấy người chơi
        if (!Global.Data.OtherRoles.TryGetValue(roleID, out _))
        {
            KTGlobal.AddNotification("Không tìm thấy người chơi tương ứng!");
            return;
        }

        /// Gửi gói tin mở cửa hàng của người chơi khác về GS
        GameInstance.Game.SpriteGoodsInstall((int) GoodsStallCmds.ShowStall, roleID, "");
    }

    /// <summary>
    /// Yêu cầu mở gian hàng của bản thân
    /// </summary>
    public void RequestOpenMyShop()
    {
        /// Nếu không tồn tại gian hàng
        if (Global.Data.StallDataItem == null)
        {
            KTGlobal.AddNotification("Mở gian hàng thất bại!");
            return;
        }
        /// Nếu đang cưỡi thì thông báo
        else if (Global.Data.RoleData.IsRiding)
        {
            KTGlobal.AddNotification("Trong trạng thái cưỡi không thể mở sạp hàng!");
            return;
        }

        /// Gửi gói tin mở cửa hàng của người chơi khác về GS
        GameInstance.Game.SpriteGoodsInstall((int) GoodsStallCmds.Start, -1, "");
    }
#endregion

#region Thiết lập cửa hàng
    /// <summary>
    /// Khung thiết lập cửa hàng
    /// </summary>
    public UIPlayerShop_SetShopName UIPlayerShop_SetShopName { get; protected set; } = null;

    /// <summary>
    /// Hiển thị khung thiết lập cửa hàng
    /// </summary>
    public void ShowUIPlayerShop_SetShopName()
    {
        if (this.UIPlayerShop_SetShopName != null)
        {
            return;
        }

        CanvasManager canvas = Global.MainCanvas.gameObject.GetComponent<CanvasManager>();
        this.UIPlayerShop_SetShopName = canvas.LoadUIPrefab<UIPlayerShop_SetShopName>("MainGame/PlayerShop/UIPlayerShop_SetShopName");
        canvas.AddUI(this.UIPlayerShop_SetShopName);

        this.UIPlayerShop_SetShopName.Close = () => {
            /// Gửi gói tin đóng cửa hàng về GS
            GameInstance.Game.SpriteGoodsInstall((int) GoodsStallCmds.Cancel, -1, "");

            /// Đóng khung
            this.CloseUIPlayerShop_SetShopName();
        };
        this.UIPlayerShop_SetShopName.Accept = (shopName) => {
            /// Nếu chưa có tên
            if (string.IsNullOrEmpty(shopName))
            {
                return;
            }

            /// Gửi gói tin đặt tên cửa hàng về GS
            GameInstance.Game.SpriteGoodsInstall((int) GoodsStallCmds.UpdateMessage, -1, shopName);

            /// Đóng khung
            this.CloseUIPlayerShop_SetShopName();
        };
    }

    /// <summary>
    /// Đóng khung thiết lập cửa hàng
    /// </summary>
    public void CloseUIPlayerShop_SetShopName()
    {
        if (this.UIPlayerShop_SetShopName != null)
        {
            GameObject.Destroy(this.UIPlayerShop_SetShopName.gameObject);
            this.UIPlayerShop_SetShopName = null;
        }
    }
#endregion

#region Bán
    /// <summary>
    /// Khung cửa hàng của người bán
    /// </summary>
    public UIPlayerShop_Sell UIPlayerShop_Sell { get; protected set; } = null;

    /// <summary>
    /// Hiển thị khung cửa hàng của người bán
    /// </summary>
    public void ShowUIPlayerShop_Sell()
    {
        if (this.UIPlayerShop_Sell != null)
        {
            return;
        }

        CanvasManager canvas = Global.MainCanvas.gameObject.GetComponent<CanvasManager>();
        this.UIPlayerShop_Sell = canvas.LoadUIPrefab<UIPlayerShop_Sell>("MainGame/PlayerShop/UIPlayerShop_Sell");
        canvas.AddUI(this.UIPlayerShop_Sell);

        this.UIPlayerShop_Sell.Close = () => {
            KTGlobal.ShowMessageBox("Đóng cửa hàng", "Xác nhận đóng cửa hàng?", () => {
                /// Gửi gói tin đóng cửa hàng về GS
                GameInstance.Game.SpriteGoodsInstall((int) GoodsStallCmds.Cancel, -1, "");

                /// Đóng khung
                this.CloseUIPlayerShop_Sell();
            }, true);
        };
        this.UIPlayerShop_Sell.AddItemToSell = (itemGD, price) => {
            /// Nếu không có vật phẩm hoặc giá cả <= 0
            if (itemGD == null || price <= 0)
            {
                return;
            }

            /// Gửi gói tin thêm vật phẩm vào cửa hàng về GS
            GameInstance.Game.SpriteGoodsInstall((int) GoodsStallCmds.AddGoods, itemGD.Id, price.ToString());
        };
        this.UIPlayerShop_Sell.RemoveItemFromSell = (itemGD) => {
            /// Nếu không có vật phẩm
            if (itemGD == null)
            {
                return;
            }

            /// Gửi gói tin xóa vật phẩm khỏi cửa hàng về GS
            GameInstance.Game.SpriteGoodsInstall((int) GoodsStallCmds.RemoveGoods, itemGD.Id, "");
        };
    }

    /// <summary>
    /// Đóng khung cửa hàng của người bán
    /// </summary>
    public void CloseUIPlayerShop_Sell()
    {
        if (this.UIPlayerShop_Sell != null)
        {
            GameObject.Destroy(this.UIPlayerShop_Sell.gameObject);
            this.UIPlayerShop_Sell = null;
        }
    }
#endregion

#region Mua
    /// <summary>
    /// Khung cửa hàng của người bán
    /// </summary>
    public UIPlayerShop_Buy UIPlayerShop_Buy { get; protected set; } = null;

    /// <summary>
    /// Hiển thị khung cửa hàng của người bán
    /// </summary>
    public void ShowUIPlayerShop_Buy()
    {
        if (this.UIPlayerShop_Buy != null)
        {
            return;
        }

        CanvasManager canvas = Global.MainCanvas.gameObject.GetComponent<CanvasManager>();
        this.UIPlayerShop_Buy = canvas.LoadUIPrefab<UIPlayerShop_Buy>("MainGame/PlayerShop/UIPlayerShop_Buy");
        canvas.AddUI(this.UIPlayerShop_Buy);

        this.UIPlayerShop_Buy.Close = () => {
            this.CloseUIPlayerShop_Buy();
            Global.Data.OtherStallDataItem = null;
        };
        this.UIPlayerShop_Buy.Buy = (itemGD) => {
            /// Nếu không có dữ liệu gian hàng
            if (Global.Data.OtherStallDataItem == null)
            {
                return;
            }

            /// Gửi gói tin người chơi mua vật phẩm lên GS
            GameInstance.Game.SpriteGoodsInstall((int) GoodsStallCmds.BuyGoods, Global.Data.OtherStallDataItem.RoleID, itemGD.Id.ToString());
        };
    }

    /// <summary>
    /// Đóng khung cửa hàng của người bán
    /// </summary>
    public void CloseUIPlayerShop_Buy()
    {
        if (this.UIPlayerShop_Buy != null)
        {
            GameObject.Destroy(this.UIPlayerShop_Buy.gameObject);
            this.UIPlayerShop_Buy = null;
        }
    }
#endregion
#endregion
}
