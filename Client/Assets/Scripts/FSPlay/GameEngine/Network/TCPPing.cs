using HSGameEngine.GameEngine.Network.Tools;

namespace FSPlay.GameEngine.Network
{
	/// <summary>
	/// 测量网络ping值的类
	/// </summary>
	public class TCPPing
	{
		private static long MoveCmdStartTicks =  0;
		private static long PosCmdStartTicks =  0;
		private static long HeartCmdStartTicks =  0;
		private static long ActionCmdStartTicks =  0;
		
		private static long AvgPingTicks =  0;
		
		/// <summary>
		/// 获取平均ping值
		/// </summary>
		/// <returns>
		/// The ping ticks.
		/// </returns>
		public static long GetPingTicks()
		{
			return AvgPingTicks;
		}
		
		/// <summary>
		/// 记录当前指令的发出时间
		/// </summary>
		/// <param name='nID'>
		/// N I.
		/// </param>
		public static void RecordSendCmd(int nID)
		{
			switch (nID)
			{
				case (int)(TCPGameServerCmds.CMD_SPR_MOVE):
					MoveCmdStartTicks = TimeManager.GetCorrectLocalTime();
					break;
				case (int)(TCPGameServerCmds.CMD_SPR_POSITION):
					PosCmdStartTicks = TimeManager.GetCorrectLocalTime();
					break;	
				case (int)(TCPGameServerCmds.CMD_SPR_CLIENTHEART):
					HeartCmdStartTicks = TimeManager.GetCorrectLocalTime();
					break;
				case (int)(TCPGameServerCmds.CMD_SPR_ACTTION):
					ActionCmdStartTicks = TimeManager.GetCorrectLocalTime();
					break;						
				default:
					break;
			}
		}
		
		/// <summary>
		/// 记录指令的收到时间
		/// </summary>
		/// <param name='nID'>
		/// N I.
		/// </param>
		public static void RecordRecCmd(int nID)
		{
			switch (nID)
			{
				case (int)(TCPGameServerCmds.CMD_SPR_MOVE):
					AvgPingTicks = (TimeManager.GetCorrectLocalTime() - MoveCmdStartTicks) / 2;
					//trace("MoveCmdStartTicks avg:" + AvgPingTicks);
					break;
				case (int)(TCPGameServerCmds.CMD_SPR_POSITION):
					AvgPingTicks = (TimeManager.GetCorrectLocalTime() - PosCmdStartTicks) / 2;
					//trace("PosCmdStartTicks avg:" + AvgPingTicks);
					break;	
				case (int)(TCPGameServerCmds.CMD_SPR_CLIENTHEART):
					AvgPingTicks = (TimeManager.GetCorrectLocalTime() - HeartCmdStartTicks) / 2;
					//trace("HeartCmdStartTicks avg:" + AvgPingTicks);
					break;
				case (int)(TCPGameServerCmds.CMD_SPR_ACTTION):
					AvgPingTicks = (TimeManager.GetCorrectLocalTime() - ActionCmdStartTicks) / 2;
					//trace("AttackCmdStartTicks avg:" + AvgPingTicks);
					break;							
				default:
					break;
			}
		}
	}
}

