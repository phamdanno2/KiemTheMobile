﻿using FSPlay.KiemVu.Entities.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace FSPlay.KiemVu.Loader
{
    /// <summary>
    /// Đối tượng chứa danh sách các cấu hình trong game
    /// </summary>
    public static partial class Loader
    {
        #region Role Avarta
        /// <summary>
        /// Danh sách Avarta nhân vật
        /// </summary>
        public static Dictionary<int, RoleAvartaXML> RoleAvartas { get; private set; } = new Dictionary<int, RoleAvartaXML>();

        /// <summary>
        /// Đọc dữ liệu từ file RoleAvarta.xml trong Bundle
        /// </summary>
        /// <param name="xmlNode"></param>
        public static void LoadRoleAvarta(XElement xmlNode)
        {
            Loader.RoleAvartas.Clear();

            foreach (XElement node in xmlNode.Elements("Avarta"))
            {
                RoleAvartaXML charFace = RoleAvartaXML.Parse(node);
                Loader.RoleAvartas[charFace.ID] = charFace;
            }
        }
        #endregion

        #region Faction
        /// <summary>
        /// Danh sách các môn phái
        /// </summary>
        public static Dictionary<int, FactionXML> Factions { get; private set; } = new Dictionary<int, FactionXML>();

        /// <summary>
        /// Đọc dữ liệu từ file Faction.xml trong Bundle
        /// </summary>
        /// <param name="xmlNode"></param>
        public static void LoadFaction(XElement xmlNode)
        {
            Loader.Factions.Clear();

            foreach (XElement node in xmlNode.Elements("Faction"))
            {
                FactionXML faction = FactionXML.Parse(node);
                Loader.Factions[faction.ID] = faction;
            }
        }
        #endregion
    }
}
