using FSPlay.KiemVu.Entities.Config;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FSPlay.KiemVu.Control.Component.Skill
{
    /// <summary>
    /// Quản lý động tác
    /// </summary>
    public partial class Bullet
    {
        /// <summary>
        /// Thực hiện động tác
        /// </summary>
        private void PlayAnimation()
		{
            /// Ngừng động tác cũ
            this.StopAnimation();
            /// Thực thi động tác mới
            this.actionCoroutine = this.StartCoroutine(this.animation.DoFlyAsync(this.Direction, this.AnimationLifeTime, this.RepeatAnimation));
        }

        /// <summary>
        /// Ngừng thực hiện động tác
        /// </summary>
        private void StopAnimation()
		{
            /// Nếu có động tác cũ
            if (this.actionCoroutine != null)
            {
                this.StopCoroutine(this.actionCoroutine);
                this.actionCoroutine = null;
            }
        }

        /// <summary>
        /// Thực hiện hiệu ứng tan biến
        /// </summary>
        private void PlayFadeOut()
		{
            /// Ngừng động tác cũ
            this.StopAnimation();
            /// Thực thi động tác mới
            this.actionCoroutine = this.StartCoroutine(this.animation.DoFadeOutAsync(this.Direction, this.AnimationLifeTime, false, 0f, false, () => {
                /// Ngừng thực hiện động tác
                this.StopAnimation();
                /// Tự hủy
                this.Destroy();
            }));
        }

        /// <summary>
        /// Thực hiện di chuyển theo đường thẳng giữa 2 vị trí
        /// </summary>
        /// <param name="fromPos"></param>
        /// <param name="toPos"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        private IEnumerator LinearMove(Vector2 fromPos, Vector2 toPos, float duration)
		{
            /// Thời gian tồn tại
            float lifeTime = 0f;

            /// Lặp liên tục chừng nào chưa hết thời gian
            while (lifeTime <= duration)
            {
                /// % thời gian đã qua
                float percent = lifeTime / duration;
                /// Vị trí mới
                Vector2 newPos = Vector2.Lerp(fromPos, toPos, percent);
                /// Cập nhật vị trí mới
                this.transform.localPosition = newPos;
                /// Tăng thời gian đã thực thi
                lifeTime += Time.deltaTime;
                /// Bỏ qua Frame
                yield return null;
            }

            /// Cập nhật vị trí đích
            this.transform.localPosition = toPos;
        }

        /// <summary>
        /// Thực hiện bay từ vị trí A đến vị trí B
        /// </summary>
        /// <returns></returns>
        private IEnumerator DoLinearFly()
		{
            /// Vector chỉ hướng bay
            Vector2 dirVector = this.ToPos - this.FromPos;
            /// Góc quay tương ứng của tia đạn
            float degree = KTMath.GetAngle360WithXAxis(dirVector);
            /// Hướng bay tương ứng
            this.Direction = KTMath.GetDirection16ByAngle360(degree);
            /// Thời gian bay
            float duration = Math.Min(Vector2.Distance(this.FromPos, this.ToPos) / this.Velocity, this.MaxLifeTime);
            duration = Math.Max(duration, 0.1f);

            /// Thực hiện hiệu ứng
            if (!this.animation.IsPausing)
			{
                this.PlayAnimation();
            }

            /// Thực hiện di chuyển
            yield return this.LinearMove(this.FromPos, this.ToPos, duration);

            /// Ngừng thực hiện động tác
            this.StopAnimation();
            /// Tự hủy
            this.Destroy();
            /// Hủy luồng bay
            this.flyCoroutine = null;
        }

        /// <summary>
        /// Thực hiện nổ tại vị trí tương ứng
        /// </summary>
        /// <returns></returns>
        private IEnumerator DoStaticExplode()
		{
            int startHeight = 10;
            if (Loader.Loader.BulletConfigs.TryGetValue(this.ResID, out BulletConfig bulletConfig))
            {
                startHeight = bulletConfig.StartHeight;
            }
            startHeight -= 10;

            /// Hướng hiện tại
            this.Direction = Entities.Enum.Direction16.Down;

            /// Thực hiện biểu diễn hiệu ứng
            if (!this.animation.IsPausing)
            {
                this.PlayAnimation();
            }

            /// Thời gian thực hiện hiệu ứng rơi
            float duration = bulletConfig != null ? bulletConfig.LifeTime / 18f : this.AnimationLifeTime;

            /// Nếu có độ cao ban đầu
            if (startHeight > 0)
            {
                /// Vị trí bắt đầu rơi
                Vector2 sPos = new Vector2(this.FromPos.x, this.FromPos.y + startHeight);
                /// Thực hiện hiệu ứng rơi từ trên xuống
                yield return this.LinearMove(sPos, this.FromPos, duration);
            }
            else
            {
                /// Thiết lập vị trí
                this.gameObject.transform.localPosition = this.FromPos;

                /// Nếu hiệu ứng không lặp lại thì chờ hết thời gian hiệu ứng
                if (!this.RepeatAnimation)
                {
                    yield return new WaitForSeconds(this.AnimationLifeTime);
                }
                /// Nếu hiệu ứng lặp lại thì chờ hết thời gian lặp
                else
                {
                    yield return new WaitForSeconds(this.MaxLifeTime);
                }
            }

            /// Thực hiện động tác biến
            this.PlayFadeOut();
            /// Hủy luồng bay
            this.flyCoroutine = null;
        }

        /// <summary>
        /// Thực hiện đạn bay đuổi theo mục tiêu
        /// </summary>
        /// <returns></returns>
        private IEnumerator DoChaseTarget()
        {
            /// Cập nhật vị trí xuất phát
            this.transform.localPosition = this.FromPos;
            /// Thời gian lần trước đổi hướng
            float timeChangedDir = 0.1f;
            /// Lặp liên tục
            while (true)
			{
                /// Nếu mục tiêu không tồn tại
                if (!this.ChaseTarget || !this.ChaseTarget.activeSelf)
                {
                    /// Ngừng thực hiện động tác
                    this.StopAnimation();
                    /// Tự hủy
                    this.Destroy();
                    break;
                }

                /// Cập nhật thời gian lần trước đổi hướng
                timeChangedDir += Time.deltaTime;
                /// Nếu đã đến thời gian đổi hướng
                if (timeChangedDir >= 0.1f)
				{
                    /// Hướng cũ
                    Entities.Enum.Direction16 oldDir = this.Direction;
                    /// Vector hướng bay
                    Vector2 dirVector = (Vector2) this.ChaseTarget.transform.localPosition - this.FromPos;
                    /// Góc quay so với trục Ox
                    float degree = KTMath.GetAngle360WithXAxis(dirVector);
                    /// Hướng bay hiện tại
                    this.Direction = KTMath.GetDirection16ByAngle360(degree);

                    /// Nếu hướng thay đổi
                    if (oldDir != this.Direction)
					{
                        /// Thực hiện biểu diễn hiệu ứng
                        if (!this.animation.IsPausing)
                        {
                            this.PlayAnimation();
                        }
                    }
                }

                /// Vị trí của mục tiêu
                Vector2 targetPos = this.ChaseTarget.transform.localPosition;
                /// Vị trí hiện tại của tia đạn
                Vector2 currentPos = this.transform.localPosition;
                /// Khoảng dịch chuyển được
                float distance = Mathf.Min(Vector2.Distance(currentPos, targetPos), this.Velocity * Time.deltaTime);
                /// Vị trí dịch đến
                currentPos = Vector2.MoveTowards(currentPos, targetPos, distance);
                /// Thiết lập vị trí
                this.transform.localPosition = currentPos;

                /// Nếu đã chạm mục tiêu
                if (Vector2.Distance(currentPos, targetPos) <= 10f)
                {
                    /// Ngừng thực hiện động tác
                    this.StopAnimation();
                    /// Tự hủy
                    this.Destroy();
                    break;
                }

                /// Bỏ qua Frame
                yield return null;
            }
            /// Hủy luồng bay
            this.flyCoroutine = null;
        }
	}
}
