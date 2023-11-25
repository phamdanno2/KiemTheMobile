using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using FSPlay.KiemVu.Utilities.UnityUI;
using FSPlay.KiemVu.UI.Main.Crafting;
using FSPlay.KiemVu.UI.Main.ItemBox;
using System.Collections;
using FSPlay.KiemVu.Entities.Config;
using Server.Data;
using FSPlay.GameEngine.Logic;

namespace FSPlay.KiemVu.UI.Main
{
    /// <summary>
    /// Khung chế tạo
    /// </summary>
    public class UICrafting : MonoBehaviour
    {
        #region Define
        /// <summary>
        /// Button đóng khung
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_Close;

        /// <summary>
        /// Tab gia công
        /// </summary>
        [SerializeField]
        private UIToggleSprite UIToggle_GatheringTab;

        /// <summary>
        /// Tab chế tạo
        /// </summary>
        [SerializeField]
        private UIToggleSprite UIToggle_CraftingTab;

        /// <summary>
        /// Text giá trị hoạt lực
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_MakingPoint;

        /// <summary>
        /// Text gía trị tinh lực
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_GatherPoint;

        /// <summary>
        /// Text yêu cầu tinh/hoạt lực tương ứng
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_RequirePoint;

        /// <summary>
        /// Dropdown danh mục
        /// </summary>
        [SerializeField]
        private TMP_Dropdown UIDropdown_Category;

        /// <summary>
        /// Dropdown loại chế tạo
        /// </summary>
        [SerializeField]
        private TMP_Dropdown UIDropdown_KindOfCrafting;

        /// <summary>
        /// Dropdown cấp độ sản phẩm
        /// </summary>
        [SerializeField]
        private TMP_Dropdown UIDropdown_ProductLevel;

        /// <summary>
        /// Text kinh nghiệm nhận được sau khi chế
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_ExpAfterCrafting;

        /// <summary>
        /// Slider độ thành thục kỹ năng nghề hiện tại
        /// </summary>
        [SerializeField]
        private UISliderText UISlider_ExpProgress;

        /// <summary>
        /// Text cấp độ kỹ năng nghề hiện tại
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_SkillLevel;

        /// <summary>
        /// Prefab vật phẩm chế tạo
        /// </summary>
        [SerializeField]
        private UICrafting_CraftingItem UI_CraftingItemPrefab;

        /// <summary>
        /// Prefab sản phẩm có thể chế tạo ra
        /// </summary>
        [SerializeField]
        private UICrafting_ProductItem UI_ProductItemPrefab;

        /// <summary>
        /// Prefab nguyên liệu chế
        /// </summary>
        [SerializeField]
        private UIItemBox UI_CraftingMaterialPrefab;

        /// <summary>
        /// Progress tiến trình chế tạo
        /// </summary>
        [SerializeField]
        private UISliderText UISlider_Progress;

        /// <summary>
        /// Button chế tạo
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_Craft;

        /// <summary>
        /// Button chế tạo toàn bộ
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_CraftAll;

        /// <summary>
        /// Button dừng chế tạo toàn bộ
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_StopCraftAll;
        #endregion

        #region Private fields
        /// <summary>
        /// RectTransform danh sách vật phẩm chế tạo
        /// </summary>
        private RectTransform transformCraftingItemsList = null;

        /// <summary>
        /// RectTransform danh sách sản phẩm
        /// </summary>
        private RectTransform transformProductsList = null;

        /// <summary>
        /// RectTransform danh sách nguyên liệu cần
        /// </summary>
        private RectTransform transformMaterialsList = null;

        /// <summary>
        /// Công thức chế hiện tại
        /// </summary>
        private Recipe currentRecipe = null;

        /// <summary>
        /// Danh sách công thức theo danh mục hiện tại
        /// </summary>
        private List<Recipe> recipes = null;

        /// <summary>
        /// Danh sách loại chế theo danh mục tương ứng
        /// </summary>
        private List<RecipeDesc> recipeKinds = null;

        /// <summary>
        /// Danh sách chế theo cấp độ
        /// </summary>
        private List<byte> recipeByLevels = null;

        /// <summary>
        /// Danh mục đang được chọn
        /// </summary>
        private int selectedCategory = -1;

        /// <summary>
        /// Loại chế đang được chọn
        /// </summary>
        private int selectedKind = -1;

        /// <summary>
        /// Cấp độ chế đang được chọn
        /// </summary>
        private int selectedLevel = -1;

