-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000048' bên dưới thành ID tương ứng
local PhoThieuKhach = Scripts[000048]
--****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function PhoThieuKhach:OnOpen(scene, npc, player, otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText("Thành tích đạt được trong lãnh thổ xuất sắc trong trận chiến để giúp các nhà lãnh đạo và cổ đông của từng thành viên thực hiện của tôi trong giải quyết công việc liên quan đến chức danh, và mua con dấu chính thức.<br>Các cấp bậc trưởng tự động, băng nhóm và xếp hạng tương quan xếp hạng<br>Đồng cổ đông, thành viên của các chức danh do trụ sở chính thồn qua các cấp.")
	dialog:AddSelection(1, "Bảo vệ quan hàm")
	dialog:AddSelection(2, "Điều chỉnh cấp quan hàm bang")
	dialog:AddSelection(3, "Điều chỉnh mức độ xếp hạng bang hội")
	dialog:AddSelection(4, "Danh vọng lãnh thổ tạp hóa")
	dialog:AddSelection(5, "Ta chỉ đi dạo")
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
function PhoThieuKhach:OnSelection(scene, npc, player, selectionID, otherParams)

	-- ************************** --
	if selectionID ==5 then
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
function PhoThieuKhach:OnItemSelected(scene, npc, player, itemID, otherParams)

	-- ************************** --

	-- ************************** --

end
