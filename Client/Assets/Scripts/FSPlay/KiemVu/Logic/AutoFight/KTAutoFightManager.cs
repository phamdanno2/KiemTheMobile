using System.Diagnostics;

namespace FSPlay.KiemVu.Logic
{
    /// <summary>
    /// Luồng thực thi tự đánh
    /// </summary>
    public partial class KTAutoFightManager : TTMonoBehaviour
    {
        #region Singleton - Instance
        /// <summary>
        /// Luồng thực thi tự đánh
        /// </summary>
        public static KTAutoFightManager Instance { get; private set; }
        #endregion

        #region Core MonoBehaviour
        /// <summary>
        /// Hàm này gọi khi đối tượng được tạo ra
        /// </summary>
        private void Awake()
        {
            KTAutoFightManager.Instance = this;
        }

        /// <summary>
        /// Hàm này gọi ở Frame đầu tiên
        /// </summary>
		private void Start()
		{
            PlayZone.GlobalPlayZone.StartCoroutine(this.AutoUseFoodAndMedicine());
		}
        #endregion

        #region Properties
        /// <summary>
        /// Có phải Auto PK không
        /// </summary>
        public bool IsAutoFighting { get; private set; }
        #endregion

        #region Public methods
        /// <summary>
        /// Mở Auto PK
        /// </summary>
        public void StartAutoPK()
		{
            this.StopAllCoroutines();
            this.StartCoroutine(this.ProcessAutoFight(false));
            if (PlayZone.GlobalPlayZone != null)
            {
                PlayZone.GlobalPlayZone.ShowTextAutoAttack();
            }
            /// Làm rỗng dữ liệu
            this.Clear();
            /// Đánh dấu đang tự đánh
            this.IsAutoFighting = true;

            this.AutoTime = Stopwatch.StartNew();

            PlayZone.GlobalPlayZone.UIBottomBar.UISkillBar.AutoFarmEnable = false;
            PlayZone.GlobalPlayZone.UIBottomBar.UISkillBar.AutoPKEnable = true;
        }

        /// <summary>
        /// Mở Auto Farm
        /// </summary>
        public void StartAutoFarm()
        {
            this.StopAllCoroutines();
            this.StartCoroutine(this.ProcessAutoFight());
            if (PlayZone.GlobalPlayZone != null)
            {
                PlayZone.GlobalPlayZone.ShowTextAutoAttack();
            }
            /// Làm rỗng dữ liệu
            this.Clear();
            /// Đánh dấu đang tự đánh
            this.IsAutoFighting = true;

            this.AutoTime = Stopwatch.StartNew();

            PlayZone.GlobalPlayZone.UIBottomBar.UISkillBar.AutoFarmEnable = true;
            PlayZone.GlobalPlayZone.UIBottomBar.UISkillBar.AutoPKEnable = false;
        }

        /// <summary>
        /// Dừng tự đánh
        /// </summary>
        public void StopAutoFight()
        {
            this.StopAllCoroutines();
            /// Làm rỗng dữ liệu
            this.Clear();

            if (PlayZone.GlobalPlayZone != null)
            {
                PlayZone.GlobalPlayZone.HideTextAutoAttack();
                PlayZone.GlobalPlayZone.UIBottomBar.UISkillBar.AutoFarmEnable = false;
                PlayZone.GlobalPlayZone.UIBottomBar.UISkillBar.AutoPKEnable = false;
            }
            /// Bỏ đánh dấu đang tự đánh
            this.IsAutoFighting = false;
        }
        #endregion
    }
}
