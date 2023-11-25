using System;
using UnityEngine;
using System.Collections;
using FSPlay.GameEngine.Logic;
using FSPlay.GameFramework.Logic;
using UnityEngine.Networking;
using FSPlay.KiemVu.Entities.Object;
using System.Xml.Linq;
using FSPlay.KiemVu.Factory;

namespace FSPlay.KiemVu.Loader
{
    /// <summary>
    /// Màn hình tải dữ liệu
    /// </summary>
    public class LoadConfig : TTMonoBehaviour
    {
        #region Private fields
        /// <summary>
        /// AssetBundle cấu hình Game
        /// </summary>
        private AssetBundle GameConfigAssetBundle = null;
        #endregion

        #region Properties
        /// <summary>
        /// Báo cáo tiến trình tải dữ liệu
        /// </summary>
        public Action<int, string> OnProgressBarReport { get; set; }

        /// <summary>
        /// Thực thi khi tiến trình tải xuống hoàn tất
        /// </summary>
        public Action OnLoadFinish { get; set; }
        #endregion


        #region Core MonoBehaviour
        /// <summary>
        /// Hàm này gọi đến khi đối tượng được kích hoạt
        /// </summary>
        private void Start()
        {
            GameObject.DontDestroyOnLoad(this);
            this.StartDownloadGameRes();
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Thực thi tải xuống Resource của Game
        /// </summary>
        private void StartDownloadGameRes()
        {
            this.StartCoroutine<bool>(this.InitGameRes(), this.CoroutineException);
        }

        /// <summary>
        /// Hàm này bắt lỗi khi có ngoại lệ xảy ra ở luồng tải dữ liệu
        /// </summary>
        private void CoroutineException()
        {
            
        }

        /// <summary>
        /// Luồng tải xuống Resource của Game
        /// </summary>
        /// <returns></returns>
        private IEnumerator InitGameRes()
        {
            this.OnProgressBarReport?.Invoke(0, "Đang đọc dữ liệu, xin đợi giây lát...");
            yield return null;

            /// Tổng số AssetBundle đã tải
            int totalDoneLoadingAssetBundles = 0;

			#region Tải AssetBundles
			string configPath = Global.WebPath(string.Format("Data/{0}", Consts.GAME_CONFIG_FILE));
            ResourceLoader.LoadAssetBundleAsync(configPath, true, (assetBundle) => {
                this.GameConfigAssetBundle = assetBundle;
                /// Tăng tổng số AssetBundle đã tải thành công
                totalDoneLoadingAssetBundles++;
            }, (errorMessage) => {
                Super.ShowMessageBox("Lỗi tải dữ liệu", "Không thể tải dữ liệu từ file Config.unity3d. Hãy kiểm tra!");
            });
            #endregion

            /// Chừng nào chưa tải xong
            while (totalDoneLoadingAssetBundles < 1)
            {
                /// Đợi
                yield return null;
            }

            //WaitForSeconds wait = new WaitForSeconds(10f);
            WaitForSeconds wait = null;

            #region Đọc dữ liệu từ các file XML bên trong
            this.OnProgressBarReport?.Invoke(1, "Đang đọc dữ liệu, xin đợi giây lát...");
            yield return wait;
            Loader.LoadPlayerPray(ResourceLoader.LoadXMLFromBundle(this.GameConfigAssetBundle, Consts.XML_ACTIIVTY_PLAYERPRAY));

            this.OnProgressBarReport?.Invoke(2, "Đang đọc dữ liệu, xin đợi giây lát...");
            yield return wait;
            Loader.LoadColonyMaps(ResourceLoader.LoadXMLFromBundle(this.GameConfigAssetBundle, Consts.XML_COLONYMAP_FILE));

            this.OnProgressBarReport?.Invoke(3, "Đang đọc dữ liệu, xin đợi giây lát...");
            yield return wait;
            Loader.LoadCrossServerMap(ResourceLoader.LoadXMLFromBundle(this.GameConfigAssetBundle, Consts.XML_CROSSSERVERMAP_FILE));

            this.OnProgressBarReport?.Invoke(4, "Đang đọc dữ liệu, xin đợi giây lát...");
            yield return wait;
            Loader.LoadWorldMap(ResourceLoader.LoadXMLFromBundle(this.GameConfigAssetBundle, Consts.XML_WORLDMAP_FILE));

            this.OnProgressBarReport?.Invoke(5, "Đang đọc dữ liệu, xin đợi giây lát...");
            yield return wait;
            Loader.LoadMapConfig(ResourceLoader.LoadXMLFromBundle(this.GameConfigAssetBundle, Consts.XML_MAP_FILE));

            this.OnProgressBarReport?.Invoke(6, "Đang đọc dữ liệu, xin đợi giây lát...");
            yield return wait;
            Loader.LoadAutoPaths(ResourceLoader.LoadXMLFromBundle(this.GameConfigAssetBundle, Consts.XML_AUTOPATH_FILE));

            this.OnProgressBarReport?.Invoke(10, "Đang đọc dữ liệu, xin đợi giây lát...");
            yield return wait;
            Loader.LoadMonsterActionSetXML(ResourceLoader.LoadXMLFromBundle(this.GameConfigAssetBundle, Consts.XML_MONSTERACTIONSET_FILE));

            this.OnProgressBarReport?.Invoke(12, "Đang đọc dữ liệu, xin đợi giây lát...");
            yield return wait;
            Loader.LoadMonsters(ResourceLoader.LoadXMLFromBundle(this.GameConfigAssetBundle, Consts.XML_MONSTER_FILE));

            this.OnProgressBarReport?.Invoke(16, "Đang đọc dữ liệu, xin đợi giây lát...");
            yield return wait;
            Loader.LoadNPCs(ResourceLoader.LoadXMLFromBundle(this.GameConfigAssetBundle, Consts.XML_NPC_FILE));

            this.OnProgressBarReport?.Invoke(18, "Đang đọc dữ liệu, xin đợi giây lát...");
            yield return wait;
            Loader.LoadRoleAvarta(ResourceLoader.LoadXMLFromBundle(this.GameConfigAssetBundle, Consts.XML_ROLEAVARTA_FILE));

            this.OnProgressBarReport?.Invoke(20, "Đang đọc dữ liệu, xin đợi giây lát...");
            yield return wait;
            Loader.LoadEffects(ResourceLoader.LoadXMLFromBundle(this.GameConfigAssetBundle, Consts.XML_EFFECT_FILE));

            this.OnProgressBarReport?.Invoke(23, "Đang đọc dữ liệu, xin đợi giây lát...");
            yield return wait;
            Loader.LoadFaction(ResourceLoader.LoadXMLFromBundle(this.GameConfigAssetBundle, Consts.XML_FACTION_FILE));
            
            this.OnProgressBarReport?.Invoke(24, "Đang đọc dữ liệu, xin đợi giây lát...");
            yield return wait;
            Loader.LoadWeaponEnhanceEffectConfig(ResourceLoader.LoadXMLFromBundle(this.GameConfigAssetBundle, Consts.XML_WEAPONENHANCEEFFECT_FILE));

            this.OnProgressBarReport?.Invoke(25, "Đang đọc dữ liệu, xin đợi giây lát...");
            yield return wait;
            Loader.LoadPropertyDictionary(ResourceLoader.LoadXMLFromBundle(this.GameConfigAssetBundle, Consts.XML_PROPERTYDICTIONARY_FILE));

            this.OnProgressBarReport?.Invoke(30, "Đang đọc dữ liệu, xin đợi giây lát...");
            yield return wait;
            Loader.LoadSkillAttribute(ResourceLoader.LoadXMLFromBundle(this.GameConfigAssetBundle, Consts.XML_SKILLATTRIBUTE_FILE));
            
            this.OnProgressBarReport?.Invoke(31, "Đang đọc dữ liệu, xin đợi giây lát...");
            yield return wait;
            Loader.LoadEnchantSkill(ResourceLoader.LoadXMLFromBundle(this.GameConfigAssetBundle, Consts.XML_ENCHANTSKILL_FILE));

            this.OnProgressBarReport?.Invoke(32, "Đang đọc dữ liệu, xin đợi giây lát...");
            yield return wait;
            Loader.LoadAutoSkill(ResourceLoader.LoadXMLFromBundle(this.GameConfigAssetBundle, Consts.XML_AUTOSKILL_FILE));

			this.OnProgressBarReport?.Invoke(33, "Đang đọc dữ liệu, xin đợi giây lát...");
			yield return wait;
			Loader.LoadSkillData(ResourceLoader.LoadXMLFromBundle(this.GameConfigAssetBundle, Consts.XML_SKILLDATA_FILE));

			this.OnProgressBarReport?.Invoke(35, "Đang đọc dữ liệu, xin đợi giây lát...");
            yield return wait;
            Loader.LoadElement(ResourceLoader.LoadXMLFromBundle(this.GameConfigAssetBundle, Consts.XML_ELEMENT_FILE));

            this.OnProgressBarReport?.Invoke(36, "Đang đọc dữ liệu, xin đợi giây lát...");
            yield return wait;
            Loader.LoadActivities(ResourceLoader.LoadXMLFromBundle(this.GameConfigAssetBundle, Consts.XML_ACTIIVTY_LIST));

            this.OnProgressBarReport?.Invoke(37, "Đang đọc dữ liệu, xin đợi giây lát...");
            yield return wait;
            Loader.LoadBulletActionSetSound(ResourceLoader.LoadBytesFromBundle(this.GameConfigAssetBundle, Consts.XML_BULLETACTIONSETSOUND));

            this.OnProgressBarReport?.Invoke(38, "Đang đọc dữ liệu, xin đợi giây lát...");
            yield return wait;
            Loader.LoadCharacterActionSetXML(ResourceLoader.LoadXMLFromBundle(this.GameConfigAssetBundle, Consts.XML_ACTIONSET_CONFIG));

            this.OnProgressBarReport?.Invoke(39, "Đang đọc dữ liệu, xin đợi giây lát...");
            yield return wait;
            Loader.LoadMantleTitles(ResourceLoader.LoadXMLFromBundle(this.GameConfigAssetBundle, Consts.XML_MANTLE_TITLE));
            Loader.LoadOfficeTitles(ResourceLoader.LoadXMLFromBundle(this.GameConfigAssetBundle, Consts.XML_OFFICE_TITLE));
            Loader.LoadRoleTitles(ResourceLoader.LoadXMLFromBundle(this.GameConfigAssetBundle, Consts.XML_ROLE_TITLE));

            this.OnProgressBarReport?.Invoke(40, "Đang đọc dữ liệu, xin đợi giây lát...");
            yield return wait;
            Loader.LoadSeashellCircle(ResourceLoader.LoadXMLFromBundle(this.GameConfigAssetBundle, Consts.XML_ACTIIVTY_SEASHELLCIRCLE));

            this.OnProgressBarReport?.Invoke(41, "Đang đọc dữ liệu, xin đợi giây lát...");
            yield return wait;
            Loader.LoadBulletConfig(ResourceLoader.LoadXMLFromBundle(this.GameConfigAssetBundle, Consts.XML_BULLETCONFIG));

            this.OnProgressBarReport?.Invoke(42, "Đang đọc dữ liệu, xin đợi giây lát...");
            yield return wait;
            Loader.LoadBulletActionSetXML(ResourceLoader.LoadXMLFromBundle(this.GameConfigAssetBundle, Consts.XML_BULLETACTIONSET_CONFIG));

            yield return wait;
            yield return this.LoadSkillActionSetSound();

            this.OnProgressBarReport?.Invoke(45, "Đang đọc dữ liệu, xin đợi giây lát...");
            yield return wait;
            Loader.LoadCharacterActionSetLayerSort(ResourceLoader.LoadXMLFromBundle(this.GameConfigAssetBundle, Consts.XML_ACTIONSET_LAYERSORT));

            this.OnProgressBarReport?.Invoke(50, "Đang đọc dữ liệu, xin đợi giây lát...");
            yield return wait;
            Loader.LoadItems(this.GameConfigAssetBundle);

            this.OnProgressBarReport?.Invoke(90, "Đang đọc dữ liệu, xin đợi giây lát...");
            yield return wait;
            Loader.LoadItemValue(ResourceLoader.LoadXMLFromBundle(this.GameConfigAssetBundle, Consts.ItemValueCalculation));
            Loader.LoadSignetExp(ResourceLoader.LoadXMLFromBundle(this.GameConfigAssetBundle, Consts.SignetExp));
            Loader.LoadEquipRefine(ResourceLoader.LoadXMLFromBundle(this.GameConfigAssetBundle, Consts.EquipRefineRecipe));

            this.OnProgressBarReport?.Invoke(94, "Đang đọc dữ liệu, xin đợi giây lát...");
            yield return wait;
            Loader.LoadTasks(ResourceLoader.LoadXMLFromBundle(this.GameConfigAssetBundle, Consts.XML_SYSTEMTASK));

            this.OnProgressBarReport?.Invoke(95, "Đang đọc dữ liệu, xin đợi giây lát...");
            yield return wait;
            byte[] actionConfigBytes = ResourceLoader.LoadBytesFromBundle(this.GameConfigAssetBundle, Consts.XML_ACTIONCONFIG);
            Loader.LoadActionConfig(actionConfigBytes);


            this.OnProgressBarReport?.Invoke(96, "Đang đọc dữ liệu, xin đợi giây lát...");
            yield return wait;
            byte[] characterActionSetSoundBytes = ResourceLoader.LoadBytesFromBundle(this.GameConfigAssetBundle, Consts.XML_CHARACTERACTIONSETSOUND);
            Loader.LoadCharacterActionSetSound(characterActionSetSoundBytes);


            this.OnProgressBarReport?.Invoke(97, "Đang đọc dữ liệu, xin đợi giây lát...");
            yield return wait;
            byte[] monsterActionSetSoundBytes = ResourceLoader.LoadBytesFromBundle(this.GameConfigAssetBundle, Consts.XML_MONSTERACTIONSETSOUND);
            Loader.LoadMonsterActionSetSound(monsterActionSetSoundBytes);


            this.OnProgressBarReport?.Invoke(98, "Đang đọc dữ liệu, xin đợi giây lát...");
            yield return wait;
            Loader.LoadLifeSkills(ResourceLoader.LoadXMLFromBundle(this.GameConfigAssetBundle, Consts.XML_LIFESKILL));


            this.OnProgressBarReport?.Invoke(99, "Đang đọc dữ liệu, xin đợi giây lát...");
            yield return wait;
            Loader.LoadReputes(ResourceLoader.LoadXMLFromBundle(this.GameConfigAssetBundle, Consts.XML_REPUTE));


            /// Tải lại các AssetBundle cần thiết
            yield return KTResourceManager.Instance.LoadAssetBundleAsync("Resources/Item/MapSprite.unity3d", false, KTResourceManager.KTResourceCacheType.CachedPermenently);
            yield return KTResourceManager.Instance.LoadAssetWithSubAssetsAsync<Sprite>("Resources/Item/MapSprite.unity3d", "MapSprite", true, null, KTResourceManager.KTResourceCacheType.CachedPermenently);

            /// Mặc định tải xuống âm thanh động tác nhân vật
            yield return KTResourceManager.Instance.LoadAssetBundleAsync(Loader.CharacterActionSetSoundBundleDir, false, KTResourceManager.KTResourceCacheType.CachedPermenently);
            /// Mặc định tải xuống âm thanh động tác quái
            yield return KTResourceManager.Instance.LoadAssetBundleAsync(Loader.MonsterActionSetSoundBundleDir, false, KTResourceManager.KTResourceCacheType.CachedPermenently);
            /// Mặc định tải xuống âm thanh động tác kỹ năng
            yield return KTResourceManager.Instance.LoadAssetBundleAsync(Loader.SkillCastSoundBundleDir, false, KTResourceManager.KTResourceCacheType.CachedPermenently);
            /// Mặc định tải xuống âm thanh đạn
            yield return KTResourceManager.Instance.LoadAssetBundleAsync(Loader.BulletActionSetXML.SoundBundleDir, false, KTResourceManager.KTResourceCacheType.CachedPermenently);


#if UNITY_EDITOR
            this.DoEditorLoad();
#endif
            
            yield return this.LoadInitResource();
            this.OnLoadFinish?.Invoke();

            this.OnProgressBarReport?.Invoke(100, "Tải xuống dữ liệu hoàn tất...");

            /// Xóa đối tượng
            GameObject.Destroy(this.gameObject);

            /// Xóa AssetBundle
            this.GameConfigAssetBundle.Unload(true);
            GameObject.Destroy(this.GameConfigAssetBundle);
            #endregion

            /// Gọi GC dọn rác
            GC.Collect();
            //--------------------------------------------------------------------------------------------------
        }

#if UNITY_EDITOR
        /// <summary>
        /// Đọc File Text từ Windows
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string ReadTextFileFromWindows(string fileName)
        {
            fileName = Application.streamingAssetsPath + "/" + "DataEditorTest" + "/" + fileName;
            if (!System.IO.File.Exists(fileName))
            {
                return null;
            }
            return System.IO.File.ReadAllText(fileName);
        }

        /// <summary>
        /// Thực hiện tải tài nguyên ở Editor
        /// </summary>
        private void DoEditorLoad()
        {
            string taskFileContent = this.ReadTextFileFromWindows("SystemTasks.xml");
            if (!string.IsNullOrEmpty(taskFileContent))
            {
                Loader.LoadTasks(XElement.Parse(taskFileContent));
            }

            string autoPathContent = this.ReadTextFileFromWindows("AutoPath.xml");
            if (!string.IsNullOrEmpty(autoPathContent))
            {
                Loader.LoadAutoPaths(XElement.Parse(autoPathContent));
            }
        }
#endif

        /// <summary>
        /// Tải xuống tài nguyên âm thanh kỹ năng
        /// </summary>
        /// <returns></returns>
        private IEnumerator LoadSkillActionSetSound()
        {
            yield return KTResourceManager.Instance.LoadAssetBundleAsync(Consts.SKILLCASTSOUND_FILE, false, KTResourceManager.KTResourceCacheType.CachedPermenently);
        }

        /// <summary>
        /// Tải xuống tài nguyên mặc định
        /// </summary>
        private IEnumerator LoadInitResource()
        {
            #region Elemental
            {
                string url = Global.WebPath(string.Format("Data/{0}/{1}", Consts.UI_DIR, "Elemental.unity3d"));
                UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(url);
                yield return www.SendWebRequest();

                if (string.IsNullOrEmpty(www.error))
                {
                    AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
                    Loader.Elements[FSPlay.KiemVu.Entities.Enum.Elemental.METAL] = new ElementData()
                    {
                        ElementType = FSPlay.KiemVu.Entities.Enum.Elemental.METAL,
                        Name = Loader.ElementXML.Metal.Name,
                    };
                    Loader.Elements[FSPlay.KiemVu.Entities.Enum.Elemental.WOOD] = new ElementData()
                    {
                        ElementType = FSPlay.KiemVu.Entities.Enum.Elemental.WOOD,
                        Name = Loader.ElementXML.Wood.Name,
                    };
                    Loader.Elements[FSPlay.KiemVu.Entities.Enum.Elemental.EARTH] = new ElementData()
                    {
                        ElementType = FSPlay.KiemVu.Entities.Enum.Elemental.EARTH,
                        Name = Loader.ElementXML.Earth.Name,
                    };
                    Loader.Elements[FSPlay.KiemVu.Entities.Enum.Elemental.WATER] = new ElementData()
                    {
                        ElementType = FSPlay.KiemVu.Entities.Enum.Elemental.WATER,
                        Name = Loader.ElementXML.Water.Name,
                    };
                    Loader.Elements[FSPlay.KiemVu.Entities.Enum.Elemental.FIRE] = new ElementData()
                    {
                        ElementType = FSPlay.KiemVu.Entities.Enum.Elemental.FIRE,
                        Name = Loader.ElementXML.Fire.Name,
                    };

                    AssetBundleRequest operation = bundle.LoadAssetWithSubAssetsAsync<UnityEngine.Sprite>(Loader.ElementXML.AtlasName);
                    for (int i = 0; i < operation.allAssets.Length; i++)
                    {
                        UnityEngine.Sprite sprite = operation.allAssets[i] as UnityEngine.Sprite;

                        if (sprite.name.Equals(Loader.ElementXML.Metal.SmallImage))
                        {
                            Loader.Elements[FSPlay.KiemVu.Entities.Enum.Elemental.METAL].SmallSprite = sprite;
                        }
                        else if (sprite.name.Equals(Loader.ElementXML.Metal.NormalImage))
                        {
                            Loader.Elements[FSPlay.KiemVu.Entities.Enum.Elemental.METAL].NormalSprite = sprite;
                        }
                        else if (sprite.name.Equals(Loader.ElementXML.Metal.BigImage))
                        {
                            Loader.Elements[FSPlay.KiemVu.Entities.Enum.Elemental.METAL].BigSprite = sprite;
                        }
                        else if (sprite.name.Equals(Loader.ElementXML.Wood.SmallImage))
                        {
                            Loader.Elements[FSPlay.KiemVu.Entities.Enum.Elemental.WOOD].SmallSprite = sprite;
                        }
                        else if (sprite.name.Equals(Loader.ElementXML.Wood.NormalImage))
                        {
                            Loader.Elements[FSPlay.KiemVu.Entities.Enum.Elemental.WOOD].NormalSprite = sprite;
                        }
                        else if (sprite.name.Equals(Loader.ElementXML.Wood.BigImage))
                        {
                            Loader.Elements[FSPlay.KiemVu.Entities.Enum.Elemental.WOOD].BigSprite = sprite;
                        }
                        else if (sprite.name.Equals(Loader.ElementXML.Earth.SmallImage))
                        {
                            Loader.Elements[FSPlay.KiemVu.Entities.Enum.Elemental.EARTH].SmallSprite = sprite;
                        }
                        else if (sprite.name.Equals(Loader.ElementXML.Earth.NormalImage))
                        {
                            Loader.Elements[FSPlay.KiemVu.Entities.Enum.Elemental.EARTH].NormalSprite = sprite;
                        }
                        else if (sprite.name.Equals(Loader.ElementXML.Earth.BigImage))
                        {
                            Loader.Elements[FSPlay.KiemVu.Entities.Enum.Elemental.EARTH].BigSprite = sprite;
                        }
                        else if (sprite.name.Equals(Loader.ElementXML.Water.SmallImage))
                        {
                            Loader.Elements[FSPlay.KiemVu.Entities.Enum.Elemental.WATER].SmallSprite = sprite;
                        }
                        else if (sprite.name.Equals(Loader.ElementXML.Water.NormalImage))
                        {
                            Loader.Elements[FSPlay.KiemVu.Entities.Enum.Elemental.WATER].NormalSprite = sprite;
                        }
                        else if (sprite.name.Equals(Loader.ElementXML.Water.BigImage))
                        {
                            Loader.Elements[FSPlay.KiemVu.Entities.Enum.Elemental.WATER].BigSprite = sprite;
                        }
                        else if (sprite.name.Equals(Loader.ElementXML.Fire.SmallImage))
                        {
                            Loader.Elements[FSPlay.KiemVu.Entities.Enum.Elemental.FIRE].SmallSprite = sprite;
                        }
                        else if (sprite.name.Equals(Loader.ElementXML.Fire.NormalImage))
                        {
                            Loader.Elements[FSPlay.KiemVu.Entities.Enum.Elemental.FIRE].NormalSprite = sprite;
                        }
                        else if (sprite.name.Equals(Loader.ElementXML.Fire.BigImage))
                        {
                            Loader.Elements[FSPlay.KiemVu.Entities.Enum.Elemental.FIRE].BigSprite = sprite;
                        }
                    }

                    bundle.Unload(false);
                }

                www.Dispose();
            }
            #endregion
        }
        #endregion
    }
}