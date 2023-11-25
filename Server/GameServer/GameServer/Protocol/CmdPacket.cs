using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Tools;

namespace Server.Protocol
{
    /// <summary>
    /// 命令包,每一个命令包都在非IO线程统一处理,这儿，需要确保同一时刻，同一套接字连接的
    /// 数据表，仅仅被一个线程处理，这样，客户端的命令包在服务器端得到线性处理，与旧版本
    /// 方式一致
    /// </summary>
    public class CmdPacket
    {
        /// <summary>
        /// 命令包
        /// </summary>
        /// <param name="nID"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        public CmdPacket(int nID, byte[] data, int count)
        {
            CmdID = nID;
            Data = new byte[count];

            DataHelper.CopyBytes(Data, 0, data, 0, count);
        }

        public int CmdID;
        public byte[] Data;
    }
}
