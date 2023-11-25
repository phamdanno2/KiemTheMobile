using FSPlay.GameEngine.Logic;
using FSPlay.KiemVu.Network;
using FSPlay.KiemVu.UI;
using FSPlay.KiemVu.UI.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Quản lý các khung giao diện trong màn chơi
/// </summary>
public partial class PlayZone
{
    #region UIInputItems
    /// <summary>
    /// Khung đặt vào danh sách vật phẩm
    /// </summary>
    public UIInputItems UIInputItems { get; protected set; }

    /// <summary>
    /// Mở khung đặt vào danh sách vật phẩm
    /// </summary>
    /// <param name="title"></param>
    /// <param name="description"></param>
    /// <param name="otherDetail"></param>
    /// <param name="tag"></param>
    public void OpenUIInputItems(string title, string description, string otherDetail, string tag)
    {
        if (this.UIInputItems != null)
        {
            return;
        }

        CanvasManager canvas = Global.MainCanvas.GetComponent<CanvasManager>();
        this.UIInputItems = canvas.LoadUIPrefab<UIInputItems>("MainGame/UIInputItems");
        canvas.AddUI(this.UIInputItems);

        this.UIInputItems.Title = title;
        this.UIInputItems.Description = description;
        this.UIInputItems.OtherDetail = otherDetail;
        this.UIInputItems.Close = this.CloseUIInputItems;
        this.UIInputItems.OK = (items) => {
            KT_TCPHandler.SendInputItems(items, tag);
        };
    }

    /// <summary>
    /// Đóng khung đặt vào danh sách vật phẩm
    /// </summary>
    public void CloseUIInputItems()
    {
        if (this.UIInputItems != null)
        {
            GameObject.Destroy(this.UIInputItems.gameObject);
            this.UIInputItems = null;
        }
    }
    #endregion

    #region UIInputEquipAndMaterials
    /// <summary>
    /// Khung đặt vào danh sách vật phẩm
    /// </summary>
    public UIInputEquipAndMaterials UIInputEquipAndMaterials { get; protected set; }

    /// <summary>
    /// Mở khung đặt vào danh sách vật phẩm
    /// </summary>
    /// <param name="title"></param>
    /// <param name="description"></param>
    /// <param name="otherDetail"></param>
    /// <param name="mustInputItems"></param>
    /// <param name="tag"></param>
    public void OpenUIInputEquipAndMaterials(string title, string description, string otherDetail, bool mustInputItems, string tag)
    {
        if (this.UIInputEquipAndMaterials != null)
        {
            return;
        }

        CanvasManager canvas = Global.MainCanvas.GetComponent<CanvasManager>();
        this.UIInputEquipAndMaterials = canvas.LoadUIPrefab<UIInputEquipAndMaterials>("MainGame/UIInputEquipAndMaterials");
        canvas.AddUI(this.UIInputEquipAndMaterials);

        this.UIInputEquipAndMaterials.Title = title;
        this.UIInputEquipAndMaterials.Description = description;
        this.UIInputEquipAndMaterials.OtherDetail = otherDetail;
        this.UIInputEquipAndMaterials.MustIncludeMaterials = mustInputItems;
        this.UIInputEquipAndMaterials.Close = this.CloseUIInputEquipAndMaterials;
        this.UIInputEquipAndMaterials.OK = (equip, items) => {
            KT_TCPHandler.SendInputEquipAndMaterials(equip, items, tag);
        };
    }

    /// <summary>
    /// Đóng khung đặt vào danh sách vật phẩm
    /// </summary>
    public void CloseUIInputEquipAndMaterials()
    {
        if (this.UIInputEquipAndMaterials != null)
        {
            GameObject.Destroy(this.UIInputEquipAndMaterials.gameObject);
            this.UIInputEquipAndMaterials = null;
        }
    }
	#endregion

	#region UIInputSecondPassword
    /// <summary>
    /// Khung nhập mật mã cấp 2
    /// </summary>
    public UIInputSecondPassword UIInputSecondPassword { get; protected set; }

    /// <summary>
    /// Mở khung nhập mật khẩu cấp 2
    /// </summary>
    public void OpenUIInputSecondPassword()
	{
        if (this.UIInputSecondPassword != null)
        {
            return;
        }

        CanvasManager canvas = Global.MainCanvas.GetComponent<CanvasManager>();
        this.UIInputSecondPassword = canvas.LoadUIPrefab<UIInputSecondPassword>("MainGame/UIInputSecondPassword");
        canvas.AddUI(this.UIInputSecondPassword);

        this.UIInputSecondPassword.Close = this.CloseUIInputSecondPassword;
        this.UIInputSecondPassword.Accept = (password) => {
            KT_TCPHandler.SendInputSecondPassword(password);
            this.CloseUIInputSecondPassword();
        };
    }

    /// <summary>
    /// Đóng khung nhập mật khẩu cấp 2
    /// </summary>
    public void CloseUIInputSecondPassword()
	{
        if (this.UIInputSecondPassword != null)
        {
            GameObject.Destroy(this.UIInputSecondPassword.gameObject);
            this.UIInputSecondPassword = null;
        }
    }
	#endregion
}
