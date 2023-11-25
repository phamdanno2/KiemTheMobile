using FSPlay.GameEngine.Logic;
using FSPlay.GameEngine.Sprite;
using FSPlay.KiemVu.Factory;
using FSPlay.KiemVu.Factory.UIManager;
using FSPlay.KiemVu.UI;
using FSPlay.KiemVu.UI.CoreUI;
using FSPlay.KiemVu.UI.Main;
using FSPlay.KiemVu.UI.Main.InputBox;
using FSPlay.KiemVu.UI.Main.ItemSlotBox;
using FSPlay.KiemVu.UI.Main.MessageBox;
using Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FSPlay.KiemVu
{
    /// <summary>
    /// Các hàm toàn cục dùng trong Kiếm Thế
    /// </summary>
    public static partial class KTGlobal
    {
        #region Game Notification
        /// <summary>
        /// Hiển thị thông báo trong Game
        /// </summary>
        /// <param name="msg"></param>
        public static void AddNotification(string msg)
        {
            CanvasManager canvas = Global.MainCanvas.GetComponent<CanvasManager>();
            canvas.UINotificationTip.AddText(msg);
        }
        #endregion

        #region System Notification
        /// <summary>
        /// Hiển thị dòng chữ chạy hệ thống
        /// </summary>
        /// <param name="msg"></param>
        public static void AddSystemMessage(string msg)
        {
            CanvasManager canvas = Global.MainCanvas.GetComponent<CanvasManager>();
            canvas.UISystemNotification.AddMessage(msg);
        }
        #endregion

        #region InputNumber
        /// <summary>
        /// Hiển thị khung nhập số
        /// </summary>
        /// <param name="text"></param>
        /// <param name="onOK"></param>
        public static void ShowInputNumber(string text, Action<int> onOK)
        {
            CanvasManager canvas = Global.MainCanvas.GetComponent<CanvasManager>();
            UIInputNumber uiInputNumber = canvas.LoadUIPrefab<UIInputNumber>("MainGame/InputBox/UIInputNumber");
            canvas.AddUI(uiInputNumber, true);

            uiInputNumber.Text = text;
            uiInputNumber.OK = onOK;
        }

        /// <summary>
        /// Hiển thị khung nhập số
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="text"></param>
        /// <param name="onOK"></param>
        public static void ShowInputNumber(string text, int minValue, int maxValue, Action<int> onOK)
        {
            CanvasManager canvas = Global.MainCanvas.GetComponent<CanvasManager>();
            UIInputNumber uiInputNumber = canvas.LoadUIPrefab<UIInputNumber>("MainGame/InputBox/UIInputNumber");
            canvas.AddUI(uiInputNumber, true);

            uiInputNumber.MinValue = minValue;
            uiInputNumber.MaxValue = maxValue;
            uiInputNumber.Text = text;
            uiInputNumber.OK = onOK;
        }

        /// <summary>
        /// Hiển thị khung nhập số
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="initValue"></param>
        /// <param name="text"></param>
        /// <param name="onOK"></param>
        public static void ShowInputNumber(string text, int minValue, int maxValue, int initValue, Action<int> onOK)
        {
            CanvasManager canvas = Global.MainCanvas.GetComponent<CanvasManager>();
            UIInputNumber uiInputNumber = canvas.LoadUIPrefab<UIInputNumber>("MainGame/InputBox/UIInputNumber");
            canvas.AddUI(uiInputNumber, true);

            uiInputNumber.MinValue = minValue;
            uiInputNumber.MaxValue = maxValue;
            uiInputNumber.InitValue = initValue;
            uiInputNumber.Text = text;
            uiInputNumber.OK = onOK;
        }


        /// <summary>
        /// Hiển thị khung nhập số
        /// </summary>
        /// <param name="text"></param>
        /// <param name="onOK"></param>
        /// <param name="onCancel"></param>
        public static void ShowInputNumber(string text, Action<int> onOK, Action onCancel)
        {
            CanvasManager canvas = Global.MainCanvas.GetComponent<CanvasManager>();
            UIInputNumber uiInputNumber = canvas.LoadUIPrefab<UIInputNumber>("MainGame/InputBox/UIInputNumber");
            canvas.AddUI(uiInputNumber, true);

            uiInputNumber.Text = text;
            uiInputNumber.OK = onOK;
            uiInputNumber.Cancel = onCancel;
        }

        /// <summary>
        /// Hiển thị khung nhập số
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="text"></param>
        /// <param name="onOK"></param>
        /// <param name="onCancel"></param>
        public static void ShowInputNumber(string text, int minValue, int maxValue, Action<int> onOK, Action onCancel)
        {
            CanvasManager canvas = Global.MainCanvas.GetComponent<CanvasManager>();
            UIInputNumber uiInputNumber = canvas.LoadUIPrefab<UIInputNumber>("MainGame/InputBox/UIInputNumber");
            canvas.AddUI(uiInputNumber, true);

            uiInputNumber.MinValue = minValue;
            uiInputNumber.MaxValue = maxValue;
            uiInputNumber.Text = text;
            uiInputNumber.OK = onOK;
            uiInputNumber.Cancel = onCancel;
        }

        /// <summary>
        /// Hiển thị khung nhập số
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="initValue"></param>
        /// <param name="text"></param>
        /// <param name="onOK"></param>
        /// <param name="onCancel"></param>
        public static void ShowInputNumber(string text, int minValue, int maxValue, int initValue, Action<int> onOK, Action onCancel)
        {
            CanvasManager canvas = Global.MainCanvas.GetComponent<CanvasManager>();
            UIInputNumber uiInputNumber = canvas.LoadUIPrefab<UIInputNumber>("MainGame/InputBox/UIInputNumber");
            canvas.AddUI(uiInputNumber, true);

            uiInputNumber.MinValue = minValue;
            uiInputNumber.MaxValue = maxValue;
            uiInputNumber.InitValue = initValue;
            uiInputNumber.Text = text;
            uiInputNumber.OK = onOK;
            uiInputNumber.Cancel = onCancel;
        }
        #endregion

        #region Input String
        /// <summary>
        /// Hiển thị khung nhập chuỗi
        /// </summary>
        /// <param name="onOK"></param>
        public static void ShowInputString(Action<string> onOK)
		{
            KTGlobal.ShowInputString("Nhập vào chuỗi bên dưới", "", onOK, null);
        }

        /// <summary>
        /// Hiển thị khung nhập chuỗi
        /// </summary>
        /// <param name="onOK"></param>
        /// <param name="onCancel"></param>
        public static void ShowInputString(Action<string> onOK, Action onCancel)
		{
            KTGlobal.ShowInputString("Nhập vào chuỗi bên dưới", "", onOK, onCancel);
        }

        /// <summary>
        /// Hiển thị khung nhập chuỗi
        /// </summary>
        /// <param name="text"></param>
        /// <param name="onOK"></param>
        /// <param name="onCancel"></param>
        public static void ShowInputString(string text, Action<string> onOK, Action onCancel)
		{
            KTGlobal.ShowInputString(text, "", onOK, onCancel);
        }

        /// <summary>
        /// Hiển thị khung nhập chuỗi
        /// </summary>
        /// <param name="text"></param>
        /// <param name="onOK"></param>
        public static void ShowInputString(string text, Action<string> onOK)
		{
            KTGlobal.ShowInputString(text, "", onOK, null);
        }

        /// <summary>
        /// Hiển thị khung nhập chuỗi
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="initValue"></param>
        /// <param name="onOK"></param>
        public static void ShowInputString(string text, string initValue, Action<string> onOK)
		{
            KTGlobal.ShowInputString(text, initValue, onOK, null);
		}

        /// <summary>
        /// Hiển thị khung nhập chuỗi
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="initValue"></param>
        /// <param name="onOK"></param>
        /// <param name="onCancel"></param>
        public static void ShowInputString(string text, string initValue, Action<string> onOK, Action onCancel)
        {
            CanvasManager canvas = Global.MainCanvas.GetComponent<CanvasManager>();
            UIInputString uiInputString = canvas.LoadUIPrefab<UIInputString>("MainGame/InputBox/UIInputString");
            canvas.AddUI(uiInputString, true);

            uiInputString.Description = text;
            uiInputString.Text = initValue;
            uiInputString.OK = onOK;
            uiInputString.Cancel = onCancel;
        }
        #endregion

        #region Game MessageBox
        /// <summary>
        /// Hiển thị bảng thông báo IN-GAME tiêu đề "Thông báo" và không có button nào
        /// </summary>
        /// <param name="content"></param>
        public static void ShowMessageBox(string content)
        {
            KTGlobal.ShowMessageBox("Thông báo", content, null, null);
        }

        /// <summary>
        /// Hiển thị bảng thông báo IN-GAME tiêu đề "Thông báo" và gồm button đồng ý hay không
        /// </summary>
        /// <param name="content"></param>
        /// <param name="isShowButtonOK"></param>
        public static void ShowMessageBox(string content, bool isShowButtonOK)
        {
            if (isShowButtonOK)
            {
                KTGlobal.ShowMessageBox("Thông báo", content, () => { }, null);
            }
            else
            {
                KTGlobal.ShowMessageBox("Thông báo", content, null, null);
            }
        }

        /// <summary>
        /// Hiển thị bảng thông báo IN-GAME tiêu đề "Thông báo" và gồm button đồng ý
        /// </summary>
        /// <param name="content"></param>
        public static void ShowMessageBox(string content, Action onOK)
        {
            KTGlobal.ShowMessageBox("Thông báo", content, onOK, null);
        }

        /// <summary>
        /// Hiển thị bảng thông báo IN-GAME tiêu đề "Thông báo", gồm button đồng ý và có button hủy bỏ hay không 
        /// </summary>
        /// <param name="content"></param>
        public static void ShowMessageBox(string content, Action onOK, bool isShowButtonCancel)
        {
            if (isShowButtonCancel)
            {
                KTGlobal.ShowMessageBox("Thông báo", content, onOK, () => { });
            }
            else
            {
                KTGlobal.ShowMessageBox("Thông báo", content, onOK, null);
            }
        }

        /// <summary>
        /// Hiển thị bảng thông báo IN-GAME tiêu đề "Thông báo", gồm button đồng ý và button hủy bỏ
        /// </summary>
        /// <param name="content"></param>
        public static void ShowMessageBox(string content, Action onOK, Action onCancel)
        {
            KTGlobal.ShowMessageBox("Thông báo", content, onOK, onCancel);
        }

        /// <summary>
        /// Hiển thị bảng thông báo IN-GAME tiêu đề chỉ định không có button nào
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        public static void ShowMessageBox(string title, string content)
        {
            KTGlobal.ShowMessageBox(title, content, null, null);
        }

        /// <summary>
        /// Hiển thị bảng thông báo IN-GAME tiêu đề chỉ định gồm button đồng ý hay không
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="isShowButtonOK"></param>
        public static void ShowMessageBox(string title, string content, bool isShowButtonOK)
        {
            if (isShowButtonOK)
            {
                KTGlobal.ShowMessageBox(title, content, () => { }, null);
            }
            else
            {
                KTGlobal.ShowMessageBox(title, content, null, null);
            }
        }

        /// <summary>
        /// Hiển thị bảng thông báo IN-GAME tiêu đề chỉ định gồm button đồng ý
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="onOK"></param>
        public static void ShowMessageBox(string title, string content, Action onOK)
        {
            KTGlobal.ShowMessageBox(title, content, onOK, null);
        }

        /// <summary>
        /// Hiển thị bảng thông báo IN-GAME tiêu đề chỉ định gồm button đồng ý và có button hủy bỏ hay không 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="onOK"></param>
        /// <param name="isShowButtonCancel"></param>
        public static void ShowMessageBox(string title, string content, Action onOK, bool isShowButtonCancel)
        {
            if (isShowButtonCancel)
            {
                KTGlobal.ShowMessageBox(title, content, onOK, () => { });
            }
            else
            {
                KTGlobal.ShowMessageBox(title, content, onOK, null);
            }
        }

        /// <summary>
        /// Hiển thị bảng thông báo IN-GAME tiêu đề chỉ định gồm 2 button đồng ý và hủy bỏ 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="onOK"></param>
        /// <param name="onCancel"></param>
        public static void ShowMessageBox(string title, string content, Action onOK, Action onCancel)
        {
            CanvasManager canvas = Global.MainCanvas.GetComponent<CanvasManager>();
            UIMessageBox uiMessageBox = canvas.LoadUIPrefab<UIMessageBox>("MainGame/UIMessageBox");
            canvas.AddUI(uiMessageBox, true);
            uiMessageBox.Title = title;
            uiMessageBox.Content = content;
            uiMessageBox.ShowButtonOK = onOK != null;
            uiMessageBox.ShowButtonCancel = onCancel != null;
            uiMessageBox.OK = onOK;
            uiMessageBox.Cancel = onCancel;
        }
        #endregion

        #region Thông báo nhặt vật phẩm
        /// <summary>
        /// Thông báo nhận được vật phẩm số lượng tương ứng
        /// </summary>
        /// <param name="itemGD"></param>
        /// <param name="count"></param>
        public static void HintNewGoodsText(GoodsData itemGD, int count)
        {
            if (itemGD == null || count <= 0)
            {
                return;
            }

            /// Thêm vào danh sách biểu diễn
            UIHintItemManager.Instance.AddHint(itemGD, count);
        }
        #endregion

        #region Skill Result
        /// <summary>
        /// Hiển thị kết quả sát thương của kỹ năng
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="type"></param>
        /// <param name="damage"></param>
        public static void ShowSkillResultText(GSprite caster, GSprite target, int type, int damage)
        {
            switch (type)
            {
                case (int) KiemVu.Entities.Enum.SkillResult.Miss:
                    /// Nếu bản thân không phải đối tượng tấn công
                    if (Global.Data.Leader != caster)
                    {
                        KTGlobal.ShowSpriteHeaderText(Global.Data.Leader, FSPlay.KiemVu.Entities.Enum.DamageType.DODGE, "");
                    }
                    /// Nếu bản thân là đối tượng tấn công
                    else
                    {
                        KTGlobal.ShowSpriteHeaderText(target, FSPlay.KiemVu.Entities.Enum.DamageType.DODGE, "");
                    }
                    break;
                case (int) KiemVu.Entities.Enum.SkillResult.Immune:
                    /// Nếu bản thân không phải đối tượng tấn công
                    if (Global.Data.Leader != caster)
                    {
                        KTGlobal.ShowSpriteHeaderText(Global.Data.Leader, FSPlay.KiemVu.Entities.Enum.DamageType.IMMUNE, "");
                    }
                    /// Nếu bản thân là đối tượng tấn công
                    else
                    {
                        KTGlobal.ShowSpriteHeaderText(target, FSPlay.KiemVu.Entities.Enum.DamageType.IMMUNE, "");
                    }
                    break;
                case (int) KiemVu.Entities.Enum.SkillResult.Adjust:
                    /// Nếu bản thân không phải đối tượng tấn công
                    if (Global.Data.Leader != caster)
                    {
                        KTGlobal.ShowSpriteHeaderText(Global.Data.Leader, FSPlay.KiemVu.Entities.Enum.DamageType.ADJUST, "");
                    }
                    /// Nếu bản thân là đối tượng tấn công
                    else
                    {
                        KTGlobal.ShowSpriteHeaderText(target, FSPlay.KiemVu.Entities.Enum.DamageType.ADJUST, "");
                    }
                    break;
                case (int) KiemVu.Entities.Enum.SkillResult.Normal:
                    /// Nếu bản thân không phải đối tượng tấn công
                    if (Global.Data.Leader != caster)
                    {
                        /// Hiện Text sát thương nhận được
                        KTGlobal.ShowSpriteHeaderText(Global.Data.Leader, FSPlay.KiemVu.Entities.Enum.DamageType.DAMAGE_TAKEN, damage.ToString());
                    }
                    /// Nếu bản thân là đối tượng tấn công
                    else
                    {
                        /// Hiện Text sát thương gây ra
                        KTGlobal.ShowSpriteHeaderText(target, FSPlay.KiemVu.Entities.Enum.DamageType.DAMAGE_DEALT, damage.ToString());
                    }
                    break;
                case (int) KiemVu.Entities.Enum.SkillResult.Crit:
                    /// Nếu bản thân không phải đối tượng tấn công
                    if (Global.Data.Leader != caster)
                    {
                        /// Hiện Text sát thương nhận được
                        KTGlobal.ShowSpriteHeaderText(Global.Data.Leader, FSPlay.KiemVu.Entities.Enum.DamageType.DAMAGE_TAKEN, damage.ToString());
                    }
                    else
                    {
                        /// Hiện Text sát thương gây ra
                        KTGlobal.ShowSpriteHeaderText(target, FSPlay.KiemVu.Entities.Enum.DamageType.CRIT_DAMAGE_DEALT, damage.ToString());
                    }
                    break;
            }
        }

        /// <summary>
        /// Hiển thị text sát thương nhận
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="type"></param>
        /// <param name="content"></param>
        public static void ShowSpriteHeaderText(GSprite sprite, FSPlay.KiemVu.Entities.Enum.DamageType type, string content)
        {
            sprite.AddHeadText(type, content);
        }

        /// <summary>
        /// Hiển thị text sát thương nhận
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="type"></param>
        /// <param name="content"></param>
        /// <param name="color"></param>
        public static void ShowSpriteHeaderText(GSprite sprite, FSPlay.KiemVu.Entities.Enum.DamageType type, string content, Color color)
        {
            sprite.AddHeadText(type, content, color);
        }
        #endregion

        #region Thêm kinh nghiệm, vàng, tinh hoạt lực
        /// <summary>
        /// Hiển thị text kinh nghiệm hoặc tiền vàng
        /// </summary>
        /// <param name="type"></param>
        /// <param name="content"></param>
        public static void ShowTextForExpMoneyOrGatherMakePoint(FSPlay.KiemVu.Entities.Enum.BottomTextDecorationType type, string content)
        {
            CanvasManager canvas = Global.MainCanvas.GetComponent<CanvasManager>();
            //UIFlyingText text = GameObject.Instantiate<UIFlyingText>(canvas.UIBottomText);
            UIFlyingText text = KTUIElementPoolManager.Instance.Instantiate<UIFlyingText>("UIBottomText");
            canvas.AddUI(text, true);
            text.gameObject.SetActive(true);
            text.Text = content;
            text.Offset = Vector2.zero;
            text.Duration = 3f;

            Color _color;
            switch (type)
            {
                case KiemVu.Entities.Enum.BottomTextDecorationType.Money:
                    ColorUtility.TryParseHtmlString("#ffff4d", out _color);
                    text.Color = _color;
                    break;
                case KiemVu.Entities.Enum.BottomTextDecorationType.BoundMoney:
                    ColorUtility.TryParseHtmlString("#ffff4d", out _color);
                    text.Color = _color;
                    break;
                case KiemVu.Entities.Enum.BottomTextDecorationType.Coupon:
                    ColorUtility.TryParseHtmlString("#ffff4d", out _color);
                    text.Color = _color;
                    break;
                case KiemVu.Entities.Enum.BottomTextDecorationType.Coupon_Bound:
                    ColorUtility.TryParseHtmlString("#ffff4d", out _color);
                    text.Color = _color;
                    break;
                case KiemVu.Entities.Enum.BottomTextDecorationType.Exp:
                    ColorUtility.TryParseHtmlString("#ff4de4", out _color);
                    text.Color = _color;
                    break;
                case KiemVu.Entities.Enum.BottomTextDecorationType.Gather:
                    ColorUtility.TryParseHtmlString("#ff4de4", out _color);
                    text.Color = _color;
                    break;
                case KiemVu.Entities.Enum.BottomTextDecorationType.Make:
                    ColorUtility.TryParseHtmlString("#ff4de4", out _color);
                    text.Color = _color;
                    break;
            }
            //text.Play();
            UIBottomTextManager.Instance.AddText(text);
        }
        #endregion

        #region Màn hình chờ tải cái gì đó
        /// <summary>
        /// Hiển thị màn hình chờ tải cái gì đó
        /// </summary>
        /// <param name="hint"></param>
        public static void ShowLoadingFrame(string hint)
        {
            UILoadingProgress uiLoadingProgress = Global.MainCanvas.GetComponent<CanvasManager>().UILoadingProgress;
            uiLoadingProgress.Hint = hint;
            uiLoadingProgress.Show();
        }

        /// <summary>
        /// Ẩn màn hình chờ tải cái gì đó
        /// </summary>
        public static void HideLoadingFrame()
        {
            UILoadingProgress uiLoadingProgress = Global.MainCanvas.GetComponent<CanvasManager>().UILoadingProgress;
            uiLoadingProgress.Hide();
        }
        #endregion

        #region Khung danh sách vật phẩm
        /// <summary>
        /// Hiện khung danh sách vật phẩm
        /// </summary>
        /// <param name="description"></param>
        /// <param name="goods"></param>
        /// <param name="itemClick"></param>
        /// <param name="close"></param>
        public static UIListItemBox ShowItemListBox(string description, List<GoodsData> goods, Action<GoodsData> itemClick, Action close)
        {
            CanvasManager canvas = Global.MainCanvas.GetComponent<CanvasManager>();
            UIListItemBox uiListItemBox = canvas.LoadUIPrefab<UIListItemBox>("MainGame/UIListItemBox");
            canvas.AddUI(uiListItemBox);
            uiListItemBox.Description = description;
            uiListItemBox.Items = goods;
            uiListItemBox.ItemClick = itemClick;
            uiListItemBox.Close = close;
            return uiListItemBox;
        }

        /// <summary>
        /// Hiện khung danh sách vật phẩm
        /// </summary>
        /// <param name="goods"></param>
        public static UIListItemBox ShowItemListBox(List<GoodsData> goods)
        {
            return KTGlobal.ShowItemListBox("Danh sách vật phẩm", goods, null, null);
        }

        /// <summary>
        /// Hiện khung danh sách vật phẩm
        /// </summary>
        /// <param name="description"></param>
        /// <param name="goods"></param>
        public static UIListItemBox ShowItemListBox(string description, List<GoodsData> goods)
        {
            return KTGlobal.ShowItemListBox(description, goods, null, null);
        }

        /// <summary>
        /// Hiện khung danh sách vật phẩm
        /// </summary>
        /// <param name="goods"></param>
        /// <param name="itemClick"></param>
        public static UIListItemBox ShowItemListBox(List<GoodsData> goods, Action<GoodsData> itemClick)
        {
            return KTGlobal.ShowItemListBox("Danh sách vật phẩm", goods, itemClick, null);
        }

        /// <summary>
        /// Hiện khung danh sách vật phẩm
        /// </summary>
        /// <param name="description"></param>
        /// <param name="goods"></param>
        /// <param name="itemClick"></param>
        public static UIListItemBox ShowItemListBox(string description, List<GoodsData> goods, Action<GoodsData> itemClick)
        {
            return KTGlobal.ShowItemListBox(description, goods, itemClick, null);
        }

        /// <summary>
        /// Hiện khung danh sách vật phẩm
        /// </summary>
        /// <param name="goods"></param>
        /// <param name="itemClick"></param>
        /// <param name="close"></param>
        public static UIListItemBox ShowItemListBox(List<GoodsData> goods, Action<GoodsData> itemClick, Action close)
        {
            return KTGlobal.ShowItemListBox("Danh sách vật phẩm", goods, itemClick, close);
        }
        #endregion
    }
}