        /// <summary>
        /// Tiến trình chế tạo
        /// </summary>
        private Coroutine progressCoroutine = null;

        /// <summary>
        /// Luồng tự thực thi chế tạo
        /// </summary>
        private Coroutine autoCraftCoroutine = null;
        #endregion

        #region Properties
        /// <summary>
        /// Sự kiện đóng khung
        /// </summary>
        public Action Close { get; set; }

        /// <summary>
        /// Sự kiện chế tạo vật phẩm
        /// </summary>
        public Action<Recipe> Craft { get; set; }
        #endregion

        #region Core MonoBehaviour
        /// <summary>
        /// Hàm này gọi khi đối tượng được tạo ra
        /// </summary>
        private void Awake()
        {
            this.transformCraftingItemsList = this.UI_CraftingItemPrefab.transform.parent.GetComponent<RectTransform>();
            this.transformProductsList = this.UI_ProductItemPrefab.transform.parent.GetComponent<RectTransform>();
            this.transformMaterialsList = this.UI_CraftingMaterialPrefab.transform.parent.GetComponent<RectTransform>();
        }

        /// <summary>
        /// Hàm này gọi ở Frame đầu tiên
        /// </summary>
        private void Start()
        {
            this.InitPrefabs();
            this.UpdatePoints();
            this.UpdateDisplayLevelAndExp();
        }
        #endregion

        #region Code UI
        /// <summary>
        /// Khởi tạo ban đầu
        /// </summary>
        private void InitPrefabs()
        {
            this.UIDropdown_Category.onValueChanged.AddListener(this.DropdownCategory_ItemSelected);
            this.UIDropdown_KindOfCrafting.onValueChanged.AddListener(this.DropdownKindOfCrafting_ItemSelected);
            this.UIDropdown_ProductLevel.onValueChanged.AddListener(this.DropdownProductLevel_ItemSelected);
            this.UIDropdown_Category.interactable = false;
            this.UIDropdown_KindOfCrafting.interactable = false;
            this.UIDropdown_ProductLevel.interactable = false;
            this.UIText_RequirePoint.text = "0";
            this.UIText_ExpAfterCrafting.text = "0";
            this.UIText_RequirePoint.transform.parent.gameObject.SetActive(false);
            this.UIText_GatherPoint.transform.parent.gameObject.SetActive(false);
            this.UIText_MakingPoint.transform.parent.gameObject.SetActive(false);

            this.UIToggle_GatheringTab.OnSelected = (isSelected) => {
                if (isSelected)
                {
                    this.ToggleGatheringTab_Selected();
                }
            };
            this.UIToggle_CraftingTab.OnSelected = (isSelected) => {
                if (isSelected)
                {
                    this.ToggleCraftingTab_Selected();
                }
            };
            this.UIButton_Close.onClick.AddListener(this.ButtonClose_Clicked);
            this.UIButton_Craft.onClick.AddListener(() => {
                this.ButtonCraft_Clicked();
            });
            this.UIButton_CraftAll.onClick.AddListener(this.ButtonCraftAll_Clicked);
            this.UIButton_StopCraftAll.onClick.AddListener(this.ButtonStopCraftAll_Clicked);

            this.UISlider_Progress.Value = 0;

            /// Mặc định chọn Tab đầu tiên
            this.ToggleGatheringTab_Selected();
        }

        /// <summary>
        /// Sự kiện khi Toggle gia công được ấn
        /// </summary>
        private void ToggleGatheringTab_Selected()
        {
            /// Làm rỗng UI
            this.UIDropdown_Category.options.Clear();
            this.UIDropdown_KindOfCrafting.options.Clear();
            this.UIDropdown_ProductLevel.options.Clear();
            this.selectedCategory = -1;
            this.selectedKind = -1;
            this.selectedLevel = -1;
            this.ClearCraftingItemsList();
            this.ClearMaterialsList();
            this.ClearProductsList();
            this.UIDropdown_Category.interactable = true;
            this.UIDropdown_KindOfCrafting.interactable = false;
            this.UIDropdown_ProductLevel.interactable = false;
            this.SetButtonsState(false);
            this.UIText_RequirePoint.text = "0";
            this.UIText_ExpAfterCrafting.text = "0";
            this.UIText_RequirePoint.transform.parent.gameObject.SetActive(false);
            this.UIText_GatherPoint.transform.parent.gameObject.SetActive(true);
            this.UIText_MakingPoint.transform.parent.gameObject.SetActive(false);

            /// Đổ dữ liệu danh sách danh mục kỹ năng
            foreach (LifeSkillData lifeSkillData in Loader.Loader.LifeSkills.TotalSkill.Where(x => x.Gene == 1).ToList())
            {
                this.UIDropdown_Category.options.Add(new TMP_Dropdown.OptionData()
                {
                    text = lifeSkillData.Name,
                });
            }

            /// Mặc định
            this.UIDropdown_Category.options.Add(new TMP_Dropdown.OptionData()
            {
                text = "--- Danh mục ---",
            });
            //this.UIDropdown_Category.captionText.text = this.UIDropdown_Category.options[this.UIDropdown_Category.options.Count - 1].text;
            this.UIDropdown_Category.value = this.UIDropdown_Category.options.Count - 1;

            /// Xây lại giao diện
            this.RebuildLayout(this.transformCraftingItemsList);
        }

