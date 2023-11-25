using GameDBServer.DB;

namespace GameDBServer.Data
{
    /// <summary>
    /// 用户活跃数据
    /// </summary>
    internal class AccountActiveData
    {
        /// <summary>
        /// 用户帐号
        /// </summary>
        [DBMapping(ColumnName = "Account")]
        public string strAccount;

        /// <summary>
        /// 帐号创建日期
        /// </summary>
        [DBMapping(ColumnName = "createTime")]
        public string strCreateTime;

        /// <summary>
        /// 连续登录天数
        /// </summary>
        [DBMapping(ColumnName = "seriesLoginCount")]
        public int nSeriesLoginCount;

        /// <summary>
        /// 最后连续登陆日期
        /// </summary>
        [DBMapping(ColumnName = "lastSeriesLoginTime")]
        public string strLastSeriesLoginTime;
    }
}