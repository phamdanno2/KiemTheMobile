using FSPlay.GameEngine.Logic;
using FSPlay.GameEngine.Network;
using FSPlay.GameEngine.Sprite;
using FSPlay.KiemVu;
using FSPlay.KiemVu.Network;
using FSPlay.KiemVu.UI;
using FSPlay.KiemVu.UI.Main;
using FSPlay.KiemVu.UI.Main.MainUI;
using FSPlay.KiemVu.UI.Main.RoleInfo;
using Server.Data;
using UnityEngine;
using static FSPlay.KiemVu.Entities.Enum;

/// <summary>
/// Quản lý các khung giao diện trong màn chơi
/// </summary>
public partial class PlayZone
{
    #region Other Role Equip Info
    public UIOtherRoleInfo UIOtherRoleInfo { get; protected set; }

    /// <summary>
    /// Mở khung soi thông tin trang bị người chơi khác
    /// </summary>
    /// <param name="roleData"></param>
    public void OpenUIOtherRoleInfo(RoleData roleData)
    {
        /// Nếu đang mở khung
        if (this.UIOtherRoleInfo != null)
        {
            return;
        }

        /// Mở khung
        CanvasManager canvas = Global.MainCanvas.GetComponent<CanvasManager>();
        this.UIOtherRoleInfo = canvas.LoadUIPrefab<UIOtherRoleInfo>("MainGame/UIOtherRoleInfo");
        canvas.AddUI(this.UIOtherRoleInfo);

        this.UIOtherRoleInfo.Close = this.CloseUIOtherRoleInfo;
        this.UIOtherRoleInfo.Data = roleData;
    }

    /// <summary>
    /// Đóng khung soi thông tin trang bị người chơi khác
    /// </summary>
    public void CloseUIOtherRoleInfo()
    {
        /// Nếu đang mở khung
        if (this.UIOtherRoleInfo != null)
        {
            GameObject.Destroy(this.UIOtherRoleInfo.gameObject);
            this.UIOtherRoleInfo = null;
        }
    }
    #endregion

    #region Browse Other Role Info
    /// <summary>
    /// Khung kiểm tra thông tin người chơi khác
    /// </summary>
    public UIBrowseOtherRoleInfo UIBrowseOtherRoleInfo { get; protected set; }

