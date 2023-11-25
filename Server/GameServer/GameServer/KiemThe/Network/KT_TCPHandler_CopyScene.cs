using GameServer.Logic;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.KiemThe.Logic
{
    /// <summary>
    /// Quản lý phụ bản
    /// </summary>
    public static partial class KT_TCPHandler
    {
        #region Phụ bản

        /// <summary>
        /// Ghi lại thông tin người chơi trước khi vào phụ bản vào DB
        /// </summary>
        /// <param name="player"></param>
        /// <param name="copySceneID"></param>
        /// <param name="preMapCode"></param>
        /// <param name="prePosX"></param>
        /// <param name="prePosY"></param>
        /// <param name="copySceneCreateTicks"></param>
        public static void RecordCopySceneInfoToDB(KPlayer player, int copySceneID, int preMapCode, int prePosX, int prePosY, long copySceneCreateTicks)
        {
            /// Chuỗi lưu thông tin
            string cmdData = string.Format("{0},{1},{2},{3},{4}", copySceneID, preMapCode, prePosX, prePosY, copySceneCreateTicks);
            /// Lưu vào DB
            Global.SaveRoleParamsStringWithNullToDB(player, RoleParamName.CopySceneRecord, cmdData, true);
        }

        /// <summary>
        /// Đọc thông tin người chơi trước khi vào phụ bản từ DB
        /// </summary>
        /// <param name="player"></param>
        /// <param name="copySceneID"></param>
        /// <param name="preMapCode"></param>
        /// <param name="prePosX"></param>
        /// <param name="prePosY"></param>
        /// <param name="copySceneCreateTicks"></param>
        public static void GetPlayerCopySceneInfoFromDB(KPlayer player, out int copySceneID, out int preMapCode, out int prePosX, out int prePosY, out long copySceneCreateTicks)
        {
            /// Mặc định kết quả
            copySceneID = -1;
            preMapCode = -1;
            prePosX = -1;
            prePosY = -1;
            copySceneCreateTicks = 0;

            /// Chuỗi lưu thông tin
            string cmdData = Global.GetRoleParamsStringWithNullFromDB(player, RoleParamName.CopySceneRecord);
            /// Nếu có chuỗi lưu thông tin
            if (!string.IsNullOrEmpty(cmdData))
            {
                string[] parameters = cmdData.Split(',');
                if (parameters.Length == 5)
                {
                    try
                    {
                        copySceneID = int.Parse(parameters[0]);
                        preMapCode = int.Parse(parameters[1]);
                        prePosX = int.Parse(parameters[2]);
                        prePosY = int.Parse(parameters[3]);
                        copySceneCreateTicks = long.Parse(parameters[4]);
                    }
                    catch (Exception ex)
                    {
                        DataHelper.WriteFormatExceptionLog(ex, "GetPlayerCopySceneInfoFromDB got error...", false);
                    }
                }
            }
        }

        #endregion Phụ bản
    }
}
