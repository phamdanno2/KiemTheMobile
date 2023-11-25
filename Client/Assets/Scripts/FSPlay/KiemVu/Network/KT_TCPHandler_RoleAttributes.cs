using FSPlay.GameEngine.Logic;
using FSPlay.GameEngine.Network;
using FSPlay.GameEngine.Sprite;
using FSPlay.KiemVu.Factory.UIManager;
using HSGameEngine.GameEngine.Network.Protocol;
using Server.Data;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static FSPlay.KiemVu.Entities.Enum;

namespace FSPlay.KiemVu.Network
{
    /// <summary>
    /// Quản lý tương tác với Socket
    /// </summary>
    public static partial class KT_TCPHandler
    {
        #region RoleAttributes
        /// <summary>
        /// Nhận dữ liệu thông tin chỉ số nhân vật
        /// </summary>
        /// <param name="cmdID"></param>
        /// <param name="bytes"></param>
        /// <param name="length"></param>
        public static void ReceiveRoleAttributes(int cmdID, byte[] bytes, int length)
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }

            KeyValuePair<RoleAttributes, bool> pairData = DataHelper.BytesToObject<KeyValuePair<RoleAttributes, bool>>(bytes, 0, length);

            bool isShowUI = pairData.Value;
            RoleAttributes attributes = pairData.Key;
            if (attributes == null)
            {
                return;
            }

            if (PlayZone.GlobalPlayZone.UIRoleInfo == null && isShowUI)
            {
                PlayZone.GlobalPlayZone.ShowUIRoleInfo();
            }