        /// <summary>
        /// Sự kiện khi Toggle chế tạo được chọn
        /// </summary>
        private void ToggleCraftingTab_Selected()
        {
            /// Làm rỗng UI
            this.UIDropdown_Category.options.Clear();
            this.UIDropdown_KindOfCrafting.options.Clear();
            this.UIDropdown_ProductLevel.options.Clear();
            this.selectedCategory = -1;
            this.selectedKind = -1;
            this.selectedLevel = -1;
            this.ClearCraftingItemsList();
            this.ClearMaterialsList();
            this.ClearProductsList();
            this.UIDropdown_Category.interactable = true;
            this.UIDropdown_KindOfCrafting.interactable = false;
            this.UIDropdown_ProductLevel.interactable = false;
            this.SetButtonsState(false);
            this.UIText_RequirePoint.text = "0";
            this.UIText_ExpAfterCrafting.text = "0";
            this.UIText_RequirePoint.transform.parent.gameObject.SetActive(false);
            this.UIText_GatherPoint.transform.parent.gameObject.SetActive(false);
            this.UIText_MakingPoint.transform.parent.gameObject.SetActive(true);

            /// Đổ dữ liệu danh sách danh mục kỹ năng
            foreach (LifeSkillData lifeSkillData in Loader.Loader.LifeSkills.TotalSkill.Where(x => x.Gene == 0).ToList())
            {
                this.UIDropdown_Category.options.Add(new TMP_Dropdown.OptionData()
                {
                    text = lifeSkillData.Name,
                });
            }

            /// Mặc định
            this.UIDropdown_Category.options.Add(new TMP_Dropdown.OptionData()
            {
                text = "--- Danh mục ---",
            });
            //this.UIDropdown_Category.captionText.text = this.UIDropdown_Category.options[this.UIDropdown_Category.options.Count - 1].text;
            this.UIDropdown_Category.value = this.UIDropdown_Category.options.Count - 1;

            /// Xây lại giao diện
            this.RebuildLayout(this.transformCraftingItemsList);
        }

        /// <summary>
        /// Sự kiện khi Button đóng khung được ấn
        /// </summary>
        private void ButtonClose_Clicked()
        {
            this.Close?.Invoke();
        }

        /// <summary>
        /// Sự kiện khi Button chế tạo được ấn
        /// </summary>
        private bool ButtonCraft_Clicked()
        {
            /// Nếu chưa có sản phẩm chế
            if (this.currentRecipe == null)
            {
                KTGlobal.AddNotification("Hãy chọn một sản phẩm chế!");
                return false;
            }

            /// Kiểm tra số nguyên liệu có
            foreach (ItemStuff itemStuff in this.currentRecipe.ListStuffRequest)
            {
                /// Nếu vật phẩm không tồn tại
                if (!Loader.Loader.Items.TryGetValue(itemStuff.ItemTemplateID, out ItemData itemData))
                {
                    continue;
                }

                /// Nếu số nguyên liệu không đủ
                if (KTGlobal.GetItemCountInBag(itemStuff.ItemTemplateID) < itemStuff.Number)
                {
                    KTGlobal.AddNotification(string.Format("Số lượng {0} không đủ!", itemData.Name));
                    return false;
                }
            }

            /// Khóa Button chức năng
            this.UIButton_Close.interactable = false;
            this.SetButtonsState(false);

            /// Thực hiện sự kiện chế tạo
            this.Craft?.Invoke(this.currentRecipe);

            /// Trả ra kết quả thành công
            return true;
        }

