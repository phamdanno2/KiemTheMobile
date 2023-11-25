-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000021' bên dưới thành ID tương ứng
local QuanLienDauSoCap = Scripts[000021]

--****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function QuanLienDauSoCap:OnOpen(scene, npc, player, otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText(""..npc:GetName()..": từ xưa đến nay, đạo võ thuật chỉ nhận thêm và tiếp tục lưu truyền, loại hình võ lâm liên đấu đợt này là <color=yellow> Tam đấu, </color> các vị có thể tự do lập thành chiến đội thi đấu.<br> <color=yellow> Giờ là Võ Lâm Liên Đấu thời gian thi đấu  </color>")
	dialog:AddSelection(61, "Đến nơi liên đấu Sơ Cấp")
	dialog:AddSelection(62, "Chiến đội liên đấu của ta")
	dialog:AddSelection(63, "Xem tình hình trận đấu")
	dialog:AddSelection(64, "Mua trang bi dánh vọng liên đấu")
	dialog:AddSelection(65, "Giới thiệu Võ Lâm Đấu Sơ Cấp")
	dialog:AddSelection(66, "kết thúc đối thoại")
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
function QuanLienDauSoCap:OnSelection(scene, npc, player, selectionID, otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	if selectionID == 64 then
		Player.OpenShop(npc, player, 76)
		GUI.CloseDialog(player)
	end
	if  selectionID == 66 then
		GUI.CloseDialog(player)
	elseif selectionID == 61 or selectionID == 62 or selectionID == 63 or selectionID == 65 then
		dialog:AddText("Chức năng chưa mở")
		dialog:Show(npc, player)  
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
function QuanLienDauSoCap:OnItemSelected(scene, npc, player, itemID, otherParams)

	-- ************************** --

	-- ************************** --

end