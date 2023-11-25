using FSPlay.GameEngine.Logic;
using FSPlay.GameEngine.Sprite;
using FSPlay.KiemVu.Logic.Settings;
using FSPlay.KiemVu.UI.Main.MainUI.NearbyEnemyPlayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FSPlay.KiemVu.UI.Main.MainUI
{
	/// <summary>
	/// Khung kẻ địch xung quanh là người chơi
	/// </summary>
	public class UINearbyEnemyPlayer : MonoBehaviour
	{
		#region Define
		/// <summary>
		/// Danh sách người chơi
		/// </summary>
		[SerializeField]
		private UINearbyEnemyPlayer_PlayerInfo[] UI_PlayerInfos;
		#endregion

		#region Properties
		/// <summary>
		/// Sự kiện chọn người chơi
		/// </summary>
		public Action<GSprite> Click { get; set; }
		#endregion

		#region Core MonoBehaviour
		/// <summary>
		/// Hàm này gọi ở Frame đầu tiên
		/// </summary>
		private void Start()
		{
			this.StartCoroutine(this.ScanEnemiesAround());
		}
		#endregion

		#region Code UI
		/// <summary>
		/// Khởi tạo ban đầu
		/// </summary>
		private void InitPrefabs()
		{

		}
		#endregion

		#region Private methods
		/// <summary>
		/// Tìm người chơi là kẻ địch xung quanh
		/// </summary>
		/// <returns></returns>
		private IEnumerator ScanEnemiesAround()
		{
			/// Danh sách kẻ địch
			List<GSprite> players = new List<GSprite>();
			/// Lặp liên tục
			while (true)
			{
				/// Nếu không có dữ liệu
				if (Global.Data == null || Global.Data.Leader == null)
				{
					/// Đợi 1s
					yield return new WaitForSeconds(1f);
					continue;
				}

				/// Làm rỗng danh sách người chơi gần
				foreach (UINearbyEnemyPlayer_PlayerInfo uiPlayerInfo in this.UI_PlayerInfos)
				{
					/// Ẩn
					uiPlayerInfo.gameObject.SetActive(false);
				}

				/// Làm rỗng danh sách kẻ địch
				players.Clear();

				/// Nếu thiết lập tìm người chơi
				if (KTAutoAttackSetting._AutoConfig._AutoPKConfig.DisplayEnemyUI)
				{
					/// Vị trí bản thân
					Vector2 leaderPos = Global.Data.Leader.PositionInVector2;
					/// Chọn ra những thằng gần nhất
					players = Global.Data.OtherRoles.Values.Where((player) =>
					{
						GSprite sprite = KTGlobal.FindSpriteByID(player.RoleID);
						if (sprite == null)
						{
							return false;
						}
						return KTGlobal.IsEnemy(sprite);
					}).OrderBy((player) =>
					{
						Vector2 playerPos = new Vector2(player.PosX, player.PosY);

						return Vector2.Distance(playerPos, leaderPos);
					}).Take(this.UI_PlayerInfos.Length).Select((player) => {
						return KTGlobal.FindSpriteByID(player.RoleID);
					}).ToList();

					/// Thứ tự
					int idx = -1;
					/// Duyệt danh sách người chơi và thêm thông tin tương ứng
					foreach (GSprite sprite in players)
					{
						/// Tăng thứ tự
						idx++;
						this.UI_PlayerInfos[idx].gameObject.SetActive(true);
						this.UI_PlayerInfos[idx].Data = sprite.RoleData;
						this.UI_PlayerInfos[idx].Click = () => {
							this.Click?.Invoke(sprite);
						};
					}
				}

				/// Đợi 1s
				yield return new WaitForSeconds(1f);
			}
		}
		#endregion
	}
}
