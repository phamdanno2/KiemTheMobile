using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using GameServer.Server;
using Server.Protocol;
using Server.TCP;
using Server.Tools;
using Server.Data;
using GameServer.Core.Executor;

namespace GameServer.Logic.SecondPassword
{
    public class SecPwdState
    {
        public string SecPwd = "";
        public DateTime AuthDeadTime = TimeUtil.NowDateTime();
        public bool NeedVerify = false;
    }

    public class SecondPasswordManager
    {
        private const int _PwdMinLen = 6;
        private const int _PwdMaxLen = 8;
        private static long _ValidSecWhenLogout = 300; //5分钟
        public static long ValidSecWhenLogout
        {
            get
            {
                return _ValidSecWhenLogout;
            }
            set
            {
                _ValidSecWhenLogout = value;
                if (_ValidSecWhenLogout < 0)
                {
                    _ValidSecWhenLogout = 300;
                }
            }
        }
        /// <summary>
        /// 这个字典必须要lock
        /// </summary>
        private static Dictionary<string, SecPwdState> _UsrSecPwdDict = new Dictionary<string, SecPwdState>();

        public static SecPwdState GetSecPwdState(string userid)
        {
            if (string.IsNullOrEmpty(userid))
            {
                return null;
            }

            SecPwdState result = null;
            lock (_UsrSecPwdDict)
            {
                _UsrSecPwdDict.TryGetValue(userid, out result);
            }
            return result;
        }

        public static void SetSecPwdState(string usrid, SecPwdState state)
        {
            if (string.IsNullOrEmpty(usrid))
            {
                return;
            }

            lock (_UsrSecPwdDict)
            {
                if (state != null)
                {
                    _UsrSecPwdDict[usrid] = state;
                }
                else
                {
                    _UsrSecPwdDict.Remove(usrid);
                }
            }
        }

        /// <summary>
        /// 设置二级密码
        /// </summary>
        public static TCPProcessCmdResults ProcessSetSecPwd(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;

            try
            {
                SetSecondPassword setReq = DataHelper.BytesToObject<SetSecondPassword>(data, 0, count);
                if (setReq == null)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("解析指令错误, cmd={0}", (int)nID));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                KPlayer client = GameManager.ClientMgr.FindClient(socket);
                if (null == client || client.RoleID != setReq.RoleID)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("根据RoleID定位GameClient对象失败, CMD={0}, Client={1}, RoleID={2}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), setReq.RoleID));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }


                //旧的加密后的二级密码, 如果尚未设置，则为空
                SecPwdState pwdState = GetSecPwdState(client.strUserID);
                string oldSecPwd_Encrypted = pwdState != null ? pwdState.SecPwd : null;
                SecondPasswordError error = SecondPasswordError.SecPwdSetSuccess;
                do
                {
                    if (!string.IsNullOrEmpty(oldSecPwd_Encrypted))
                    {
                        // 存在旧的二级密码，直接进行密文比较
                        if (setReq.OldSecPwd == null
                            || oldSecPwd_Encrypted != setReq.OldSecPwd)
                        {
                            error = SecondPasswordError.SecPwdVerifyFailed;
                            break;
                        }
                    }

                    //解密后的新二级密码
                    string newSecPwd_Decrypted = SecondPasswordRC4.Decrypt(setReq.NewSecPwd);
                    if (string.IsNullOrEmpty(newSecPwd_Decrypted))
                    {
                        error = SecondPasswordError.SecPwdIsNull;
                        break;
                    }

                    if (!Regex.IsMatch(newSecPwd_Decrypted, "^[a-zA-Z0-9_]+$"))
                    {
                        error = SecondPasswordError.SecPwdCharInvalid;
                        break;
                    }

                    if (newSecPwd_Decrypted.Length < _PwdMinLen)
                    {
                        error = SecondPasswordError.SecPwdIsTooShort;
                        break;
                    }

                    if (newSecPwd_Decrypted.Length > _PwdMaxLen)
                    {
                        error = SecondPasswordError.SecPwdIsTooLong;
                        break;
                    }

                    //直接存储密文
                    string cmd2db = string.Format("{0}:{1}", client.strUserID, setReq.NewSecPwd);
                    string[] dbFields = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_DB_UPDATE_USR_SECOND_PASSWORD, cmd2db, client.ServerId);
                    if (null == dbFields || dbFields.Length != 2)
                    {
                        error = SecondPasswordError.SecPwdDBFailed;
                        break;
                    }

                    error = SecondPasswordError.SecPwdSetSuccess;
                    break;

                } while (false);

                if (error == SecondPasswordError.SecPwdSetSuccess)
                {
                    // 设置二级密码成功了，更新状态
                    if (pwdState == null)
                    {
                        pwdState = new SecPwdState();
                    }
                    pwdState.SecPwd = setReq.NewSecPwd;
                    pwdState.NeedVerify = false;
                    SetSecPwdState(client.strUserID, pwdState);
                }

                int has = 0, need = 0;
                if (pwdState != null)
                {
                    has = 1;
                    need = pwdState.NeedVerify ? 1 : 0;
                }

                string rsp = string.Format("{0}:{1}:{2}:{3}", setReq.RoleID, (int)error, has, need);
                GameManager.ClientMgr.SendToClient(client, rsp, nID);
                return TCPProcessCmdResults.RESULT_OK;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(socket), false);
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }

