﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSPlay.GameEngine.Network
{
    /// <summary>
    /// Quản lý Game
    /// </summary>
	public static class GameInstance
	{
        /// <summary>
        /// Đối tượng TCP Game
        /// </summary>
        public static TCPGame Game = new TCPGame();
	}
}
