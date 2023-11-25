using FSPlay.GameEngine.Logic;
using FSPlay.GameEngine.Network;
using HSGameEngine.GameEngine.Network.Protocol;
using Server.Data;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSPlay.KiemVu.Network
{
    /// <summary>
    /// Quản lý tương tác với Socket
    /// </summary>
    public static partial class KT_TCPHandler
    {
        /// <summary>
        /// Gửi yêu cầu Mở khung danh sách gia tộc
        /// </summary>
        public static void SendToSeverFamilyList()
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }
            string strcmd = string.Format("{0}", Global.Data.RoleData.RoleID);
            GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_KT_FAMILY_GETLISTFAMILY)));
        }

        /// <summary>
        /// Sự kiện xin vào tộc
        /// </summary>
        public static void SendToSeverFamilyAskToJoin(int familyID)
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }
            string strcmd = string.Format("{0}:{1}", Global.Data.RoleData.RoleID, familyID);
            GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_KT_FAMILY_REQUESTJOIN)));
        }

        /// <summary>
        /// Kích Thành viên ra khỏi tộc
        /// </summary>
        /// <param name="roleID"></param>
        public static void SendToSeverKickMemberOutFamily(int roleID)
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }
            string strcmd = string.Format("{0}:{1}", roleID, Global.Data.RoleData.FamilyID);
            GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_KT_FAMILY_KICKMEMBER)));
        }
        /// <summary>
        /// Bản thân rời tộc
        /// </summary>
        public static void SendToSeverOutFamily()
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }
            string strcmd = string.Format("{0}:{1}", Global.Data.RoleData.RoleID, Global.Data.RoleData.FamilyID);
            GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_KT_FAMILY_QUIT)));
        }

        /// <summary>
        /// Thay đổi chức vị
        /// </summary>
        /// <param name="roleID"></param>
        /// <param name="rank"></param>
        public static void SendToSeverChangeRank(int roleID, int rank)
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }
            string strcmd = string.Format("{0}:{1}", roleID, rank);
            GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_KT_FAMILY_CHANGE_RANK)));
        }

        /// <summary>
        /// Hủy Gia tộc
        /// </summary>
        /// <param name="familyInfo"></param>
        public static void SendToSeverDestroyFamily(Family family)
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }
            string strcmd = string.Format("{0}:{1}", Global.Data.RoleData.RoleID, family.FamilyID);
            GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_KT_FAMILY_DESTROY)));
        }
        /// <summary>
        /// Sự kiện mở khung gia tộc
        /// </summary>
        /// <param name="familyInfo"></param>
        public static void SendToSeverOpenFamily()
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }
            string strcmd = string.Format("{0}", Global.Data.RoleData.FamilyID);
            GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_KT_FAMILY_OPEN)));
        }
        /// <summary>
        /// Trả lời yêu cầu tham gia tộc
        /// </summary>
        /// <param name="familyInfo"></param>
        public static void SendToSeverFamilyResponseRequest(int RoleID, int Accpect)
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }
            string strcmd = string.Format("{0}:{1}:{2}", RoleID, Accpect, Global.Data.RoleData.FamilyID);
            GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_KT_FAMILY_RESPONSE_REQUEST)));
        }


        /// <summary>
        /// Thay đổi thông tin tôn chỉ gia chỉ
        /// </summary>
        /// <param name="Slogen"></param>
        public static void SendToSeverChangeSlogsn(string Slogen)
        {
            ChangeSlogenFamily changeSlogan = new ChangeSlogenFamily()
            {
                Slogen = Slogen,
                FamilyID = Global.Data.RoleData.FamilyID,
            };
            byte[] cmdData = DataHelper.ObjectToBytes<ChangeSlogenFamily>(changeSlogan);
            GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, cmdData, 0, cmdData.Length, (int) (TCPGameServerCmds.CMD_KT_FAMILY_CHANGENOTIFY)));

        }
        /// <summary>
        /// Thay đổi thông tin yêu vào vào tộc
        /// </summary>
        /// <param name="Slogen"></param>
        public static void SendToSeverChangeRequestFamily(string Slogen)
        {

            ChangeSlogenFamily changeSlogan = new ChangeSlogenFamily()
            {
                Slogen = Slogen,
                FamilyID = Global.Data.RoleData.FamilyID,
            };
            byte[] cmdData = DataHelper.ObjectToBytes<ChangeSlogenFamily>(changeSlogan);
            GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, cmdData, 0, cmdData.Length, (int) (TCPGameServerCmds.CMD_KT_FAMILY_CHANGE_REQUESTJOIN_NOTIFY)));
        }
        /// <summary>
        /// Tạo Gia tộc
        /// </summary>
        /// <param name="bangHuiName"></param>
        /// <param name="bhBulletin"></param>
        public static void SpriteCreateFamily(string NameFamily)
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }
            string strcmd = string.Format("{0}:{1}", Global.Data.RoleData.RoleID, NameFamily);
            GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, strcmd, (int) (TCPGameServerCmds.CMD_KT_FAMILY_CREATE)));
        }
    }
}