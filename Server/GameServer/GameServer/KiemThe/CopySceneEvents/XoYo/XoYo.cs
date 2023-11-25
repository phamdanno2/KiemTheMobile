using GameServer.KiemThe.Entities;
using GameServer.Logic;
using Server.Data;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace GameServer.KiemThe.CopySceneEvents.XiaoYaoGu
{
	/// <summary>
	/// Phụ bản Vượt ải Tiêu Dao Cốc
	/// </summary>
	public static class XoYo
	{
		#region Define
		/// <summary>
		/// Loại ải
		/// </summary>
		public enum XoYoEventType
		{
			/// <summary>
			/// Giết quái và Boss
			/// </summary>
			KillMonster,
			/// <summary>
			/// Tránh bẫy và giết quái và Boss
			/// </summary>
			HideTrapAndKillMonster,
			/// <summary>
			/// Mở cơ quan và giết quái
			/// </summary>
			UnlockTriggerAndKillMonster
		}

		/// <summary>
		/// Thưởng theo thứ hạng trong tháng
		/// </summary>
		public class MonthlyStorageInfo
		{
			/// <summary>
			/// Thông tin thứ hạng
			/// </summary>
			public class RankInfo
			{
				/// <summary>
				/// Từ thứ hạng
				/// </summary>
				public int FromRank { get; set; }

				/// <summary>
				/// Đến thứ hạng
				/// </summary>
				public int ToRank { get; set; }

				/// <summary>
				/// Danh sách quà thưởng
				/// </summary>
				public List<GoodsData> Awards { get; set; }

				/// <summary>
				/// Chuyển đối tượng từ XMLNode
				/// </summary>
				/// <param name="xmlNode"></param>
				/// <returns></returns>
				public static RankInfo Parse(XElement xmlNode)
				{
					RankInfo rankInfo = new RankInfo()
					{
						FromRank = int.Parse(xmlNode.Attribute("FromRank").Value),
						ToRank = int.Parse(xmlNode.Attribute("ToRank").Value),
						Awards = new List<GoodsData>(),
					};

					/// Duyệt danh sách quà thưởng
					foreach (string pair in xmlNode.Attribute("Awards").Value.Split(';'))
					{
						string[] fields = pair.Split('_');
						int itemID = int.Parse(fields[0]);
						int quantity = int.Parse(fields[1]);
						int binding = int.Parse(fields[2]);

						/// Tạo vật phẩm tương ứng
						GoodsData itemGD = new GoodsData()
						{
							GoodsID = itemID,
							GCount = quantity,
							Binding = binding,
						};
						/// Thêm vào danh sách
						rankInfo.Awards.Add(itemGD);
					}

					return rankInfo;
				}
			}

			/// <summary>
			/// Danh sách quà thưởng theo thứ hạng
			/// </summary>
			public List<RankInfo> AwardsByRange { get; set; }

			/// <summary>
			/// Chuyển đối tượng từ XMLNode
			/// </summary>
			/// <param name="xmlNode"></param>
			/// <returns></returns>
			public static MonthlyStorageInfo Parse(XElement xmlNode)
			{
				MonthlyStorageInfo storageInfo = new MonthlyStorageInfo()
				{
					AwardsByRange = new List<RankInfo>(),
				};

				/// Duyệt danh sách xếp hạng
				foreach (XElement node in xmlNode.Elements("RankInfo"))
				{
					storageInfo.AwardsByRange.Add(RankInfo.Parse(node));
				}

				return storageInfo;
			}
		}

		/// <summary>
		/// Danh vọng đạt đượt khi tham gia vượt ải Tiêu Dao Cốc
		/// </summary>
		public class ReputeInfo
		{
			/// <summary>
			/// Độ khó của ải
			/// </summary>
			public int Difficulty { get; set; }

			/// <summary>
			/// Nếu vượt qua 1 vòng
			/// </summary>
			public int OneRound { get; set; }

			/// <summary>
			/// Nếu vượt qua 2 vòng
			/// </summary>
			public int TwoRound { get; set; }

			/// <summary>
			/// Nếu vượt qua 3 vòng
			/// </summary>
			public int ThreeRound { get; set; }

			/// <summary>
			/// Nếu vượt qua 4 vòng
			/// </summary>
			public int FourRound { get; set; }

			/// <summary>
			/// Nếu vượt qua 5 vòng
			/// </summary>
			public int FiveRound { get; set; }

			/// <summary>
			/// Nếu thua vòng
			/// </summary>
			public int LossRound { get; set; }

			/// <summary>
			/// Chuyển đối tượng từ XMLNode
			/// </summary>
			/// <param name="xmlNode"></param>
			/// <returns></returns>
			public static ReputeInfo Parse(XElement xmlNode)
			{
				return new ReputeInfo()
				{
					Difficulty = int.Parse(xmlNode.Attribute("Difficulty").Value),
					OneRound = int.Parse(xmlNode.Attribute("OneRound").Value),
					TwoRound = int.Parse(xmlNode.Attribute("TwoRound").Value),
					ThreeRound = int.Parse(xmlNode.Attribute("ThreeRound").Value),
					FourRound = int.Parse(xmlNode.Attribute("FourRound").Value),
					FiveRound = int.Parse(xmlNode.Attribute("FiveRound").Value),
					LossRound = int.Parse(xmlNode.Attribute("LossRound").Value),
				};
			}
		}

		/// <summary>
		/// Thông tin quái
		/// </summary>
		public class MonsterInfo
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
			/// Thời gian xuất hiện
			/// </summary>
			public int SpawnAfter { get; set; }

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
			public static MonsterInfo Parse(XElement xmlNode)
			{
				MonsterInfo monsterInfo = new MonsterInfo()
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
					SpawnAfter = int.Parse(xmlNode.Attribute("SpawnAfter").Value),
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
			/// Thời gian xuất hiện
			/// </summary>
			public int SpawnAfter { get; set; }

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
					SpawnAfter = int.Parse(xmlNode.Attribute("SpawnAfter").Value),
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
		/// Thông tin bẫy
		/// </summary>
		public class TrapInfo
		{
			/// <summary>
			/// ID Res
			/// </summary>
			public int ID { get; set; }

			/// <summary>
			/// Vị trí X
			/// </summary>
			public int PosX { get; set; }

			/// <summary>
			/// Vị trí Y
			/// </summary>
			public int PosY { get; set; }

			/// <summary>
			/// Thời gian xuất hiện
			/// </summary>
			public int SpawnAfter { get; set; }

			/// <summary>
			/// Thời gian tự hủy
			/// </summary>
			public int DestroyAfter { get; set; }

			/// <summary>
			/// Danh sách vòng sáng sẽ sử dụng
			/// </summary>
			public List<SkillLevelRef> Auras { get; set; }

			/// <summary>
			/// Chuyển đối tượng từ XMLNode
			/// </summary>
			/// <param name="xmlNode"></param>
			/// <returns></returns>
			public static TrapInfo Parse(XElement xmlNode)
			{
				TrapInfo trapInfo = new TrapInfo()
				{
					ID = int.Parse(xmlNode.Attribute("ID").Value),
					PosX = int.Parse(xmlNode.Attribute("PosX").Value),
					PosY = int.Parse(xmlNode.Attribute("PosY").Value),
					SpawnAfter = int.Parse(xmlNode.Attribute("SpawnAfter").Value),
					DestroyAfter = int.Parse(xmlNode.Attribute("DestroyAfter").Value),
					Auras = new List<SkillLevelRef>(),
				};

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
							trapInfo.Auras.Add(skillRef);
						}
						catch (Exception ex)
						{
							Console.WriteLine(ex.ToString());
							LogManager.WriteLog(LogTypes.Exception, ex.ToString());
						}
					}
				}

				/// Trả về kết quả
				return trapInfo;
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
		/// Thông tin chi tiết ải
		/// </summary>
		public class MapInfo
		{
			/// <summary>
			/// ID bản đồ
			/// </summary>
			public int ID { get; set; }

			/// <summary>
			/// Loại ải
			/// </summary>
			public XoYoEventType Type { get; set; }

			/// <summary>
			/// Thời gian vượt ải
			/// </summary>
			public int Duration { get; set; }

			/// <summary>
			/// Tọa độ vào X
			/// </summary>
			public int EnterPosX { get; set; }

			/// <summary>
			/// Tọa độ vào Y
			/// </summary>
			public int EnterPosY { get; set; }

			/// <summary>
			/// Độ khó của ải
			/// </summary>
			public int Difficulty { get; set; }

			/// <summary>
			/// Danh sách quái
			/// </summary>
			public List<MonsterInfo> Monsters { get; set; }

			/// <summary>
			/// Danh sách Boss
			/// </summary>
			public List<BossInfo> Bosses { get; set; }

			/// <summary>
			/// Danh sách bẫy
			/// </summary>
			public List<TrapInfo> Traps { get; set; }

			/// <summary>
			/// Danh sách cơ quan
			/// </summary>
			public List<TriggerInfo> Triggers { get; set; }

			/// <summary>
			/// Danh sách quái bảo vệ cơ quan
			/// </summary>
			public List<TriggerGuardianInfo> Guardians { get; set; }

			/// <summary>
			/// Chuyển đối tượng từ XMLNode
			/// </summary>
			/// <param name="xmlNode"></param>
			/// <returns></returns>
			public static MapInfo Parse(XElement xmlNode)
			{
				MapInfo mapInfo = new MapInfo()
				{
					ID = int.Parse(xmlNode.Attribute("ID").Value),
					Type = (XoYoEventType) Enum.Parse(typeof(XoYoEventType), xmlNode.Attribute("Type").Value),
					Duration = int.Parse(xmlNode.Attribute("Duration").Value),
					EnterPosX = int.Parse(xmlNode.Attribute("EnterPosX").Value),
					EnterPosY = int.Parse(xmlNode.Attribute("EnterPosY").Value),
					Difficulty = int.Parse(xmlNode.Attribute("Difficulty").Value),
					Monsters = new List<MonsterInfo>(),
					Bosses = new List<BossInfo>(),
					Traps = new List<TrapInfo>(),
					Triggers = new List<TriggerInfo>(),
					Guardians = new List<TriggerGuardianInfo>(),
				};

				/// Duyệt danh sách quái
				foreach (XElement node in xmlNode.Elements("Monster"))
				{
					mapInfo.Monsters.Add(MonsterInfo.Parse(node));
				}
				/// Duyệt danh sách Boss
				foreach (XElement node in xmlNode.Elements("Boss"))
				{
					mapInfo.Bosses.Add(BossInfo.Parse(node));
				}
				/// Duyệt danh sách bẫy
				foreach (XElement node in xmlNode.Elements("Trap"))
				{
					mapInfo.Traps.Add(TrapInfo.Parse(node));
				}
				/// Duyệt danh sách bẫy
				foreach (XElement node in xmlNode.Elements("Trigger"))
				{
					mapInfo.Triggers.Add(TriggerInfo.Parse(node));
				}
				/// Duyệt danh sách bẫy
				foreach (XElement node in xmlNode.Elements("Guardian"))
				{
					mapInfo.Guardians.Add(TriggerGuardianInfo.Parse(node));
				}

				/// Trả về kết quả
				return mapInfo;
			}
		}

		/// <summary>
		/// Thông tin bản đồ chờ
		/// </summary>
		public class WaitingMapInfo
		{
			/// <summary>
			/// ID bản đồ
			/// </summary>
			public int ID { get; set; }

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
			public static WaitingMapInfo Parse(XElement xmlNode)
			{
				return new WaitingMapInfo()
				{
					ID = int.Parse(xmlNode.Attribute("ID").Value),
					EnterPosX = int.Parse(xmlNode.Attribute("EnterPosX").Value),
					EnterPosY = int.Parse(xmlNode.Attribute("EnterPosY").Value),
				};
			}
		}

		/// <summary>
		/// Thông tin bản đồ báo danh
		/// </summary>
		public class RegisterMapInfo
		{
			/// <summary>
			/// ID bản đồ
			/// </summary>
			public int ID { get; set; }

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
			public static RegisterMapInfo Parse(XElement xmlNode)
			{
				return new RegisterMapInfo()
				{
					ID = int.Parse(xmlNode.Attribute("ID").Value),
					EnterPosX = int.Parse(xmlNode.Attribute("EnterPosX").Value),
					EnterPosY = int.Parse(xmlNode.Attribute("EnterPosY").Value),
				};
			}
		}
		#endregion

		#region Init
		/// <summary>
		/// Danh sách danh vọng đạt được theo độ khó
		/// </summary>
		public static List<ReputeInfo> ReputeInfos { get; set; } = new List<ReputeInfo>();

		/// <summary>
		/// Danh sách quà thưởng theo xếp hạng tháng
		/// </summary>
		public static MonthlyStorageInfo MonthlyStorageInfos { get; set; }

		/// <summary>
		/// Danh sách các ải Tiêu Dao Cốc
		/// </summary>
		public static List<MapInfo> Events { get; private set; } = new List<MapInfo>();

		/// <summary>
		/// Thông tin bản đồ chờ
		/// </summary>
		public static WaitingMapInfo WaitingMap { get; private set; }

		/// <summary>
		/// Thông tin bản đồ báo danh
		/// </summary>
		public static RegisterMapInfo RegisterMap { get; private set; }

		/// <summary>
		/// ID sự kiện
		/// </summary>
		public static int EventID { get; private set; }

		/// <summary>
		/// Số thành viên đội tối thiểu yêu cầu
		/// </summary>
		public static int LimitMember { get; private set; }

		/// <summary>
		/// Cấp độ tối thiểu yêu cầu
		/// </summary>
		public static int RequireLevel { get; private set; }

		/// <summary>
		/// Số vòng tham gia mỗi ngày tối đa
		/// </summary>
		public static int LimitRoundEachDay { get; private set; }

		/// <summary>
		/// Tổng số ải tối đa mỗi vòng
		/// </summary>
		public static int MaxStage { get; private set; }

		/// <summary>
		/// Thời gian đợi ải bắt đầu
		/// </summary>
		public const int BeginWaitTime = 30000;

		/// <summary>
		/// Thời gian tự chuyển khi ải kết thúc
		/// </summary>
		public const int FinishWaitTime = 60000;

		/// <summary>
		/// Khởi tạo
		/// </summary>
		public static void Init()
		{
			XElement xmlNode = Global.GetGameResXml("Config/KT_CopyScenes/XoYo.xml");
			/// Đọc danh sách quà thưởng theo tháng
			XoYo.MonthlyStorageInfos = MonthlyStorageInfo.Parse(xmlNode.Element("MontlyStorageAward"));
			/// Duyệt danh sách danh vọng
			foreach (XElement node in xmlNode.Element("Repute").Elements("Stage"))
			{
				XoYo.ReputeInfos.Add(ReputeInfo.Parse(node));
			}
			/// Duyệt danh sách ải
			foreach (XElement node in xmlNode.Elements("Map"))
			{
				XoYo.Events.Add(MapInfo.Parse(node));
			}
			XoYo.WaitingMap = WaitingMapInfo.Parse(xmlNode.Element("WaitingMap"));
			XoYo.RegisterMap = RegisterMapInfo.Parse(xmlNode.Element("RegisterMap"));
			XoYo.EventID = int.Parse(xmlNode.Attribute("EventID").Value);
			XoYo.LimitMember = int.Parse(xmlNode.Attribute("LimitMember").Value);
			XoYo.RequireLevel = int.Parse(xmlNode.Attribute("RequireLevel").Value);
			XoYo.LimitRoundEachDay = int.Parse(xmlNode.Attribute("LimitRoundEachDay").Value);
			XoYo.MaxStage = int.Parse(xmlNode.Attribute("MaxStage").Value);
		}
		#endregion
	}
}
