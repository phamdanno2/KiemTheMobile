using FSPlay.KiemVu.Control.Component;
using FSPlay.KiemVu.Control.Component.Skill;
using FSPlay.KiemVu.Control.Map;
using FSPlay.KiemVu.Utilities.UnityComponent;
using UnityEngine;

namespace FSPlay.KiemVu.Factory
{
    /// <summary>
    /// Đối tượng quản lý các Object của Game
    /// </summary>
    public class Object2DFactory
    {
        #region Cache for logic
        /// <summary>
        /// Tọa độ Fake Role hiện tại
        /// </summary>
        private static float FakeRolePosX = 0;
        #endregion

        /// <summary>
        /// Tạo đối tượng nhân vật chính
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Character MakeLeader()
        {
            GameObject role2D = FSPlay.KiemVu.Loader.ResourceLoader.LoadPrefabFromResources("KiemVu/Prefabs/Object/Leader");
            return role2D.GetComponent<Character>();
        }

        /// <summary>
        /// Tạo đối tượng người chơi khác
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Character MakeOtherRole()
        {
            GameObject role2D = FSPlay.KiemVu.Loader.ResourceLoader.LoadPrefabFromResources("KiemVu/Prefabs/Object/OtherRole");
            return role2D.GetComponent<Character>();
        }

        /// <summary>
        /// Tạo đối tượng tham chiếu nhân vật hoặc người chơi
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static CharacterPreview MakeRolePreview()
        {
            GameObject role2D = FSPlay.KiemVu.Loader.ResourceLoader.LoadPrefabFromResources("KiemVu/Prefabs/Object/RolePreview");
            role2D.transform.localPosition = new Vector2(Object2DFactory.FakeRolePosX, 0);
            Object2DFactory.FakeRolePosX += 200;
            if (Object2DFactory.FakeRolePosX >= 2000000)
            {
                Object2DFactory.FakeRolePosX = 200;
            }
            return role2D.GetComponent<CharacterPreview>();
        }

        /// <summary>
        /// Tạo đối tượng quái vật
        /// </summary>
        /// <returns></returns>
        public static Monster MakeMonster()
        {
            GameObject monster2D = FSPlay.KiemVu.Loader.ResourceLoader.LoadPrefabFromResources("KiemVu/Prefabs/Object/Monster");
            return monster2D.GetComponent<Monster>();
        }

        /// <summary>
        /// Tạo đối tượng NPC
        /// </summary>
        /// <returns></returns>
        public static Monster MakeNPC()
        {
            GameObject monster2D = FSPlay.KiemVu.Loader.ResourceLoader.LoadPrefabFromResources("KiemVu/Prefabs/Object/NPC");
            return monster2D.GetComponent<Monster>();
        }

        /// <summary>
        /// Tạo đối tượng điểm truyền tống
        /// </summary>
        /// <returns></returns>
        public static Teleport MakeTeleport()
        {
            GameObject teleport2D = FSPlay.KiemVu.Loader.ResourceLoader.LoadPrefabFromResources("KiemVu/Prefabs/Object/Teleport");
            return teleport2D.GetComponent<Teleport>();
        }

        /// <summary>
        /// Tạo đối tượng hiệu ứng
        /// </summary>
        /// <returns></returns>
        public static Effect MakeEffect()
        {
            GameObject effect2D = FSPlay.KiemVu.Loader.ResourceLoader.LoadPrefabFromResources("KiemVu/Prefabs/Object/Effect");
            return effect2D.GetComponent<Effect>();
        }

        /// <summary>
        /// Tạo đối tượng đạn bay
        /// </summary>
        /// <returns></returns>
        public static Bullet MakeBullet()
        {
            GameObject bullet2D = FSPlay.KiemVu.Loader.ResourceLoader.LoadPrefabFromResources("KiemVu/Prefabs/Object/Bullet");
            return bullet2D.GetComponent<Bullet>();
        }

        /// <summary>
        /// Tạo đối tượng hiệu ứng đạn nổ
        /// </summary>
        /// <returns></returns>
        public static ExplodeEffect MakeExplodeEffect()
        {
            GameObject explodeEffect2D = FSPlay.KiemVu.Loader.ResourceLoader.LoadPrefabFromResources("KiemVu/Prefabs/Object/ExplodeEffect");
            return explodeEffect2D.GetComponent<ExplodeEffect>();
        }

        /// <summary>
        /// Tạo mới đối tượng Audio Player
        /// </summary>
        /// <returns></returns>
        public static AudioPlayer MakeStandaloneAudioPlayer()
        {
            GameObject player = FSPlay.KiemVu.Loader.ResourceLoader.LoadPrefabFromResources("KiemVu/Prefabs/Object/StandaloneAudioPlayer");
            return player.GetComponent<AudioPlayer>();
        }

        /// <summary>
        /// Tạo mới đối tượng ký hiệu chọn mục tiêu
        /// </summary>
        /// <returns></returns>
        public static SelectTargetDeco MakeSelectTargetDeco()
        {
            GameObject selectTargetDeco = FSPlay.KiemVu.Loader.ResourceLoader.LoadPrefabFromResources("KiemVu/Prefabs/Object/SelectTargetDeco");
            return selectTargetDeco.GetComponent<SelectTargetDeco>();
        }

        /// <summary>
        /// Tạo đối tượng Debug Object
        /// </summary>
        /// <returns></returns>
        public static DebugObject MakeDebugObject()
        {
            GameObject debugObject2D = FSPlay.KiemVu.Loader.ResourceLoader.LoadPrefabFromResources("KiemVu/Prefabs/Object/DebugObject");
            return debugObject2D.GetComponent<DebugObject>();
        }
    }
}