    /// <summary>
    /// Khởi tạo khung kiểm tra thông tin người chơi khác
    /// </summary>
    protected void InitializeUIBrowseOtherRoleInfo()
    {
        if (this.UIBrowseOtherRoleInfo != null)
        {
            return;
        }

        CanvasManager canvas = Global.MainCanvas.GetComponent<CanvasManager>();
        this.UIBrowseOtherRoleInfo = canvas.LoadUIPrefab<UIBrowseOtherRoleInfo>("MainGame/MainUI/UIBrowseOtherRoleInfo");
        canvas.AddMainUI(this.UIBrowseOtherRoleInfo);

        this.UIBrowseOtherRoleInfo.Close = () => {
            this.HideBrowseOtherRoleInfo();
        };
        this.UIBrowseOtherRoleInfo.InviteToTeam = () => {
            /// Nếu bản thân không có nhóm
            if (Global.Data.RoleData.TeamID == -1)
            {
                return;
            }
            /// Nếu bản thân không phải trưởng nhóm
            else if (Global.Data.RoleData.TeamLeaderRoleID != Global.Data.RoleData.RoleID)
            {
                return;
            }
            /// Nếu không có dữ liệu đối phương
            else if (this.UIBrowseOtherRoleInfo.RoleData == null)
            {
                return;
            }
            /// Nếu đối phương đã có nhóm
            else if (this.UIBrowseOtherRoleInfo.RoleData.TeamID != -1)
            {
                return;
            }

            KT_TCPHandler.SendInviteToTeam(this.UIBrowseOtherRoleInfo.RoleData.RoleID);
        };
        this.UIBrowseOtherRoleInfo.AskToJoinTeam = () => {
            /// Nếu bản thân đã có nhóm
            if (Global.Data.RoleData.TeamID != -1)
            {
                return;
            }
            /// Nếu không có dữ liệu đối phương
            else if (this.UIBrowseOtherRoleInfo.RoleData == null)
            {
                return;
            }
            /// Nếu đối phương chưa có nhóm
            else if (this.UIBrowseOtherRoleInfo.RoleData.TeamID == -1)
            {
                return;
            }

            KT_TCPHandler.SendAskToJoinTeam(this.UIBrowseOtherRoleInfo.RoleData.RoleID);
        };
        this.UIBrowseOtherRoleInfo.OpenEquipInfo = () => {
            /// Nếu không có dữ liệu đối phương
            if (this.UIBrowseOtherRoleInfo.RoleData == null)
            {
                return;
            }
            /// Gửi yêu cầu kiểm tra thông tin trang bị của người chơi
            KT_TCPHandler.RequestGetOtherPlayerEquipInfo(this.UIBrowseOtherRoleInfo.RoleData.RoleID);
        };
        this.UIBrowseOtherRoleInfo.AddFriend = () => {
            GameInstance.Game.SpriteAskFriend(this.UIBrowseOtherRoleInfo.RoleData.RoleID);
        };
        this.UIBrowseOtherRoleInfo.Exchange = () => {
            /// Nếu không có dữ liệu đối phương
            if (this.UIBrowseOtherRoleInfo.RoleData == null)
            {
                return;
            }
            /// Nếu không tìm thấy người chơi tương ứng
            if (!Global.Data.OtherRoles.TryGetValue(this.UIBrowseOtherRoleInfo.RoleData.RoleID, out _))
            {
                KTGlobal.AddNotification("Không tìm thấy người chơi tương ứng xung quanh!");
                return;
            }

            /// Gửi yêu cầu mở khung giao dịch
            GameInstance.Game.SpriteGoodsExchange(this.UIBrowseOtherRoleInfo.RoleData.RoleID, (int) GoodsExchangeCmds.Request, -1);
        };
        this.UIBrowseOtherRoleInfo.Fight = () => {
            /// Nếu không có dữ liệu đối phương
            if (this.UIBrowseOtherRoleInfo.RoleData == null)
            {
                return;
            }
            /// Nếu không tìm thấy người chơi tương ứng
            if (!Global.Data.OtherRoles.TryGetValue(this.UIBrowseOtherRoleInfo.RoleData.RoleID, out _))
            {
                KTGlobal.AddNotification("Không tìm thấy người chơi tương ứng xung quanh!");
                return;
            }

            /// Tăng sát khí hay không
            bool isIncreasePKValue = true;
            /// Nếu bản đồ tự do PK
            if (Global.Data.GameScene.CurrentMapData.Setting.FreePK)
            {
                isIncreasePKValue = false;
            }
            /// Nếu đối tượng có sát khí
            else if (this.UIBrowseOtherRoleInfo.RoleData.PKValue > 0)
            {
                /// Nếu bản thân có sát khí
                if (Global.Data.RoleData.PKValue > 0)
                {
                    isIncreasePKValue = true;
                }
                else
                {
                    isIncreasePKValue = false;
                }
            }
            /// Xác nhận tuyên chiến
            KTGlobal.ShowMessageBox("Tuyên chiến", string.Format("Xác nhận tuyên chiến với <color=#0acaff>{0}</color>? Giết đối phương <color=green>{1}</color> làm tăng sát khí bản thân.", this.UIBrowseOtherRoleInfo.RoleData.RoleName, isIncreasePKValue ? "sẽ" : "không"), () => {
                KT_TCPHandler.SendActiveFight(this.UIBrowseOtherRoleInfo.RoleData.RoleID);
            }, true);
        };
        this.UIBrowseOtherRoleInfo.Challenge = () => {
            /// Nếu không có dữ liệu đối phương
            if (this.UIBrowseOtherRoleInfo.RoleData == null)
            {
                return;
            }
            /// Nếu không tìm thấy người chơi tương ứng
            if (!Global.Data.OtherRoles.TryGetValue(this.UIBrowseOtherRoleInfo.RoleData.RoleID, out _))
            {
                KTGlobal.AddNotification("Không tìm thấy người chơi tương ứng xung quanh!");
                return;
            }

            KT_TCPHandler.SendAskChallenge(this.UIBrowseOtherRoleInfo.RoleData.RoleID);
        };
        this.UIBrowseOtherRoleInfo.PrivateChat = () => {
            /// Nếu không có dữ liệu đối phương
            if (this.UIBrowseOtherRoleInfo.RoleData == null)
            {
                return;
            }
            /// Thực hiện Chat mật
            KTGlobal.OpenPrivateChatBoxWith(this.UIBrowseOtherRoleInfo.RoleData.RoleName);
        };
        this.UIBrowseOtherRoleInfo.InviteToGuild = () => {
            /// Nếu không có dữ liệu đối phương
            if (this.UIBrowseOtherRoleInfo.RoleData == null)
            {
                return;
            }
            /// Gửi yêu cầu mời vào bang
            KT_TCPHandler.SendInviteToGuild(this.UIBrowseOtherRoleInfo.RoleData.RoleID);
        };
        this.UIBrowseOtherRoleInfo.AskToJoinGuild = () => {
            /// Nếu không có dữ liệu đối phương
            if (this.UIBrowseOtherRoleInfo.RoleData == null)
            {
                return;
            }
            /// Gửi yêu cầu xin vào bang
            KT_TCPHandler.SendAskToJoinGuild(this.UIBrowseOtherRoleInfo.RoleData.GuildID, this.UIBrowseOtherRoleInfo.RoleData.RoleID);
        };
    }

