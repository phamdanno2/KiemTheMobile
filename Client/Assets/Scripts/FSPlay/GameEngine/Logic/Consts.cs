﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSPlay.GameEngine.Logic
{
    /// <summary>
    /// Hằng số thường dùng
    /// </summary>
	public class Consts
    {
        #region Danh sách các file cấu hình Bundle
        /// <summary>
        /// File cấu hình game
        /// </summary>
        public const string GAME_CONFIG_FILE = "Config.unity3d";
        public const string GAME_CONFIG_NAME = "Config";

        /// <summary>
        /// File cấu hình Script Lua
        /// </summary>
        public const string LUASCRIPT_FILE = "Extension.unity3d";
        public const string LUASCRIPT_NAME = "Extension";

        /// <summary>
        /// File âm thanh
        /// </summary>
        public const string SKILLCASTSOUND_FILE = "Resources/Sound/SkillCastSound.unity3d";
        #endregion

        #region Danh sách các file XML trong Bundle

        /// <summary>
        /// File XML chứa danh sách Script Lua
        /// </summary>
        public const string XML_SCRIPTINDEX_FILE = "Assets/Extension/ScriptIndex.xml";

        /// <summary>
        /// File XML chứa thông tin động tác quái vật
        /// </summary>
        public const string XML_MONSTERACTIONSET_FILE = "Assets/Config/MonsterActionSet.xml";

        /// <summary>
        /// File XML chứa thông tin avarta nhân vật
        /// </summary>
        public const string XML_ROLEAVARTA_FILE = "Assets/Config/RoleAvarta.xml";

        /// <summary>
        /// File XML chứa thông tin hiệu ứng
        /// </summary>
        public const string XML_EFFECT_FILE = "Assets/Config/StateEffect.xml";

        /// <summary>
        /// File XML chứa thông tin PropertyDictionary
        /// </summary>
        public const string XML_PROPERTYDICTIONARY_FILE = "Assets/Config/PropertyDictionary.xml";

        /// <summary>
        /// File XML chứa thông tin kỹ năng
        /// </summary>
        public const string XML_SKILLDATA_FILE = "Assets/Config/SkillData.xml";

        /// <summary>
        /// File XML chứa thuộc tính kỹ năng
        /// </summary>
        public const string XML_SKILLATTRIBUTE_FILE = "Assets/Config/SkillPropertiesLua.xml";

        /// <summary>
        /// File XML chứa thông tin kỹ năng bổ trợ
        /// </summary>
        public const string XML_ENCHANTSKILL_FILE = "Assets/Config/EnchantSkill.xml";

        /// <summary>
        /// File XML chứa thông tin kỹ năng tự kích hoạt
        /// </summary>
        public const string XML_AUTOSKILL_FILE = "Assets/Config/AutoSkill.xml";

        /// <summary>
        /// File XML chứa thông tin ngũ hành
        /// </summary>
        public const string XML_ELEMENT_FILE = "Assets/Config/Element.xml";

        /// <summary>
        /// File XML chứa thông tin môn phái
        /// </summary>
        public const string XML_FACTION_FILE = "Assets/Config/Faction.xml";

        /// <summary>
        /// File XML chứa thông tin hiệu ứng cường hóa vũ khí
        /// </summary>
        public const string XML_WEAPONENHANCEEFFECT_FILE = "Assets/Config/WeaponEnhanceConfig.xml";

        #region Action Set
        /// <summary>
        /// Config động tác nhân vật
        /// </summary>
        public const string XML_ACTIONSET_CONFIG = "Assets/Config/CharacterActionSet.xml";

        /// <summary>
        /// Ngựa
        /// </summary>
        public const string XML_ACTIONSET_EFFECT = "Assets/Config/EffectActionSet/Effect.xml";

        /// <summary>
        /// Thứ tự sắp xếp động tác
        /// </summary>
        public const string XML_ACTIONSET_LAYERSORT = "Assets/Config/CharacterActionSet/CharacterActionSetLayerSort.xml";

        /// <summary>
        /// Config động tác theo tên vũ khí
        /// </summary>
        public const string XML_ACTIONCONFIG = "Assets/Config/CharacterActionSet/ActionConfig.xml";

        /// <summary>
        /// Config động tác quái
        /// </summary>
        public const string XML_ACTIONSET_NPC = "Assets/Config/MonsterActionSet/Npc.xml";

        /// <summary>
        /// Config âm thanh theo động tác
        /// </summary>
        public const string XML_CHARACTERACTIONSETSOUND = "Assets/Config/CharacterActionSet/CharacterActionSetSound.xml";

        /// <summary>
        /// Config âm thanh theo động tác
        /// </summary>
        public const string XML_MONSTERACTIONSETSOUND = "Assets/Config/MonsterActionSet/MonsterActionSetSound.xml";
        #endregion

        #region Bullet Action Set
        /// <summary>
        /// Cấu hình Logic đạn
        /// </summary>
        public const string XML_BULLETCONFIG = "Assets/Config/BulletConfig.xml";

        /// <summary>
        /// Config động tác đạn và các thông tin khác
        /// </summary>
        public const string XML_BULLETACTIONSET_CONFIG = "Assets/Config/BulletActionSet.xml";

        /// <summary>
        /// Config hiệu ứng đạn
        /// </summary>
        public const string XML_BULLETACTIONSET = "Assets/Config/SkillActionSet/BulletActionSet.xml";

        /// <summary>
        /// Config âm thanh đạn
        /// </summary>
        public const string XML_BULLETACTIONSETSOUND = "Assets/Config/SkillActionSet/BulletActionSetSound.xml";
        #endregion

        #region Items
        /// <summary>
        /// Config phù
        /// </summary>
        public const string BasicItem_amulet = "Assets/Config/Items/BasicItem/amulet.xml";
        /// <summary>
        /// Config áo
        /// </summary>
        public const string BasicItem_armor = "Assets/Config/Items/BasicItem/armor.xml";
        /// <summary>
        /// Config đai
        /// </summary>
        public const string BasicItem_belt = "Assets/Config/Items/BasicItem/belt.xml";
        /// <summary>
        /// Config mật tịch
        /// </summary>
        public const string BasicItem_book = "Assets/Config/Items/BasicItem/book.xml";
        /// <summary>
        /// Config giày
        /// </summary>
        public const string BasicItem_boots = "Assets/Config/Items/BasicItem/boots.xml";
        /// <summary>
        /// Config quan ấn
        /// </summary>
        public const string BasicItem_chop = "Assets/Config/Items/BasicItem/chop.xml";
        /// <summary>
        /// Config hộ uyển
        /// </summary>
        public const string BasicItem_cuff = "Assets/Config/Items/BasicItem/cuff.xml";
        /// <summary>
        /// Config mũ
        /// </summary>
        public const string BasicItem_helm = "Assets/Config/Items/BasicItem/helm.xml";
        /// <summary>
        /// Config ngựa
        /// </summary>
        public const string BasicItem_horse = "Assets/Config/Items/BasicItem/horse.xml";
        /// <summary>
        /// Config phi phong
        /// </summary>
        public const string BasicItem_mantle = "Assets/Config/Items/BasicItem/mantle.xml";
        /// <summary>
        /// Config vũ khí cận chiến
        /// </summary>
        public const string BasicItem_meleeweapon = "Assets/Config/Items/BasicItem/meleeweapon.xml";
        /// <summary>
        /// Config liên
        /// </summary>
        public const string BasicItem_necklace = "Assets/Config/Items/BasicItem/necklace.xml";
        /// <summary>
        /// Config bội
        /// </summary>
        public const string BasicItem_pendant = "Assets/Config/Items/BasicItem/pendant.xml";
        /// <summary>
        /// Config vũ khí tầm xa
        /// </summary>
        public const string BasicItem_rangeweapon = "Assets/Config/Items/BasicItem/rangeweapon.xml";
        /// <summary>
        /// Config nhẫn
        /// </summary>
        public const string BasicItem_ring = "Assets/Config/Items/BasicItem/ring.xml";
        /// <summary>
        /// Config ngũ hành ấn
        /// </summary>
        public const string BasicItem_signet = "Assets/Config/Items/BasicItem/signet.xml";
        /// <summary>
        /// Config trận pháp
        /// </summary>
        public const string BasicItem_zhen = "Assets/Config/Items/BasicItem/zhen.xml";


        /// <summary>
        /// Config chế liên
        /// </summary>
        public const string Crafting_amulet = "Assets/Config/Items/Crafting/amulet.xml";
        /// <summary>
        /// Config chế y phục
        /// </summary>
        public const string Crafting_armor = "Assets/Config/Items/Crafting/armor.xml";
        /// <summary>
        /// Config chế đai
        /// </summary>
        public const string Crafting_belt = "Assets/Config/Items/Crafting/belt.xml";
        /// <summary>
        /// Config chế giày
        /// </summary>
        public const string Crafting_boots = "Assets/Config/Items/Crafting/boots.xml";
        /// <summary>
        /// Config chế hộ uyển
        /// </summary>
        public const string Crafting_cuff = "Assets/Config/Items/Crafting/cuff.xml";
        /// <summary>
        /// Config chế mũ
        /// </summary>
        public const string Crafting_helm = "Assets/Config/Items/Crafting/helm.xml";
        /// <summary>
        /// Config chế vũ khí cận chiến
        /// </summary>
        public const string Crafting_meleeweapon = "Assets/Config/Items/Crafting/meleeweapon.xml";
        /// <summary>
        /// Config chế liên
        /// </summary>
        public const string Crafting_necklace = "Assets/Config/Items/Crafting/necklace.xml";
        /// <summary>
        /// Config chế bội
        /// </summary>
        public const string Crafting_pendant = "Assets/Config/Items/Crafting/pendant.xml";
        /// <summary>
        /// Config chế vũ khí tầm xa
        /// </summary>
        public const string Crafting_rangeweapon = "Assets/Config/Items/Crafting/rangeweapon.xml";
        /// <summary>
        /// Config chế nhẫn
        /// </summary>
        public const string Crafting_ring = "Assets/Config/Items/Crafting/ring.xml";

        /// <summary>
        /// Config bộ xanh lá
        /// </summary>
        public const string SetItem_greenequip = "Assets/Config/Items/SetItem/greenequip.xml";

        /// <summary>
        /// Config dược phẩm
        /// </summary>
        public const string Other_medicine = "Assets/Config/Items/Other/medicine.xml";
        /// <summary>
        /// Config vật phẩm có Script LUA
        /// </summary>
        public const string Other_scriptitem = "Assets/Config/Items/Other/scriptitem.xml";
        /// <summary>
        /// Config nguyên liệu chế
        /// </summary>
        public const string Other_stuffitem = "Assets/Config/Items/Other/stuffitem.xml";
        /// <summary>
        /// Config vật phẩm nhiệm vụ
        /// </summary>
        public const string Other_taskquest = "Assets/Config/Items/Other/taskquest.xml";

        /// <summary>
        /// Config thuộc tính trang bị
        /// </summary>
        public const string MagicAttribLevel = "Assets/Config/Items/MagicAttribLevel.xml";
        /// <summary>
        /// Config thuộc tính kích hoạt theo bộ
        /// </summary>
        public const string SuiteActiveProp = "Assets/Config/Items/SuiteActiveProp.xml";

        /// <summary>
        /// Config chỉ số vật phẩm
        /// </summary>
        public const string ItemValueCalculation = "Assets/Config/Items/ItemValueCaculation.xml";

        /// <summary>
        /// Config Exp ngũ hành ấn
        /// </summary>
        public const string SignetExp = "Assets/Config/Items/ItemExp/SignnetExp.xml";

        /// <summary>
        /// Config danh sách công thức luyện hóa trang bị
        /// </summary>
        public const string EquipRefineRecipe = "Assets/Config/Items/Refine.xml";
        #endregion

        #region Task
        /// <summary>
        /// File XML nhiệm vụ
        /// </summary>
        public const string XML_SYSTEMTASK = "Assets/Config/SystemTasks.xml";
        #endregion

        #region LifeSkill
        /// <summary>
        /// File XML kỹ năng sống
        /// </summary>
        public const string XML_LIFESKILL = "Assets/Config/Items/LifeSkill.xml";
        #endregion

        #region Repute
        /// <summary>
        /// File XML danh vọng hệ thống
        /// </summary>
        public const string XML_REPUTE = "Assets/Config/repute.xml";
        #endregion

        #region Animated Title
        /// <summary>
        /// Config danh hiệu Phi phong
        /// </summary>
        public const string XML_MANTLE_TITLE = "Assets/Config/AnimatedTitle/MantleTItle.xml";

        /// <summary>
        /// Config danh hiệu Quan hàm
        /// </summary>
        public const string XML_OFFICE_TITLE = "Assets/Config/AnimatedTitle/OfficeTitle.xml";
        #endregion

        #region Danh hiệu
        /// <summary>
        /// Config danh hiệu
        /// </summary>
        public const string XML_ROLE_TITLE = "Assets/Config/Title.xml";
        #endregion

        #region Bách Bảo Rương
        /// <summary>
        /// Config Bách Bảo Rương
        /// </summary>
        public const string XML_ACTIIVTY_SEASHELLCIRCLE = "Assets/Config/Activity/KTSeashellCircle.xml";
        #endregion

        #region Chúc phúc
        /// <summary>
        /// Config Bách Bảo Rương
        /// </summary>
        public const string XML_ACTIIVTY_PLAYERPRAY = "Assets/Config/Activity/PlayerPray.xml";
        #endregion

        #region Hoạt động
        /// <summary>
        /// Config Bách Bảo Rương
        /// </summary>
        public const string XML_ACTIIVTY_LIST = "Assets/Config/Activity/ActivityList.xml";
        #endregion
        #endregion

        #region Resource
        /// <summary>
        /// Đường dẫn folder Resources
        /// </summary>
        public const string RESOURCES_DIR = "Resources";

        /// <summary>
        /// Đường dẫn folder UI
        /// </summary>
        public const string UI_DIR = "Resources/UI";
        #endregion

        #region Danh sách các file cấu hình Game
        /// <summary>
        /// Danh sách các bộ không dùng
        /// </summary>
        public const string XML_MODELFILTER_FILE = "Assets/Config/ModelFilter.xml";

        /// <summary>
        /// File cấu hình quái
        /// </summary>
        public const string XML_MONSTER_FILE = "Assets/Config/Monster.xml";

        /// <summary>
        /// File cấu hình quái
        /// </summary>
        public const string XML_NPC_FILE = "Assets/Config/Npc.xml";

        /// <summary>
        /// File XML chứa thông tin bản đồ
        /// </summary>
        public const string XML_MAP_FILE = "Assets/Config/MapConfig.xml";

        /// <summary>
        /// File XML chứa thông tin bản đồ thế giới
        /// </summary>
        public const string XML_WORLDMAP_FILE = "Assets/Config/WorldMap.xml";

        /// <summary>
        /// File XML chứa thông tin bản đồ liên máy chủ
        /// </summary>
        public const string XML_CROSSSERVERMAP_FILE = "Assets/Config/CrossServerMap.xml";

        /// <summary>
        /// File XML chứa thông tin bản đồ lãnh thổ
        /// </summary>
        public const string XML_COLONYMAP_FILE = "Assets/Config/ColonyMap.xml";

        /// <summary>
        /// File XML chứa thông tin các vị trí dịch chuyển tự động
        /// </summary>
        public const string XML_AUTOPATH_FILE = "Assets/Config/AutoPath.xml";
        #endregion

#if UNITY_EDITOR
        /// <summary>
        /// Key Verify Account
        /// </summary>
        public const string HTTP_MD5_KEY = "tmsk_mu_06";
#elif UNITY_IPHONE && APPS
        /// <summary>
        /// Key Verify Account
        /// </summary>
		public const string HTTP_MD5_KEY = "HWjKO26fEJvZ27f8v0Qu9EGZ3k3phFO4NCt8A"; 
#else
        /// <summary>
        /// Key Verify Account
        /// </summary>
		public const string HTTP_MD5_KEY = "tmsk_mu_06"; 
#endif
    }
}
