-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000000' bên dưới thành ID tương ứng
local BaiHuTang_3_Script = Scripts[400005]

-- ************************** --
-- Danh sách quái
local MonsterList = {
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 969, PosY = 1287, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 926, PosY = 1488, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 1117, PosY = 1603, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 1148, PosY = 1433, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 1146, PosY = 1170, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 1331, PosY = 1076, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 1361, PosY = 1296, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 1447, PosY = 1576, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 1497, PosY = 1820, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 1749, PosY = 1963, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 1800, PosY = 1765, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 1843, PosY = 1539, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 1897, PosY = 1299, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 1919, PosY = 1081, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 1919, PosY = 890, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 1919, PosY = 715, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 2241, PosY = 571, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 2269, PosY = 829, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 2449, PosY = 974, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 2561, PosY = 753, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 2568, PosY = 533, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 2965, PosY = 494, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 2953, PosY = 747, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 2985, PosY = 991, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 3274, PosY = 797, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 3414, PosY = 642, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 3590, PosY = 839, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 3412, PosY = 1139, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 3796, PosY = 932, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 3633, PosY = 1318, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 3977, PosY = 1137, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 4384, PosY = 1168, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 3687, PosY = 1546, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 3973, PosY = 1657, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 4352, PosY = 1486, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 3399, PosY = 1756, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 3507, PosY = 1965, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 3205, PosY = 2100, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 2907, PosY = 1917, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 2760, PosY = 2278, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 2569, PosY = 2009, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 2359, PosY = 2197, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 2380, PosY = 1920, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 1977, PosY = 2026, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 2146, PosY = 1823, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 1811, PosY = 1939, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 1873, PosY = 1703, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 2695, PosY = 1584, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 2861, PosY = 1519, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 3054, PosY = 1436, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 2878, PosY = 1328, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 2682, PosY = 1448, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 2498, PosY = 1561, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 2290, PosY = 1450, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 2480, PosY = 1346, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
	{IDs = {[103] = 2685, [108] = 2687, [112] = 3687}, Name = "", Title = "", PosX = 2685, PosY = 1234, BaseHP = 1000, HPIncreaseEachLevel = 300, AIType = 0, RespawnTick = 10000, ScriptID = -1},
}
-- Boss
local Boss = {
	IDs = {[103] = 2686, [108] = 2688, [112] = 3688},
	Name = "",
	Title = "",
	BaseHP = 100000,
	HPIncreaseEachLevel = 120000,
	AIType = 3,
	SpawnAfter = 120000,
	RandomPos = {
		{PosX = 2684, PosY = 1441},
		{PosX = 2019, PosY = 1810},
		{PosX = 1969, PosY = 943},
		{PosX = 3432, PosY = 974},
		{PosX = 3545, PosY = 1794},
	},
	ScriptID = -1,
}
-- Cổng dịch chuyển
local Teleport = {ID = 5340, Name = "Đại điện Bạch Hổ Đường",  PosX = 4240, PosY = 2116, Radius = 100, ScriptID = 400006}
-- Vị trí khi người chơi bị Kick ra khỏi bản đồ
local OutSceneInfo = {
	[232] = {
		SceneID = 225,			-- Đại điện Bạch Hổ Đường (sơ)
		PosX = 4964,
		PosY = 2357,
	},
	[240] = {
		SceneID = 233,			-- Đại điện Bạch Hổ Đường (cao)
		PosX = 4964,
		PosY = 2357,
	},
}
-- ************************** --
-- Danh sách thứ tự các tham biến lưu cục bộ trong hoạt động
local ActivityParamIndexes = {
	Step = 0,					-- Bước hoạt động
	Level = 1,					-- Cấp hoạt động
	LastNotifyTick = 2,			-- Thời gian thông báo thông tin chờ ra Boss lần trước
}
-- ************************** --
local NotifyActivityInfoToPlayersEveryTick = 5000	-- Thời gian thông báo với người chơi để cập nhật thông tin hoạt động
-- ************************** --
local EventID = Global_Events.BaiHuTang				-- ID sự kiện
-- ************************** --