    /// <summary>
    /// Hiển thị khung kiểm tra thông tin người chơi khác
    /// </summary>
    /// <param name="roleID"></param>
    public void ShowBrowseOtherRoleInfo(int roleID)
    {
        if (this.UIBrowseOtherRoleInfo == null)
        {
            return;
        }

        if (Global.Data.OtherRoles.TryGetValue(roleID, out RoleData roleData))
        {
            this.UIBrowseOtherRoleInfo.RoleData = roleData;
            this.UIBrowseOtherRoleInfo.Show();
        }
    }

    /// <summary>
    /// Ẩn khung kiểm tra thông tin người chơi khác
    /// </summary>
    public void HideBrowseOtherRoleInfo()
    {
        if (this.UIBrowseOtherRoleInfo == null)
        {
            return;
        }

        this.UIBrowseOtherRoleInfo.Hide();
    }
    #endregion

    #region Role Info
    /// <summary>
    /// Khung thông tin nhân vật
    /// </summary>
    public UIRoleInfo UIRoleInfo { get; protected set; }

    /// <summary>
    /// Hiển thị khung thông tin nhân vật
    /// </summary>
    /// <param name="attributes"></param>
    public void ShowUIRoleInfo()
    {
        CanvasManager canvas = Global.MainCanvas.GetComponent<CanvasManager>();
        this.UIRoleInfo = canvas.LoadUIPrefab<UIRoleInfo>("MainGame/RoleInfo/UIRoleInfo");
        canvas.AddUI(this.UIRoleInfo);

        this.UIRoleInfo.Close = () => {
            this.CloseUIRoleInfo();
        };
        this.UIRoleInfo.OpenSelectAvarta = () => {
            this.ShowUISelectAvarta();
        };
        this.UIRoleInfo.Unequip = (itemGD) => {
            GameInstance.Game.SpriteModGoods((int) ModGoodsTypes.EquipUnload, itemGD.Id, itemGD.GoodsID, Global.Data.ShowReserveEquip ? itemGD.Using + 1000 : -1, itemGD.Site, 1, -1);

            /// Đóng khung Tooltip vật phẩm
            KTGlobal.CloseItemInfo();
        };
        this.UIRoleInfo.Advertise = (itemGD) => {
            this.ShowUIAdvertiseItem(itemGD);

            /// Đóng khung Tooltip vật phẩm
            KTGlobal.CloseItemInfo();
        };
        this.UIRoleInfo.ChangeSubSet = () => {
            KT_TCPHandler.SendChangeSubEquip();
        };
        this.UIRoleInfo.SetAsCurrentRoleTitle = (titleID) => {
            KT_TCPHandler.SendChangeMyselfRoleTitle(titleID);
        };
    }

