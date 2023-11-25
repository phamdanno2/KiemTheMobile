﻿using FSPlay.GameEngine.Logic;
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
        #region Mở/đóng khung bất kỳ
        /// <summary>
        /// Nhận yêu cầu mở khung bất kỳ
        /// </summary>
        /// <param name="cmdID"></param>
        /// <param name="bytes"></param>
        /// <param name="length"></param>
        public static void ReceiveOpenUI(int cmdID, byte[] bytes, int length)
        {
            G2C_OpenUI openUI = DataHelper.BytesToObject<G2C_OpenUI>(bytes, 0, length);
            if (openUI == null)
            {
                return;
            }

            switch (openUI.UIName)
            {
                case "UIEnhance":
                {
                    PlayZone.GlobalPlayZone.OpenUIEnhance();
                    break;
                }
                case "UISignetEnhance":
                {
                    PlayZone.GlobalPlayZone.OpenUISignetEnhance();
                    break;
                }
                case "UICrystalStoneSynthesis":
                {
                    PlayZone.GlobalPlayZone.OpenUICrystalStoneSynthesis();
                    break;
                }
                case "UISplitEquipCrystalStones":
                {
                    PlayZone.GlobalPlayZone.OpenUISplitEquipCrystalStones(openUI.Parameters[0], openUI.Parameters[1] / 100f);
                    break;
                }
                case "UISplitCrystalStone":
                {
                    PlayZone.GlobalPlayZone.OpenUISplitCrystalStone(openUI.Parameters[0], openUI.Parameters[1] / 100f);
                    break;
                }
                case "UICreateGuild":
				{
                    PlayZone.GlobalPlayZone.OpenUICreateGuild();
                    break;
				}
                case "UICreateFamily":
                {
                    PlayZone.GlobalPlayZone.OpenUICreateFamily();
                    break;
                }
                case "UIFamilyList":
                {
                    KT_TCPHandler.SendToSeverFamilyList();
                    break;
                }
                case "UIGiftCode":
                {
                    PlayZone.GlobalPlayZone.OpenUIGiftCode();
                    break;
                }
                case "UIEquipLevelUp":
                {
                    PlayZone.GlobalPlayZone.OpenUIEquipLevelUp();
                    break;
                }
                case "UIEquipRefineToFS":
                {
                    PlayZone.GlobalPlayZone.OpenUIEquipRefineToFS();
                    break;
                }
                default:
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Nhận yêu cầu đóng khung bất kỳ
        /// </summary>
        /// <param name="cmdID"></param>
        /// <param name="bytes"></param>
        /// <param name="length"></param>
        public static void ReceiveCloseUI(int cmdID, byte[] bytes, int length)
        {
            string strCmd = new ASCIIEncoding().GetString(bytes);

            switch (strCmd)
            {
                case "UIEnhance":
                {
                    PlayZone.GlobalPlayZone.CloseUIEnhance();
                    break;
                }
                case "UICrystalStoneSynthesis":
                {
                    PlayZone.GlobalPlayZone.CloseUICrystalStoneSynthesis();
                    break;
                }
                case "UISplitEquipCrystalStones":
                {
                    PlayZone.GlobalPlayZone.CloseUISplitEquipCrystalStones();
                    break;
                }
                case "UISplitCrystalStone":
                {
                    PlayZone.GlobalPlayZone.CloseUISplitCrystalStone();
                    break;
                }
                default:
                {
                    break;
                }
            }
        }
        #endregion

        #region Nhập danh sách vật phẩm
        /// <summary>
        /// Nhận yêu cầu từ Server mở khung nhập danh sách vật phẩm
        /// </summary>
        /// <param name="cmdID"></param>
        /// <param name="bytes"></param>
        /// <param name="length"></param>
        public static void ReceiveShowInputItems(int cmdID, byte[] bytes, int length)
        {
            G2C_InputItems inputItems = DataHelper.BytesToObject<G2C_InputItems>(bytes, 0, length);
            if (inputItems == null)
            {
                return;
            }

            /// Mở khung
            PlayZone.GlobalPlayZone.OpenUIInputItems(inputItems.Title, inputItems.Description, inputItems.OtherDetail, inputItems.Tag);
        }

        /// <summary>
        /// Gửi thông tin vật phẩm nhập vào
        /// </summary>
        /// <param name="items"></param>
        /// <param name="tag"></param>
        public static void SendInputItems(List<GoodsData> items, string tag)
        {
            C2G_InputItems inputItems = new C2G_InputItems()
            {
                Items = items,
                Tag = tag,
            };
            byte[] bytes = DataHelper.ObjectToBytes<C2G_InputItems>(inputItems);
            GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_SHOW_INPUTITEMS)));
        }
        #endregion

        #region Nhập trang bị và danh sách vật phẩm nguyên liệu
        /// <summary>
        /// Nhận yêu cầu từ Server mở khung nhập trang bị và danh sách vật phẩm nguyên liệu
        /// </summary>
        /// <param name="cmdID"></param>
        /// <param name="bytes"></param>
        /// <param name="length"></param>
        public static void ReceiveShowInputEquipAndMaterials(int cmdID, byte[] bytes, int length)
        {
            G2C_InputEquipAndMaterials inputItems = DataHelper.BytesToObject<G2C_InputEquipAndMaterials>(bytes, 0, length);
            if (inputItems == null)
            {
                return;
            }

            /// Mở khung
            PlayZone.GlobalPlayZone.OpenUIInputEquipAndMaterials(inputItems.Title, inputItems.Description, inputItems.OtherDetail, inputItems.MustIncludeMaterials, inputItems.Tag);
        }

        /// <summary>
        /// Gửi thông tin trang bị và danh sách vật phẩm nguyên liệu
        /// </summary>
        /// <param name="equip"></param>
        /// <param name="items"></param>
        /// <param name="tag"></param>
        public static void SendInputEquipAndMaterials(GoodsData equip, List<GoodsData> items, string tag)
        {
            C2G_InputEquipAndMaterials inputItems = new C2G_InputEquipAndMaterials()
            {
                Equip = equip,
                Materials = items,
                Tag = tag,
            };
            byte[] bytes = DataHelper.ObjectToBytes<C2G_InputEquipAndMaterials>(inputItems);
            GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_SHOW_INPUTEQUIPANDMATERIALS)));
        }
		#endregion

		#region GiftCode
        /// <summary>
        /// Gửi yêu cầu nhập GiftCode
        /// </summary>
        /// <param name="inputString"></param>
        public static void SendInputGiftCode(string inputString)
		{
            string strCmd = string.Format("{0}:{1}", Global.Data.RoleData.RoleID, inputString);
            byte[] bytes = new UTF8Encoding().GetBytes(strCmd);
            GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_SPR_GETSONGLIGIFT)));
        }
		#endregion
	}
}
