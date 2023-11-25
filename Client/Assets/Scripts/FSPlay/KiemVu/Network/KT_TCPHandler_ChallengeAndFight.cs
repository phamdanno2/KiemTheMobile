using FSPlay.GameEngine.Logic;
using FSPlay.GameEngine.Network;
using FSPlay.GameEngine.Sprite;
using HSGameEngine.GameEngine.Network.Protocol;
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
        #region Tỷ thí
        /// <summary>
        /// Gửi lời mời tỷ thí đến người chơi ID tương ứng
        /// </summary>
        /// <param name="roleID"></param>
        public static void SendAskChallenge(int roleID)
        {
            string strCmd = string.Format("{0}", roleID);
            byte[] bytes = new ASCIIEncoding().GetBytes(strCmd);
            GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_ASK_CHALLENGE)));
        }

        /// <summary>
        /// Nhận gói tin thông báo lời mời tỷ thí
        /// </summary>
        /// <param name="fields"></param>
        public static void ReceiveAskChallenge(string[] fields)
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }

            /// ID đối tượng gửi lời mời
            int inviterID = int.Parse(fields[0]);
            /// Tên đối tượng gửi lời mời
            string inviterName = fields[1];

            KTGlobal.ShowMessageBox("Mời thách đấu", string.Format("Người chơi <color=#66daf4>[{0}]</color> muốn tỷ thí với bạn, đồng ý không?", inviterName), () => {
                KT_TCPHandler.SendAgreeChallenge(inviterID);
            }, () => {
                KT_TCPHandler.SendRefuseChallenge(inviterID);
            });
        }

        /// <summary>
        /// Gửi yêu cầu đồng ý lời mời thách đấu
        /// </summary>
        /// <param name="inviterID"></param>
        public static void SendAgreeChallenge(int inviterID)
        {
            string strCmd = string.Format("{0}:{1}", inviterID, 1);
            byte[] bytes = new ASCIIEncoding().GetBytes(strCmd);
            GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_C2G_RESPONSE_CHALLENGE)));
        }

        /// <summary>
        /// Gửi yêu cầu từ chối lời mời thách đấu
        /// </summary>
        /// <param name="inviterID"></param>
        public static void SendRefuseChallenge(int inviterID)
        {
            string strCmd = string.Format("{0}:{1}", inviterID, 0);
            byte[] bytes = new ASCIIEncoding().GetBytes(strCmd);
            GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_C2G_RESPONSE_CHALLENGE)));
        }

        /// <summary>
        /// Nhận thông báo bắt đầu tỷ thí
        /// </summary>
        /// <param name="fields"></param>
        public static void ReceiveBeginChallenge(string[] fields)
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }

            /// ID đối tượng tỷ thí
            int partnerID = int.Parse(fields[0]);
            /// Tên đối tượng tỷ thí
            string partnerName = fields[1];

            KTGlobal.AddNotification(string.Format("<color=red>Bắt đầu tỷ thí với <color=yellow>{0}</color>!</color>", partnerName));
            KTGlobal.ChallengePartnerID = partnerID;
        }

        /// <summary>
        /// Nhận thông báo kết thúc tỷ thí
        /// </summary>
        /// <param name="fields"></param>
        public static void RecieveStopChallenge(string[] fields)
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }

            /// ID đối tượng thắng
            int winnerID = int.Parse(fields[0]);
            /// ID đối phương
            int partnerID = int.Parse(fields[1]);
            /// Tên đối phương
            string targetName = fields[2];

            KTGlobal.AddNotification(string.Format("<color=red>Kết thúc tỷ thí với <color=yellow>{0}</color>!</color>", targetName));
            KTGlobal.ChallengePartnerID = -1;

            /// Nếu có đối tượng thắng cuộc
            if (winnerID != -1)
            {
                /// Đối phương
                GSprite partner = KTGlobal.FindSpriteByID(partnerID);

                /// Nếu bản thân thắng
                if (winnerID == Global.Data.RoleData.RoleID)
                {
                    KTGlobal.PlayChallengeEffect(Global.Data.Leader, true);
                    if (partner != null)
                    {
                        KTGlobal.PlayChallengeEffect(partner, false);
                    }
                }
                /// Nếu bản thân thua
                else
                {
                    KTGlobal.PlayChallengeEffect(Global.Data.Leader, false);
                    if (partner != null)
                    {
                        KTGlobal.PlayChallengeEffect(partner, true);
                    }
                }
            }
        }
        #endregion

        #region Tuyên chiến
        /// <summary>
        /// Gửi yêu cầu tuyên chiến với người chơi có ID tương ứng
        /// </summary>
        /// <param name="roleID"></param>
        public static void SendActiveFight(int roleID)
        {
            string strCmd = string.Format("{0}", roleID);
            byte[] bytes = new ASCIIEncoding().GetBytes(strCmd);
            GameInstance.Game.GameClient.SendData(TCPOutPacket.MakeTCPOutPacket(GameInstance.Game.GameClient.OutPacketPool, bytes, 0, bytes.Length, (int) (TCPGameServerCmds.CMD_KT_ASK_ACTIVEFIGHT)));
        }

        /// <summary>
        /// Nhận thông báo bắt đầu tuyên chiến
        /// </summary>
        /// <param name="fields"></param>
        public static void ReceiveBeginActiveFight(string[] fields)
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }

            /// ID đối tượng tuyên chiến
            int targetID = int.Parse(fields[0]);
            /// Tên đối tượng tuyên chiến
            string targetName = fields[1];

            if (!KTGlobal.ActiveFightWith.Contains(targetID))
            {
                KTGlobal.ActiveFightWith.Add(targetID);
            }
        }

        /// <summary>
        /// Nhận thông báo kết thúc tuyên chiến
        /// </summary>
        /// <param name="fields"></param>
        public static void RecieveStopActiveFight(string[] fields)
        {
            if (!Global.Data.PlayGame)
            {
                return;
            }

            /// ID đối tượng tuyên chiến
            int targetID = int.Parse(fields[0]);
            /// Tên đối tượng tuyên chiến
            string targetName = fields[1];

            if (KTGlobal.ActiveFightWith.Contains(targetID))
            {
                KTGlobal.ActiveFightWith.Remove(targetID);
            }
        }
        #endregion
    }
}
