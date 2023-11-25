using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FSPlay.KiemVu.UI.Main.ItemBox;
using FSPlay.KiemVu.Entities.Config;
using Server.Data;
using System.Collections;

namespace FSPlay.KiemVu.UI.Main.AutoFight
{
	/// <summary>
	/// Tab AutoPK
	/// </summary>
	public class UIAutoFight_AutoPKTab : MonoBehaviour
	{
		#region Define
		/// <summary>
		/// Toggle tự dùng thuốc
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Toggle UIToggle_AutoUseMedicine;

		/// <summary>
		/// Toggle tự ăn thức ăn
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Toggle UIToggle_AutoEatFood;

		/// <summary>
		/// Button tự mời đội
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Toggle UIToggle_AutoInviteToTeam;

		/// <summary>
		/// Button tự đồng ý vào đội
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Toggle UIToggle_AutoAcceptInviteToTeam;

		/// <summary>
		/// Text ngưỡng tự dùng thuốc sinh lực
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_AutoUseMedicineHP;

		/// <summary>
		/// Button nhập ngưỡng tự dùng thuốc sinh lực
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Button UIButton_InputAutoUsemedicineHP;

		/// <summary>
		/// Ô thuốc hồi sinh lực
		/// </summary>
		[SerializeField]
		private UIItemBox UIItem_HPMedicine;

		/// <summary>
		/// Text ngưỡng tự dùng thuốc nội lực
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_AutoUseMedicineMP;

		/// <summary>
		/// Button nhập ngưỡng tự dùng thuốc nội lực
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Button UIButton_InputAutoUsemedicineMP;

		/// <summary>
		/// Ô thuốc hồi nội lực
		/// </summary>
		[SerializeField]
		private UIItemBox UIItem_MPMedicine;

		/// <summary>
		/// Toggle Nga My tự động Buff
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Toggle UIToggle_EMAutoBuff;

		/// <summary>
		/// Text ngưỡng sinh lực đồng đội Nga My sẽ tự Buff
		/// </summary>
		[SerializeField]
		private TextMeshProUGUI UIText_EMAutoHP;

		/// <summary>
		/// Button nhập ngưỡng sinh lực đồng đội Nga My sẽ tự Buff
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Button UIButton_InputEMAutoHP;

		/// <summary>
		/// Toggle Nga My tự hồi sinh đồng đội
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Toggle UIToggle_EMAutoRevive;

		/// <summary>
		/// Toggle tự phản kháng khi bị PK
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Toggle UIToggle_AutoReflectAttack;

		/// <summary>
		/// Toggle ưu tiên khắc hệ
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Toggle UIToggle_SeriesConquePriority;

		/// <summary>
		/// Toggle ưu tiên mục tiêu ít máu
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Toggle UIToggle_LowHPEnemyPriority;

		/// <summary>
		/// Toggle hiện mục tiêu xung quanh
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Toggle UIToggle_ShowNearbyEnemy;

		/// <summary>
		/// Toggle sử dụng kỹ năng tân thủ
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Toggle UIToggle_UseNewbieSkill;

		/// <summary>
		/// Toggle đuổi mục tiêu
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Toggle UIToggle_ChaseTarget;

		/// <summary>
		/// Sử dụng kỹ năng theo Combo
		/// </summary>
		[SerializeField]
		private UnityEngine.UI.Toggle UIToggle_UseSkillByCombo;

		/// <summary>
		/// Prefab ô kỹ năng
		/// </summary>
		[SerializeField]
		private UIAutoFight_SkillButton UIButton_SkillPrefab;
		#endregion

		#region Private fields
		/// <summary>
		/// RectTransform danh sách kỹ năng
		/// </summary>
		private RectTransform transformSkillList = null;
		#endregion

		#region Properties
		/// <summary>
		/// Khung gốc
		/// </summary>
		public UIAutoFight Parent { get; set; }

		/// <summary>
		/// Tự dùng thuốc
		/// </summary>
		public bool AutoUseMedicine
		{
			get
			{
				return this.UIToggle_AutoUseMedicine.isOn;
			}
			set
			{
				this.UIToggle_AutoUseMedicine.isOn = value;
			}
		}

		/// <summary>
		/// Tự dùng thức ăn
		/// </summary>
		public bool AutoEatFood
		{
			get
			{
				return this.UIToggle_AutoEatFood.isOn;
			}
			set
			{
				this.UIToggle_AutoEatFood.isOn = value;
			}
		}

		/// <summary>
		/// Tự mời vào nhóm
		/// </summary>
		public bool AutoInviteToTeam
		{
			get
			{
				return this.UIToggle_AutoInviteToTeam.isOn;
			}
			set
			{
				this.UIToggle_AutoInviteToTeam.isOn = value;
			}
		}

