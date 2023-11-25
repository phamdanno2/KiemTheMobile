using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System.Text;
using Server.Tools;

namespace GameServer.Logic
{
    /// <summary>
    /// 通用的xml缓存管理
    /// </summary>
    public class GeneralCachingXmlMgr
    {
        /// <summary>
        /// xml节点缓存
        /// </summary>
        private static Dictionary<string, XElement> CachingXmlDict = new Dictionary<string, XElement>();

        /// <summary>
        /// 缓存xml
        /// </summary>
        /// <param name="xmlFileName"></param>
        private static XElement CachingXml(string xmlFileName)
        {
            XElement xml = null;
            try
            {
                xml = XElement.Load(xmlFileName);
                lock (CachingXmlDict)
                {
                    CachingXmlDict[xmlFileName] = xml;
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteException(ex.ToString() + "xmlFileName=" + xmlFileName);
                return null;
            }

            return xml;
        }

        /// <summary>
        /// 从缓存中获取xml
        /// </summary>
        /// <param name="xmlFileName"></param>
        /// <returns></returns>
        public static XElement GetXElement(string xmlFileName)
        {
            XElement xml = null;
            lock (CachingXmlDict)
            {
                if (CachingXmlDict.TryGetValue(xmlFileName, out xml))
                {
                    return xml;
                }
            }

            return CachingXml(xmlFileName);
        }

        /// <summary>
        /// 重新加载到缓存
        /// </summary>
        /// <param name="xmlFileName"></param>
        /// <returns></returns>
        public static XElement Reload(string xmlFileName)
        {
            return CachingXml(xmlFileName);
        }

        /// <summary>
        /// 从缓存中清除，强迫使用时重新加载
        /// </summary>
        /// <param name="xmlFileName"></param>
        public static void RemoveCachingXml(string xmlFileName)
        {
            lock (CachingXmlDict)
            {
                CachingXmlDict.Remove(xmlFileName);
            }
        }
    }
}
