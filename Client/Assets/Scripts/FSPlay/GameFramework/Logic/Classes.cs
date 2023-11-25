using System.Collections.Generic;
using Server.Data;

namespace FSPlay.GameFramework.Logic
{
	public static class NameErrorCodes
    {
        public const int ErrorSuccess = 1; //成功,这个值不会直接返回给客户端,而是根据具体逻辑返回特定的值
        public const int ErrorServerDisabled = -2; //服务器禁止创建
        public const int ErrorInvalidCharacter = -3; //名字包含特殊字符
        public const int ErrorNameHasBeUsed = -4; //名字已经被占用
        public const int Error_ZhanMeng_Has_In_ZhanMeng = -1010; //已经在战盟中
        public const int Error_ZhanMeng_Duplicate_Name = -1011; //战盟名字已被占用
    }


    /// <summary>
    /// Dữ liệu máy chủ
    /// </summary>
    public class XuanFuServerData
    {
        /// <summary>
        /// Máy chủ đề cử
        /// </summary>
        public BuffServerInfo RecommendServer { get; set; }
        /// <summary>
        /// Máy chủ lần trước
        /// </summary>
        public BuffServerInfo LastServer { get; set; }
        /// <summary>
        /// Danh sách máy chủ đã vào
        /// </summary>
        public List<BuffServerInfo> RecordServerInfos { get; set; }
        /// <summary>
        /// Danh sách máy chủ đề cử
        /// </summary>
        public List<BuffServerInfo> RecommendServerInfos { get; set; }
        /// <summary>
        /// Danh sách máy chủ
        /// </summary>
        public List<BuffServerInfo> ServerInfos { get; set; }
    }


    /// <summary>
    /// ID cơ bản của đối tượng
    /// 32 bit = 2,147,483,648
    /// 7D2B 0000 = 2,099,970,048
    /// 7F00 0000 = 2,130,706,432
    /// 7F01 0000 = 2,130,771,968
    /// 7F40 0000 = 2,134,900,736
    /// 7F41 0000 = 2,134,966,272
    /// 7F42 0000 = 2,135,031,808
    /// 7F43 0000 = 2,135,097,344
    /// 7F50 0000 = 2,135,949,312
    /// </summary>
    public static class SpriteBaseIds
    {
        /// <summary>
        /// Base của nhân vật
        /// </summary>
        public const int RoleBaseId = 0;
        /// <summary>
        /// Base của NPC
        /// </summary>
        public const int NpcBaseId = 0x7F000000;
        /// <summary>
        /// Base của quái
        /// </summary>
        public const int MonsterBaseId = 0x7F010000;
        /// <summary>
        /// Base của điểm thu thập
        /// </summary>
        public const int GrowPointBaseId = 0x7F400000;
        /// <summary>
        /// Base của điểm di động
        /// </summary>
        public const int DynamicAreaBaseId = 0x7F410000;
        /// <summary>
        /// Base của vật phẩm rơi dưới Map
        /// </summary>
        public const int GoodsPackBaseId = 0x7F420000;
        /// <summary>
        /// Base của Fake Role
        /// </summary>
        public const int FakeRoleBaseId = 0x7F430000;
        /// <summary>
        /// Base của cổng Teleport
        /// </summary>
        public const int TeleportBaseId = 0x7F440000;
        /// <summary>
        /// Base của BOT
        /// </summary>
        public const int BotBaseId = 0x7F450000;
        /// <summary>
        /// Max
        /// </summary>
        public const int MaxId = 0x7F500000;
    }
}