        // 清除二级密码
        public static TCPProcessCmdResults ProcClrSecPwd(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;

            try
            {
                VerifySecondPassword verifyReq = DataHelper.BytesToObject<VerifySecondPassword>(data, 0, count);
                if (verifyReq == null)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("解析指令错误, cmd={0}", (int)nID));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                string uid = GameManager.OnlineUserSession.FindUserID(socket);
                if (string.IsNullOrEmpty(uid) || string.IsNullOrEmpty(verifyReq.UserID) || uid != verifyReq.UserID)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("玩家请求清除二级密码，但是玩家发送的uid错误, {0}", Global.GetSocketRemoteEndPoint(socket)));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                int errcode;

                SecPwdState pwdState = GetSecPwdState(verifyReq.UserID);
                if (pwdState == null)
                {
                    //没有二级密码，客户端却请求删除
                    errcode = (int)SecondPasswordError.SecPwdIsNotSet;
                }
                else
                {
                    if (string.IsNullOrEmpty(verifyReq.SecPwd) || verifyReq.SecPwd != pwdState.SecPwd)
                    {
                        errcode = (int)SecondPasswordError.SecPwdVerifyFailed;
                    }
                    else if (!ClearUserSecPwd(verifyReq.UserID))
                    {
                        errcode = (int)SecondPasswordError.SecPwdDBFailed;
                    }
                    else
                    {
                        errcode = (int)SecondPasswordError.SecPwdClearSuccess;
                    }
                }

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, errcode.ToString(), nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(socket), false);
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }

        public static void OnUsrLogout(string userID)
        {
            SecPwdState pwdState = GetSecPwdState(userID);
            if (pwdState != null)
            {
                pwdState.AuthDeadTime = TimeUtil.NowDateTime().AddSeconds((int)ValidSecWhenLogout);
                SetSecPwdState(userID, pwdState);
            }
        }

        /// <summary>
        /// user登录的时候，初始化user的二级密码信息
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="alreadyOnline"></param>
        /// <returns></returns>
        public static SecPwdState InitUserState(string userID, bool alreadyOnline)
        {

            if (string.IsNullOrEmpty(userID))
            {
                return null;
            }

            SecPwdState pwdState = GetSecPwdState(userID);
            if (pwdState == null)
            {
                // 如果gameserver没有缓存userid对应的二级密码信息，说明尚未加载过或者不存在
                string[] result = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_DB_GET_USR_SECOND_PASSWORD, userID, GameManager.LocalServerId);
                if (result != null && result.Length == 2 && !string.IsNullOrEmpty(result[1]))
                {
                    // 加载了二级密码，断定是第一次登录，强制验证
                    pwdState = new SecPwdState();
                    pwdState.SecPwd = result[1];
                    pwdState.NeedVerify = true;
                }
            }
            else
            {
                // 找到了二级密码信息，那么一定存在二级密码
                if (alreadyOnline)
                {
                    //账号被顶的情况，那么强制再次验证
                    pwdState.NeedVerify = true;
                }

                if (!pwdState.NeedVerify)
                {
                    //检测下距离上次登出隔了多久，超过限制的话，还让他验证
                    if (TimeUtil.NowDateTime() > pwdState.AuthDeadTime)
                    {
                        pwdState.NeedVerify = true;
                    }
                }
            }

            if (pwdState != null)
            {
                SetSecPwdState(userID, pwdState);
            }

            return pwdState;
        }

        /// <summary>
        /// 客户端请求user的二级密码状态
        /// </summary>
        public static TCPProcessCmdResults ProcessUsrCheckState(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;

            string cmdData = null;
            try
            {
                cmdData = new UTF8Encoding().GetString(data, 0, count);
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("解析指令字符串错误, CMD={0}, Client={1}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket)));
                return TCPProcessCmdResults.RESULT_FAILED;
            }

            try
            {
                // usrid:zoneid
                string[] fields = cmdData.Split(':');
                if (2 != fields.Length)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("指令参数个数错误, CMD={0}, Client={1}, Recv={2}", (TCPGameServerCmds)nID, Global.GetSocketRemoteEndPoint(socket), fields.Length));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                string userid = fields[0];

                /*
                 * 在CMD_LOGIN_ON的时候，会初始化usrid对应的二级密码状态
                 * 所以客户端请求二级密码状态时，已经被正确初始化
                 * 服务器返回has：need
                 * has：是否有二级密码
                 * need：是否需要验证二级密码
                 */
                SecPwdState pwdState = GetSecPwdState(userid);
                string cmdRsp = null;
                if (pwdState != null)
                {
                    cmdRsp = string.Format("{0}:{1}", 1, pwdState.NeedVerify ? 1 : 0);
                }
                else
                {
                    cmdRsp = string.Format("{0}:{1}", 0, 0);
                }

                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, cmdRsp, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(socket), false);
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }

        /// <summary>
        /// 客户端请求验证usrid的二级密码
        /// </summary>
        public static TCPProcessCmdResults ProcessUsrVerify(TCPManager tcpMgr, TMSKSocket socket, TCPClientPool tcpClientPool, TCPRandKey tcpRandKey, TCPOutPacketPool pool, int nID, byte[] data, int count, out TCPOutPacket tcpOutPacket)
        {
            tcpOutPacket = null;

            try
            {
                VerifySecondPassword verifyReq = DataHelper.BytesToObject<VerifySecondPassword>(data, 0, count);
                if (verifyReq == null)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("解析指令错误, cmd={0}", (int)nID));
                    return TCPProcessCmdResults.RESULT_FAILED;
                }

                int errcode, has, need;

                SecPwdState pwdState = GetSecPwdState(verifyReq.UserID);
                if (pwdState == null)
                {
                    //没有二级密码，客户端却请求验证，设为验证成功
                    errcode = (int)SecondPasswordError.SecPwdVerifySuccess;
                    has = 0;
                    need = 0;
                }
                else
                {
                    //有二级密码，开始验证

                    if (string.IsNullOrEmpty(verifyReq.SecPwd) || verifyReq.SecPwd != pwdState.SecPwd)
                    {
                        errcode = (int)SecondPasswordError.SecPwdVerifyFailed;
                        has = 1;
                        need = 1;
                    }
                    else
                    {
                        //验证成功了，更新usrid的验证状态
                        errcode = (int)SecondPasswordError.SecPwdVerifySuccess;
                        has = 1;
                        need = 0;
                        pwdState.NeedVerify = false;
                    }
                }


                string rsp = string.Format("{0}:{1}:{2}", errcode, has, need);
                tcpOutPacket = TCPOutPacket.MakeTCPOutPacket(pool, rsp, nID);
                return TCPProcessCmdResults.RESULT_DATA;
            }
            catch (Exception ex)
            {
                DataHelper.WriteFormatExceptionLog(ex, Global.GetDebugHelperInfo(socket), false);
            }

            return TCPProcessCmdResults.RESULT_FAILED;
        }

        private static bool Update2DB(string useid, string secpwd)
        {
            string cmd2db = string.Format("{0}:{1}", useid, secpwd);
            string[] dbFields = Global.ExecuteDBCmd((int)TCPGameServerCmds.CMD_DB_UPDATE_USR_SECOND_PASSWORD, cmd2db, GameManager.LocalServerId);
            if (null == dbFields || dbFields.Length != 2)
            {
                return false;
            }

            return true;
        }

        private static bool Clear2DB(string userid)
        {
            return Update2DB(userid, "");
        }

        public static bool ClearUserSecPwd(string usrid)
        {
            if (string.IsNullOrEmpty(usrid)) return false;

            TMSKSocket clientSocket = GameManager.OnlineUserSession.FindSocketByUserID(usrid);
            KPlayer otherClient = null;
            if (null != clientSocket)
            {
                otherClient = GameManager.ClientMgr.FindClient(clientSocket);
            }

            if (otherClient != null)
            {
                SecPwdState state = GetSecPwdState(usrid);
                if (state == null)
                {
                    // 没有二级密码, 认为清除成功
                    return true;
                }
                else
                {
                    if (Clear2DB(usrid))
                    {
                        SetSecPwdState(usrid, null);

                        int has = 0;
                        int need = 0;
                        string ntf = string.Format("{0}:{1}:{2}:{3}", otherClient.RoleID, (int)SecondPasswordError.SecPwdSetSuccess, has, need);
                        GameManager.ClientMgr.SendToClient(otherClient, ntf, (int)TCPGameServerCmds.CMD_SECOND_PASSWORD_SET);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                // 被清除密码的账号不在线
                if (Clear2DB(usrid))
                {
                    SetSecPwdState(usrid, null);
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }
    }
}
