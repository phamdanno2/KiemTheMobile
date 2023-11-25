-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000000' bên dưới thành ID tương ứng
local NPC_Test1 = Scripts[000001]

-- ****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function NPC_Test1:OnOpen(scene, npc, player,otherParams)

	-- ************************** --
	--GUI.ShowNotification(player, "My name is " .. player:GetName())
	--GUI.ShowNotification(player, "NPC name is " .. npc:GetName())
	--GUI.ShowNotification(player, "Scene name is " .. scene:GetName())
	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	-- ************************** --
	--[[dialog:AddText("Hội thoại từ <color=green>NPC Test</color>, được thực hiện trên Lua.")
	dialog:AddSelection(1, "Thiết lập cấp độ")
	dialog:AddSelection(2, "Reset tiềm năng vĩnh viễn")
	dialog:AddSelection(3, "Tẩy điểm tiềm năng")
	dialog:AddSelection(4, "Tẩy điểm kỹ năng")
	dialog:AddText(""..npc:GetName()..": xin chào!")]]
	dialog:AddSelection(220, "kết thúc đối thoại")
	dialog:Show(npc, player)
	-- ************************** --

end

-- ****************************************************** --
--	Hàm này được gọi khi có sự kiện người chơi ấn vào một trong số các chức năng cung cấp bởi NPC thông qua NPC Dialog
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
--		selectionID: number - ID chức năng
-- ****************************************************** --
function NPC_Test1:OnSelection(scene, npc, player, selectionID,otherParams)

	-- ************************** --
	if selectionID ==220 then
		GUI.CloseDialog(player)
	end
	if selectionID == 1 then
		local dialog = GUI.CreateNPCDialog()
		dialog:AddText("Chọn cấp độ muốn thiết lập.")
		dialog:AddSelection(10, "Cấp 1")
		dialog:AddSelection(11, "Cấp 10")
		dialog:AddSelection(12, "Cấp 20")
		dialog:AddSelection(13, "Cấp 30")
		dialog:AddSelection(14, "Cấp 40")
		dialog:AddSelection(15, "Cấp 50")
		dialog:AddSelection(16, "Cấp 60")
		dialog:AddSelection(17, "Cấp 70")
		dialog:AddSelection(18, "Cấp 80")
		dialog:AddSelection(19, "Cấp 90")
		dialog:AddSelection(20, "Cấp 100")
		dialog:AddSelection(21, "Cấp 110")
		dialog:AddSelection(22, "Cấp 120")
		dialog:Show(npc, player)
		
		return
	end
	-- ************************** --
	if selectionID >= 10 and selectionID <= 22 then
		local level = (selectionID - 10) * 10
		level = math.max(level, 1)
		NPC_Test1:PlayerSetLevel(npc, player, level)
		
		return
	end
	-- ************************** --
	if selectionID == 2 then
		local currentRemainPoint = player:GetBonusRemainPotentialPoint()
		player:AddBonusRemainPotentialPoint(-currentRemainPoint)
		
		NPC_Test1:ShowDialog(npc, player, "Thêm " .. (-currentRemainPoint) .. " điểm tiềm năng thành công")
		
		return
	end
	-- ************************** --
	if selectionID == 3 then
		player:UnAssignRemainPotentialPoints()
		NPC_Test1:ShowDialog(npc, player, "Tẩy điểm tiềm năng thành công!")
		
		return
	end
	-- ************************** --
	if selectionID == 4 then
		player:ResetAllSkillsLevel()
		NPC_Test1:ShowDialog(npc, player, "Tẩy điểm kỹ năng thành công!")
		
		return
	end
	-- ************************** --

end

-- ****************************************************** --
--	Hàm này được gọi khi có sự kiện người chơi chọn một trong các vật phẩm, và ấn nút Xác nhận cung cấp bởi NPC thông qua NPC Dialog
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
--		itemID: number - ID vật phẩm được chọn
-- ****************************************************** --
function NPC_Test1:OnItemSelected(scene, npc, player, item,otherParams)

	-- ************************** --
	
	-- ************************** --

end

-- ======================================================= --
-- ======================================================= --
function NPC_Test1:PlayerSetLevel(npc, player, level)

	-- ************************** --
	player:SetLevel(level)
	NPC_Test1:ShowDialog(npc, player, "Thiết lập cấp độ " .. level .. " thành công")
	-- ************************** --

end

-- ======================================================= --
-- ======================================================= --
function NPC_Test1:ShowDialog(npc, player, text)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText(text)
	dialog:Show(npc, player)
	-- ************************** --
	
end