-- ================================================================= --
-- ================================================================= --

-- ****************************************************** --
--	Chuẩn bị Bạch Hổ Đường
--		activity: Activity - Sự kiện hiện tại
--		scene: Scene - Bản đổ hiện tại
-- ****************************************************** --
function BaiHuTang_3_Script:Prepare(activity, scene)

	-- ************************** --
	self:ClearMonsters(scene)
	self:ClearNPCs(scene)
	self:ClearDynamicAreas(scene)
	self:ClearGrowPoints(scene)
	-- ************************** --

end

-- ****************************************************** --
--	Bắt đầu Bạch Hổ Đường
--		activity: Activity - Sự kiện hiện tại
--		scene: Scene - Bản đổ hiện tại
-- ****************************************************** --
function BaiHuTang_3_Script:OnStart(activity, scene)

	-- ************************** --
	local baseID = scene:GetID() * 10
	-- ************************** --
	activity:SetLocalVariable(baseID + ActivityParamIndexes.LastNotifyTick, 0)
	-- ************************** --
	local level = activity:GetLocalVariable(ActivityParamIndexes.Level)
	self:CreateMonsters(activity, scene, level)
	-- ************************** --
	activity:SetLocalVariable(baseID + ActivityParamIndexes.Step, 0)
	-- ************************** --
	-- Đánh dấu thời gian thông báo thông tin sự kiện
	activity:SetLocalVariable(baseID + ActivityParamIndexes.LastNotifyTick, activity:GetLifeTime())
	-- Cập nhật thông tin sự kiện
	self:UpdateEventTimeAndDetails(scene, Boss.SpawnAfter - activity:GetLifeTime(), string.format("Đợi <color=yellow>%s</color> xuất hiện.", "Hộ Đồ Sứ"))
	-- ************************** --

end

-- ****************************************************** --
--	Tick Bạch Hổ Đường
--		activity: Activity - Sự kiện hiện tại
--		scene: Scene - Bản đổ hiện tại
-- ****************************************************** --
function BaiHuTang_3_Script:OnTick(activity, scene)

	-- ************************** --
	local baseID = scene:GetID() * 10
	-- ************************** --
	local nStep = activity:GetLocalVariable(baseID + ActivityParamIndexes.Step)
	-- ************************** --
	if nStep == 0 then
		-- Nếu đã đến thời gian ra Boss
		if activity:GetLifeTime() >= Boss.SpawnAfter then
			local level = activity:GetLocalVariable(ActivityParamIndexes.Level)
			activity:SetLocalVariable(baseID + ActivityParamIndexes.Step, 100)
			self:CreateBoss(activity, scene, level)
			
			-- Đánh dấu thời gian thông báo thông tin sự kiện
			activity:SetLocalVariable(baseID + ActivityParamIndexes.LastNotifyTick, activity:GetLifeTime())
			-- Cập nhật thông tin sự kiện
			self:UpdateEventTimeAndDetails(scene, activity:GetDuration() - activity:GetLifeTime(), string.format("Đánh bại <color=yellow>%s</color>.", "Hộ Đồ Sứ"))
		else
			-- Nếu đã đến thời gian thông báo thông tin sự kiện
			if activity:GetLifeTime() - activity:GetLocalVariable(baseID + ActivityParamIndexes.LastNotifyTick) >= NotifyActivityInfoToPlayersEveryTick then
				-- Đánh dấu thời gian thông báo thông tin sự kiện
				activity:SetLocalVariable(baseID + ActivityParamIndexes.LastNotifyTick, activity:GetLifeTime())
				-- Cập nhật thông tin sự kiện
				self:UpdateEventTimeAndDetails(scene, Boss.SpawnAfter - activity:GetLifeTime(), string.format("Đợi <color=yellow>%s</color> xuất hiện.", "Hộ Đồ Sứ"))
			end
		end
	elseif nStep == 1 then
		-- Nếu Boss chưa chết
		if self:IsBossAlive(scene) == true then
			-- Nếu đã đến thời gian thông báo thông tin sự kiện
			if activity:GetLifeTime() - activity:GetLocalVariable(baseID + ActivityParamIndexes.LastNotifyTick) >= NotifyActivityInfoToPlayersEveryTick then
				-- Đánh dấu thời gian thông báo thông tin sự kiện
				activity:SetLocalVariable(baseID + ActivityParamIndexes.LastNotifyTick, activity:GetLifeTime())
				-- Cập nhật thông tin sự kiện
				self:UpdateEventTimeAndDetails(scene, activity:GetDuration() - activity:GetLifeTime(), string.format("Đánh bại <color=yellow>%s</color>.", "Hộ Đồ Sứ"))
			end
			
			return
		end
		self:TipAllPlayers(scene, "Hoàn tất Bạch Hổ Đường, có thể trở lại Đại Điện rồi!")
		self:CreateTeleport(activity, scene)
		activity:SetLocalVariable(baseID + ActivityParamIndexes.Step, 100)
			
		-- Đánh dấu thời gian thông báo thông tin sự kiện
		activity:SetLocalVariable(baseID + ActivityParamIndexes.LastNotifyTick, activity:GetLifeTime())
		-- Cập nhật thông tin sự kiện
		self:UpdateEventTimeAndDetails(scene, activity:GetDuration() - activity:GetLifeTime(), "Hoàn tất <color=green>Bạch Hổ Đường</color>.")
	end
	-- ************************** --

