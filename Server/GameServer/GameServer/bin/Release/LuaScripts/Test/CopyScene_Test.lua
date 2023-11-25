-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000000' bên dưới thành ID tương ứng
local CopyScene_Test = Scripts[400000]

-- ****************************************************** --
--	Hàm này được gọi khi phụ bản bắt đầu
--		copyScene: CopyScene - Phụ bản hiện tại
-- ****************************************************** --
function CopyScene_Test:OnInit(copyScene)

	-- ************************** --
	local copySceneInitTick = System.GetCurrentTimeMilis()
	-- ************************** --
	copyScene:SetLocalVariable(0, copySceneInitTick)
	copyScene:SetLocalVariable(1, 0)
	-- ************************** --
	CopyScene_Test:NotificationAllPlayers(copyScene, "OnInit => Tick = " .. copySceneInitTick)
	-- ************************** --
	--local ret = EventManager.CreateMonster(copyScene, 63, 2698, 3102, "Quái phụ bản", "Quái test ở phụ bản", 0, 2, 1, -1)
	-- ************************** --

end

-- ****************************************************** --
--	Hàm này gọi liên tục mỗi khoảng chừng nào phụ bản còn tồn tại
--		copyScene: CopyScene - Phụ bản hiện tại
-- ****************************************************** --
function CopyScene_Test:OnTick(copyScene)

	-- ************************** --
	local lastTick = copyScene:GetLocalVariable(0)
	local currentTick = System.GetCurrentTimeMilis()
	-- ************************** --
	local tickTime = currentTick - lastTick
	-- ************************** --
	CopyScene_Test:NotificationAllPlayers(copyScene, "OnTick => " .. tickTime)
	-- ************************** --
	if tickTime >= 5000 and copyScene:GetLocalVariable(1) == 0 then
		copyScene:SetLocalVariable(1, 1)
		local monsterBuilder = EventManager.CreateMonsterBuilder(copyScene)
		monsterBuilder:SetResID(63)
		monsterBuilder:SetPos(2674, 1513)
		monsterBuilder:SetName("Quái phụ bản")
		monsterBuilder:SetTitle("Quái test ở phụ bản")
		monsterBuilder:SetDirection(-1)
		monsterBuilder:SetSeries(2)
		monsterBuilder:SetAIType(1)
		monsterBuilder:SetScriptID(-1)
		monsterBuilder:SetTag("")
		monsterBuilder:SetRespawnTick(-1)
		monsterBuilder:Build()
		if ret == true then
			CopyScene_Test:NotificationAllPlayers(copyScene, "Create Monster success")
		else
			CopyScene_Test:NotificationAllPlayers(copyScene, "Create Monster faild")
		end
	elseif tickTime >= 10000 and copyScene:GetLocalVariable(1) == 1 then
		copyScene:SetLocalVariable(1, 2)
		local npcBuilder = EventManager.CreateNPCBuilder(copyScene)
		npcBuilder:SetResID(82)
		npcBuilder:SetPos(2671, 1292)
		npcBuilder:SetName("NPC Phụ bản")
		npcBuilder:SetTitle("Chức năng Test phụ bản")
		npcBuilder:SetDirection(-1)
		npcBuilder:SetScriptID(0)
		npcBuilder:SetTag("")
		local ret = npcBuilder:Build()
		if ret == true then
			CopyScene_Test:NotificationAllPlayers(copyScene, "Create NPC success")
		else
			CopyScene_Test:NotificationAllPlayers(copyScene, "Create NPC faild")
		end
	elseif tickTime >= 15000 and copyScene:GetLocalVariable(1) == 2 then
		copyScene:SetLocalVariable(1, 3)
		local monsters = copyScene:GetMonsters()
		for idx, monster in pairs(monsters) do
			local ret = EventManager.DeleteMonster(monster)
			if ret == true then
				CopyScene_Test:NotificationAllPlayers(copyScene, "Delete all Monsters success")
			end
		end
	elseif tickTime >= 20000 and copyScene:GetLocalVariable(1) == 3 then
		copyScene:SetLocalVariable(1, 4)
		local npcs = copyScene:GetNPCs()
		for idx, npc in pairs(npcs) do
			local ret = EventManager.DeleteNPC(npc)
			if ret == true then
				CopyScene_Test:NotificationAllPlayers(copyScene, "Delete all NPCs success")
			end
		end
	end
	-- ************************** --