		/// <summary>
		/// Tự đồng ý vào nhóm
		/// </summary>
		public bool AutoAcceptInviteToTeam
		{
			get
			{
				return this.UIToggle_AutoAcceptInviteToTeam.isOn;
			}
			set
			{
				this.UIToggle_AutoAcceptInviteToTeam.isOn = value;
			}
		}

		private int _AutoUseMedicineHP = 0;
		/// <summary>
		/// Ngưỡng tự dùng thuốc hồi sinh lực
		/// </summary>
		public int AutoUseMedicineHP
		{
			get
			{
				return this._AutoUseMedicineHP;
			}
			set
			{
				this._AutoUseMedicineHP = value;
				this.UIText_AutoUseMedicineHP.text = value.ToString();
			}
		}

		private int _AutoUseMedicineMP = 0;
		/// <summary>
		/// Ngưỡng tự dùng thuốc hồi nội lực
		/// </summary>
		public int AutoUseMedicineMP
		{
			get
			{
				return this._AutoUseMedicineMP;
			}
			set
			{
				this._AutoUseMedicineMP = value;
				this.UIText_AutoUseMedicineMP.text = value.ToString();
			}
		}

		private int _HPMedicineID = -1;
		/// <summary>
		/// ID thuốc hồi sinh lực
		/// </summary>
		public int HPMedicineID
		{
			get
			{
				return this._HPMedicineID;
			}
			set
			{
				this._HPMedicineID = value;

				/// Xóa thông tin vật phẩm tương ứng
				this.UIItem_HPMedicine.Data = null;

				/// Thông tin thuốc tương ứng
				if (!Loader.Loader.Items.TryGetValue(value, out ItemData itemData))
				{
					return;
				}

				/// Tạo vật phẩm
				GoodsData itemGD = KTGlobal.CreateItemPreview(itemData);
				itemGD.Binding = 1;

				/// Đổ dữ liệu vào ô
				this.UIItem_HPMedicine.Data = itemGD;
			}
		}

		private int _MPMedicineID = -1;
		/// <summary>
		/// ID thuốc hồi nội lực
		/// </summary>
		public int MPMedicineID
		{
			get
			{
				return this._MPMedicineID;
			}
			set
			{
				this._MPMedicineID = value;

				/// Xóa thông tin vật phẩm tương ứng
				this.UIItem_MPMedicine.Data = null;

				/// Thông tin thuốc tương ứng
				if (!Loader.Loader.Items.TryGetValue(value, out ItemData itemData))
				{
					return;
				}

				/// Tạo vật phẩm
				GoodsData itemGD = KTGlobal.CreateItemPreview(itemData);
				itemGD.Binding = 1;

				/// Đổ dữ liệu vào ô
				this.UIItem_MPMedicine.Data = itemGD;
			}
		}

		/// <summary>
		/// Nga My tự Buff máu
		/// </summary>
		public bool EMAutoBuff
		{
			get
			{
				return this.UIToggle_EMAutoBuff.isOn;
			}
			set
			{
				this.UIToggle_EMAutoBuff.isOn = value;
			}
		}

		private int _EMAutoHP = 0;
		/// <summary>
		/// Ngưỡng sinh lực Nga My tự Buff
		/// </summary>
		public int EMAutoHP
		{
			get
			{
				return this._EMAutoHP;
			}
			set
			{
				this._EMAutoHP = value;
				this.UIText_EMAutoHP.text = value.ToString();
			}
		}

		/// <summary>
		/// Nga My tự hồi sinh đồng đội
		/// </summary>
		public bool EMAutoRevive
		{
			get
			{
				return this.UIToggle_EMAutoRevive.isOn;
			}
			set
			{
				this.UIToggle_EMAutoRevive.isOn = value;
			}
		}

		/// <summary>
		/// Tự phản kháng khi bị PK
		/// </summary>
		public bool AutoReflectAttack
		{
			get
			{
				return this.UIToggle_AutoReflectAttack.isOn;
			}
			set
			{
				this.UIToggle_AutoReflectAttack.isOn = value;
			}
		}

		/// <summary>
		/// Ưu tiên khắc hệ
		/// </summary>
		public bool SeriesConquePriority
		{
			get
			{
				return this.UIToggle_SeriesConquePriority.isOn;
			}
			set
			{
				this.UIToggle_SeriesConquePriority.isOn = value;
			}
		}

		/// <summary>
		/// Ưu tiên mục tiêu ít máu
		/// </summary>
		public bool LowHPEnemyPriority
		{
			get
			{
				return this.UIToggle_LowHPEnemyPriority.isOn;
			}
			set
			{
				this.UIToggle_LowHPEnemyPriority.isOn = value;
			}
		}

