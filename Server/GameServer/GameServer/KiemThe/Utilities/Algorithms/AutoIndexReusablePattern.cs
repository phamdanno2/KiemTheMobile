//#define ENABLE_REUSE

#if ENABLE_REUSE
using System.Collections.Concurrent;
#endif

namespace GameServer.KiemThe.Utilities.Algorithms
{
    /// <summary>
    /// Quản lý ID tự tăng có thể tái sử dụng được về sau
    /// </summary>
    public class AutoIndexReusablePattern
    {
        /// <summary>
        /// ID tự tăng
        /// </summary>
        private int AutoID = 0;

#if ENABLE_REUSE
        /// <summary>
        /// Danh sách ID chưa được sử dụng
        /// </summary>
        private readonly ConcurrentQueue<int> FreeIDsList = new ConcurrentQueue<int>();
#endif

        /// <summary>
        /// Quản lý ID tự tăng có thể tái sử dụng được về sau
        /// </summary>
        public AutoIndexReusablePattern()
        {
            this.AutoID = 0;
        }

        /// <summary>
        /// Trả về ID tự động mới
        /// </summary>
        public int GetNewID()
        {
#if ENABLE_REUSE
            /// Nếu tồn tại ID cũ thì lấy ra dùng luôn
            if (this.FreeIDsList.Count > 0 && this.FreeIDsList.TryDequeue(out int id))
            {
                return id;
            }

            /// Nếu không tồn tại ID cũ thì tạo mới
            this.AutoID++;
#else
            /// Rạo mới
            this.AutoID = (this.AutoID + 1) % 100000007;
#endif

            /// Trả về kết quả
            return this.AutoID;
        }

        /// <summary>
        /// Trả về ID đã được sử dụng trước đó để tái sử dụng
        /// </summary>
        /// <param name="id"></param>
        public void ReturnID(int id)
        {
#if ENABLE_REUSE
            this.FreeIDsList.Enqueue(id);
#endif
        }
    }
}
