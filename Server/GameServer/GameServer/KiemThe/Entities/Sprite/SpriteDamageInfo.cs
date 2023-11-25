﻿using GameServer.Logic;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using static GameServer.KiemThe.Logic.KTSkillManager;

namespace GameServer.KiemThe.Entities.Sprite
{
	/// <summary>
	/// Hàng đợi thông tin sát thương
	/// </summary>
	public class SpriteDamageQueue
	{
		/// <summary>
		/// Đối tượng
		/// </summary>
		public GameObject Object { get; set; }

		/// <summary>
		/// Loại sát thương
		/// </summary>
		public SkillResult Type { get; set; }

		/// <summary>
		/// Sát thương
		/// </summary>
		public int Damage { get; set; }
	}

	/// <summary>
	/// Thông tin sát thương nhận được hoặc gây ra cho đối tượng tương ứng
	/// </summary>
	public class SpriteDamageInfo : IDisposable
	{
		/// <summary>
		/// Đối tượng
		/// </summary>
		public GameObject Object { get; set; }

		/// <summary>
		/// Thông tin sát thương
		/// </summary>
		public Dictionary<SkillResult, int> Damages { get; set; }

		/// <summary>
		/// Hủy đối tượng
		/// </summary>
		public void Dispose()
		{
			this.Object = null;
			this.Damages.Clear();
			this.Damages = null;
		}
	}
}