		/// <summary>
		/// Hiện mục tiêu ở gần
		/// </summary>
		public bool ShowNearbyEnemy
		{
			get
			{
				return this.UIToggle_ShowNearbyEnemy.isOn;
			}
			set
			{
				this.UIToggle_ShowNearbyEnemy.isOn = value;
			}
		}

		/// <summary>
		/// Sử dụng kỹ năng tân thủ
		/// </summary>
		public bool UseNewbieSkill
		{
			get
			{
				return this.UIToggle_UseNewbieSkill.isOn;
			}
			set
			{
				this.UIToggle_UseNewbieSkill.isOn = value;
			}
		}

		/// <summary>
		/// Đuổi mục tiêu
		/// </summary>
		public bool ChaseTarget
		{
			get
			{
				return this.UIToggle_ChaseTarget.isOn;
			}
			set
			{
				this.UIToggle_ChaseTarget.isOn = value;
			}
		}

		/// <summary>
		/// Sử dụng kỹ năng theo Combo đã thiết lập
		/// </summary>
		public bool UseSkillByCombo
		{
			get
			{
				return this.UIToggle_UseSkillByCombo.isOn;
			}
			set
			{
				this.UIToggle_UseSkillByCombo.isOn = value;
			}
		}

		/// <summary>
		/// Danh sách kỹ năng
		/// </summary>
		public List<int> Skills { get; set; }
		#endregion

		#region Core MonoBehaviour
		/// <summary>
		/// Hàm này gọi khi đối tượng được tạo ra
		/// </summary>
		private void Awake()
		{
			this.transformSkillList = this.UIButton_SkillPrefab.transform.parent.GetComponent<RectTransform>();
		}

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
			this.UIButton_InputAutoUsemedicineHP.onClick.AddListener(this.ButtonInputAutoUseMedicineHP_Clicked);
			this.UIButton_InputAutoUsemedicineMP.onClick.AddListener(this.ButtonInputAutoUseMedicineMP_Clicked);
			this.UIButton_InputEMAutoHP.onClick.AddListener(this.ButtonInputEMAutoHP_Clicked);

			this.UIItem_HPMedicine.Click = this.ButtonHPMedicine_Clicked;
			this.UIItem_MPMedicine.Click = this.ButtonMPMedicine_Clicked;
		}

		/// <summary>
		/// Sự kiện khi Button chọn thuốc hồi sinh lực được ấn
		/// </summary>
		private void ButtonHPMedicine_Clicked()
		{
			/// Danh sách thuốc
			List<ItemData> items = new List<ItemData>();
			/// Duyệt danh sách vật phẩm tương ứng
			foreach (ItemData itemData in KTGlobal.ListHPMedicines.Values)
			{
				/// Nếu có vật phẩm tương ứng trong người
				if (KTGlobal.HaveItem(itemData.ItemID))
				{
					items.Add(itemData);
				}
			}

			/// Hiện khung chọn thuốc
			this.Parent.ShowSelectItem(items, (itemData) => {
				this.HPMedicineID = itemData.ItemID;
			});
		}

		/// <summary>
		/// Sự kiện khi Button chọn thuốc hồi nội lực được ấn
		/// </summary>
		private void ButtonMPMedicine_Clicked()
		{
			/// Danh sách thuốc
			List<ItemData> items = new List<ItemData>();
			/// Duyệt danh sách vật phẩm tương ứng
			foreach (ItemData itemData in KTGlobal.ListMPMedicines.Values)
			{
				/// Nếu có vật phẩm tương ứng trong người
				if (KTGlobal.HaveItem(itemData.ItemID))
				{
					items.Add(itemData);
				}
			}

			/// Hiện khung chọn thuốc
			this.Parent.ShowSelectItem(items, (itemData) => {
				this.MPMedicineID = itemData.ItemID;
			});
		}

		/// <summary>
		/// Sự kiện khi Button kỹ năng được chọn
		/// </summary>
		/// <param name="idx"></param>
		private void ButtonSkill_Clicked(int idx)
		{
			/// Hiện khung chọn kỹ năng
			this.Parent.ShowSelectSkill((skillData) => {
				/// Vị trí tương ứng
				UIAutoFight_SkillButton uiSkillButton = this.FindSkillButton(idx);
				/// Nếu không tồn tại
				if (uiSkillButton == null)
				{
					return;
				}

				/// Đổ dữ liệu
				uiSkillButton.Data = skillData;
				/// Gắn lại vào Property
				this.Skills[idx - 1] = skillData.ID;
			});
		}

		/// <summary>
		/// Sự kiện khi Button tự dùng thuốc hồi sinh lực được ấn
		/// </summary>
		private void ButtonInputAutoUseMedicineHP_Clicked()
		{
			KTGlobal.ShowInputNumber("Nhập mức sinh lực tối thiểu sẽ tự dùng thuốc.", 0, 100, (number) => {
				this.AutoUseMedicineHP = number;
			});
		}

