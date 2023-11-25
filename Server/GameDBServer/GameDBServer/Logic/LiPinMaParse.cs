using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Server.Tools;

namespace GameDBServer.Logic
{
    /// <summary>
    /// 礼品码生成和解析类
    /// </summary>
    public class LiPinMaParse
    {
        #region 唯一ID和名称

        /// <summary>
        /// 产生唯一的ID
        /// </summary>
        /// <returns></returns>
        private static string GenerateUniqueId()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }

            return string.Format("{0:X2}", i - DateTime.Now.Ticks);
        }

        #endregion 唯一ID和名称

        /// <summary>
        /// 生成礼品码
        /// </summary>
        /// <param name="ptid"></param>
        /// <param name="ptrepeat"></param>
        /// <param name="zoneID"></param>
        /// <returns></returns>
        public static string GenerateLiPinMa(int ptid, int ptrepeat, int zoneID)
        {
            string randStr = GenerateUniqueId().Substring(0, 12);
            string lipinma_data = string.Format("NZ{0:000}{1:0}{2:000}{3}", ptid, ptrepeat, zoneID, randStr);
            byte[] bytesData = new UTF8Encoding().GetBytes(lipinma_data);

            CRC32 crc32 = new CRC32();
            crc32.update(bytesData);
            uint crc32Val = crc32.getValue() % 255;
            string str = string.Format("{0:X}", crc32Val);

            lipinma_data += str;
            return lipinma_data;
        }

        /// <summary>
        /// 解析礼品码
        /// </summary>
        /// <param name="lipinma"></param>
        /// <param name="ptid"></param>
        /// <param name="ptrepeat"></param>
        /// <param name="zoneID"></param>
        /// <returns></returns>
        public static bool ParseLiPinMa(string lipinma, out int ptid, out int ptrepeat, out int zoneID, out int nMaxUseNum)
        {
            ptid = -1;
            ptrepeat = 0;
            zoneID = 0;
            nMaxUseNum = 1;
            int nAddLength = 0;
            if (lipinma.Length > 23)
            {
                nAddLength = 2;
            }

            if (lipinma.Length < (22 + nAddLength) || lipinma.Length > (23 + nAddLength))
            {
                return false;
            }

            lipinma = lipinma.ToUpper();
            if ("NZ" != lipinma.Substring(0, 2))
            {
                return false;
            }

            string crc32Str = lipinma.Substring(21 + nAddLength, Math.Min(2, lipinma.Length - (21 + nAddLength)));
            int crc32Val = Global.SafeConvertToInt32(crc32Str, 16);

            byte[] bytesData = new UTF8Encoding().GetBytes(lipinma.Substring(0, (21 + nAddLength)));

            CRC32 crc32 = new CRC32();
            crc32.update(bytesData);
            uint check_crc32Val = crc32.getValue() % 255;
            if (crc32Val != (int)check_crc32Val)
            {
                return false;
            }

            ptid = Convert.ToInt32(lipinma.Substring(2, 3));
            ptrepeat = Convert.ToInt32(lipinma.Substring(5, 1));
            zoneID = Convert.ToInt32(lipinma.Substring(6, 3));
            if (nAddLength > 0)
            {
                nMaxUseNum = Convert.ToInt32(lipinma.Substring(9, 2));
            }

            return true;
        }

        /// <summary>
        /// 解析礼品码
        /// </summary>
        /// <param name="lipinma"></param>
        /// <param name="ptid"></param>
        /// <param name="ptrepeat"></param>
        /// <param name="zoneID"></param>
        /// <returns></returns>
        public static bool ParseLiPinMaNX(string lipinma, out int ptid, out int ptrepeat, out int zoneID, out int nMaxUseNum)
        {
            ptid = -1;
            ptrepeat = 0;
            zoneID = 0;
            nMaxUseNum = 1;
            int nAddLength = 0;
            if (lipinma.Length > 24)
            {
                nAddLength = 2;
            }

            if (lipinma.Length < (23 + nAddLength) || lipinma.Length > (24 + nAddLength))
            {
                return false;
            }

            lipinma = lipinma.ToUpper();
            if ("NX" != lipinma.Substring(0, 2))
            {
                return false;
            }

            string crc32Str = lipinma.Substring(22 + nAddLength, Math.Min(2, lipinma.Length - (22 + nAddLength)));
            int crc32Val = Global.SafeConvertToInt32(crc32Str, 16);

            byte[] bytesData = new UTF8Encoding().GetBytes(lipinma.Substring(0, (22 + nAddLength)));

            CRC32 crc32 = new CRC32();
            crc32.update(bytesData);
            uint check_crc32Val = crc32.getValue() % 255;
            if (crc32Val != (int)check_crc32Val)
            {
                return false;
            }

            ptid = Convert.ToInt32(lipinma.Substring(2, 4));
            ptrepeat = Convert.ToInt32(lipinma.Substring(6, 1));
            zoneID = Convert.ToInt32(lipinma.Substring(7, 3));
            if (nAddLength > 0)
            {
                nMaxUseNum = Convert.ToInt32(lipinma.Substring(10, 2));
            }

            return true;
        }

        /// <summary>
        /// 解析礼品码
        /// </summary>
        /// <param name="lipinma"></param>
        /// <param name="ptid"></param>
        /// <param name="ptrepeat"></param>
        /// <param name="zoneID"></param>
        /// <returns></returns>
        public static bool ParseLiPinMa2(string lipinma, out int ptid, out int ptrepeat, out int zoneID, out int nMaxUseNum)
        {
            ptid = -1;
            ptrepeat = 0;
            zoneID = 0;
            nMaxUseNum = 1;
            int nAddLength = 0;
            if (lipinma.Length > 23)
            {
                nAddLength = 2;
            }

            if (lipinma.Length < (22 + nAddLength) || lipinma.Length > (23 + nAddLength))
            {
                return false;
            }

            lipinma = lipinma.ToUpper();
            if ("NZ" != lipinma.Substring(0, 2))
            {
                return false;
            }

            ptid = Convert.ToInt32(lipinma.Substring(2, 3));
            ptrepeat = Convert.ToInt32(lipinma.Substring(5, 1));
            zoneID = Convert.ToInt32(lipinma.Substring(6, 3));
            if (nAddLength > 0)
            {
                nMaxUseNum = Convert.ToInt32(lipinma.Substring(9, 2));
            }

            string crc32Str = lipinma.Substring(9 + nAddLength);
            MD5 md5 = MD5.Create();
            byte[] data0 = new byte[9 + 4 * 4];
            for (int i = 0; i < 5; i++)
            {
                data0[i] = Convert.ToByte(crc32Str.Substring(2 * i + 4, 2), 16);
            }

            data0[5] = 0x1f;
            data0[6] = 0x16;
            data0[7] = 0x05;
            data0[8] = 0x96;
            Array.Copy(BitConverter.GetBytes(ptid), 0, data0, 9 + 0 * 4, 4);
            Array.Copy(BitConverter.GetBytes(ptrepeat), 0, data0, 9 + 1 * 4, 4);
            Array.Copy(BitConverter.GetBytes(zoneID), 0, data0, 9 + 2 * 4, 4);
            Array.Copy(BitConverter.GetBytes(nMaxUseNum), 0, data0, 9 + 3 * 4, 4);
            byte[] data1 = md5.ComputeHash(data0);
            for (int i = 0; i < 2; i++)
            {
                if (Convert.ToByte(crc32Str.Substring(2 * i, 2), 16) != data1[i])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 解析礼品码
        /// </summary>
        /// <param name="lipinma"></param>
        /// <param name="ptid"></param>
        /// <param name="ptrepeat"></param>
        /// <param name="zoneID"></param>
        /// <returns></returns>
        public static bool ParseLiPinMaNX2(string lipinma, out int ptid, out int ptrepeat, out int zoneID, out int nMaxUseNum)
        {
            ptid = -1;
            ptrepeat = 0;
            zoneID = 0;
            nMaxUseNum = 1;
            int nAddLength = 0;
            if (lipinma.Length > 24)
            {
                nAddLength = 2;
            }

            if (lipinma.Length < (23 + nAddLength) || lipinma.Length > (24 + nAddLength))
            {
                return false;
            }

            lipinma = lipinma.ToUpper();
            if ("NX" != lipinma.Substring(0, 2))
            {
                return false;
            }

            ptid = Convert.ToInt32(lipinma.Substring(2, 4));
            ptrepeat = Convert.ToInt32(lipinma.Substring(6, 1));
            zoneID = Convert.ToInt32(lipinma.Substring(7, 3));
            if (nAddLength > 0)
            {
                nMaxUseNum = Convert.ToInt32(lipinma.Substring(10, 2));
            }

            string crc32Str = lipinma.Substring(10 + nAddLength);
            MD5 md5 = MD5.Create();
            byte[] data0 = new byte[9 + 4 * 4];
            for (int i = 0; i < 5; i++)
            {
                data0[i] = Convert.ToByte(crc32Str.Substring(2 * i + 4, 2), 16);
            }

            data0[5] = 0x1f;
            data0[6] = 0x16;
            data0[7] = 0x05;
            data0[8] = 0x96;
            Array.Copy(BitConverter.GetBytes(ptid), 0, data0, 9 + 0 * 4, 4);
            Array.Copy(BitConverter.GetBytes(ptrepeat), 0, data0, 9 + 1 * 4, 4);
            Array.Copy(BitConverter.GetBytes(zoneID), 0, data0, 9 + 2 * 4, 4);
            Array.Copy(BitConverter.GetBytes(nMaxUseNum), 0, data0, 9 + 3 * 4, 4);
            byte[] data1 = md5.ComputeHash(data0);
            for (int i = 0; i < 2; i++)
            {
                if (Convert.ToByte(crc32Str.Substring(2 * i, 2), 16) != data1[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
