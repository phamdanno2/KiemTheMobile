-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000062' bên dưới thành ID tương ứng
local DoanTuTinh = Scripts[000062]

--****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function DoanTuTinh:OnOpen(scene, npc, player, otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText("<color=#ffff33>"..npc:GetName().."</color> : Trăm năm nay Đoàn Thị chấn hưng trị quốc, vẫn không thành 1 nước lớn mạnh, nguyên nhân do hai Ô Man Tộc và Bạch Di không đồng lòng chung sức. Nay mong hào kiệt thiên hạ góp sức giúp ta hoàn thành ước nguyện tổ tiên  <br><color=#009900>Nếu muốn quay về Tân Thủ Thôn, hãy đến gặp người Truyền Tống Môn Phái</color>")
	dialog:AddSelection(19, "Đưa ta đi Đoàn Thị")
	dialog:AddSelection(2, "Rời khỏi")
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
function DoanTuTinh:OnSelection(scene, npc, player, selectionID,otherParams)
	
	-- ************************** --
	if Global_NameMapPhaiID[selectionID] ~= nil then
		player:ChangeScene(Global_NameMapPhaiID[selectionID].ID,Global_NameMapPhaiID[selectionID].PosX,Global_NameMapPhaiID[selectionID].PosY)
	end
	if selectionID == 2 then
		GUI.CloseDialog(player)
	end
	
	-- ****
	-- ************************** --

end

-- ****************************************************** --
--	Hàm này được gọi khi có sự kiện người chơi chọn một trong các vật phẩm, và ấn nút Xác nhận cung cấp bởi NPC thông qua NPC Dialog
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
--		itemID: number - ID vật phẩm được chọn
-- ****************************************************** --
function DoanTuTinh:OnItemSelected(scene, npc, player, itemID,otherParams)

	-- ************************** --
	
	-- ************************** --

end