		/// <summary>
		/// Sự kiện khi Button tự dùng thuốc hồi nội lực được ấn
		/// </summary>
		private void ButtonInputAutoUseMedicineMP_Clicked()
		{
			KTGlobal.ShowInputNumber("Nhập mức nội lực tối thiểu sẽ tự dùng thuốc.", 0, 100, (number) => {
				this.AutoUseMedicineMP = number;
			});
		}

		/// <summary>
		/// Sự kiện khi Button Nga My tự Buff sinh lực được ấn
		/// </summary>
		private void ButtonInputEMAutoHP_Clicked()
		{
			KTGlobal.ShowInputNumber("Nhập mức nội lực tối thiểu của đồng đội mà Nga My sẽ tự Buff.", 0, 100, (number) => {
				this.EMAutoHP = number;
			});
		}
		#endregion

		#region Private methods
		/// <summary>
		/// Thực thi sự kiện bỏ qua một số Frame
		/// </summary>
		/// <param name="skip"></param>
		/// <param name="work"></param>
		/// <returns></returns>
		private IEnumerator ExecuteSkipFrames(int skip, Action work)
		{
			for (int i = 1; i <= skip; i++)
			{
				yield return null;
			}
			work?.Invoke();
		}

		/// <summary>
		/// Xây lại giao diện
		/// </summary>
		private void RebuildLayout()
		{
			/// Nếu đối tượng không kích hoạt
			if (!this.gameObject.activeSelf)
			{
				return;
			}
			/// Thực hiện xây lại giao diện ở Frame tiếp theo
			this.StartCoroutine(this.ExecuteSkipFrames(1, () => {
				UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(this.transformSkillList);
			}));
		}

		/// <summary>
		/// Làm rỗng danh sách kỹ năng
		/// </summary>
		private void ClearSkillList()
		{
			foreach (Transform child in this.transformSkillList.transform)
			{
				if (child.gameObject != this.UIButton_SkillPrefab.gameObject)
				{
					GameObject.Destroy(child.gameObject);
				}
			}
		}

		/// <summary>
		/// Khởi tạo các ô mặc định kỹ năng
		/// </summary>
		private void MakeDefaultSlot()
		{
			for (int i = 1; i <= UIAutoFight.NumberOfSkills; i++)
			{
				UIAutoFight_SkillButton uiSkillButton = GameObject.Instantiate<UIAutoFight_SkillButton>(this.UIButton_SkillPrefab);
				uiSkillButton.transform.SetParent(this.transformSkillList, false);
				uiSkillButton.gameObject.SetActive(true);
				uiSkillButton.Data = null;
				uiSkillButton.Slot = i;
				uiSkillButton.Click = () => {
					this.ButtonSkill_Clicked(uiSkillButton.Slot);
				};
				uiSkillButton.ShowArrow = i < UIAutoFight.NumberOfSkills;
			}
		}

		/// <summary>
		/// Tìm ô kỹ năng tại vị trí tương ứng
		/// </summary>
		/// <param name="idx"></param>
		/// <returns></returns>
		private UIAutoFight_SkillButton FindSkillButton(int idx)
		{
			foreach (Transform child in this.transformSkillList.transform)
			{
				if (child.gameObject != this.UIButton_SkillPrefab.gameObject)
				{
					UIAutoFight_SkillButton uiSkillButton = child.GetComponent<UIAutoFight_SkillButton>();
					if (uiSkillButton.Slot == idx)
					{
						return uiSkillButton;
					}
				}
			}
			return null;
		}

		/// <summary>
		/// Làm mới dữ liệu
		/// </summary>
		private void Refresh()
		{
			/// Xóa rỗng danh sách kỹ năng
			this.ClearSkillList();

			/// Tạo mặc định các ô kỹ năng
			this.MakeDefaultSlot();

			/// Nếu không có dữ liệu kỹ năng
			if (this.Skills == null)
			{
				return;
			}

			/// Vị trí
			int idx = 0;
			/// Duyệt danh sách kỹ năng
			foreach (int skillID in this.Skills)
			{
				/// Tăng vị trí lên
				idx++;
				/// Nếu không có kỹ năng ở vị trí này
				if (skillID == -1)
				{
					continue;
				}

				/// Kỹ năng tương ứng
				if (!Loader.Loader.Skills.TryGetValue(skillID, out SkillDataEx skillData))
				{
					continue;
				}

				/// Vị trí tương ứng
				UIAutoFight_SkillButton uiSkillButton = this.FindSkillButton(idx);
				/// Nếu không tồn tại
				if (uiSkillButton == null)
				{
					continue;
				}

				/// Đổ dữ liệu
				uiSkillButton.Data = skillData;
			}

			/// Xây lại giao diện
			this.RebuildLayout();
		}
		#endregion
	}
}
