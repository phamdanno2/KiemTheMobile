using UnityEngine;
using TMPro;

namespace FSPlay.KiemVu.UI.Main.LocalMap.ColonyMap
{
    /// <summary>
    /// Khung thông tin bang hội chiếm hữu lãnh thổ
    /// </summary>
    public class UILocalMap_ColonyMapTab_GuildInfoNode : MonoBehaviour
    {
        #region Define
        /// <summary>
        /// Image icon
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Image UIImage_Icon;

        /// <summary>
        /// Text tên bang hội
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_GuildName;
        #endregion

        #region Properties
        /// <summary>
        /// Màu bang hội
        /// </summary>
        public Color Color
        {
            get
            {
                return this.UIImage_Icon.color;
            }
            set
            {
                this.UIImage_Icon.color = value;
                this.UIText_GuildName.color = value;
            }
        }

        /// <summary>
        /// Tên bang hội
        /// </summary>
        public string GuildName
        {
            get
            {
                return this.UIText_GuildName.text;
            }
            set
            {
                this.UIText_GuildName.text = value;
            }
        }
        #endregion
    }
}
