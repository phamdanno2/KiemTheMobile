using Server.Data;
using System.Collections.Generic;
using System.Threading;

namespace GameServer.KiemThe.Core.Trade
{
    public class GoodsExchangeManager
    {
        private long BaseAutoID = 0;

        public int GetNextAutoID()
        {
            return (int)(Interlocked.Increment(ref BaseAutoID) & 0x7fffffff);
        }

        private Dictionary<int, ExchangeData> _GoodsExchangeDict = new Dictionary<int, ExchangeData>();

        /// <summary>
        ///  Thêm mới 1 giao dịch
        /// </summary>
        /// <param name="exchangeID"></param>
        /// <param name="ed"></param>
        public void AddData(int exchangeID, ExchangeData ed)
        {
            lock (_GoodsExchangeDict)
            {
                _GoodsExchangeDict[exchangeID] = ed;
            }
        }

        // Remove giao dịch
        public void RemoveData(int exchangeID)
        {
            lock (_GoodsExchangeDict)
            {
                if (_GoodsExchangeDict.ContainsKey(exchangeID))
                {
                    _GoodsExchangeDict.Remove(exchangeID);
                }
            }
        }

        /// <summary>
        ///  Tìm 1 giao dịch
        /// </summary>
        /// <param name="exchangeID"></param>
        /// <returns></returns>
        public ExchangeData FindData(int exchangeID)
        {
            ExchangeData ed = null;
            lock (_GoodsExchangeDict)
            {
                _GoodsExchangeDict.TryGetValue(exchangeID, out ed);
            }

            return ed;
        }
    }
}