    /// <summary>
    /// Đóng khung thông tin nhân vật
    /// </summary>
    public void CloseUIRoleInfo()
    {
        if (this.UIRoleInfo != null)
        {
            GameObject.Destroy(this.UIRoleInfo.gameObject);
            this.UIRoleInfo = null;
            return;
        }
    }

    #region Remain Point manager
    /// <summary>
    /// Khung cộng điểm tiềm năng nhân vật
    /// </summary>
    public UIRoleRemainPoint UIRoleRemainPoint { get; protected set; }

    /// <summary>
    /// Mở khung cộng điểm tiềm năng nhân vật
    /// </summary>
    public void ShowUIRoleRemainPoint()
    {
        if (this.UIRoleRemainPoint != null)
        {
            return;
        }

        CanvasManager canvas = Global.MainCanvas.GetComponent<CanvasManager>();
        this.UIRoleRemainPoint = canvas.LoadUIPrefab<UIRoleRemainPoint>("MainGame/RoleInfo/UIRoleRemainPoint");
        canvas.AddUI(this.UIRoleRemainPoint);

        if (this.UIRoleInfo != null && this.UIRoleInfo.RoleAttributes != null)
        {
            this.UIRoleRemainPoint.Str = this.UIRoleInfo.RoleAttributes.Str;
            this.UIRoleRemainPoint.Dex = this.UIRoleInfo.RoleAttributes.Dex;
            this.UIRoleRemainPoint.Sta = this.UIRoleInfo.RoleAttributes.Sta;
            this.UIRoleRemainPoint.Int = this.UIRoleInfo.RoleAttributes.Int;
            this.UIRoleRemainPoint.RemainPoint = this.UIRoleInfo.RoleAttributes.RemainPoint;
        }
        this.UIRoleRemainPoint.Close = () => {
            this.CloseUIRoleRemainPoint();
        };
    }

    /// <summary>
    /// Đóng khung cộng điểm tiềm năng nhân vật
    /// </summary>
    public void CloseUIRoleRemainPoint()
    {
        if (this.UIRoleRemainPoint == null)
        {
            return;
        }

        GameObject.Destroy(this.UIRoleRemainPoint.gameObject);
        this.UIRoleRemainPoint = null;
    }
    #endregion
    #endregion

    #region Thông tin máu, khí, thể lực
    /// <summary>
    /// Cập nhật máu, khí và thể lực nhân vật
    /// </summary>
    public void RefreshRoleFace()
    {
        if (this.UIRolePart == null)
        {
            return;
        }

        this.UIRolePart.HP = Global.Data.RoleData.CurrentHP;
        this.UIRolePart.HPMax = Global.Data.RoleData.MaxHP;
        this.UIRolePart.MP = Global.Data.RoleData.CurrentMP;
        this.UIRolePart.MPMax = Global.Data.RoleData.MaxMP;
        this.UIRolePart.Vitality = Global.Data.RoleData.CurrentStamina;
        this.UIRolePart.VitalityMax = Global.Data.RoleData.MaxStamina;
    }

    /// <summary>
    /// Làm mới thông tin môn phái của Leader
    /// </summary>
    public void UpdateLeaderRoleFaction()
    {
        this.UIRolePart.FactionID = Global.Data.RoleData.FactionID;
    }

