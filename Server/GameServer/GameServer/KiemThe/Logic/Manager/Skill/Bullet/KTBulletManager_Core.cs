using GameServer.KiemThe.Entities;
using GameServer.KiemThe.Utilities;
using GameServer.Logic;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.KiemThe.Logic
{
    /// <summary>
    /// Quản lý đạn bay
    /// </summary>
    public partial class KTBulletManager
    {
		/// <summary>
		/// Trả về danh sách các mục tiêu bị đạn chạm phải tại vị trí chỉ định
		/// </summary>
		/// <param name="bullet">Đạn</param>
		/// <param name="pos">Vị trí chỉ định</param>
		/// <param name="explodeRadius">Phạm vi nổ</param>
		/// <returns></returns>
		private List<GameObject> GetExplosionTargets(Bullet bullet, UnityEngine.Vector2 pos, int explodeRadius)
        {
            List<GameObject> list = new List<GameObject>();

            if (bullet == null)
            {
                return list;
            }

            try
            {
                /// Trả về danh sách các kẻ địch xung quanh
                List<GameObject> objects = null;

                /// Nếu mục tiêu là kẻ địch
                if (bullet.TargetType == "enemy")
                {
                    objects = KTLogic.GetEnemiesAroundPos(bullet.Caster, pos, explodeRadius);
                }
                /// Nếu mục tiêu là đồng đội
                else if (bullet.TargetType == "team" || bullet.TargetType == "teamnoself")
                {
                    if (bullet.Caster is KPlayer)
                    {
                        KPlayer player = bullet.Caster as KPlayer;
                        /// Nếu có nhóm
                        if (player.TeamID != -1 && KTTeamManager.IsTeamExist(player.TeamID))
                        {
                            List<KPlayer> teammates = KTLogic.GetNearByTeammates(player, explodeRadius, bullet.MaxTargetTouch <= 0 ? -1 : bullet.MaxTargetTouch);
                            if (teammates.Count > 0)
                            {
                                objects = new List<GameObject>();
                                foreach (KPlayer teammate in teammates)
                                {
                                    if (bullet.TargetType == "teamnoself" && teammate != player)
                                    {
                                        objects.Add(teammate);
                                    }
                                }
                            }
                        }
                        /// Nếu không có nhóm
                        else if (bullet.TargetType == "team")
                        {
                            objects = new List<GameObject>();
                            objects.Add(player);
                        }
                    }
                    else
                    {
                        /// Danh sách đối tượng xung quanh cùng CampID
                        objects = KTLogic.GetNearBySameCampObject<GameObject>(bullet.Caster, explodeRadius);
                    }
                }

                /// Nếu không tìm thấy thì thoát luôn
                if (objects == null)
                {
                    return list;
                }

                /// Duyệt toàn bộ danh sách, lọc ra tất cả các đối tượng thỏa mãn
                foreach (GameObject go in objects)
                {
                    /// Nếu là chủ nhân của đạn thì bỏ qua
                    if (go == bullet.Caster)
                    {
                        continue;
                    }
                    /// Nếu đã chạm vào đối tượng thì bỏ qua
                    else if (bullet.TouchTargets.Contains(go))
                    {
                        continue;
                    }

                    ///// Nếu đối tượng đang trong trạng thái khinh công thì bỏ qua
                    //if (go.m_eDoing == KE_NPC_DOING.do_jump)
                    //{
                    //    continue;
                    //}

                    /// Lấy vị trí hiện tại của đối tượng
                    UnityEngine.Vector2 objPos = new UnityEngine.Vector2((float) go.CurrentPos.X, (float) go.CurrentPos.Y);

                    /// Nếu đối tượng nằm trong đường tròn xung quanh vị trí nổ
                    if (KTMath.IsPointInsideCircle(objPos, pos, explodeRadius))
                    {
                        /// Thêm đối tượng vào danh sách
                        list.Add(go);

                        /// Đối tượng đã được thêm vào danh sách, không cần xuống dưới kiểm tra các điều kiện còn lại nữa
                        continue;
                    }
                }

                /// Sắp xếp danh sách tăng dần theo khoảng cách
                list = list.OrderBy(x => UnityEngine.Vector2.Distance(new UnityEngine.Vector2((float) x.CurrentPos.X, (float) x.CurrentPos.Y), pos)).ToList();

                return list;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Skill, ex.ToString());
                return list;
            }
        }

        /// <summary>
        /// Trả về danh sách các mục tiêu bị chạm phải trên đường bay của đạn
        /// </summary>
        /// <param name="bullet">Đạn</param>
        /// <param name="fromPos">Vị trí bắt đầu</param>
        /// <param name="toPos">Vị trí đích</param>
        /// <param name="explodeRadius">Phạm vi nổ</param>
        /// <returns></returns>
        private List<BulletExplode> GetExplosionTargets(Bullet bullet, UnityEngine.Vector2 fromPos, UnityEngine.Vector2 toPos, int explodeRadius)
        {
            List<BulletExplode> list = new List<BulletExplode>();

            if (bullet == null)
            {
                return list;
            }

            try
            {
                /// Tính Vector chỉ phương
                UnityEngine.Vector2 dirVector = toPos - fromPos;
                /// Tạo đường thẳng từ Vector chỉ phương
                KTMath.Line bulletLine = KTMath.GetLineFromDirectionalVector(fromPos, dirVector);

                /// Trả về danh sách các kẻ địch xung quanh
                List<GameObject> objects = null;

                /// Nếu mục tiêu là kẻ địch
                if (bullet.TargetType == "enemy")
                {
                    objects = KTLogic.GetEnemiesBetweenTwoPoints(bullet.Caster, fromPos, toPos, explodeRadius);
                }

                /// Nếu không tìm thấy thì thoát luôn
                if (objects == null)
                {
                    return list;
                }

                /// Duyệt toàn bộ danh sách, lọc ra tất cả các đối tượng thỏa mãn
                foreach (GameObject go in objects)
                {
                    /// Nếu là chủ nhân của đạn thì bỏ qua
                    if (go == bullet.Caster)
                    {
                        continue;
                    }
                    /// Nếu đối tượng đã từng có trong danh sách va chạm của đạn thì bỏ qua
                    else if (bullet.TouchTargets.Contains(go))
                    {
                        continue;
                    }

                    ///// Nếu đối tượng đang trong trạng thái khinh công thì bỏ qua
                    //if (go.m_eDoing == KE_NPC_DOING.do_jump)
                    //{
                    //    continue;
                    //}

                    /// Lấy vị trí hiện tại của đối tượng
                    UnityEngine.Vector2 objPos = new UnityEngine.Vector2((float) go.CurrentPos.X, (float) go.CurrentPos.Y);

                    /// Nếu đối tượng nằm cách đường thẳng hướng bay của đạn một khoảng vừa với phạm vi đạn nổ
                    if (KTMath.GetDistanceOfPointToLine(objPos, bulletLine) <= explodeRadius)
                    {
                        /// Tọa độ hình chiếu vuông góc của điểm lên đường bay của đạn
                        UnityEngine.Vector2 projectionPoint;

                        /// Nếu nằm trên đường bay của đạn
                        if (KTMath.IsPointInParagraph(objPos, fromPos, toPos))
                        {
                            projectionPoint = objPos;
                        }
                        else
                        {
                            projectionPoint = KTMath.GetPerpendicularProjectionOfPointInLine(objPos, bulletLine);
                        }

                        /// Nếu nằm trong phạm vi hình chữ nhật với phạm vi dọc theo đường bay của đạn
                        if (KTMath.IsPointInParagraph(projectionPoint, fromPos, toPos))
                        {
                            /// Khoảng cách từ vị trí hiện tại của đạn đến vị trí gặp đối tượng và phát nổ
                            float distance = UnityEngine.Vector2.Distance(fromPos, projectionPoint);

                            /// Thời gian bay từ vị trí hiện tại tới vị trí gặp đối tượng
                            float time = distance / bullet.Velocity;

                            /// Thêm đối tượng vào danh sách
                            list.Add(new BulletExplode()
                            {
                                Target = go,
                                Time = time,
                                ExplodePos = projectionPoint,
                            });

                            /// Đối tượng đã được thêm vào danh sách, không cần xuống dưới kiểm tra các điều kiện còn lại nữa
                            continue;
                        }
                    }

                    /// Nếu đối tượng nằm trên đường tròn ở đầu mút điểm FromPos
                    if (KTMath.IsPointInsideCircle(objPos, fromPos, explodeRadius))
                    {
                        /// Thời gian bay từ vị trí hiện tại tới vị trí gặp đối tượng
                        float time = 0f;

                        /// Thêm đối tượng vào danh sách
                        list.Add(new BulletExplode()
                        {
                            Target = go,
                            Time = time,
                            ExplodePos = fromPos,
                        });

                        /// Đối tượng đã được thêm vào danh sách, không cần xuống dưới kiểm tra các điều kiện còn lại nữa
                        continue;
                    }

                    /// Nếu đối tượng nằm trên đường tròn ở đầu mút điểm FinishPos
                    if (KTMath.IsPointInsideCircle(objPos, toPos, explodeRadius))
                    {
                        /// Khoảng cách từ vị trí hiện tại của đạn đến vị trí gặp đối tượng và phát nổ
                        float distance = UnityEngine.Vector2.Distance(fromPos, toPos);

                        /// Thời gian bay từ vị trí hiện tại tới vị trí gặp đối tượng
                        float time = distance / bullet.Velocity;

                        /// Thêm đối tượng vào danh sách
                        list.Add(new BulletExplode()
                        {
                            Target = go,
                            Time = time,
                            ExplodePos = toPos,
                        });

                        /// Đối tượng đã được thêm vào danh sách, không cần xuống dưới kiểm tra các điều kiện còn lại nữa
                        continue;
                    }
                }

                /// Sắp xếp danh sách tăng dần theo thời gian
                list = list.OrderBy(x => x.Time).ToList();

                return list;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogTypes.Skill, ex.ToString());
                return list;
            }
        }
	}
}
