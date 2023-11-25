-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000021' bên dưới thành ID tương ứng
local TayTuyDaiSu = Scripts[000156]

--****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function TayTuyDaiSu:OnOpen(scene, npc, player, otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText("Ta Sẽ giúp ngươi phân phối lại điểm tiềm năng và kỹ năng . Phía sau có 1 sơn động phân phối xong có thể đến đó thử nghiệm hiểu quả .nếu không vừa ý thì quay lại tìm ta .Khi hài lòng thì truyền tống môn phái sẽ đưa người về")
	dialog:AddSelection(1, "Tẩy điểm tiềm năng ")
	dialog:AddSelection(2, "Tẩy điêm kỹ năng")
	dialog:AddSelection(3, "Tẩy điểm tiềm năng và kỹ năng")
	dialog:AddSelection(4, "Để ta suy nghĩ đã")
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
function TayTuyDaiSu:OnSelection(scene, npc, player, selectionID, otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	if selectionID == 1 then
		player:UnAssignRemainPotentialPoints()
		player:ResetAllSkillsLevel()
		dialog:AddText("Tẩy tủy thành công ! Ngươi có thể phân phối	lại điểm tiềm năng.")
		dialog:AddSelection(4,"Kết thúc đối thoại")
		dialog:Show(npc, player)
	elseif selectionID == 2 then
		player:ResetAllSkillsLevel()
		dialog:AddText("Tẩy tủy thành công ! Ngươi có thể phân phối	lại điểm kỹ năng.")
		dialog:AddSelection(4,"Kết thúc đối thoại")
		dialog:Show(npc, player)
	elseif selectionID == 3 then
		player:UnAssignRemainPotentialPoints()
		player:ResetAllSkillsLevel()
		dialog:AddText("Tẩy tủy thành công ! Ngươi có thể phân phối	lại điểm kỹ năng và tiềm năng.")
		dialog:AddSelection(4,"Kết thúc đối thoại")
	elseif selectionID == 4 then
		GUI:CloseDialog(player)		
	end
	-- ************************** --
	-- ************************** --

end

-- ****************************************************** --
--	Hàm này được gọi khi có sự kiện người chơi chọn một trong các vật phẩm, và ấn nút Xác nhận cung cấp bởi NPC thông qua NPC Dialog
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
--		itemID: number - ID vật phẩm được chọn
-- ****************************************************** --
function TayTuyDaiSu:OnItemSelected(scene, npc, player, itemID, otherParams)

	-- ************************** --

	-- ************************** --

end