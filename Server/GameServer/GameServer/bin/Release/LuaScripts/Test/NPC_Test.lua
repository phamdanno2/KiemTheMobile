-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000000' bên dưới thành ID tương ứng
local NPC_Test = Scripts[000000]

-- ****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
--		otherParams: Key-Value {number, string} - Danh sách các tham biến khác
-- ****************************************************** --
function NPC_Test:OnOpen(scene, npc, player, otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	--[[[dialog:AddText("Hội thoại từ <color=green>" .. npc:GetName() .. "</color>, được thực hiện trên Lua bởi <color=yellow>" .. player:GetName() .. "</color>.")
	dialog:AddSelection(1, "Gia nhập môn phái")
	dialog:AddSelection(2, "Tạo BOT Test 10x")
	dialog:AddSelection(3, "Tạo BOT Test 20x")
	dialog:AddSelection(4, "Đến <color=yellow>Tương Dương Phủ</color>")
	dialog:AddSelection(5, "Mở khung cường hóa Ngũ Hành Ấn")
	dialog:AddSelection(6, "Mở khung cường hóa")
	dialog:AddSelection(7, "Mở khung ghép Huyền Tinh")
	dialog:AddSelection(8, "Mở khung tách Huyền Tinh từ trang bị")
	dialog:AddSelection(9, "Mở khung phân giải Huyền Tinh cấp cao")
	dialog:AddSelection(10, "Tạo Boss Test")
	dialog:AddSelection(11, "Hiện bảng danh sách vật phẩm")
	dialog:AddSelection(12, "Hiện bảng chọn vật phẩm")
	dialog:AddSelection(13, "Báo danh Bạch Hổ Đường - Tương Dương")]]
	dialog:AddSelection(15, "xin vao gia toc")
	dialog:AddText(""..npc:GetName()..": xin chào!")
	dialog:AddSelection(14, "kết thúc đối thoại")
	
	dialog:Show(npc, player)
	-- ************************** --

end

-- ****************************************************** --
--	Hàm này được gọi khi có sự kiện người chơi ấn vào một trong số các chức năng cung cấp bởi NPC thông qua NPC Dialog
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
--		selectionID: number - ID chức năng
--		otherParams: Key-Value {number, string} - Danh sách các tham biến khác
-- ****************************************************** --
function NPC_Test:OnSelection(scene, npc, player, selectionID, otherParams)

	-- ************************** --
	if selectionID ==15 then
		GUI.Opent(player,"UIFamilyList")
	return
	end
	if selectionID ==14 then
		GUI.CloseDialog(player)
	end
	if selectionID == 1 then
		local dialog = GUI.CreateNPCDialog()
		dialog:AddText("Chọn môn phái muốn gia nhập.")
		for key, value in pairs(Global_FactionName) do
			dialog:AddSelection(100 + key, value)
		end
		dialog:Show(npc, player)
		
		return
	end
	-- ************************** --
	if selectionID >= 100 and selectionID <= #Global_FactionName + 100 then
		NPC_Test:JoinFaction(scene, npc, player, selectionID - 100)
		
		return
	end
	-- ************************** --
	if selectionID == 2 or selectionID == 3 then
		local randomArmor = {
			Female = {3181, 3182, 3183, 3184, 3185, 3186, 3187, 3188, 3189, 3190, 3191},
			Male = {3171, 3172, 3173, 3174, 3175, 3176, 3177, 3178, 3179, 3180},
		}
		local randomWeapon = {3766, 3776, 3786, 3736, 3746, 3756}
		local randomHelm = {
			Female = {3429, 3430, 3431, 3432, 3433, 3434, 3435, 3436, 3437, 3438},
			Male = {3419, 3420, 3421, 3422, 3423, 3424, 3425, 3426, 3427, 3428},
		}
		local randomMantle = {
			Female = {3669, 3670, 3671, 3672, 3673, 3674, 3675, 3676, 3677, 3678},
			Male = {3619, 3620, 3621, 3622, 3623, 3624, 3625, 3626, 3627, 3628},
		}
		local randomHorse = {3459, 3460, 3461, 3462, 3463, 3464, 3465, 3466, 3467, 3468, 3469, 3470}
		local randRange = 500
		
		local count = 1
		if selectionID == 2 then
			count = 10
		elseif selectionID == 3 then
			count = 20
		end
		
		for i = 1, count do
			local npcPos = npc:GetPos()
			local randPosX, randPosY = npcPos:GetX() + math.random(randRange) - math.random(randRange), npcPos:GetY() + math.random(randRange) - math.random(randRange)
			local randSex = math.random(0, 1)
			local randRiding = math.random(0, 1)
			local randMaxHP = math.random(100, 2000)
			local randHP = math.random(50, randMaxHP)
			local randSeries = math.random(1, 5)
		
			local botBuilder = EventManager.CreateBotBuilder(scene)
			botBuilder:SetPos(randPosX, randPosY)
			botBuilder:SetName("Bot")
			botBuilder:SetTitle("Bot Title")
			botBuilder:SetLevel(100)
			botBuilder:SetFaction(3, 1)
			botBuilder:SetSex(randSex)
			if randSex == 0 then
				botBuilder:SetAvarta(2)
			else
				botBuilder:SetAvarta(101)
			end
			if randRiding == 0 then
				botBuilder:SetIsRiding(false)
			else
				botBuilder:SetIsRiding(true)
			end
			botBuilder:SetAIID(-1)
			botBuilder:SetDirection(-1)
			botBuilder:AddEquip(randomWeapon[math.random(#randomWeapon)], randSeries, 16)
			botBuilder:AddEquip(randomHorse[math.random(#randomHorse)], 0, 0)
			if randSex == 0 then
				botBuilder:AddEquip(randomArmor.Male[math.random(#randomArmor.Male)], randSeries, 0)
				botBuilder:AddEquip(randomHelm.Male[math.random(#randomHelm.Male)], randSeries, 0)
				botBuilder:AddEquip(randomMantle.Male[math.random(#randomMantle.Male)], 0, 0)
			else
				botBuilder:AddEquip(randomArmor.Female[math.random(#randomArmor.Female)], randSeries, 0)
				botBuilder:AddEquip(randomHelm.Female[math.random(#randomHelm.Female)], randSeries, 0)
				botBuilder:AddEquip(randomMantle.Female[math.random(#randomMantle.Female)], 0, 0)
			end
			botBuilder:SetHP(randHP)
			botBuilder:SetHPMax(randMaxHP)
			botBuilder:Build()
		end
		
		
		NPC_Test:ShowDialog(npc, player, "Tạo BOT thành công!")
		return
	end
	-- ************************** --
	if selectionID == 4 then
		player:ChangeScene(25, 11530, 2070)
		return
	end
	-- ************************** --
	if selectionID == 5 then
		GUI.OpenUI(player, "UISignetEnhance")
		
		return
	end
	-- ************************** --
	if selectionID == 6 then
		GUI.OpenUI(player, "UIEnhance")
		
		return
	end
	-- ************************** --
	if selectionID == 7 then
		GUI.OpenUI(player, "UICrystalStoneSynthesis")
		
		return
	end
	-- ************************** --
	if selectionID == 8 then
		GUI.OpenUI(player, "UISplitEquipCrystalStones", 8, 80)
		
		return
	end
	-- ************************** --
	if selectionID == 9 then
		GUI.OpenUI(player, "UISplitCrystalStone", 10, 80)
		
		return
	end
	-- ************************** --
	if selectionID == 10 then
		-- Boss
		local Boss = {
			ID = 2661,
			Name = "",
			Title = "",
			BaseHP = 10000,
			HPIncreaseEachLevel = 10000,
			AIType = 2,
			SpawnAfter = 10000,
			ScriptID = -1,
		}
		
		local level = 120

		local monsterBuilder = EventManager.CreateMonsterBuilder(scene)
		monsterBuilder:SetResID(Boss.ID)
		monsterBuilder:SetPos(player:GetPos())
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
		monsterBuilder:Build()
		
		return
	end
	-- ************************** --
	if selectionID == 11 then
		local dialog = GUI.CreateNPCDialog()
		dialog:AddText("Danh sách vật phẩm kèm Selection")
		dialog:AddSelection(111, "Selection 1")
		dialog:AddSelection(112, "Selection 2")
		dialog:AddItem(132, 100, 1)
		dialog:AddItem(132, 50, 0)
		dialog:AddItem(164, 1, 1)
		dialog:SetItemSelectable(false)
		dialog:Show(npc, player)
		return
	end
	-- ************************** --
	if selectionID == 111 and selectionID == 112 then
		local dialog = GUI.CreateNPCDialog()
		dialog:AddText("Chọn Selection 1")
		dialog:Show(npc, player)
		return
	end
	-- ************************** --
	if selectionID == 12 then
		local dialog = GUI.CreateNPCDialog()
		dialog:AddText("Danh sách vật phẩm kèm Selection")
		dialog:AddSelection(111, "Selection 1")
		dialog:AddSelection(112, "Selection 2")
		dialog:AddItem(132, 100, 1)
		dialog:AddItem(132, 50, 0)
		dialog:AddItem(164, 1, 1)
		dialog:SetItemSelectable(true)
		dialog:Show(npc, player)
		return
	end
	-- ************************** --
	if selectionID == 13 then
		player:ChangeScene(25, 6945, 2784)
	end
	-- ************************** --

end

-- ****************************************************** --
--	Hàm này được gọi khi có sự kiện người chơi chọn một trong các vật phẩm, và ấn nút Xác nhận cung cấp bởi NPC thông qua NPC Dialog
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
--		selectedItemInfo: SelectItem - Vật phẩm được chọn
--		otherParams: Key-Value {number, string} - Danh sách các tham biến khác
-- ****************************************************** --
function NPC_Test:OnItemSelected(scene, npc, player, selectedItemInfo, otherParams)

	-- ************************** --
	local itemID = selectedItemInfo:GetItemID()
	local itemNumber = selectedItemInfo:GetItemCount()
	local binding = selectedItemInfo:GetBinding()
	-- ************************** --
	NPC_Test:ShowDialog(npc, player, string.format("Chọn vật phẩm ID: %d, Count: %d, Binding: %d!", itemID, itemNumber, binding))
	-- ************************** --

end

-- ======================================================= --
-- ======================================================= --
function NPC_Test:JoinFaction(scene, npc, player, factionID)
	
	-- ************************** --
	local ret = player:JoinFaction(factionID)
	-- ************************** --
	if ret == -1 then
		NPC_Test:ShowDialog(npc, player, "Người chơi không tồn tại!")
		return
	elseif ret == -2 then
		NPC_Test:ShowDialog(npc, player, "Môn phái không tồn tại!")
		return
	elseif ret == 0 then
		NPC_Test:ShowDialog(npc, player, "Giới tính của bạn không phù hợp với môn phái này!")
		return
	elseif ret == 1 then
		NPC_Test:ShowDialog(npc, player, "Gia nhập phái <color=blue>" .. player:GetFactionName() .. "</color> thành công!")
		return
	else
		NPC_Test:ShowDialog(npc, player, "Chuyển phái thất bại, lỗi chưa rõ!")
		return
	end
	-- ************************** --

end

-- ======================================================= --
-- ======================================================= --
function NPC_Test:GetValueTest(scene, npc, player)
	
	-- ************************** --
	return 1, 2
	-- ************************** --

end

-- ======================================================= --
-- ======================================================= --
function NPC_Test:ShowDialog(npc, player, text)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText(text)
	dialog:Show(npc, player)
	-- ************************** --
	
end