-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000041' bên dưới thành ID tương ứng
local HoangThuong = Scripts[000041]

--****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function HoangThuong:OnOpen(scene, npc, player, otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText("Chỉ cần độ thân mật đật đến 100 , đẳng cấp phải đạt đến 50 thì đến chỗ ta lập lập bang hội")
	dialog:AddSelection(1,"Lập Bang Hội")
	dialog:AddSelection(2,"Đổi phe bang hội")
	dialog:AddSelection(3,"Nhận lợi tức bang hội")
	dialog:AddSelection(4,"Thưởng ưu tú bang hội")
	dialog:AddSelection(5,"đóng")
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
function HoangThuong:OnSelection(scene, npc, player, selectionID,otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	if selectionID == 5 then
		GUI.CloseDialog(player)
	elseif  selectionID == 1 or selectionID == 2 or selectionID == 3 or selectionID == 4  then
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
function HoangThuong:OnItemSelected(scene, npc, player, itemID,otherParams)

	-- ************************** --

	-- ************************** --

end