end

-- ****************************************************** --
--	Kết thúc Bạch Hổ Đường
--		activity: Activity - Sự kiện hiện tại
--		scene: Scene - Bản đổ hiện tại
-- ****************************************************** --
function BaiHuTang_3_Script:OnClose(activity, scene)

	-- ************************** --
	self:ClearMonsters(scene)
	self:ClearNPCs(scene)
	self:ClearDynamicAreas(scene)
	self:ClearGrowPoints(scene)
	-- ************************** --
	--self:KickOutAllPlayers(scene)
	-- ************************** --

end

-- ****************************************************** --
--	Sự kiện khi người chơi vào bản đồ
--		activity: Activity - Sự kiện hiện tại
--		player: Player - Người chơi tương ứng
-- ****************************************************** --
function BaiHuTang_3_Script:OnEnterScene(scene, player)

	-- ************************** --
	-- Nếu không phải thời gian hoạt động
	if Global_Params.BaiHuTang_Stage ~= 4 then
		GUI.ShowNotification(player, "Hiện không phải thời gian Bạch Hổ Đường!")
		self:KickOutPlayer(scene, player)
		return
	end
	-- ************************** --
	EventManager.OpenEventBroadboard(player, EventID)
	-- ************************** --
	player:SetPKMode(Global_PKMode.Guild)
	-- ************************** --

end

-- ****************************************************** --
--	Sự kiện khi người chơi rời bản đồ
--		activity: Activity - Sự kiện hiện tại
--		player: Player - Người chơi tương ứng
-- ****************************************************** --
function BaiHuTang_3_Script:OnLeave(scene, player)

	-- ************************** --
	EventManager.CloseEventBroadboard(player, EventID)
	-- ************************** --

end

-- ****************************************************** --
--	Sự kiện khi người chơi giết quái
--		activity: Activity - Sự kiện hiện tại
--		player: Player - Người chơi tương ứng
--		deadObj: {Player, Monster} - Đối tượng bị giết
-- ****************************************************** --
function BaiHuTang_3_Script:OnKillObject(scene, player, deadObj)

	-- ************************** --
	if deadObj:GetObjectType() == Global_ObjectTypes.OT_MONSTER and deadObj:GetTag() == "Boss" then
		self:TipAllPlayers(scene, string.format("%s đã đánh bại %s!", player:GetName(), deadObj:GetName()))
	end
	-- ************************** --

