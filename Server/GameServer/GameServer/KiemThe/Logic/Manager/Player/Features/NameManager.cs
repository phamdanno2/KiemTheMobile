using System;
using System.Text;
using Server.Tools.Pattern;
using GameServer.Server;
using Server.TCP;
using Server.Protocol;
using Server.Tools;
using Server.Data;
using GameServer.KiemThe.Logic;
using System.Threading;
using GameServer.KiemThe;

namespace GameServer.Logic.Name
{
    /// <summary>
	/// Mã lỗi trả về khi đổi tên
	/// </summary>
	public enum ChangeNameError
    {
        /// <summary>
        /// Thành công
        /// </summary>
        Success = 0,
        /// <summary>
        /// Tên không hợp lệ
        /// </summary>
        InvalidName = 1,
        /// <summary>
        /// Lỗi DB
        /// </summary>
        DBFailed = 2,
        /// <summary>
        /// Tên đã được sử dụng
        /// </summary>
        NameAlreayUsed = 3,
        /// <summary>
        /// Nhân vật không tồn tại
        /// </summary>
        NotContainRole = 4,
    }

    /// <summary>
    /// Quản lý đổi tên
    /// </summary>
    public class NameManager : SingletonTemplate<NameManager>
    {
        /// <summary>
        /// Quản lý đổi tên
        /// </summary>
        private NameManager() { }

        /// <summary>
        /// Độ dài tên tối thiểu
        /// </summary>
        private const int NameMinLen = 6;

        /// <summary>
        /// Độ dài tên tối đa
        /// </summary>
        private const int NameMaxLen = 18;

        /// <summary>
        /// Sử dụng đa luồng
        /// </summary>
        private const bool UseMultiThread = true;

        /// <summary>
        /// Thực hiện đổi tên
        /// </summary>
        /// <param name="player"></param>
        /// <param name="newName"></param>
        private void DoChangeName(KPlayer player, string newName, Action<string, string> onSuccess)
		{
            try
            {
                /// Kết quả đổi tên
                string[] fields = Global.SendToDB((int) TCPGameServerCmds.CMD_SPR_CHANGE_NAME, string.Format("{0}:{1}:{2}:{3}", player.strUserID, player.ZoneID, player.RoleID, newName), GameManager.LocalServerId);
                /// Nếu có kết quả trả về
                if (fields != null && fields.Length == 2)
                {
                    /// Mã kết quả
                    int ret = int.Parse(fields[0]);
                    /// Tên cũ
                    string oldName = fields[1];

                    /// Nếu tên không hợp lệ
                    if (ret == (int) ChangeNameError.InvalidName)
                    {
                        PlayerManager.ShowNotification(player, "Tên có chứa ký tự đặc biệt, không thể đổi!");
                    }
                    /// Nếu tên không hợp lệ
                    else if (ret == (int) ChangeNameError.DBFailed)
                    {
                        PlayerManager.ShowNotification(player, "Lỗi hệ thống, hãy thử lại sau!");
                    }
                    /// Nếu tên không hợp lệ
                    else if (ret == (int) ChangeNameError.NameAlreayUsed)
                    {
                        PlayerManager.ShowNotification(player, "Tên đã được sử dụng, hãy chọn một tên khác!");
                    }
                    /// Nếu tên không hợp lệ
                    else if (ret == (int) ChangeNameError.NotContainRole)
                    {
                        PlayerManager.ShowNotification(player, "Nhân vật không tồn tại, hãy thử lại!");
                    }
                    /// Nếu thành công
                    else if (ret == (int) ChangeNameError.Success)
                    {
                        PlayerManager.ShowNotification(player, "Đổi tên thành công!");
                        /// Thực thi sự kiện đổi tên thành công
                        this.OnChangeNameSuccess(player, oldName, newName);
                        /// Thực hiện Callback
                        onSuccess?.Invoke(oldName, newName);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Exception, ex.ToString());
            }
        }

        /// <summary>
        /// Thực hiện đổi tên
        /// </summary>
        /// <param name="player"></param>
        /// <param name="newName"></param>
        /// <param name="onSuccess"></param>
        public void ProcessChangeName(KPlayer player, string newName, Action<string, string> onSuccess)
		{
            /// Toác
            if (player == null)
			{
                return;
			}

            /// Nếu là liên máy chủ
            if (player.ClientSocket.IsKuaFuLogin)
            {
                PlayerManager.ShowNotification(player, "Ở liên máy chủ không cho phép đổi tên!");
                return;
            }

            /// Nếu tên trống hoặc có ký tự đặc biệt
            if (string.IsNullOrEmpty(newName) || !Utils.CheckValidString(newName))
            {
                PlayerManager.ShowNotification(player, "Tên có chứa ký tự đặc biệt, không thể đổi!");
                return;
            }
            /// Nếu độ dài của tên không hợp lệ
            else if (!this.IsNameLengthOK(newName))
            {
                PlayerManager.ShowNotification(player, string.Format("Tên phải có độ dài từ {0} đến {1} ký tự!", NameManager.NameMinLen, NameManager.NameMaxLen));
                return;
            }

            //---------------fix jackson kiểm tra đổi tên có ký tự ; _ làm lỗi công thành chiến
            if (newName.Contains(";") || newName.Contains("_"))
            {
                PlayerManager.ShowNotification(player, "Tên có chứa ký tự đặc biệt, không thể đổi...001");
                return;
            }

            /// Nếu sử dụng đa luồng
            if (NameManager.UseMultiThread)
			{
                Thread thread = new Thread(() => {
                    this.DoChangeName(player, newName, onSuccess);
                });
                thread.IsBackground = false;
                thread.Start();
            }
			/// Nếu không sử dụng đa luồng
			else
			{
                this.DoChangeName(player, newName, onSuccess);
            }
        }

        /// <summary>
        /// Hàm gọi khi đổi tên thành công
        /// </summary>
        /// <param name="player"></param>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        private void OnChangeNameSuccess(KPlayer player, string oldName, string newName)
        {
            if (string.IsNullOrEmpty(oldName) || string.IsNullOrEmpty(newName))
            {
                return;
            }

            /// Thực thi sự kiện đổi tên thành công
            RoleName2IDs.OnChangeName(player.RoleID, oldName, newName);


            /// Thiết lập tên cho bản thân
            player.GetRoleData().RoleName = newName;
            /// Thông báo tới thằng khác bản thân đổi tên
            KT_TCPHandler.NotifyOthersMyNameChanged(player);
        }

        /// <summary>
        /// Kiểm tra độ dài tên có hợp lệ không
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsNameLengthOK(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            if (name.Length < NameManager.NameMinLen || name.Length > NameManager.NameMaxLen)
            {
                return false;
            }

            return true;
        }
    }
}