        /// <summary>
        /// Sự kiện khi Button chế tạo toàn bộ được ấn
        /// </summary>
        private void ButtonCraftAll_Clicked()
        {
            this.StartAutoCraft();
        }

        /// <summary>
        /// Sự kiện khi Button chế tạo toàn bộ được ấn
        /// </summary>
        private void ButtonStopCraftAll_Clicked()
        {
            this.StopAutoCraft();
        }

        /// <summary>
        /// Sự kiện khi Button vật phẩm có thể chế tạo được chọn
        /// </summary>
        /// <param name="uiItem"></param>
        private void ButtonCraftingItem_Clicked(UICrafting_CraftingItem uiItem)
        {
            /// Thiết lập trạng thái Button chức năng
            this.SetButtonsState(false);

            /// Nếu không có dữ liệu
            if (uiItem.Data == null)
            {
                return;
            }

            /// Kỹ năng sống hiện tại
            LifeSkillData lifeSkillData = Loader.Loader.LifeSkills.TotalSkill.Where(x => x.Belong == this.selectedCategory).FirstOrDefault();
            /// Nếu kỹ năng sống không tồn tại
            if (lifeSkillData == null)
            {
                return;
            }

            /// Nếu đủ điểm
            if ((lifeSkillData.Gene == 0 && Global.Data.RoleData.MakePoint >= uiItem.Data.Cost) || (lifeSkillData.Gene == 1 && Global.Data.RoleData.GatherPoint >= uiItem.Data.Cost))
            {
                /// Mở Button chức năng
                this.SetButtonsState(true);
            }

            /// Kinh nghiệm có được
            this.UIText_ExpAfterCrafting.text = uiItem.Data.ExpGain.ToString();

            /// Xóa rỗng danh sách sản phẩm và nguyên liệu cần
            this.ClearProductsList();
            this.ClearMaterialsList();

            /// Thêm danh sách sản phẩm tương ứng
            foreach (ItemCraf itemCraft in uiItem.Data.ListProduceOut)
            {
                if (Loader.Loader.Items.TryGetValue(itemCraft.ItemTemplateID, out ItemData itemData))
                {
                    this.AddProduct(itemData, itemCraft.Rate);
                }
            }
            /// Thêm danh sách nguyên liệu cần tương ứng
            foreach (ItemStuff itemStuff in uiItem.Data.ListStuffRequest)
            {
                if (Loader.Loader.Items.TryGetValue(itemStuff.ItemTemplateID, out ItemData itemData))
                {
                    this.AddMaterial(itemData, itemStuff.Number);
                }
            }

            /// Yêu cầu tinh/hoạt lực
            if (this.UIToggle_CraftingTab.Active)
            {
                if (Global.Data.RoleData.MakePoint >= uiItem.Data.Cost)
                {
                    this.UIText_RequirePoint.text = string.Format("<color=green>{0}</color>", uiItem.Data.Cost);
                }
                else
                {
                    this.UIText_RequirePoint.text = string.Format("<color=red>{0}</color>", uiItem.Data.Cost);
                }
            }
            else
            {
                if (Global.Data.RoleData.GatherPoint >= uiItem.Data.Cost)
                {
                    this.UIText_RequirePoint.text = string.Format("<color=green>{0}</color>", uiItem.Data.Cost);
                }
                else
                {
                    this.UIText_RequirePoint.text = string.Format("<color=red>{0}</color>", uiItem.Data.Cost);
                }
            }
            this.UIText_RequirePoint.transform.parent.gameObject.SetActive(true);

            /// Xây lại giao diện
            this.RebuildLayout(this.transformProductsList);
            this.RebuildLayout(this.transformMaterialsList);

            /// Ghi lại loại vật phẩm chế được chọn
            this.currentRecipe = uiItem.Data;
        }

