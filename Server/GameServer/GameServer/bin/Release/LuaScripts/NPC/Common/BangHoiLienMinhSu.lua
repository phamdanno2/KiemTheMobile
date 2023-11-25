-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000015' bên dưới thành ID tương ứng
local BangHoiLienMinhSu = Scripts[000015]

--****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function BangHoiLienMinhSu:OnOpen(scene, npc, player,otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText("Bang của người nếu muốn tiến hành các thao tác liên minh, có thể đăng ký ở chỗ ta.")
	dialog:AddSelection(1, "Tạo liên minh")
	dialog:AddSelection(2, "Gia Nhập liên minh")
	dialog:AddSelection(3, "Rời liên minh")
	dialog:AddSelection(4, "Chuyển Minh Chủ")
	dialog:AddSelection(5, "Phân bố lãnh thổ")
	dialog:AddSelection(6, "Kết Thúc đối thoại")
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
function BangHoiLienMinhSu:OnSelection(scene, npc, player, selectionID, otherParams)
	
	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	if selectionID == 6 then
		GUI.CloseDialog(player)
	elseif selectionID == 1  or selectionID == 2  or selectionID == 3  or selectionID == 4 or selectionID == 5 then
		dialog:AddText("Chức năng chưa mở")
		dialog:Show(npc, player)
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
function BangHoiLienMinhSu:OnItemSelected(scene, npc, player, itemID, otherParams)

	-- ************************** --
	
	-- ************************** --

end