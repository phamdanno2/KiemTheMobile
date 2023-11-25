-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000000' bên dưới thành ID tương ứng
local TrinhBatCo = Scripts[000139]

-- ****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function TrinhBatCo:OnOpen(scene, npc, player,otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText("Tôi có thể đưa bạn đến chỗ phó bản, mời chọn phó bản muốn đến")
	dialog:AddSelection(1, "Hậu Sơn Phục Ngưu")
	dialog:AddSelection(2, "Bách Man Sơn")
	dialog:AddSelection(3, "Hải Lăng Vương Mộ")
	dialog:AddSelection(4, "Ngạc Luân Hà Nguyên")
	dialog:AddSelection(5, "Kết Thúc đối thoại")
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
function TrinhBatCo:OnSelection(scene, npc, player, selectionID, otherParams)

	if selectionID == 5 then
		GUI.CloseDialog(player)
	end
	-- ************************** --
	if selectionID >= 1 and selectionID <= 3 then
		-- Loại phụ bản
		local nIndex = selectionID - 1
		-- Gọi hàm hệ thống
		local strRet = EventManager.MilitaryCamp_CheckCondition(player, nIndex)
		-- Toác
		if strRet ~= "OK" then
			local dialog = GUI.CreateNPCDialog()
			dialog.AddText(strRet)
			dialog.AddSelection(5, "Kết thúc đối thoại")
			dialog.Show(npc, player)
			return
		end
		
		-- Bắt đầu phụ bản
		EventManager.MilitaryCamp_Begin(player, nIndex)
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
function TrinhBatCo:OnItemSelected(scene, npc, player, itemID, otherParams)

	-- ************************** --
	
	-- ************************** --

end

-- ======================================================= --
-- ======================================================= --
function TrinhBatCo:JoinFaction(scene, npc, player, factionID)
	
	-- ************************** --
	local ret = player:JoinFaction(factionID)
	-- ************************** --
	if ret == -1 then
		TrinhBatCo:ShowDialog(npc, player, "Người chơi không tồn tại!")
		return
	elseif ret == -2 then
		TrinhBatCo:ShowDialog(npc, player, "Môn phái không tồn tại!")
		return
	elseif ret == 0 then
		TrinhBatCo:ShowDialog(npc, player, "Giới tính của bạn không phù hợp với môn phái này!")
		return
	elseif ret == 1 then
		TrinhBatCo:ShowDialog(npc, player, "Gia nhập phái <color=blue>" .. player:GetFactionName() .. "</color> thành công!")
		return
	else
		TrinhBatCo:ShowDialog(npc, player, "Chuyển phái thất bại, lỗi chưa rõ!")
		return
	end
	-- ************************** --

end

-- ======================================================= --
-- ======================================================= --
function TrinhBatCo:GetValueTest(scene, npc, player)
	
	-- ************************** --
	return 1, 2
	-- ************************** --

end

-- ======================================================= --
-- ======================================================= --
function TrinhBatCo:ShowDialog(npc, player, text)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText(text)
	dialog:Show(npc, player)
	-- ************************** --
	
end