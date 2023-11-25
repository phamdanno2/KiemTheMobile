using GameServer.Logic;

namespace GameServer.KiemThe.Logic
{
	/// <summary>
	/// Quản lý Logic của kỹ năng
	/// </summary>
	public static partial class KTSkillManager
    {
        /// <summary>
        /// Thêm kết quả kỹ năng vào danh sách Packet gửi đi
        /// </summary>
        /// <param name="caster"></param>
        /// <param name="target"></param>
        /// <param name="type"></param>
        /// <param name="damage"></param>
        public static void AppendSkillResult(GameObject caster, GameObject target, SkillResult type, int damage)
        {
            /// Nếu toác
            if (caster == null || target == null)
			{
                return;
			}

            if (caster is KPlayer)
			{
                //KT_TCPHandler.SendSkillResultToMySelf(caster as KPlayer, caster, target, type, damage);
                /// Thêm sát thương vào
                (caster as KPlayer).AddDamageDealt(target, type, damage);
            }
            if (target is KPlayer)
			{
                //KT_TCPHandler.SendSkillResultToMySelf(target as KPlayer, caster, target, type, damage);
                /// Thêm sát thương vào
                (target as KPlayer).AddReceiveDamage(caster, type, damage);
            }
        }
    }
}
