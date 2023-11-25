using GameServer.Logic;
using Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.KiemThe.Network.Entities
{
    /// <summary>
    /// Đối tượng xây gói tin tạo đạn gửi về Client
    /// </summary>
    public class BulletPacketBuilder
    {
        /// <summary>
        /// Danh sách các gói tin tương ứng được gửi đi
        /// </summary>
        private readonly List<G2C_CreateBullet> packets = new List<G2C_CreateBullet>();

        /// <summary>
        /// Thêm dữ liệu đạn vào danh sách
        /// </summary>
        /// <param name="bulletID">ID đạn</param>
        /// <param name="resID">ID Res của đạn</param>
        /// <param name="fromPos">Vị trí xuất phát</param>
        /// <param name="toPos">Vị trí đích</param>
        /// <param name="chaseTarget">Mục tiêu đuổi theo</param>
        /// <param name="isTrap">Có phải bẫy không</param>
        /// <param name="velocity">Vận tốc đạn bay</param>
        /// <param name="lifeTime">Thời gian tồn tại</param>
        /// <param name="loopAnimation">Lặp lại hiệu ứng liên tục đến hết thời gian tồn tại</param>
        /// <param name="delay">Thời gian delay trước khi ra đạn</param>
        /// <param name="casterFollowBullet">Đối tượng xuất chiêu có thực hiện bay theo đạn không</param>
        public void Append(int bulletID, int resID, UnityEngine.Vector2 fromPos, UnityEngine.Vector2 toPos, GameObject chaseTarget, int velocity, float lifeTime, bool loopAnimation, float delay, GameObject caster)
        {
            G2C_CreateBullet createBullet = new G2C_CreateBullet()
            {
                BulletID = bulletID,
                ResID = resID,
                FromX = (int) fromPos.x,
                FromY = (int) fromPos.y,
                ToX = (int) toPos.x,
                ToY = (int) toPos.y,
                TargetID = chaseTarget == null ? -1 : chaseTarget.RoleID,
                Velocity = velocity,
                LifeTime = lifeTime,
                LoopAnimation = loopAnimation,
                Delay = delay,
                CasterID = caster.RoleID,
            };
            this.packets.Add(createBullet);
        }

        /// <summary>
        /// Xây danh sách gói tin
        /// </summary>
        /// <returns></returns>
        public List<G2C_CreateBullet> Build()
        {
            return this.packets;
        }
    }
}