end

-- ================================================================= --
-- ================================================================= --

-- ****************************************************** --
--	Xóa toàn bộ quái khỏi phụ bản
-- ****************************************************** --
function BaiHuTang_3_Script:ClearMonsters(scene)

	-- ************************** --
	for k, monster in ipairs(scene:GetMonsters()) do
		EventManager.DeleteMonster(monster)
	end
	-- ************************** --

end

-- ****************************************************** --
--	Xóa toàn bộ NPC khỏi phụ bản
-- ****************************************************** --
function BaiHuTang_3_Script:ClearNPCs(scene)

	-- ************************** --
	for k, npc in ipairs(scene:GetNPCs()) do
		EventManager.DeleteNPC(npc)
	end
	-- ************************** --

end

-- ****************************************************** --
--	Xóa toàn bộ khu vực động khỏi phụ bản
-- ****************************************************** --
function BaiHuTang_3_Script:ClearDynamicAreas(scene)

	-- ************************** --
	for k, dynArea in ipairs(scene:GetDynamicAreas()) do
		EventManager.DeleteDynamicArea(dynArea)
	end
	-- ************************** --

end

-- ****************************************************** --
--	Xóa toàn bộ điểm thu thập khỏi phụ bản
-- ****************************************************** --
function BaiHuTang_3_Script:ClearGrowPoints(scene)

	-- ************************** --
	for k, growPoint in ipairs(scene:GetGrowPoints()) do
		EventManager.DeleteGrowPoint(growPoint)
	end
	-- ************************** --

end

-- ================================================================= --
-- ================================================================= --

-- ****************************************************** --
-- Kiểm tra Boss có còn sống không
-- ****************************************************** --
function BaiHuTang_3_Script:IsBossAlive(scene)

	-- ************************** --
	for k, monster in ipairs(scene:GetMonsters()) do
		if monster:GetTag() == "Boss" then
			return true
		end
	end
	-- ************************** --
	return false
	-- ************************** --
	
end

-- ****************************************************** --
-- Tạo quái
-- ****************************************************** --
function BaiHuTang_3_Script:CreateMonsters(activity, scene, level)

	-- ************************** --
	for idx, monsterInfo in ipairs(MonsterList) do
		local monsterBuilder = EventManager.CreateMonsterBuilder(scene)
		monsterBuilder:SetResID(monsterInfo.IDs[activity:GetID()])
		monsterBuilder:SetPos(monsterInfo.PosX, monsterInfo.PosY)
		monsterBuilder:SetName(monsterInfo.Name)
		monsterBuilder:SetTitle(monsterInfo.Title)
		monsterBuilder:SetLevel(level)
		monsterBuilder:SetMaxHP(monsterInfo.BaseHP + level * monsterInfo.HPIncreaseEachLevel)
		monsterBuilder:SetDirection(-1)
		monsterBuilder:SetSeries(0)
		monsterBuilder:SetAIType(monsterInfo.AIType)
		monsterBuilder:SetScriptID(monsterInfo.ScriptID)
		monsterBuilder:SetTag("")
		monsterBuilder:SetRespawnTick(10000)
		monsterBuilder:SetRespawnCondition(function()
			if activity:IsAlive() == true then
				return true
			else
				return false
			end
		end)
		monsterBuilder:Build()
	end
	-- ************************** --

end