end

-- ****************************************************** --
--	Hàm này gọi khi phụ bản bị đóng
--		copyScene: CopyScene - Phụ bản hiện tại
-- ****************************************************** --
function CopyScene_Test:OnClose(copyScene)

	-- ************************** --
	System.WriteToConsole("CopyScene_Test:OnClose")
	-- ************************** --

end

-- ****************************************************** --
--	Hàm này gọi khi người chơi giết đối tượng nào khác trong phụ bản
--		copyScene: CopyScene - Phụ bản hiện tại
--		player: Player - Đối tượng người chơi
--		obj: {Monster, Player} -  Đối tượng bị giết, có thể là quái hoặc người chơi khác
-- ****************************************************** --
function CopyScene_Test:OnKillObject(copyScene, player, obj)

	-- ************************** --
	CopyScene_Test:NotificationAllPlayers(copyScene, "OnKillObject")
	-- ************************** --

end

-- ****************************************************** --
--	Hàm này gọi khi người chơi bị chết bởi đối tượng khác trong phụ bản
--		copyScene: CopyScene - Phụ bản hiện tại
--		player: Player - Đối tượng người chơi
--		obj: {Monster, Player} -  Đối tượng giết, có thể là quái hoặc người chơi khác
-- ****************************************************** --
function CopyScene_Test:OnPlayerDie(copyScene, player, obj)

	-- ************************** --
	CopyScene_Test:NotificationAllPlayers(copyScene, "OnPlayerDie")
	-- ************************** --

end

-- ****************************************************** --
--	Hàm này gọi khi người chơi hồi sinh tại phụ bản
--		copyScene: CopyScene - Phụ bản hiện tại
--		player: Player - Đối tượng người chơi
-- ****************************************************** --
function CopyScene_Test:OnPlayerRelive(copyScene, player)

	-- ************************** --
	CopyScene_Test:NotificationAllPlayers(copyScene, "OnPlayerRelive")
	-- ************************** --

end

-- ****************************************************** --
--	Hàm này gọi khi người chơi bị mất kết nối
--		copyScene: CopyScene - Phụ bản hiện tại
--		player: Player - Đối tượng người chơi
-- ****************************************************** --
function CopyScene_Test:OnPlayerDisconnected(copyScene, player)

	-- ************************** --
	CopyScene_Test:NotificationAllPlayers(copyScene, "OnPlayerDisconnected")
	System.WriteToConsole("CopyScene_Test:OnPlayerDisconnected => " .. player:GetName())
	-- ************************** --

end

-- ****************************************************** --
--	Hàm này gọi khi người chơi kết nối lại
--		copyScene: CopyScene - Phụ bản hiện tại
--		player: Player - Đối tượng người chơi
-- ****************************************************** --
function CopyScene_Test:OnPlayerReconnected(copyScene, player)

	-- ************************** --
	CopyScene_Test:NotificationAllPlayers(copyScene, "OnPlayerReconnected")
	System.WriteToConsole("CopyScene_Test:OnPlayerReconnected => " .. player:GetName())
	-- ************************** --

end

-- ****************************************************** --
--	Hàm này gọi khi người chơi rời khỏi phụ bản
--		copyScene: CopyScene - Phụ bản hiện tại
--		player: Player - Đối tượng người chơi
-- ****************************************************** --
function CopyScene_Test:OnPlayerLeave(copyScene, player)

	-- ************************** --
	CopyScene_Test:NotificationAllPlayers(copyScene, "OnPlayerLeave")
	System.WriteToConsole("CopyScene_Test:OnPlayerLeave => " .. player:GetName())
	-- ************************** --

end




-- ======================================================== --
-- ======================================================== --
function CopyScene_Test:NotificationAllPlayers(copyScene, text)

	-- ************************** --
	for idx, player in pairs(copyScene:GetPlayers()) do
		GUI.ShowNotification(player, text)
	end
	-- ************************** --

end