        /// <summary>
        /// Sự kiện khi danh mục kỹ năng được chọn
        /// </summary>
        /// <param name="idx"></param>
        private void DropdownCategory_ItemSelected(int idx)
        {
            /// Làm rỗng UI
            this.UIDropdown_KindOfCrafting.options.Clear();
            this.UIDropdown_ProductLevel.options.Clear();
            this.selectedKind = -1;
            this.selectedLevel = -1;
            this.ClearCraftingItemsList();
            this.ClearMaterialsList();
            this.ClearProductsList();
            this.UIDropdown_ProductLevel.interactable = false;
            this.SetButtonsState(false);
            this.UIText_RequirePoint.text = "0";
            this.UIText_ExpAfterCrafting.text = "0";
            this.UIText_RequirePoint.transform.parent.gameObject.SetActive(false);

            /// Nếu idx vượt quá
            if (idx < 0 || idx >= this.UIDropdown_Category.options.Count - 1)
            {
                return;
            }

            string lifeSkillName = this.UIDropdown_Category.options[idx].text;
            /// Kỹ năng sống tương ứng
            LifeSkillData lifeSkillData = Loader.Loader.LifeSkills.TotalSkill.Where(x => x.Name == lifeSkillName).FirstOrDefault();
            /// Danh sách công thức tương ứng
            this.recipes = Loader.Loader.LifeSkills.TotalRecipe.Where(x => x.Belong == lifeSkillData.Belong).ToList();

            /// Mở tương tác các Dropdown con
            this.UIDropdown_KindOfCrafting.interactable = this.recipes != null && this.recipes.Count > 0;

            /// Làm rỗng danh sách chọn
            this.UIDropdown_KindOfCrafting.options.Clear();

            /// Đổ dữ liệu loại chế
            HashSet<short> recipeByKinds = new HashSet<short>(this.recipes.GroupBy(x => x.Kind).Select(x => x.FirstOrDefault()).Select(x => x.Kind));
            this.recipeKinds = Loader.Loader.LifeSkills.TotalRecipeDesc.Where(x => recipeByKinds.Contains(x.KindId)).ToList();
            foreach (RecipeDesc recipeKind in this.recipeKinds)
            {
                this.UIDropdown_KindOfCrafting.options.Add(new TMP_Dropdown.OptionData()
                {
                    text = recipeKind.Name,
                });
            }
            /// Mặc định
            this.UIDropdown_KindOfCrafting.options.Add(new TMP_Dropdown.OptionData()
            {
                text = "--- Loại chế ---",
            });
            //this.UIDropdown_KindOfCrafting.captionText.text = this.UIDropdown_KindOfCrafting.options[this.UIDropdown_KindOfCrafting.options.Count - 1].text;
            this.UIDropdown_KindOfCrafting.value = this.UIDropdown_KindOfCrafting.options.Count - 1;

            /// Đánh dấu danh mục đang được chọn
            this.selectedCategory = lifeSkillData.Belong;

            /// Lấy thông tin kỹ năng sống tương ứng
            this.UpdateDisplayLevelAndExp();
        }

        /// <summary>
        /// Sự kiện khi loại chế tạo được chọn
        /// </summary>
        /// <param name="idx"></param>
        private void DropdownKindOfCrafting_ItemSelected(int idx)
        {
            this.UIDropdown_ProductLevel.options.Clear();
            this.ClearCraftingItemsList();
            this.ClearMaterialsList();
            this.ClearProductsList();
            this.SetButtonsState(false);
            this.UIText_RequirePoint.text = "0";
            this.UIText_ExpAfterCrafting.text = "0";
            this.UIText_RequirePoint.transform.parent.gameObject.SetActive(false);

            /// Nếu idx vượt quá
            if (idx < 0 || idx >= this.recipeKinds.Count)
            {
                return;
            }

            /// Mở tương tác các Dropdown con
            this.UIDropdown_ProductLevel.interactable = this.recipeKinds != null && this.recipeKinds.Count > 0;

            /// Cấp độ kỹ năng nghề hiện tại
            this.GetCurrentLifeSkillLevelAndExp(out int currentLifeSkillLevel, out int currentLifeSkillExp);

            /// Làm rỗng danh sách chọn
            this.UIDropdown_ProductLevel.options.Clear();

            /// Đổ dữ liệu cấp độ chế
            this.recipeByLevels = new List<byte>(this.recipes.Where(x => x.Kind == this.recipeKinds[idx].KindId && x.SkillLevel <= currentLifeSkillLevel).GroupBy(x => x.SkillLevel).Select(x => x.FirstOrDefault()).Select(x => x.SkillLevel));
            foreach (int level in this.recipeByLevels)
            {
                this.UIDropdown_ProductLevel.options.Add(new TMP_Dropdown.OptionData()
                {
                    text = string.Format("Cấp độ {0}", level),
                });
            }

            /// Mặc định
            this.UIDropdown_ProductLevel.options.Add(new TMP_Dropdown.OptionData()
            {
                text = "--- Cấp độ ---",
            });
            //this.UIDropdown_ProductLevel.captionText.text = this.UIDropdown_ProductLevel.options[this.UIDropdown_ProductLevel.options.Count - 1].text;
            this.UIDropdown_ProductLevel.value = this.UIDropdown_ProductLevel.options.Count - 1;

            /// Đánh dấu danh mục đang được chọn
            this.selectedKind = this.recipeKinds[idx].KindId;
        }

