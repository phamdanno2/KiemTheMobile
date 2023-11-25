using GameServer.KiemThe.Entities;
using GameServer.Logic;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace GameServer.KiemThe.CopySceneEvents.Family
{
	/// <summary>
	/// Phụ bản Vượt ải gia tộc
	/// </summary>
	public static class FamilyFuBen
	{
		#region Define
		/// <summary>
		/// Thiết lập
		/// </summary>
		public class EventConfig
		{
			/// <summary>
			/// ID sự kiện
			/// </summary>
			public int EventID { get; set; }

			/// <summary>
			/// Thời gian duy trì phụ bản (Mili-giây)
			/// </summary>
			public int Duration { get; set; }

			/// <summary>
			/// Số lượng Boss được gọi ra bởi Câu Hồn Ngọc tối đa
			/// </summary>
			public int MaxCallBoss { get; set; }

			/// <summary>
			/// Thời gian chuẩn bị trước khi mở ải (Mili-giây)
			/// </summary>
			public int PrepareTime { get; set; }

			/// <summary>
			/// Thời gian chờ tự đóng phụ bản khi hoàn thành vượt ải (Mili-giây)
			/// </summary>
			public int FinishWaitTime { get; set; }

			/// <summary>
			/// Số lượt tham gia phụ bản tối đa của mỗi người chơi trong tuần
			/// </summary>
			public int MaxParticipatedTimesPerWeek { get; set; }

			/// <summary>
			/// Cấp độ tối tiểu tham gia vượt ải
			/// </summary>
			public int MinLevel { get; set; }

			/// <summary>
			/// Chuyển đối tượng từ XMLNode
			/// </summary>
			/// <param name="xmlNode"></param>
			/// <returns></returns>
			public static EventConfig Parse(XElement xmlNode)
			{
				return new EventConfig()
				{
					EventID = int.Parse(xmlNode.Attribute("EventID").Value),
					Duration = int.Parse(xmlNode.Attribute("Duration").Value),
					MaxCallBoss = int.Parse(xmlNode.Attribute("MaxCallBoss").Value),
					PrepareTime = int.Parse(xmlNode.Attribute("PrepareDuration").Value),
					FinishWaitTime = int.Parse(xmlNode.Attribute("FinishWaitTime").Value),
					MaxParticipatedTimesPerWeek = int.Parse(xmlNode.Attribute("MaxParticipatedTimesPerWeek").Value),
					MinLevel = int.Parse(xmlNode.Attribute("MinLevel").Value),
				};
			}
		}

		/// <summary>
		/// Thông tin bản đồ phụ bản
		/// </summary>
		public class MapInfo
		{
			/// <summary>
			/// ID bản đồ
			/// </summary>
			public int MapID { get; set; }

			/// <summary>
			/// Vị trí vào X
			/// </summary>
			public int EnterPosX { get; set; }

			/// <summary>
			/// Vị trí vào Y
			/// </summary>
			public int EnterPosY { get; set; }

			/// <summary>
			/// Chuyển đối tượng từ XMLNode
			/// </summary>
			/// <param name="xmlNode"></param>
			/// <returns></returns>
			public static MapInfo Parse(XElement xmlNode)
			{
				return new MapInfo()
				{
					MapID = int.Parse(xmlNode.Attribute("MapID").Value),
					EnterPosX = int.Parse(xmlNode.Attribute("EnterPosX").Value),
					EnterPosY = int.Parse(xmlNode.Attribute("EnterPosY").Value),
				};
			}
		}

		/// <summary>
		/// Thông tin vật phẩm
		/// </summary>
		public class EventItem
		{
			/// <summary>
			/// ID vật phẩm Câu Hồn Ngọc (sơ)
			/// </summary>
			public int CallBoss55ItemID { get; set; }

			/// <summary>
			/// ID vật phẩm Câu Hồn Ngọc (trung)
			/// </summary>
			public int CallBoss75ItemID { get; set; }

			/// <summary>
			/// ID vật phẩm Câu Hồn Ngọc (cao)
			/// </summary>
			public int CallBoss95ItemID { get; set; }

			/// <summary>
			/// ID vật phẩm đồng tiền cổ
			/// </summary>
			public int FamilyCoinItemID { get; set; }

			/// <summary>
			/// Chuyển đối tượng từ XMLNode
			/// </summary>
			/// <param name="xmlNode"></param>
			/// <returns></returns>
			public static EventItem Parse(XElement xmlNode)
			{
				return new EventItem()
				{
					CallBoss55ItemID = int.Parse(xmlNode.Attribute("CallBoss55ItemID").Value),
					CallBoss75ItemID = int.Parse(xmlNode.Attribute("CallBoss75ItemID").Value),
					CallBoss95ItemID = int.Parse(xmlNode.Attribute("CallBoss95ItemID").Value),
					FamilyCoinItemID = int.Parse(xmlNode.Attribute("FamilyCoinItemID").Value),
				};
			}
		}

		/// <summary>
		/// Quy tắc trò chơi
		/// </summary>
		public class GameRule
		{
			/// <summary>
			/// Thông tin bước
			/// </summary>
			public class StepInfo
			{
				/// <summary>
				/// ID bước
				/// </summary>
				public int ID { get; set; }

				/// <summary>
				/// Cho phép dùng Câu Hồn Ngọc tạo Boss không
				/// </summary>
				public bool AllowCallBoss { get; set; }

				/// <summary>
				/// Danh sách cơ quan
				/// </summary>
				public List<TriggerInfo> Triggers { get; set; }

				/// <summary>
				/// Danh sách hộ vệ cơ quan
				/// </summary>
				public List<TriggerGuardianInfo> Guardians { get; set; }

				/// <summary>
				/// Danh sách Boss
				/// </summary>
				public List<BossInfo> Bosses { get; set; }

				/// <summary>
				/// Danh sách cổng dịch chuyển
				/// </summary>
				public List<TeleportInfo> Teleports { get; set; }

				/// <summary>
				/// Chuyển đối tượng từ XMLNode
				/// </summary>
				/// <param name="xmlNode"></param>
				/// <returns></returns>
				public static StepInfo Parse(XElement xmlNode)
				{
					StepInfo step = new StepInfo()
					{
						ID = int.Parse(xmlNode.Attribute("ID").Value),
						AllowCallBoss = bool.Parse(xmlNode.Attribute("AllowCallBoss").Value),
						Triggers = new List<TriggerInfo>(),
						Guardians = new List<TriggerGuardianInfo>(),
						Bosses = new List<BossInfo>(),
						Teleports = new List<TeleportInfo>(),
					};

					foreach (XElement node in xmlNode.Elements("Trigger"))
					{
						step.Triggers.Add(TriggerInfo.Parse(node));
					}

					foreach (XElement node in xmlNode.Elements("Guardian"))
					{
						step.Guardians.Add(TriggerGuardianInfo.Parse(node));
					}

					foreach (XElement node in xmlNode.Elements("Boss"))
					{
						step.Bosses.Add(BossInfo.Parse(node));
					}

					foreach (XElement node in xmlNode.Elements("Teleport"))
					{
						step.Teleports.Add(TeleportInfo.Parse(node));
					}

					return step;
				}
			}

			/// <summary>
			/// Cơ quan
			/// </summary>
			public class TriggerInfo
			{
				/// <summary>
				/// ID Res
				/// </summary>
				public int ID { get; set; }

				/// <summary>
				/// Tên cơ quan
				/// </summary>
				public string Name { get; set; }

				/// <summary>
				/// Thời gian mở
				/// </summary>
				public int CollectTick { get; set; }

				/// <summary>
				/// Vị trí X
				/// </summary>
				public int PosX { get; set; }

				/// <summary>
				/// Vị trí Y
				/// </summary>
				public int PosY { get; set; }

				/// <summary>
				/// Thứ tự cơ quan trong ải
				/// </summary>
				public int Index { get; set; }

				/// <summary>
				/// Chuyển đối tượng từ XMLNode
				/// </summary>
				/// <param name="xmlNode"></param>
				/// <returns></returns>
				public static TriggerInfo Parse(XElement xmlNode)
				{
					TriggerInfo triggerInfo = new TriggerInfo()
					{
						ID = int.Parse(xmlNode.Attribute("ID").Value),
						Name = xmlNode.Attribute("Name").Value,
						CollectTick = int.Parse(xmlNode.Attribute("CollectTick").Value),
						PosX = int.Parse(xmlNode.Attribute("PosX").Value),
						PosY = int.Parse(xmlNode.Attribute("PosY").Value),
						Index = int.Parse(xmlNode.Attribute("Index").Value),
					};

					/// Trả về kết quả
					return triggerInfo;
				}
			}

			/// <summary>
			/// Thông tin quái bảo vệ cơ quan
			/// </summary>
			public class TriggerGuardianInfo
			{
				/// <summary>
				/// ID quái
				/// </summary>
				public int ID { get; set; }

				/// <summary>
				/// Tên quái
				/// </summary>
				public string Name { get; set; }

				/// <summary>
				/// Danh hiệu quái
				/// </summary>
				public string Title { get; set; }

				/// <summary>
				/// Vị trí X
				/// </summary>
				public int PosX { get; set; }

				/// <summary>
				/// Vị trí Y
				/// </summary>
				public int PosY { get; set; }

				/// <summary>
				/// Sinh lực cơ bản
				/// </summary>
				public int BaseHP { get; set; }

				/// <summary>
				/// Sinh lực tăng thêm mỗi cấp
				/// </summary>
				public int HPIncreaseEachLevel { get; set; }

				/// <summary>
				/// Loại AI
				/// </summary>
				public MonsterAIType AIType { get; set; }

				/// <summary>
				/// ID Script AI điều khiển
				/// </summary>
				public int AIScriptID { get; set; }

				/// <summary>
				/// Cơ quan bảo vệ
				/// </summary>
				public int TriggerIndex { get; set; }

				/// <summary>
				/// Danh sách kỹ năng sẽ sử dụng
				/// </summary>
				public List<SkillLevelRef> Skills { get; set; }

				/// <summary>
				/// Danh sách vòng sáng sẽ sử dụng
				/// </summary>
				public List<SkillLevelRef> Auras { get; set; }

				/// <summary>
				/// Chuyển đối tượng từ XMLNode
				/// </summary>
				/// <param name="xmlNode"></param>
				/// <returns></returns>
				public static TriggerGuardianInfo Parse(XElement xmlNode)
				{
					TriggerGuardianInfo monsterInfo = new TriggerGuardianInfo()
					{
						ID = int.Parse(xmlNode.Attribute("ID").Value),
						Name = xmlNode.Attribute("Name").Value,
						Title = xmlNode.Attribute("Title").Value,
						PosX = int.Parse(xmlNode.Attribute("PosX").Value),
						PosY = int.Parse(xmlNode.Attribute("PosY").Value),
						BaseHP = int.Parse(xmlNode.Attribute("BaseHP").Value),
						HPIncreaseEachLevel = int.Parse(xmlNode.Attribute("HPIncreaseEachLevel").Value),
						AIType = (MonsterAIType) int.Parse(xmlNode.Attribute("AIType").Value),
						AIScriptID = int.Parse(xmlNode.Attribute("AIScriptID").Value),
						TriggerIndex = int.Parse(xmlNode.Attribute("TriggerIndex").Value),
						Skills = new List<SkillLevelRef>(),
						Auras = new List<SkillLevelRef>(),
					};

					/// Chuỗi mã hóa danh sách kỹ năng sử dụng
					string skillsString = xmlNode.Attribute("Skills").Value;
					/// Nếu có kỹ năng
					if (!string.IsNullOrEmpty(skillsString))
					{
						/// Duyệt danh sách kỹ năng
						foreach (string skillStr in skillsString.Split(';'))
						{
							string[] fields = skillStr.Split('_');
							try
							{
								int skillID = int.Parse(fields[0]);
								int skillLevel = int.Parse(fields[1]);
								int cooldown = int.Parse(fields[2]);

								/// Thông tin kỹ năng tương ứng
								SkillDataEx skillData = KSkill.GetSkillData(skillID);
								/// Nếu kỹ năng không tồn tại
								if (skillData == null)
								{
									throw new Exception(string.Format("Skill ID = {0} not found!", skillID));
								}

								/// Nếu cấp độ dưới 0
								if (skillLevel <= 0)
								{
									throw new Exception(string.Format("Skill ID = {0} level must be greater than 0", skillID));
								}

								/// Kỹ năng theo cấp
								SkillLevelRef skillRef = new SkillLevelRef()
								{
									Data = skillData,
									AddedLevel = skillLevel,
									Exp = cooldown,
								};

								/// Thêm vào danh sách
								monsterInfo.Skills.Add(skillRef);
							}
							catch (Exception ex)
							{
								Console.WriteLine(ex.ToString());
								LogManager.WriteLog(LogTypes.Exception, ex.ToString());
							}
						}
					}

					/// Chuỗi mã hóa danh sách vòng sáng sử dụng
					string aurasString = xmlNode.Attribute("Auras").Value;
					/// Nếu có kỹ năng
					if (!string.IsNullOrEmpty(aurasString))
					{
						/// Duyệt danh sách kỹ năng
						foreach (string skillStr in aurasString.Split(';'))
						{
							string[] fields = skillStr.Split('_');
							try
							{
								int skillID = int.Parse(fields[0]);
								int skillLevel = int.Parse(fields[1]);

								/// Thông tin kỹ năng tương ứng
								SkillDataEx skillData = KSkill.GetSkillData(skillID);
								/// Nếu kỹ năng không tồn tại
								if (skillData == null)
								{
									throw new Exception(string.Format("Skill ID = {0} not found!", skillID));
								}

								/// Nếu cấp độ dưới 0
								if (skillLevel <= 0)
								{
									throw new Exception(string.Format("Skill ID = {0} level must be greater than 0", skillID));
								}

								/// Nếu không phải vòng sáng
								if (!skillData.IsArua)
								{
									throw new Exception(string.Format("Skill ID = {0} is not Aura!", skillID));
								}

								/// Kỹ năng theo cấp
								SkillLevelRef skillRef = new SkillLevelRef()
								{
									Data = skillData,
									AddedLevel = skillLevel,
								};

								/// Thêm vào danh sách
								monsterInfo.Auras.Add(skillRef);
							}
							catch (Exception ex)
							{
								Console.WriteLine(ex.ToString());
								LogManager.WriteLog(LogTypes.Exception, ex.ToString());
							}
						}
					}

					/// Trả về kết quả
					return monsterInfo;
				}
			}

			/// <summary>
			/// Thông tin quái
			/// </summary>
			public class BossInfo
			{
				/// <summary>
				/// ID quái
				/// </summary>
				public int ID { get; set; }

				/// <summary>
				/// Tên quái
				/// </summary>
				public string Name { get; set; }

				/// <summary>
				/// Danh hiệu quái
				/// </summary>
				public string Title { get; set; }

				/// <summary>
				/// Vị trí X
				/// </summary>
				public int PosX { get; set; }

				/// <summary>
				/// Vị trí Y
				/// </summary>
				public int PosY { get; set; }

				/// <summary>
				/// Sinh lực cơ bản
				/// </summary>
				public int BaseHP { get; set; }

				/// <summary>
				/// Sinh lực tăng thêm mỗi cấp
				/// </summary>
				public int HPIncreaseEachLevel { get; set; }

				/// <summary>
				/// Loại AI
				/// </summary>
				public MonsterAIType AIType { get; set; }

				/// <summary>
				/// ID Script AI điều khiển
				/// </summary>
				public int AIScriptID { get; set; }

				/// <summary>
				/// Danh sách kỹ năng sẽ sử dụng
				/// </summary>
				public List<SkillLevelRef> Skills { get; set; }

				/// <summary>
				/// Danh sách vòng sáng sẽ sử dụng
				/// </summary>
				public List<SkillLevelRef> Auras { get; set; }

				/// <summary>
				/// Chuyển đối tượng từ XMLNode
				/// </summary>
				/// <param name="xmlNode"></param>
				/// <returns></returns>
				public static BossInfo Parse(XElement xmlNode)
				{
					BossInfo monsterInfo = new BossInfo()
					{
						ID = int.Parse(xmlNode.Attribute("ID").Value),
						Name = xmlNode.Attribute("Name").Value,
						Title = xmlNode.Attribute("Title").Value,
						PosX = int.Parse(xmlNode.Attribute("PosX").Value),
						PosY = int.Parse(xmlNode.Attribute("PosY").Value),
						BaseHP = int.Parse(xmlNode.Attribute("BaseHP").Value),
						HPIncreaseEachLevel = int.Parse(xmlNode.Attribute("HPIncreaseEachLevel").Value),
						AIType = (MonsterAIType) int.Parse(xmlNode.Attribute("AIType").Value),
						AIScriptID = int.Parse(xmlNode.Attribute("AIScriptID").Value),
						Skills = new List<SkillLevelRef>(),
						Auras = new List<SkillLevelRef>(),
					};

					/// Chuỗi mã hóa danh sách kỹ năng sử dụng
					string skillsString = xmlNode.Attribute("Skills").Value;
					/// Nếu có kỹ năng
					if (!string.IsNullOrEmpty(skillsString))
					{
						/// Duyệt danh sách kỹ năng
						foreach (string skillStr in skillsString.Split(';'))
						{
							string[] fields = skillStr.Split('_');
							try
							{
								int skillID = int.Parse(fields[0]);
								int skillLevel = int.Parse(fields[1]);
								int cooldown = int.Parse(fields[2]);

								/// Thông tin kỹ năng tương ứng
								SkillDataEx skillData = KSkill.GetSkillData(skillID);
								/// Nếu kỹ năng không tồn tại
								if (skillData == null)
								{
									throw new Exception(string.Format("Skill ID = {0} not found!", skillID));
								}

								/// Nếu cấp độ dưới 0
								if (skillLevel <= 0)
								{
									throw new Exception(string.Format("Skill ID = {0} level must be greater than 0", skillID));
								}

								/// Kỹ năng theo cấp
								SkillLevelRef skillRef = new SkillLevelRef()
								{
									Data = skillData,
									AddedLevel = skillLevel,
									Exp = cooldown,
								};

								/// Thêm vào danh sách
								monsterInfo.Skills.Add(skillRef);
							}
							catch (Exception ex)
							{
								Console.WriteLine(ex.ToString());
								LogManager.WriteLog(LogTypes.Exception, ex.ToString());
							}
						}
					}

					/// Chuỗi mã hóa danh sách vòng sáng sử dụng
					string aurasString = xmlNode.Attribute("Auras").Value;
					/// Nếu có kỹ năng
					if (!string.IsNullOrEmpty(aurasString))
					{
						/// Duyệt danh sách kỹ năng
						foreach (string skillStr in aurasString.Split(';'))
						{
							string[] fields = skillStr.Split('_');
							try
							{
								int skillID = int.Parse(fields[0]);
								int skillLevel = int.Parse(fields[1]);

								/// Thông tin kỹ năng tương ứng
								SkillDataEx skillData = KSkill.GetSkillData(skillID);
								/// Nếu kỹ năng không tồn tại
								if (skillData == null)
								{
									throw new Exception(string.Format("Skill ID = {0} not found!", skillID));
								}

								/// Nếu cấp độ dưới 0
								if (skillLevel <= 0)
								{
									throw new Exception(string.Format("Skill ID = {0} level must be greater than 0", skillID));
								}

								/// Nếu không phải vòng sáng
								if (!skillData.IsArua)
								{
									throw new Exception(string.Format("Skill ID = {0} is not Aura!", skillID));
								}

								/// Kỹ năng theo cấp
								SkillLevelRef skillRef = new SkillLevelRef()
								{
									Data = skillData,
									AddedLevel = skillLevel,
								};

								/// Thêm vào danh sách
								monsterInfo.Auras.Add(skillRef);
							}
							catch (Exception ex)
							{
								Console.WriteLine(ex.ToString());
								LogManager.WriteLog(LogTypes.Exception, ex.ToString());
							}
						}
					}

					/// Trả về kết quả
					return monsterInfo;
				}
			}

			/// <summary>
			/// Thông tin cổng dịch chuyển
			/// </summary>
			public class TeleportInfo
			{
				/// <summary>
				/// Tên cổng
				/// </summary>
				public string Name { get; set; }

				/// <summary>
				/// Vị trí X
				/// </summary>
				public int PosX { get; set; }

				/// <summary>
				/// Vị trí Y
				/// </summary>
				public int PosY { get; set; }

				/// <summary>
				/// Vị trí bản đồ dịch tới (-1 nếu là bản đồ hiện tại)
				/// </summary>
				public int ToMapID { get; set; }

				/// <summary>
				/// Vị trí dịch tới X
				/// </summary>
				public int ToPosX { get; set; }

				/// <summary>
				/// Vị trí dịch tới Y
				/// </summary>
				public int ToPosY { get; set; }

				/// <summary>
				/// Tạo ra ngay lập tức cùng Step không
				/// </summary>
				public bool SpawnImmediate { get; set; }

				/// <summary>
				/// Chuyển đối tượng từ XMLNode
				/// </summary>
				/// <param name="xmlNode"></param>
				/// <returns></returns>
				public static TeleportInfo Parse(XElement xmlNode)
				{
					return new TeleportInfo()
					{
						Name = xmlNode.Attribute("Name").Value,
						PosX = int.Parse(xmlNode.Attribute("PosX").Value),
						PosY = int.Parse(xmlNode.Attribute("PosY").Value),
						ToMapID = int.Parse(xmlNode.Attribute("ToMapID").Value),
						ToPosX = int.Parse(xmlNode.Attribute("ToPosX").Value),
						ToPosY = int.Parse(xmlNode.Attribute("ToPosY").Value),
						SpawnImmediate = bool.Parse(xmlNode.Attribute("SpawnImmediate").Value),
					};
				}
			}

			/// <summary>
			/// Danh sách Step
			/// </summary>
			public List<StepInfo> Steps { get; set; }

			/// <summary>
			/// Chuyển đối tượng từ XMLNode
			/// </summary>
			/// <param name="xmlNode"></param>
			/// <returns></returns>
			public static GameRule Parse(XElement xmlNode)
			{
				GameRule gameRule = new GameRule()
				{
					Steps = new List<StepInfo>(),
				};

				foreach (XElement node in xmlNode.Elements("Step"))
				{
					gameRule.Steps.Add(StepInfo.Parse(node));
				}

				return gameRule;
			}
		}
		#endregion

		#region Core
		/// <summary>
		/// Thiết lập phụ bản
		/// </summary>
		public static EventConfig Config { get; set; }

		/// <summary>
		/// Thông tin bản đồ phụ bản
		/// </summary>
		public static MapInfo Map { get; set; }

		/// <summary>
		/// Danh sách vật phẩm trong phụ bản
		/// </summary>
		public static EventItem Items { get; set; }

		/// <summary>
		/// Quy tắc chơi
		/// </summary>
		public static GameRule Rule { get; set; }

		/// <summary>
		/// Khởi tạo phụ bản Vượt ải gia tộc
		/// </summary>
		public static void Init()
		{
			XElement xmlNode = Global.GetGameResXml("Config/KT_CopyScenes/FamilyFuBen.xml");
			/// Đọc dữ liệu thiết lập
			FamilyFuBen.Config = EventConfig.Parse(xmlNode.Element("Config"));
			/// Đọc dữ liệu bản đồ
			FamilyFuBen.Map = MapInfo.Parse(xmlNode.Element("Map"));
			/// Đọc dữ liệu vật phẩm
			FamilyFuBen.Items = EventItem.Parse(xmlNode.Element("Item"));
			/// Đọc dữ liệu quy tắc chơi
			FamilyFuBen.Rule = GameRule.Parse(xmlNode.Element("GameRule"));
		}
		#endregion
	}
}
