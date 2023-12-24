-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000055' bên dưới thành ID tương ứng
local LaTuan = Scripts[000055]

--****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function LaTuan:OnOpen(scene, npc, player, otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText("<color=#ffff33>"..npc:GetName().."</color> :Trong trận chiến Thái Thạch mấy mươn năn trước, Lão Bang chủ Hà Nhân của Cái Bang ta đã dẫn theo tứ đại trưởng lão cùng mấy ngàn đệ tử chống lại quân Kim, giúp Ngu Doãn Văn đại nhân giữ được Trường Giang. Nhưng việc này cũng khiến Cái Bang ta bị tổn thất nặng nề. Ta muốn tìm những chí sĩ yêu nước gia nhập Cái Bang, giúp sức chống quân Kim.<br><color=#009900>Nếu muốn quay về Tân Thủ Thôn, hãy đến gặp người Truyền Tống Môn Phái</color>")
	dialog:AddSelection(15, "Đưa ta đi Cái Bang")
	dialog:AddSelection(100, "Rời khỏi")
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
function LaTuan:OnSelection(scene, npc, player, selectionID,otherParams)

	-- ************************** --
	if Global_NameMapPhaiID[selectionID] ~= nil then
		player:ChangeScene(Global_NameMapPhaiID[selectionID].ID,Global_NameMapPhaiID[selectionID].PosX,Global_NameMapPhaiID[selectionID].PosY)
	end
	
	if selectionID == 100 then
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
function LaTuan:OnItemSelected(scene, npc, player, itemID,otherParams)

	-- ************************** --
	
	-- ************************** --

end