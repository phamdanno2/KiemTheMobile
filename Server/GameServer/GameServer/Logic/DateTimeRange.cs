namespace GameServer.Logic
{
    /// <summary>
    /// 时间段限制
    /// </summary>
    public class DateTimeRange
    {
        /// <summary>
        /// 开始的小时
        /// </summary>
        public int FromHour = 0;

        /// <summary>
        /// 开始的分钟
        /// </summary>
        public int FromMinute = 0;

        /// <summary>
        /// 结束的小时
        /// </summary>
        public int EndHour = 0;

        /// <summary>
        /// 结束的分钟
        /// </summary>
        public int EndMinute = 0;
    }
}