    /// <summary>
    /// Thông báo sự thay đổi máu, khí, thể lực
    /// </summary>
    /// <param name="enemyRoleID">ID đối tượng, -1 nếu là Leader</param>
    /// <param name="refreshLeader"></param>
    protected void NotifyLeaderRoleFace(int enemyRoleID)
    {
        if (this.UIOtherRoleFace == null)
        {
            return;
        }

        if (this.objectFaceRoleID != enemyRoleID)
        {
            return;
        }
        if (this.objectFaceType == GSpriteTypes.Other)
        {
            if (Global.Data.OtherRoles.TryGetValue(this.objectFaceRoleID, out RoleData rd))
            {
                this.UIOtherRoleFace.Level = rd.Level;
                this.UIOtherRoleFace.RoleID = rd.RoleID;

                this.UIOtherRoleFace.HP = rd.CurrentHP;
                this.UIOtherRoleFace.HPMax = rd.MaxHP;

                this.UIOtherRoleFace.FactionID = rd.FactionID;
                this.UIOtherRoleFace.RoleAvartaID = rd.RolePic;
            }
        }
        else if (this.objectFaceType == GSpriteTypes.Bot)
        {
            if (Global.Data.Bots.TryGetValue(this.objectFaceRoleID, out RoleData rd))
            {
                this.UIOtherRoleFace.Level = rd.Level;
                this.UIOtherRoleFace.RoleID = rd.RoleID;

                this.UIOtherRoleFace.HP = rd.CurrentHP;
                this.UIOtherRoleFace.HPMax = rd.MaxHP;

                this.UIOtherRoleFace.FactionID = rd.FactionID;
                this.UIOtherRoleFace.RoleAvartaID = rd.RolePic;
            }
        }
        else if (this.objectFaceType == GSpriteTypes.Monster)
        {
            if (Global.Data.SystemMonsters.TryGetValue(this.objectFaceRoleID, out MonsterData md))
            {
                this.UIMonsterFace.HP = md.HP;
                this.UIMonsterFace.HPMax = md.MaxHP;

                this.UIMonsterFace.Elemental = (Elemental) md.Elemental;
            }
        }
    }

    /// <summary>
    /// Thông báo sự kiện thay đổi thuộc tính máu
    /// </summary>
    /// <param name="injuredRoleID"></param>
    /// <param name="attackerRoleID"></param>
    /// <param name="forceShow"></param>
    public void NotifyRoleFace(int injuredRoleID, int attackerRoleID, bool forceShow = false)
    {
        this.NotifyLeaderRoleFace(attackerRoleID);

        if (forceShow)
        {
            GSprite sprite = KTGlobal.FindSpriteByID(injuredRoleID);
            if (sprite != null)
            {
                this.objectFaceRoleID = injuredRoleID;
                this.objectFaceType = sprite.SpriteType;
            }
        }

        if (this.objectFaceRoleID != injuredRoleID)
        {
            return;
        }

        if (this.objectFaceType == GSpriteTypes.Monster)
        {
            if (Global.Data.SystemMonsters.TryGetValue(this.objectFaceRoleID, out MonsterData md))
            {
                if (!this.UIMonsterFace.Visible)
                {
                    this.SetFaceVisiable(1);
                    this.ShowObjectRoleFace(new SpriteNotifyEventArgs()
                    {
                        RoleID = this.objectFaceRoleID,
                        SpriteType = this.objectFaceType,
                    });
                }

                if (md.MaxHP > 0)
                {
                    this.UIMonsterFace.HP = md.HP;
                    this.UIMonsterFace.HPMax = md.MaxHP;
                }
                this.UIMonsterFace.Data = md;
                this.UIMonsterFace.Name = md.RoleName;
                this.UIMonsterFace.Elemental = (Elemental) md.Elemental;
            }
        }
        else if (this.objectFaceType == GSpriteTypes.Other)
        {
            if (Global.Data.OtherRoles.ContainsKey(this.objectFaceRoleID))
            {
                if (!this.UIOtherRoleFace.Visible)
                {
                    this.SetFaceVisiable(0);
                    this.ShowObjectRoleFace(new SpriteNotifyEventArgs()
                    {
                        RoleID = this.objectFaceRoleID,
                        SpriteType = this.objectFaceType,
                    });
                }

                Global.Data.OtherRoles.TryGetValue(this.objectFaceRoleID, out RoleData rd);
                this.UIOtherRoleFace.Level = rd.Level;
                this.UIOtherRoleFace.RoleID = rd.RoleID;
                if (rd.MaxHP > 0)
                {
                    this.UIOtherRoleFace.HP = rd.CurrentHP;
                    this.UIOtherRoleFace.HPMax = rd.MaxHP;
                }
                this.UIOtherRoleFace.FactionID = rd.FactionID;
                this.UIOtherRoleFace.RoleAvartaID = rd.RolePic;
                this.UIOtherRoleFace.Data = rd;
                this.UIOtherRoleFace.Name = rd.RoleName;
                this.UIOtherRoleFace.AvartaClickable = true;
            }
        }
        else if (this.objectFaceType == GSpriteTypes.Bot)
        {
            if (Global.Data.Bots.ContainsKey(this.objectFaceRoleID))
            {
                if (!this.UIOtherRoleFace.Visible)
                {
                    this.SetFaceVisiable(0);
                    this.ShowObjectRoleFace(new SpriteNotifyEventArgs()
                    {
                        RoleID = this.objectFaceRoleID,
                        SpriteType = this.objectFaceType,
                    });
                }

                Global.Data.Bots.TryGetValue(this.objectFaceRoleID, out RoleData rd);
                this.UIOtherRoleFace.Level = rd.Level;
                this.UIOtherRoleFace.RoleID = rd.RoleID;
                if (rd.MaxHP > 0)
                {
                    this.UIOtherRoleFace.HP = rd.CurrentHP;
                    this.UIOtherRoleFace.HPMax = rd.MaxHP;
                }
                this.UIOtherRoleFace.FactionID = rd.FactionID;
                this.UIOtherRoleFace.RoleAvartaID = rd.RolePic;
                this.UIOtherRoleFace.Data = rd;
                this.UIOtherRoleFace.Name = rd.RoleName;
                this.UIOtherRoleFace.AvartaClickable = false;
            }
        }
    }
    #endregion

