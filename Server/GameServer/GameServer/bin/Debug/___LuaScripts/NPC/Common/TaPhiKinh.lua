-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000057' bên dưới thành ID tương ứng
local TaPhiKinh = Scripts[000057]

--****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function TaPhiKinh:OnOpen(scene, npc, player,otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText("<color=#ffff33>"..npc:GetName().."</color> :Triều đình và  Nam Tống sau hoài nghi Long Hưng đã có mấy mươn năm phục hồi nguyên khí, thật ra Thiên Nhẫn ta đâu muốn có chiến tranh? có chiến trang là có người chết. Nhưng bọn Thát Đát, Tây Hạ luôn có dã tâm, không thể không đề phòng được.<br><color=#009900>Nếu muốn quay về Tân Thủ Thôn, hãy đến gặp người Truyền Tống Môn Phái</color>")
	dialog:AddSelection(10, "Đưa ta Thiên Nhẫn")
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
function TaPhiKinh:OnSelection(scene, npc, player, selectionID, otherParams)

	-- ************************** --
		if selectionID == 10 then
		   player:ChangeScene(Global_NameMapPhaiID[10].ID,Global_NameMapPhaiID[10].PosX,Global_NameMapPhaiID[10].PosY)
		elseif  selectionID == 2 then
			GUI.CloseDialog(player)
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
function TaPhiKinh:OnItemSelected(scene, npc, player, itemID, otherParams)

	-- ************************** --
	
	-- ************************** --

end