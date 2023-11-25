-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000046' bên dưới thành ID tương ứng
local LeBoThiLang = Scripts[000046]
--****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function LeBoThiLang:OnOpen(scene, npc, player, otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText("Hoàng thượng trên triều đang tổ chức lễ biểu dương, muốn tham gia buổi lễ mời lên điện")
	dialog:AddSelection(1, "Giao nạp bá chủ ấn")
	dialog:AddSelection(2, "Đi đến buổi lễ")
	dialog:AddSelection(3, "Xem bảng điểm xếp hạng")
	dialog:AddSelection(4, "Lãnh phần thưởng")
	dialog:AddSelection(5, "Tạo một bức tượng")
	dialog:AddSelection(6, "Lãnh thưởng mức độ tôn kính")
	dialog:AddSelection(7, "chỉ cần xem qua")
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
function LeBoThiLang:OnSelection(scene, npc, player, selectionID, otherParams)

	-- ************************** --
	if selectionID ==7 then
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
function LeBoThiLang:OnItemSelected(scene, npc, player, itemID, otherParams)

	-- ************************** --

	-- ************************** --

end
