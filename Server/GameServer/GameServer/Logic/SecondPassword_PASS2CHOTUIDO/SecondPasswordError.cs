using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameServer.Logic.SecondPassword
{
    public enum SecondPasswordError
    {
        SecPwdVerifySuccess,
        SecPwdVerifyFailed,
        SecPwdIsNotSet,
        SecPwdCharInvalid,
        SecPwdIsNull,
        SecPwdIsTooShort,
        SecPwdIsTooLong,
        SecPwdSetSuccess,
        SecPwdDBFailed,
        SecPwdClearSuccess,
    }
}
