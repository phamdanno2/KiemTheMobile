using GameServer.Logic;
using System;
using System.IO;
using System.Xml.Linq;

namespace GameServer.KiemThe
{
	/// <summary>
	/// Thiết lập hệ thống
	/// </summary>
	public class ServerConfig
	{
		#region Singleton - Instance
		/// <summary>
		/// Thiết lập hệ thống
		/// </summary>
		public static ServerConfig Instance { get; private set; }

		/// <summary>
		/// Thiết lập hệ thống
		/// </summary>
		private ServerConfig() { }
		#endregion

		/// <summary>
		/// CCU tối đa
		/// </summary>
		public int MaxCCU { get; set; }

		/// <summary>
		/// Giới hạn tài khoản trên địa chỉ IP
		/// </summary>
		public int LimitAccountPerIPAddress { get; set; }

		/// <summary>
		/// Mở luồng thực thi di chuyển ngẫu nhiên của quái
		/// </summary>
		public bool EnableMonsterAIRandomMove { get; set; }

		/// <summary>
		/// Mở luồng thực thi AI quái
		/// </summary>
		public bool EnableMonsterAI { get; set; }



		public bool KuaFuServerEnable { get; set; }

		/// <summary>
		/// Số luồng UpdateGrid chạy đồng thời
		/// </summary>
		public int MaxUpdateGridThread { get; set; }

		/// <summary>
		/// Số luồng StoryBoard chạy đồng thời
		/// </summary>
		public int MaxPlayerStoryBoardThread { get; set; }

		/// <summary>
		/// SỐ luồng Monster chạy đồng thời
		/// </summary>
		public int MaxMonsterTimer { get; set; }

		/// <summary>
		/// Số luồng Buff chạy đồng thời
		/// </summary>
		public int MaxBuffTimer { get; set; }

		/// <summary>
		/// Giới hạn cấp độ
		/// </summary>
		public int LimitLevel { get; set; }

		/// <summary>
		/// Kích hoạt Captcha
		/// </summary>
		public bool EnableCaptcha { get; set; }

		/// <summary>
		/// Kích hoạt Captcha cho các thiết bị iOS
		/// </summary>
		public bool EnableCaptchaForIOS { get; set; }

		/// <summary>
		/// Thời gian xuất hiện tối thiểu
		/// </summary>
		public int CaptchaAppearMinPeriod { get; set; }

		/// <summary>
		/// Thời gian xuất hiện tối đa
		/// </summary>
		public int CaptchaAppearMaxPeriod { get; set; }

		/// <summary>
		/// Captcha chỉ xuất hiện ở người chơi có nhóm
		/// </summary>
		public bool CaptchaTeamPlayersOnly { get; set; }

		/// <summary>
		/// Thời gian xuất hiện Captcha trong ngày
		/// </summary>
		public DateTime CaptchaAppearFromTime { get; set; }

		/// <summary>
		/// Thời gian kết thúc xuất hiện Captcha trong ngày
		/// </summary>
		public DateTime CaptchaAppearToTime { get; set; }

		/// <summary>
		/// Kinh nghiệm nhận được khi trả lời đúng Captcha
		/// </summary>
		public int CaptchaExpAddPerLevel { get; set; }

		/// <summary>
		/// Bạc khóa nhận được khi trả lời đúng Captcha
		/// </summary>
		public int CaptchaBoundMoneyAddPerLevel { get; set; }

		/// <summary>
		/// Kích hoạt Captcha cho các thiết bị iOS
		/// </summary>
		public bool EnableCaptchaForBattle { get; set; }
		
		/// <summary>
		/// Khởi tạo
		/// </summary>
		public static void Init()
		{
			XElement xmlNode = XElement.Parse(File.ReadAllText("ServerConfig.xml"));
			ServerConfig.Instance = new ServerConfig()
			{
				MaxCCU = int.Parse(xmlNode.Element("LimitAccount").Attribute("MaxCCU").Value),
				LimitAccountPerIPAddress = int.Parse(xmlNode.Element("LimitAccount").Attribute("LimitAccountPerIPAddress").Value),
				EnableMonsterAIRandomMove = bool.Parse(xmlNode.Element("MonsterAI").Attribute("EnableMonsterAIRandomMove").Value),
				EnableMonsterAI = bool.Parse(xmlNode.Element("MonsterAI").Attribute("EnableMonsterAI").Value),
				MaxUpdateGridThread = int.Parse(xmlNode.Element("Threading").Attribute("MaxUpdateGridThread").Value),
				MaxPlayerStoryBoardThread = int.Parse(xmlNode.Element("Threading").Attribute("MaxPlayerStoryBoardThread").Value),
				MaxMonsterTimer = int.Parse(xmlNode.Element("Threading").Attribute("MaxMonsterTimer").Value),
				MaxBuffTimer = int.Parse(xmlNode.Element("Threading").Attribute("MaxBuffTimer").Value),
				LimitLevel = int.Parse(xmlNode.Element("GameConfig").Attribute("LimitLevel").Value),
				KuaFuServerEnable = bool.Parse(xmlNode.Element("GameConfig").Attribute("KuaFuServerEnable").Value),

				EnableCaptcha = bool.Parse(xmlNode.Element("Captcha").Attribute("EnableCaptcha").Value),
				EnableCaptchaForIOS = bool.Parse(xmlNode.Element("Captcha").Attribute("EnableCaptchaForIOS").Value),
				CaptchaAppearMinPeriod = int.Parse(xmlNode.Element("Captcha").Attribute("CaptchaAppearMinPeriod").Value),
				CaptchaAppearMaxPeriod = int.Parse(xmlNode.Element("Captcha").Attribute("CaptchaAppearMaxPeriod").Value),
				CaptchaTeamPlayersOnly = bool.Parse(xmlNode.Element("Captcha").Attribute("CaptchaTeamPlayersOnly").Value),
				CaptchaExpAddPerLevel = int.Parse(xmlNode.Element("Captcha").Attribute("CaptchaExpAddPerLevel").Value),
				CaptchaBoundMoneyAddPerLevel = int.Parse(xmlNode.Element("Captcha").Attribute("CaptchaBoundMoneyAddPerLevel").Value),
				EnableCaptchaForBattle = bool.Parse(xmlNode.Element("Captcha").Attribute("EnableCaptchaForBattle").Value),
			};

            /// Thời gian bắt đầu xuất hiện
            {
				string timeString = xmlNode.Element("Captcha").Attribute("CaptchaAppearFromTime").Value;
				string[] fields = timeString.Split(':');
				int hour = int.Parse(fields[0]);
                int minute = int.Parse(fields[1]);
				/// Ngày hôm nay
				DateTime now = DateTime.Now;
				ServerConfig.Instance.CaptchaAppearFromTime = new DateTime(now.Year, now.Month, now.Day, hour, minute, 0);
			}

            /// Thời gian kết thúc xuất hiện
            {
				string timeString = xmlNode.Element("Captcha").Attribute("CaptchaAppearToTime").Value;
				string[] fields = timeString.Split(':');
				int hour = int.Parse(fields[0]);
                int minute = int.Parse(fields[1]);
				/// Ngày hôm nay
				DateTime now = DateTime.Now;
				ServerConfig.Instance.CaptchaAppearToTime = new DateTime(now.Year, now.Month, now.Day, hour, minute, 0);
			}
		}
	}
}