-- ****************************************************** --
-- Tạo Boss
-- ****************************************************** --
function BaiHuTang_3_Script:CreateBoss(activity, scene, level)

	-- ************************** --
	--self:TipAllPlayers(scene, "CREATE BOSS")
	-- ************************** --
	local randPos = Boss.RandomPos[math.random(#Boss.RandomPos)]
	-- ************************** --
	local monsterBuilder = EventManager.CreateMonsterBuilder(scene)
	monsterBuilder:SetResID(Boss.IDs[activity:GetID()])
	monsterBuilder:SetPos(randPos.PosX, randPos.PosY)
	monsterBuilder:SetName(Boss.Name)
	monsterBuilder:SetTitle(Boss.Title)
	monsterBuilder:SetLevel(level)
	monsterBuilder:SetMaxHP(Boss.BaseHP + level * Boss.HPIncreaseEachLevel)
	monsterBuilder:SetDirection(-1)
	monsterBuilder:SetSeries(0)
	monsterBuilder:SetAIType(Boss.AIType)
	monsterBuilder:SetScriptID(Boss.ScriptID)
	monsterBuilder:SetTag("Boss")
	monsterBuilder:SetRespawnTick(-1)
	monsterBuilder:SetDone(function(monster)
		self:TipAllPlayers(scene, string.format("%s đã xuất hiện!", monster:GetName()))
		activity:SetLocalVariable(scene:GetID() * 10 + ActivityParamIndexes.Step, 1)
	end)
	monsterBuilder:Build()
	-- ************************** --
	
end

-- ****************************************************** --
-- Tạo cổng dịch chuyển
-- ****************************************************** --
function BaiHuTang_3_Script:CreateTeleport(activity, scene)

	-- ************************** --
	local dynAreaBuilder = EventManager.CreateDynamicAreaBuilder(scene)
	dynAreaBuilder:SetResID(Teleport.ID)
	dynAreaBuilder:SetPos(Teleport.PosX, Teleport.PosY)
	dynAreaBuilder:SetName(Teleport.Name)
	dynAreaBuilder:SetRadius(Teleport.Radius)
	dynAreaBuilder:SetScriptID(Teleport.ScriptID)
	dynAreaBuilder:SetTag(string.format("%d_%d_%d", OutSceneInfo[scene:GetID()].SceneID, OutSceneInfo[scene:GetID()].PosX, OutSceneInfo[scene:GetID()].PosY))
	dynAreaBuilder:Build()
	-- ************************** --
	
end

-- ================================================================= --
-- ================================================================= --

-- ****************************************************** --
-- Cập nhật lại thông tin sự kiện cho toàn bộ người chơi
-- ****************************************************** --
function BaiHuTang_3_Script:UpdateEventTimeAndDetails(scene, timeLeftTick, detailStrings)

	-- ************************** --
	for idx, player in ipairs(scene:GetPlayers()) do
		EventManager.UpdateEventDetails(player, "Bạch Hổ Đường - Tầng 3", timeLeftTick, detailStrings)
	end
	-- ************************** --

end

-- ****************************************************** --
-- Thiết lập trạng thái PK của người chơi
-- ****************************************************** --
function BaiHuTang_3_Script:ChangePlayersPKMode(scene)

	-- ************************** --
	for idx, player in ipairs(scene:GetPlayers()) do
		player:SetPKMode(Global_PKMode.Guild)
	end
	-- ************************** --

end

-- ****************************************************** --
-- Đưa người chơi ra ngoài
-- ****************************************************** --
function BaiHuTang_3_Script:KickOutPlayer(scene, player)

	-- ************************** --
	player:ChangeScene(OutSceneInfo[scene:GetID()].SceneID, OutSceneInfo[scene:GetID()].PosX, OutSceneInfo[scene:GetID()].PosY)
	-- ************************** --

end

-- ****************************************************** --
-- Đưa tất cả người chơi ra ngoài
-- ****************************************************** --
function BaiHuTang_3_Script:KickOutAllPlayers(scene)

	-- ************************** --
	for idx, player in ipairs(scene:GetPlayers()) do
		self:KickOutPlayer(scene, player)
	end
	-- ************************** --

end

-- ****************************************************** --
-- Thông báo tới tất cả người chơi
-- ****************************************************** --
function BaiHuTang_3_Script:TipAllPlayers(scene, text)

	-- ************************** --
	for idx, player in ipairs(scene:GetPlayers()) do
		GUI.ShowNotification(player, text)
	end
	-- ************************** --

end