    #region Thông tin chỉ số nhân vật
    /// <summary>
    /// Khung thông tin chỉ số nhân vật ở MainUI
    /// </summary>
    public UIRolePart UIRolePart { get; protected set; }

    /// <summary>
    /// Cập nhật UI thông tin chỉ số nhân vật
    /// </summary>
    protected void InitPlayerSelfInfo()
    {
        if (this.UIRolePart != null)
        {
            return;
        }

        CanvasManager canvas = Global.MainCanvas.GetComponent<CanvasManager>();
        this.UIRolePart = canvas.LoadUIPrefab<UIRolePart>("MainGame/MainUI/UIRolePart");
        canvas.AddMainUI(this.UIRolePart);

        this.UIRolePart.RoleFaceSelected = (isSelected) => {
            if (this.UIBottomBar == null)
            {
                return;
            }

            if (!isSelected)
            {
                this.UIBottomBar.ShowUISkillBar();
            }
            else
            {
                this.UIBottomBar.ShowUIControlButtons();
            }
        };

        this.UIRolePart.RoleName = Global.Data.RoleData.RoleName;
        this.UIRolePart.RoleAvartaID = Global.Data.RoleData.RolePic;
        this.UIRolePart.FactionID = Global.Data.RoleData.FactionID;
        this.UIRolePart.PKMode = Global.Data.RoleData.PKMode;
        this.UIRolePart.HP = Global.Data.RoleData.CurrentHP;
        this.UIRolePart.HPMax = Global.Data.RoleData.MaxHP;
        this.UIRolePart.MP = Global.Data.RoleData.CurrentMP;
        this.UIRolePart.MPMax = Global.Data.RoleData.MaxMP;
        this.UIRolePart.Vitality = Global.Data.RoleData.CurrentStamina;
        this.UIRolePart.VitalityMax = Global.Data.RoleData.MaxStamina;
        this.UIRolePart.Exp = Global.Data.RoleData.Experience;
        this.UIRolePart.ExpMax = Global.Data.RoleData.MaxExperience;
        this.UIRolePart.RoleLevel = Global.Data.RoleData.Level;

        this.UIRolePart.UIBuffList.RefreshDataList();
        this.UIRolePart.UIPKSelection.PKModeSelected = (pkMode) => {
            GameInstance.Game.SpriteUpdatePKMode((int) pkMode);
        };
    }
    #endregion
    
    #region Kinh nghiệm
    /// <summary>
    /// Cập nhật kinh nghiệm và cấp độ của Leader
    /// </summary>
    protected void RefreshLeaderExpAndLevel()
    {
        if (this.UIRolePart == null)
        {
            return;
        }
        this.UIRolePart.RoleLevel = Global.Data.RoleData.Level;
        this.UIRolePart.Exp = Global.Data.RoleData.Experience;
        this.UIRolePart.ExpMax = Global.Data.RoleData.MaxExperience;
    }
    #endregion
}
