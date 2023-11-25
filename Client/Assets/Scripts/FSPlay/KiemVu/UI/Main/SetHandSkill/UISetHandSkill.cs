using FSPlay.GameEngine.Logic;
using FSPlay.KiemVu.Entities.Config;
using FSPlay.KiemVu.UI.Main.SetHandSkill;
using System;
using System.Reflection;
using UnityEngine;

namespace FSPlay.KiemVu.UI.Main
{
	/// <summary>
	/// Khung đặt kỹ năng vào 2 tay
	/// </summary>
	public class UISetHandSkill : MonoBehaviour
	{
        #region Define
        [SerializeField]
        private UnityEngine.UI.Button UIButton_Reset;

        /// <summary>
        /// Sự kiện đóng khung
        /// </summary>
        [SerializeField]
		private UnityEngine.UI.Button UIButton_Close;

		/// <summary>
		/// Button kỹ năng chính tay trái
		/// </summary>
		[SerializeField]
		private UISetHandSkill_Button UIButton_LeftHandMainSkill;

		/// <summary>
		/// Button kỹ năng phụ số 1 tay trái
		/// </summary>
		[SerializeField]
		private UISetHandSkill_Button UIButton_LeftHandSkill1;

		/// <summary>
		/// Button kỹ năng phụ số 2 tay trái
		/// </summary>
		[SerializeField]
		private UISetHandSkill_Button UIButton_LeftHandSkill2;

		/// <summary>
		/// Button kỹ năng phụ số 3 tay trái
		/// </summary>
		[SerializeField]
		private UISetHandSkill_Button UIButton_LeftHandSkill3;

		/// <summary>
		/// Button kỹ năng phụ số 4 tay trái
		/// </summary>
		[SerializeField]
		private UISetHandSkill_Button UIButton_LeftHandSkill4;

		/// <summary>
		/// Button kỹ năng vòng sáng
		/// </summary>
		[SerializeField]
		private UISetHandSkill_Button UIButton_AuraSkill;
		#endregion

		#region Private fields
		/// <summary>
		/// Danh sách kỹ năng được xếp vào ô kỹ năng nhanh
		/// </summary>
		private readonly int[] skills = new int[10] { -1,-1,-1,-1,-1,-1,-1,-1, -1, -1 };

		/// <summary>
		/// ID kỹ năng vòng sáng
		/// </summary>
		private int AuraSkillID = -1;
		#endregion

		#region Properties
		/// <summary>
		/// Sự kiện đóng khung
		/// </summary>
		public Action Close { get; set; }
		#endregion

		#region Core MonoBehaviour
		/// <summary>
		/// Hàm này gọi ở Frame đầu tiên
		/// </summary>
		private void Start()
		{
			this.InitPrefabs();
			this.Refresh();
		}
		#endregion

		#region Code UI
		/// <summary>
		/// Khởi tạo ban đầu
		/// </summary>
		private void InitPrefabs()
		{
            this.UIButton_Reset.onClick.AddListener(this.ButtonReset_Clicked);
            
			this.UIButton_Close.onClick.AddListener(this.ButtonClose_Clicked);

			this.UIButton_LeftHandMainSkill.Click = () => {
				this.OpenSelectSkillFrame(this.UIButton_LeftHandMainSkill, 0, 0);
			};
			this.UIButton_LeftHandSkill1.Click = () => {
				this.OpenSelectSkillFrame(this.UIButton_LeftHandSkill1, 1, 1);
			};
			this.UIButton_LeftHandSkill2.Click = () => {
				this.OpenSelectSkillFrame(this.UIButton_LeftHandSkill2, 1, 2);
			};
			this.UIButton_LeftHandSkill3.Click = () => {
				this.OpenSelectSkillFrame(this.UIButton_LeftHandSkill3, 1, 3);
			};
			this.UIButton_LeftHandSkill4.Click = () => {
				this.OpenSelectSkillFrame(this.UIButton_LeftHandSkill4, 1, 4);
			};

			this.UIButton_AuraSkill.Click = () => {
				this.OpenSelectSkillFrame(this.UIButton_AuraSkill, 2);
			};
		}
        private void ButtonReset_Clicked()
        {
            this.SetSkill(this.UIButton_LeftHandMainSkill, -1);
            this.SetSkill(this.UIButton_LeftHandSkill1, -1);
            this.SetSkill(this.UIButton_LeftHandSkill2, -1);
            this.SetSkill(this.UIButton_LeftHandSkill3, -1);
            this.SetSkill(this.UIButton_LeftHandSkill4, -1);

            this.SetSkill(this.UIButton_AuraSkill, -1);

            PlayZone.GlobalPlayZone.UIBottomBar.UISkillBar.ResetSkill();
        }

