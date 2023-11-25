using FSPlay.KiemVu.Entities.ActionSet;
using FSPlay.KiemVu.Entities.Config;
using FSPlay.KiemVu.Factory.Animation;
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
        #region Effect
        /// <summary>
        /// Danh sách hiệu ứng
        /// </summary>
        public static Dictionary<int, StateEffectXML> ListEffects { get; private set; } = new Dictionary<int, StateEffectXML>();

        /// <summary>
        /// Tải danh sách hiệu ứng từ XML Node
        /// </summary>
        /// <param name="xmlNode"></param>
        public static void LoadEffects(XElement xmlNode)
        {
            foreach (XElement node in xmlNode.Elements("StateEffect"))
            {
                StateEffectXML effect = StateEffectXML.Parse(node);
                Loader.ListEffects[effect.ID] = effect;
            }
        }
        #endregion
    }
}