            if (PlayZone.GlobalPlayZone.UIRoleInfo != null)
            {
                PlayZone.GlobalPlayZone.UIRoleInfo.RoleAttributes = attributes;
            }
        }

        /// <summary>
        /// Gửi gói tin lấy chỉ số nhân vật lên Server
        /// </summary>
        public static void SendGetRoleAttributes()
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }

            /// ID kỹ năng đang ở ô chính
            int skillID = -1;
            /// Lấy kỹ năng ở vị trí mặc định
            if (!string.IsNullOrEmpty(Global.Data.RoleData.MainQuickBarKeys))
            {
                string quickKey = Global.Data.RoleData.MainQuickBarKeys;
                string[] keys = quickKey.Split('|');
                if (keys.Length == 10)
                {
                    try
                    {
                        skillID = int.Parse(keys[0]);
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            string strcmd = string.Format("{0}:{1}", GameInstance.Game.CurrentSession.RoleID, skillID);
            byte[] bytes = new UTF8Encoding().GetBytes(strcmd);
            GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_ROLE_ATRIBUTES)));
        }
        #endregion

        #region Avarta nhân vật
        /// <summary>
        /// Nhận thông báo Avarta nhân vật thay đổi
        /// </summary>
        /// <param name="fields"></param>
        public static void ReceiveRoleAvartaChanged(string[] fields)
        {
            try
            {
                /// ID đối tượng
                int roleID = int.Parse(fields[0]);
                /// ID Avarta
                int avartaID = int.Parse(fields[1]);

                /// Nếu là bản thân
                if (Global.Data.RoleData.RoleID == roleID)
                {
                    /// Cập nhật ID Avarta
                    Global.Data.RoleData.RolePic = avartaID;
                }
                /// Nếu là người chơi khác
                else if (Global.Data.OtherRoles.TryGetValue(roleID, out RoleData rd))
                {
                    /// Cập nhật ID Avarta
                    rd.RolePic = avartaID;
                }

                /// Thông báo cập nhật sự thay đổi Avarta của đối tượng tương ứng
                UIRoleAvartaManager.Instance.UpdateAvarta(roleID, avartaID);
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Gửi yêu cầu thay đổi Avarta nhân vật
        /// </summary>
        /// <param name="avartaID"></param>
        public static void SendRoleAvartaChanged(int avartaID)
        {
            string strCmd = string.Format("{0}", avartaID);
            byte[] bytes = new ASCIIEncoding().GetBytes(strCmd);
            GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_CHANGE_AVARTA)));

        }
        #endregion

        #region Tinh hoạt lực, kỹ năng sống
        /// <summary>
        /// Nhận thông báo tinh hoạt lực thay đổi
        /// </summary>
        /// <param name="fields"></param>
        public static void ReceiveUpdateGatherMakePoint(string[] fields)
        {
            try
            {
                int gatherPoint = int.Parse(fields[0]);
                int makePoint = int.Parse(fields[1]);

                /// Giá trị cũ
                int oldGather = Global.Data.RoleData.GatherPoint;
                int oldMake = Global.Data.RoleData.MakePoint;

                /// Thiết lập giá trị
                Global.Data.RoleData.GatherPoint = gatherPoint;
                Global.Data.RoleData.MakePoint = makePoint;

                /// Nếu tăng thì thông báo
                if (oldGather < gatherPoint)
                {
                    int nAdd = gatherPoint - oldGather;
                    KTGlobal.ShowTextForExpMoneyOrGatherMakePoint(BottomTextDecorationType.Gather, string.Format("Tinh lực +{0}", nAdd));
                }
                if (oldMake < makePoint)
                {
                    int nAdd = makePoint - oldMake;
                    KTGlobal.ShowTextForExpMoneyOrGatherMakePoint(BottomTextDecorationType.Make, string.Format("Hoạt lực +{0}", nAdd));
                }

                /// Cập nhật dữ liệu cho UI
                if (PlayZone.GlobalPlayZone.UIRoleInfo != null)
                {
                    PlayZone.GlobalPlayZone.UIRoleInfo.UpdateRoleData();
                }
                if (PlayZone.GlobalPlayZone.UICrafting != null)
                {
                    PlayZone.GlobalPlayZone.UICrafting.UpdatePoints();
                }
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Gói tin thông tin kỹ năng sống (cấp độ, kinh nghiệm)
        /// </summary>
        /// <param name="fields"></param>
        public static void ReceiveUpdateLifeSkillLevel(string[] fields)
        {
            try
            {
                int lifeSkillID = int.Parse(fields[0]);
                int level = int.Parse(fields[1]);
                int exp = int.Parse(fields[2]);

                /// Nếu dữ liệu tồn tại
                if (Global.Data.RoleData.LifeSkills.TryGetValue(lifeSkillID, out LifeSkillPram lifeSkillParam))
                {
                    lifeSkillParam.LifeSkillLevel = level;
                    lifeSkillParam.LifeSkillExp = exp;
                }

                if (PlayZone.GlobalPlayZone.UICrafting != null)
                {
                    PlayZone.GlobalPlayZone.UICrafting.UpdateCurrentLifeSkillLevelAndExp(lifeSkillID);
                }
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Gửi yêu cầu chế đồ
        /// </summary>
        /// <param name="recipeID"></param>
        public static void SendCraftItem(int recipeID)
        {
            string strCmd = string.Format("{0}", recipeID);
            byte[] bytes = new ASCIIEncoding().GetBytes(strCmd);
            GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_BEGIN_CRAFT)));
        }

        /// <summary>
        /// Gói tin thông báo bắt đầu chế tạo
        /// </summary>
        /// <param name="fields"></param>
        public static void ReceiveBeginCraft(string[] fields)
        {
            try
            {
                if (PlayZone.GlobalPlayZone.UICrafting != null)
                {
                    PlayZone.GlobalPlayZone.UICrafting.StartProgress();
                }
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Gói tin thông báo hoàn tất chế tạo
        /// </summary>
        /// <param name="fields"></param>
        public static void ReceiveFinishCraft(string[] fields)
        {
            try
            {
                int lifeSkillID = int.Parse(fields[0]);
                /// Nếu thao tác thất bại
                if (lifeSkillID == -1)
                {
                    if (PlayZone.GlobalPlayZone.UICrafting != null)
                    {
                        PlayZone.GlobalPlayZone.UICrafting.FinishProgress();
                    }
                }
                /// Nếu thao tác thành công
                else
                {
                    int level = int.Parse(fields[1]);
                    int exp = int.Parse(fields[2]);

                    /// Nếu dữ liệu tồn tại
                    if (Global.Data.RoleData.LifeSkills.TryGetValue(lifeSkillID, out LifeSkillPram lifeSkillParam))
                    {
                        lifeSkillParam.LifeSkillLevel = level;
                        lifeSkillParam.LifeSkillExp = exp;
                    }

                    if (PlayZone.GlobalPlayZone.UICrafting != null)
                    {
                        PlayZone.GlobalPlayZone.UICrafting.UpdateCurrentLifeSkillLevelAndExp(lifeSkillID);
                        PlayZone.GlobalPlayZone.UICrafting.FinishProgress();
                    }
                }
            }
            catch (Exception) { }
        }
        #endregion

        #region Tên và danh hiệu thay đổi
        /// <summary>
        /// Nhận thông báo tên đối tượng thay đổi
        /// </summary>
        /// <param name="cmdID"></param>
        /// <param name="bytes"></param>
        /// <param name="length"></param>
        public static void ReceiveNameChanged(int cmdID, byte[] bytes, int length)
        {
            try
            {
                G2C_RoleNameChanged nameChanged = DataHelper.BytesToObject<G2C_RoleNameChanged>(bytes, 0, length);

                /// Đối tượng tương ứng
                GSprite sprite = KTGlobal.FindSpriteByID(nameChanged.RoleID);

                /// Nếu là bản thân
                if (Global.Data.RoleData.RoleID == nameChanged.RoleID)
                {
                    Global.Data.RoleData.RoleName = nameChanged.RoleName;
                    sprite.ComponentCharacter.UpdateName();
                    /// Cập nhật vào UI
                    if (PlayZone.GlobalPlayZone.UIRolePart != null)
					{
                        PlayZone.GlobalPlayZone.UIRolePart.RoleName = nameChanged.RoleName;
                    }
                }
                /// Nếu là người chơi khác
                else if (Global.Data.OtherRoles.TryGetValue(nameChanged.RoleID, out RoleData rd))
                {
                    rd.RoleName = nameChanged.RoleName;
                    sprite.ComponentCharacter.UpdateName();
                }
                /// Nếu là quái
                else if (Global.Data.SystemMonsters.TryGetValue(nameChanged.RoleID, out MonsterData md))
                {
                    md.RoleName = nameChanged.RoleName;
                    sprite.ComponentMonster.UpdateName();
                }
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Nhận thông báo danh hiệu đối tượng thay đổi
        /// </summary>
        /// <param name="cmdID"></param>
        /// <param name="bytes"></param>
        /// <param name="length"></param>
        public static void ReceiveTitleChanged(int cmdID, byte[] bytes, int length)
        {
            try
            {
                G2C_RoleTitleChanged titleChanged = DataHelper.BytesToObject<G2C_RoleTitleChanged>(bytes, 0, length);

                /// Đối tượng tương ứng
                GSprite sprite = KTGlobal.FindSpriteByID(titleChanged.RoleID);

                /// Nếu là bản thân
                if (Global.Data.RoleData.RoleID == titleChanged.RoleID)
                {
                    Global.Data.RoleData.Title = titleChanged.Title;
                    Global.Data.RoleData.GuildTitle = titleChanged.GuildTitle;
                    sprite.ComponentCharacter.UpdateGuildTitle();
                    sprite.ComponentCharacter.UpdateTitle();
                }
                /// Nếu là người chơi khác
                else if (Global.Data.OtherRoles.TryGetValue(titleChanged.RoleID, out RoleData rd))
                {
                    rd.Title = titleChanged.Title;
                    rd.GuildTitle = titleChanged.GuildTitle;
                    sprite.ComponentCharacter.UpdateGuildTitle();
                    sprite.ComponentCharacter.UpdateTitle();
                }
                /// Nếu là quái
                else if (Global.Data.SystemMonsters.TryGetValue(titleChanged.RoleID, out MonsterData md))
                {
                    md.Title = titleChanged.Title;
                    sprite.ComponentMonster.UpdateTitle();
                }
            }
            catch (Exception) { }
        }
        #endregion

        #region Danh vọng và vinh dự
        /// <summary>
        /// Nhận thông báo vinh dự tài phú thay đổi
        /// </summary>
        /// <param name="fields"></param>
        public static void ReciveRoleValueChanged(string[] fields)
        {
            try
            {
                int roleID = int.Parse(fields[0]);
                long roleValue = int.Parse(fields[1]);

                /// Đối tượng tương ứng
                GSprite sprite = KTGlobal.FindSpriteByID(roleID);

                /// Nếu là bản thân
                if (Global.Data.RoleData.RoleID == roleID)
                {
                    Global.Data.RoleData.TotalValue = roleValue;
                }
                /// Nếu là người chơi khác
                else if (Global.Data.OtherRoles.TryGetValue(roleID, out RoleData rd))
                {
                    rd.TotalValue = roleValue;
                }

                /// Cập nhật hiển thị Phi Phong
                sprite.ComponentCharacter.UpdateRoleValue();

                /// Nếu đang hiện khng
                if (PlayZone.GlobalPlayZone.UIRoleInfo != null)
                {
                    PlayZone.GlobalPlayZone.UIRoleInfo.UpdateRoleData();
                }
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Nhận thông báo danh vọng thay đổi
        /// </summary>
        /// <param name="cmdID"></param>
        /// <param name="bytes"></param>
        /// <param name="length"></param>
        public static void ReceiveReputeChanged(int cmdID, byte[] bytes, int length)
        {
            try
            {
                ReputeInfo reputeInfo = DataHelper.BytesToObject<ReputeInfo>(bytes, 0, length);
                if (reputeInfo == null)
                {
                    return;
                }

                ReputeInfo oldReputeInfo = Global.Data.RoleData.Repute.Where(x => x.DBID == reputeInfo.DBID).FirstOrDefault();
                oldReputeInfo.Level = reputeInfo.Level;
                oldReputeInfo.Exp = reputeInfo.Exp;

                /// Nếu đang hiển thị khung
                if (PlayZone.GlobalPlayZone.UIRoleInfo != null)
                {
                    PlayZone.GlobalPlayZone.UIRoleInfo.RefreshRepute();
                }
            }
            catch (Exception) { }
        }
        #endregion

        #region Thay đổi trạng thái cưỡi
        /// <summary>
        /// Nhận thông báo từ Server trạng thái cưỡi của đối tượng thay đổi
        /// </summary>
        /// <param name="fields"></param>
        public static void ReceiveHorseStateChanged(string[] fields)
        {
            try
            {
                int roleID = int.Parse(fields[0]);
                int state = int.Parse(fields[1]);

                /// Đối tượng tương ứng
                GSprite sprite = null;

                /// Nếu là bản thân
                if (Global.Data.RoleData.RoleID == roleID)
                {
                    Global.Data.RoleData.IsRiding = state == 1;
                    sprite = Global.Data.Leader;
                }
                /// Nếu là người chơi khác
                else if (Global.Data.OtherRoles.TryGetValue(roleID, out RoleData rd))
                {
                    rd.IsRiding = state == 1;
                    sprite = KTGlobal.FindSpriteByID(roleID);
                }

                /// Nếu không tìm thấy đối tượng tương ứng
                if (sprite == null)
                {
                    return;
                }

                /// Cập nhật trạng thái
                sprite.ComponentCharacter.Data.IsRiding = state == 1;
                /// Tiếp tục động tác hiện tại
                sprite.ComponentCharacter.ResumeCurrentAction();
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Gửi sự kiện thay đổi trạng thái cưỡi lên Server
        /// </summary>
        /// <param name="state"></param>
        public static void SendChangeToggleHorseState()
        {
            byte[] cmdData = new ASCIIEncoding().GetBytes("");
            GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, cmdData, 0, cmdData.Length, (int) (TCPGameServerCmds.CMD_KT_TOGGLE_HORSE_STATE)));
        }
        #endregion

        #region Đổi trang bị dự phòng
        /// <summary>
        /// Gửi yêu cầu đổi set dự phòng
        /// </summary>
        public static void SendChangeSubEquip()
        {
            byte[] bytes = new ASCIIEncoding().GetBytes("");
            GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_C2G_CHANGE_SUBSET)));
        }
		#endregion

		#region Bang hội, gia tộc và chức vụ
        /// <summary>
        /// Nhận thông báo bang hội và gia tộc thay đổi
        /// </summary>
        /// <param name="fields"></param>
        public static void ReceiveGuildAndFamilyRankChange(string[] fields)
		{
            int roleID = int.Parse(fields[0]);
            int guildID = int.Parse(fields[1]);
            int guildRank = int.Parse(fields[2]);
            int familyID = int.Parse(fields[3]);
            int familyRank = int.Parse(fields[4]);
            int officeRank = int.Parse(fields[5]);

            RoleData rd = null;
            /// Nếu là bản thân
            if (Global.Data.RoleData.RoleID == roleID)
			{
                rd = Global.Data.RoleData;
            }
            /// Nếu là người chơi khác
            else if (Global.Data.OtherRoles.TryGetValue(roleID, out rd))
			{

			}

            /// Nếu toác
            if (rd == null)
			{
                return;
			}

            /// Thiết lập dữ liệu
            rd.GuildID = guildID;
            rd.GuildRank = guildRank;
            rd.FamilyID = familyID;
            rd.FamilyRank = familyRank;

            /// Quan hàm cũ
            int oldOfficeRank = rd.OfficeRank;
            /// Gắn quan hàm mới
            rd.OfficeRank = officeRank;
            /// Nếu có sự thay đổi quan hàm
            if (rd.OfficeRank != oldOfficeRank)
			{
                /// Đối tượng tương ứng
                GSprite sprite = KTGlobal.FindSpriteByID(roleID);
                /// Nếu tồn tại
                if (sprite != null)
				{
                    sprite.ComponentCharacter.UpdateRoleOfficeRank();
				}
            }
        }
		#endregion

		#region Danh hiệu nhân vật
        /// <summary>
        /// Nhận thông báo danh hiệu nhân vật của người chơi thay đổi
        /// </summary>
        /// <param name="fields"></param>
        public static void ReceivePlayerRoleTitleChanged(string[] fields)
		{
            /// ID người chơi
            int roleID = int.Parse(fields[0]);
            /// ID danh hiệu
            int titleID = int.Parse(fields[1]);

            /// Đối tượng tương ứng
            GSprite sprite = KTGlobal.FindSpriteByID(roleID);
            /// Nếu đối tượng không tồn tại
            if (sprite == null)
			{
                return;
			}
            /// Nếu không phải người chơi
            else if (sprite.ComponentCharacter == null)
			{
                return;
			}

            /// Thiết lập danh hiệu hiện tại
            sprite.RoleData.SelfCurrentTitleID = titleID;

            /// Thực hiện cập nhật danh hiệu hiện tại
            sprite.ComponentCharacter.UpdateMyselfRoleTitle();

            /// Nếu đang mở khung nhân vật
            if (PlayZone.GlobalPlayZone.UIRoleInfo != null)
			{
                PlayZone.GlobalPlayZone.UIRoleInfo.RefreshCurrentTitle();
			}
		}

        /// <summary>
        /// Gửi yêu cầu thay đổi danh hiệu nhân vật của bản thân
        /// </summary>
        /// <param name="titleID"></param>
        public static void SendChangeMyselfRoleTitle(int titleID)
		{
            string strCmd = string.Format("{0}", titleID);
            byte[] cmdData = new ASCIIEncoding().GetBytes(strCmd);
            GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, cmdData, 0, cmdData.Length, (int) (TCPGameServerCmds.CMD_KT_UPDATE_CURRENT_ROLETITLE)));
        }

        /// <summary>
        /// Nhận thông báo thêm/xóa danh hiệu nhân vật của bản thân
        /// </summary>
        /// <param name="fields"></param>
        public static void ReceiveModRoleTitle(string[] fields)
		{
            /// Loại thao tác
            int method = int.Parse(fields[0]);
            /// ID danh hiệu
            int titleID = int.Parse(fields[1]);

            /// Nếu không tồn tại danh hiệu
            if (!Loader.Loader.RoleTitles.ContainsKey(titleID))
			{
                return;
			}

            /// Nếu là thêm
            if (method == 1)
			{
                /// Nếu danh sách đang NULL
                if (Global.Data.RoleData.SelfTitles == null)
				{
                    Global.Data.RoleData.SelfTitles = new List<int>();
				}

                /// Nếu trong danh sách chưa tồn tại
                if (!Global.Data.RoleData.SelfTitles.Contains(titleID))
                {
                    /// Thêm vào danh sách
                    Global.Data.RoleData.SelfTitles.Add(titleID);
                }
                

                /// Nếu đang hiển thị khung
                if (PlayZone.GlobalPlayZone.UIRoleInfo != null)
				{
                    PlayZone.GlobalPlayZone.UIRoleInfo.AddRoleTitle(titleID);
				}
			}
            /// Nếu là xóa
            else if (method == -1)
			{
                /// Xóa trong danh sách
                if (Global.Data.RoleData.SelfTitles != null)
                {
                    Global.Data.RoleData.SelfTitles.RemoveAll(x => x == titleID);
                }

                /// Nếu đang hiển thị khung
                if (PlayZone.GlobalPlayZone.UIRoleInfo != null)
                {
                    PlayZone.GlobalPlayZone.UIRoleInfo.RemoveRoleTitle(titleID);
                }
            }
		}
		#endregion

		#region Uy danh và vinh dự thay đổi
        /// <summary>
        /// Nhận gói tin thông báo uy danh và vinh dự thay đổi
        /// </summary>
        /// <param name="fields"></param>
        public static void ReceivePrestigeAndHonorChanged(string[] fields)
		{
            /// Uy danh
            int prestige = int.Parse(fields[0]);
            /// Vinh dự võ lâm
            int worldHonor = int.Parse(fields[1]);
            /// Vinh dự thi đấu môn phái
            int factionHonor = int.Parse(fields[2]);
            /// Vinh dự võ lâm liên đấu
            int worldMartial = int.Parse(fields[3]);

            /// Cập nhất vào RoleData
            Global.Data.RoleData.Prestige = prestige;
            Global.Data.RoleData.WorldHonor = worldHonor;

            /// Nếu đang mở khung soi thông tin nhân vật
            if (PlayZone.GlobalPlayZone.UIRoleInfo != null)
			{
                /// Cập nhật thay đổi vinh dự võ lâm và uy danh
                PlayZone.GlobalPlayZone.UIRoleInfo.UpdatePrestigeAndWorldHonor();
            }
		}
		#endregion

		#region Mật khẩu cấp 2
        /// <summary>
        /// Nhận yêu cầu mở khung nhập mật khẩu cấp 2
        /// </summary>
        public static void ReceiveOpenInputSecondPassword()
		{
            PlayZone.GlobalPlayZone.OpenUIInputSecondPassword();
		}

        /// <summary>
        /// Gửi phản hồi nhật mật khẩu cấp 2
        /// </summary>
        /// <param name="password"></param>
        public static void SendInputSecondPassword(string password)
		{
            string strCmd = string.Format("{0}", password);
            byte[] bytes = new ASCIIEncoding().GetBytes(strCmd);
            GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_INPUT_SECONDPASSWORD)));
        }
		#endregion
	}
}