        /// <summary>
        /// Sự kiện khi Button đóng khung được ấn
        /// </summary>
        private void ButtonClose_Clicked()
		{
			this.Close?.Invoke();
		}
		#endregion

		#region Private methods
		/// <summary>
		/// Mở khung chọn kỹ năng
		/// </summary>
		/// <param name="uiButton"></param>
		/// <param name="type"></param>
		/// <param name="index"></param>
		private void OpenSelectSkillFrame(UISetHandSkill_Button uiButton, int type, int index = -1)
		{
			/// Mở khung chọn kỹ năng
			PlayZone.GlobalPlayZone.ShowUIQuickKeyChooser(type, (chosenSkillID) => {
				if (index != -1)
				{
					PlayZone.GlobalPlayZone.UIBottomBar.UISkillBar.AddSkill(chosenSkillID, index);
				}
				else
				{
					PlayZone.GlobalPlayZone.UIBottomBar.UISkillBar.AddAruaSkill(chosenSkillID);
				}

				/// Thiết lập kỹ năng tương ứng vào ô
				this.SetSkill(uiButton, chosenSkillID);
			});
		}

		/// <summary>
		/// Đặt kỹ năng vào ô tương ứng
		/// </summary>
		/// <param name="uiButton"></param>
		/// <param name="skillID"></param>
		private void SetSkill(UISetHandSkill_Button uiButton, int skillID)
		{
			/// Nếu kỹ năng tồn tại
			if (Loader.Loader.Skills.TryGetValue(skillID, out SkillDataEx skillData))
			{
				uiButton.Data = skillData;
			}
			else
			{
				uiButton.Data = null;
			}
		}

		/// <summary>
		/// Làm mới danh sách kỹ năng đã đặt
		/// </summary>
		private void Refresh()
		{
			/// Kỹ năng 2 tay
			{
				string quickKey = Global.Data.RoleData.MainQuickBarKeys;
				string[] keys = quickKey.Split('|');
				if (keys.Length == 10)
				{
					for (int i = 0; i < keys.Length; i++)
					{
						try
						{
							this.skills[i] = int.Parse(keys[i]);
						}
						catch (Exception)
						{
							this.skills[i] = -1;
						}
					}
				}

				/// Đặt kỹ năng tương ứng
				this.SetSkill(this.UIButton_LeftHandMainSkill, this.skills[0]);
				this.SetSkill(this.UIButton_LeftHandSkill1, this.skills[1]);
				this.SetSkill(this.UIButton_LeftHandSkill2, this.skills[2]);
				this.SetSkill(this.UIButton_LeftHandSkill3, this.skills[3]);
				this.SetSkill(this.UIButton_LeftHandSkill4, this.skills[4]);
			}

			/// Kỹ năng vòng sáng
			{
				string auraKey = Global.Data.RoleData.OtherQuickBarKeys;
				string[] keys = auraKey.Split('_');

				this.AuraSkillID = -1;
				if (keys.Length == 2)
				{
					try
					{
						this.AuraSkillID = int.Parse(keys[0]);
					}
					catch (Exception) { }
				}

				this.SetSkill(this.UIButton_AuraSkill, this.AuraSkillID);
			}
		}
		#endregion
	}
}
