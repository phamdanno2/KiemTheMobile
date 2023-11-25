using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Threading;
using System.Xml.Linq;
using GameServer.Interface;
using Server.Data;
using Server.Tools;

namespace GameServer.Logic
{
    /// <summary>
    /// Xml节点列表
    /// </summary>
    public class SystemXmlItems
    {
        #region 基础变量

        /// <summary>
        /// 物品列表
        /// </summary>
        private Dictionary<int, SystemXmlItem> _SystemXmlItemDict = null;

        /// <summary>
        /// 第一次加载是否成功
        /// </summary>
        private bool FirstLoadOK = false;

        /// <summary>
        /// xml访问路径
        /// </summary>
        private string FileName = "";

        /// <summary>
        /// 根节点名称
        /// </summary>
        private string RootName = "";

        /// <summary>
        /// 键名称
        /// </summary>
        private string KeyName = "";

        /// <summary>
        /// 访问资源的目录类型
        /// </summary>
        private int ResType = 0;

        #endregion 基础变量

        #region 属性

        /// <summary>
        /// 物品列表
        /// </summary>
        public Dictionary<int, SystemXmlItem> SystemXmlItemDict
        {
            get { return _SystemXmlItemDict; }
        }

        public int MaxKey { get; private set; }

        #endregion 属性

        #region 方法

        /// <summary>
        /// 从Xml中加载
        /// </summary>
        /// <param name="fileName"></param>
        private Dictionary<int, SystemXmlItem> _LoadFromXMlFile(string fileName, string rootName, string keyName, int resType)
        {
            XElement xml = null;

            try
            {
                string fullPathFileName = "";
                if (0 == resType)
                {
                    fullPathFileName = Global.GameResPath(fileName);
                }
                else if (1 == resType)
                {
                    fullPathFileName = Global.IsolateResPath(fileName);
                }

                xml = XElement.Load(fullPathFileName);
                if (null == xml)
                {
                    throw new Exception(string.Format("加载系统xml配置文件:{0}, 失败。没有找到相关XML配置文件!", fullPathFileName));
                }

                FileName = fileName;
                RootName = rootName;
                KeyName = keyName;
                ResType = resType;
            }
            catch (Exception)
            {
                throw new Exception(string.Format("加载系统xml配置文件:{0}, 失败。没有找到相关XML配置文件!", fileName));
            }

            SystemXmlItem systemXmlItem = null;
            IEnumerable<XElement> nodes = null;
            if ("" == rootName)
            {
                nodes = xml.Elements();
            }
            else
            {
                nodes = xml.Elements(rootName).Elements();
            }

            Dictionary<int, SystemXmlItem> systemXmlItemDict = new Dictionary<int, SystemXmlItem>();
            foreach (var node in nodes)
            {
                systemXmlItem = new SystemXmlItem()
                {
                    XMLNode = node,
                };

                int key = (int)Global.GetSafeAttributeLong(node, keyName);
                systemXmlItemDict[key] = systemXmlItem;
                if (key > MaxKey)
                {
                    MaxKey = key;
                }
            }

            //第一次加载是否成功
            FirstLoadOK = true;

            return systemXmlItemDict;
        }

        /// <summary>
        /// 从Xml中加载
        /// </summary>
        /// <param name="fileName"></param>
        public void LoadFromXMlFile(string fileName, string rootName, string keyName, int resType = 0)
        {
            //从Xml中加载
            _SystemXmlItemDict = _LoadFromXMlFile(fileName, rootName, keyName, resType);
        }

        /// <summary>
        /// 重新加载Xml
        /// </summary>
        /// <param name="fileName"></param>
        public int ReloadLoadFromXMlFile()
        {
            //如果第一次加载无成功，则以后也不加载
            if (!FirstLoadOK)
            {
                return -2;
            }

            try
            {
                //从Xml中加载(原子访问)
                _SystemXmlItemDict = _LoadFromXMlFile(FileName, RootName, KeyName, ResType);
            }
            catch (Exception)
            {
                return -1;
            }

            return 0;
        }

        #endregion 方法
    }
}