        /// <summary>
        /// Sự kiện khi cấp độ chế tạo được chọn
        /// </summary>
        /// <param name="idx"></param>
        private void DropdownProductLevel_ItemSelected(int idx)
        {
            this.ClearCraftingItemsList();
            this.ClearMaterialsList();
            this.ClearProductsList();
            this.SetButtonsState(false);
            this.UIText_RequirePoint.text = "0";
            this.UIText_ExpAfterCrafting.text = "0";
            this.UIText_RequirePoint.transform.parent.gameObject.SetActive(false);

            /// Nếu idx vượt quá
            if (idx < 0 || idx >= this.recipeByLevels.Count)
            {
                return;
            }

            /// Cấp độ kỹ năng nghề hiện tại
            this.GetCurrentLifeSkillLevelAndExp(out int currentLifeSkillLevel, out int currentLifeSkillExp);

            /// Đổ dữ liệu cho danh sách vật phẩm chế
            List<Recipe> thisRecipes = this.recipes.Where(x => x.Belong == this.selectedCategory && x.Kind == this.selectedKind && x.SkillLevel == this.recipeByLevels[idx]).ToList();
            foreach (Recipe recipe in thisRecipes)
            {
                ItemCraf itemCraft = recipe.ListProduceOut.FirstOrDefault();
                if (itemCraft != null && Loader.Loader.Items.TryGetValue(itemCraft.ItemTemplateID, out ItemData itemData))
                {
                    this.AddCraftingItem(recipe);
                }
            }
            /// Xây lại giao diện
            this.RebuildLayout(this.transformCraftingItemsList);

            /// Đánh dấu cấp độ đang được chọn
            this.selectedLevel = this.recipeByLevels[idx];
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Bắt đầu chạy tiến trình chế tạo
        /// </summary>
        /// <param name="duration"></param>
        /// <returns></returns>
        private IEnumerator DoProgress(float duration)
        {
            /// Thiết lập thời gian tồn tại
            float lifeTime = 0f;
            /// Cập nhật giá trị cho ProgressBar
            this.UISlider_Progress.Value = 0;
            /// Bỏ qua Frame
            yield return null;
            /// Lặp liên tục chừng nào chưa hết thời gian
            while (lifeTime < duration)
            {
                /// Tăng thời gian tồn tại
                lifeTime += Time.deltaTime;
                /// Cập nhật giá trị cho ProgressBar
                this.UISlider_Progress.Value = (int) (lifeTime * 100 / duration);
                /// Bỏ qua Frame
                yield return null;
            }
            /// Cập nhật giá trị cho ProgressBar
            this.UISlider_Progress.Value = 100;
            /// Bỏ qua Frame
            yield return null;
            /// Hủy luồng
            this.progressCoroutine = null;
        }

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
        /// Xây lại giao diện tương ứng
        /// </summary>
        /// <param name="rectTransform"></param>
        private void RebuildLayout(RectTransform rectTransform)
        {
            this.StartCoroutine(this.ExecuteSkipFrames(1, () => {
                UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
            }));
        }

        /// <summary>
        /// Làm rỗng danh sách vật phẩm chế tạo
        /// </summary>
        private void ClearCraftingItemsList()
        {
            foreach (Transform child in this.transformCraftingItemsList.transform)
            {
                if (child.gameObject != this.UI_CraftingItemPrefab.gameObject)
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
            this.RebuildLayout(this.transformCraftingItemsList);
        }

        /// <summary>
        /// Làm rỗng danh sách sản phẩm
        /// </summary>
        private void ClearProductsList()
        {
            foreach (Transform child in this.transformProductsList.transform)
            {
                if (child.gameObject != this.UI_ProductItemPrefab.gameObject)
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
            this.RebuildLayout(this.transformProductsList);
        }

        /// <summary>
        /// Làm rỗng danh sách nguyên liệu yêu cầu
        /// </summary>
        private void ClearMaterialsList()
        {
            foreach (Transform child in this.transformMaterialsList.transform)
            {
                if (child.gameObject != this.UI_CraftingMaterialPrefab.gameObject)
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
            this.RebuildLayout(this.transformMaterialsList);
        }

        /// <summary>
        /// Thêm vật phẩm vào danh sách có thể chế tạo
        /// </summary>
        /// <param name="itemData"></param>
        private void AddCraftingItem(Recipe recipe)
        {
            UICrafting_CraftingItem uiItem = GameObject.Instantiate<UICrafting_CraftingItem>(this.UI_CraftingItemPrefab);
            uiItem.transform.SetParent(this.transformCraftingItemsList, false);
            uiItem.gameObject.SetActive(true);

            uiItem.Data = recipe;
            uiItem.Selected = () => {
                this.ButtonCraftingItem_Clicked(uiItem);
            };
        }

        /// <summary>
        /// Thêm sản phẩm vào danh sách
        /// </summary>
        /// <param name="itemData"></param>
        /// <param name="rate"></param>
        private void AddProduct(ItemData itemData, int rate)
        {
            UICrafting_ProductItem uiItem = GameObject.Instantiate<UICrafting_ProductItem>(this.UI_ProductItemPrefab);
            uiItem.transform.SetParent(this.transformProductsList, false);
            uiItem.gameObject.SetActive(true);

            uiItem.Data = KTGlobal.CreateItemPreview(itemData);
            uiItem.Rate = rate;
        }

        /// <summary>
        /// Thêm nguyên liệu vào danh sách
        /// </summary>
        /// <param name="itemData"></param>
        private void AddMaterial(ItemData itemData, int count)
        {
            UIItemBox uiItem = GameObject.Instantiate<UIItemBox>(this.UI_CraftingMaterialPrefab);
            uiItem.transform.SetParent(this.transformMaterialsList, false);
            uiItem.gameObject.SetActive(true);

            GoodsData itemGD = KTGlobal.CreateItemPreview(itemData);
            itemGD.GCount = count;
            uiItem.Data = itemGD;
        }

        /// <summary>
        /// Thiết lập trạng thái cho các Button chức năng
        /// </summary>
        /// <param name="isEnable"></param>
        private void SetButtonsState(bool isEnable)
        {
            this.UIButton_Craft.interactable = isEnable;
            this.UIButton_CraftAll.interactable = isEnable;
        }

        /// <summary>
        /// Trả về cấp độ kỹ năng sống hiện tại
        /// </summary>
        /// <param name="currentLifeSkillLevel"></param>
        /// <param name="currentLifeSkillExp"></param>
        /// <returns></returns>
        private bool GetCurrentLifeSkillLevelAndExp(out int currentLifeSkillLevel, out int currentLifeSkillExp)
        {
            /// Nếu dữ liệu kỹ năng sống tồn tại
            if (Global.Data.RoleData.LifeSkills.TryGetValue(this.selectedCategory, out LifeSkillPram lifeSkillParam))
            {
                currentLifeSkillLevel = lifeSkillParam.LifeSkillLevel;
                currentLifeSkillExp = lifeSkillParam.LifeSkillExp;
                return true;
            }
            else
            {
                currentLifeSkillLevel = -1;
                currentLifeSkillExp = -1;
                return false;
            }
        }

        /// <summary>
        /// Cập nhật hiển thị cấp độ và kinh nghiệm kỹ năng sống tương ứng
        /// </summary>
        private void UpdateDisplayLevelAndExp()
        {
            this.GetCurrentLifeSkillLevelAndExp(out int currentLifeSkillLevel, out int currentLifeSkillExp);
            if (currentLifeSkillLevel != -1)
            {
                this.UIText_SkillLevel.text = currentLifeSkillLevel.ToString();
            }
            else
            {
                this.UIText_SkillLevel.text = "N/A";
            }

            if (currentLifeSkillExp != -1)
            {
                LifeSkillExp lifeSkillExp = Loader.Loader.LifeSkills.TotalExp.Where(x => x.Level == currentLifeSkillLevel + 1).FirstOrDefault();
                if (lifeSkillExp == null)
                {
                    this.UISlider_ExpProgress.CustomProgressText = () => {
                        return "Cấp tối đa";
                    };
                    this.UISlider_ExpProgress.MinValue = 0;
                    this.UISlider_ExpProgress.MaxValue = 1;
                    this.UISlider_ExpProgress.Value = 0;
                    this.UISlider_ExpProgress.ForceUpdateText();
                }
                else
                {
                    this.UISlider_ExpProgress.CustomProgressText = () => {
                        return string.Format("{0}/{1}", this.UISlider_ExpProgress.Value, this.UISlider_ExpProgress.MaxValue);
                    };
                    this.UISlider_ExpProgress.MinValue = 0;
                    this.UISlider_ExpProgress.MaxValue = lifeSkillExp.Exp;
                    this.UISlider_ExpProgress.Value = currentLifeSkillExp;
                    this.UISlider_ExpProgress.ForceUpdateText();
                }
            }
            else
            {
                this.UISlider_ExpProgress.CustomProgressText = () => {
                    return "N/A";
                };
                this.UISlider_ExpProgress.MinValue = 0;
                this.UISlider_ExpProgress.MaxValue = 1;
                this.UISlider_ExpProgress.Value = 0;
                this.UISlider_ExpProgress.ForceUpdateText();
            }
        }

        /// <summary>
        /// Luồng thực thi tự chế tạo
        /// </summary>
        /// <returns></returns>
        private IEnumerator DoAutoCraft()
        {
            /// Lặp liên tục
            while (true)
            {
                /// Nếu Button chế đang sáng
                if (this.UIButton_Craft.interactable)
                {
                    /// Thực thi sự kiện Click
                    bool ret = this.ButtonCraft_Clicked();
                    /// Nếu thực thi thất bại do lỗi gì đó
                    if (!ret)
                    {
                        /// Ngừng tự chế tạo
                        this.StopAutoCraft();
                    }
                }
                /// Bỏ qua frame
                yield return null;
            }
        }

        /// <summary>
        /// Bắt đầu thực thi tự chế tạo
        /// </summary>
        private void StartAutoCraft()
        {
            /// Nếu đang thực thi
            if (this.autoCraftCoroutine != null)
            {
                this.StopCoroutine(this.autoCraftCoroutine);
            }
            this.autoCraftCoroutine = this.StartCoroutine(this.DoAutoCraft());

            /// Ẩn Button chế toàn bộ và hiện Button hủy
            this.UIButton_CraftAll.gameObject.SetActive(false);
            this.UIButton_StopCraftAll.gameObject.SetActive(true);
        }

        /// <summary>
        /// Ngừng thực thi tự chế tạo
        /// </summary>
        private void StopAutoCraft()
        {
            /// Nếu đang thực thi
            if (this.autoCraftCoroutine != null)
            {
                this.StopCoroutine(this.autoCraftCoroutine);
                this.autoCraftCoroutine = null;
            }

            /// Hiện Button chế toàn bộ và ẩn Button hủy
            this.UIButton_CraftAll.gameObject.SetActive(true);
            this.UIButton_StopCraftAll.gameObject.SetActive(false);
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Cập nhật thông tin tinh hoạt lực
        /// </summary>
        public void UpdatePoints()
        {
            this.UIText_GatherPoint.text = Global.Data.RoleData.GatherPoint.ToString();
            this.UIText_MakingPoint.text = Global.Data.RoleData.MakePoint.ToString();
        }

        /// <summary>
        /// Cập nhật giá trị kinh nghiệm, cấp độ của kỹ năng nghề hiện tại
        /// </summary>
        /// <param name="lifeSkillID"></param>
        public void UpdateCurrentLifeSkillLevelAndExp(int lifeSkillID)
        {
            /// Nếu kỹ năng khác kỹ năng đang chọn hiện tại
            if (lifeSkillID != this.selectedCategory)
            {
                return;
            }

            this.UpdateDisplayLevelAndExp();
        }

        /// <summary>
        /// Bắt đầu tiến trình chế tạo
        /// </summary>
        public void StartProgress()
        {
            /// Nếu không có tiến trình
            if (this.currentRecipe == null)
            {
                return;
            }

            /// Nếu luồng tồn tại
            if (this.progressCoroutine  != null)
            {
                this.StopCoroutine(this.progressCoroutine);
            }
            /// Thực thi luồng chế tạo
            this.progressCoroutine = this.StartCoroutine(this.DoProgress(this.currentRecipe.MakeTime / 18f));
        }

        /// <summary>
        /// Kết thúc tiến trình chế tạo
        /// </summary>
        public void FinishProgress()
        {
            this.UIButton_Close.interactable = true;
            /// Thiết lập trạng thái Button chức năng
            this.SetButtonsState(true);
        }
        #endregion
    }
}
