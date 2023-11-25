-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000020' bên dưới thành ID tương ứng
local QuanLienDauCaoCap = Scripts[000020]

--****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function QuanLienDauCaoCap:OnOpen(scene, npc, player, otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText(""..npc:GetName()..": từ xưa đến nay, đạo võ thuật chỉ nhận thêm và tiếp tục lưu truyền, loại hình võ lâm liên đấu đợt này là <color=yellow> Tam đấu, </color> các vị có thể tự do lập thành chiến đội thi đấu.<br> <color=yellow> Giờ là Võ Lâm Liên Đấu thời gian thi đấu  </color>")
	dialog:AddSelection(51, "<color=yellow> Ta muốn đổi danh vọng Nhẫn Bạch Ngân / Nhẫn Hoàng Kim </color> ")
	dialog:AddSelection(52, "Đến nơi liên đấu Cao Cấp")
	dialog:AddSelection(53, "Chiến đội liên đấu của ta")
	dialog:AddSelection(54, "Xem tình hình trận đấu")
	dialog:AddSelection(55, "Mua trang bi dánh vọng liên đấu")
	dialog:AddSelection(56, "Giới thiệu Võ Lâm Đấu Cao cấp") 
	
	dialog:AddSelection(57, "Kết thúc đối thoại")
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
function QuanLienDauCaoCap:OnSelection(scene, npc, player, selectionID, otherParams)

	-- ************************** --
	 local dialog = GUI.CreateNPCDialog()
	if selectionID == 51 then
		dialog:AddText("Chức năng chưa mở")
		dialog:Show(npc, player)
	elseif  selectionID == 55 then
		dialog:AddText("Chức năng chưa mở")
		dialog:Show(npc, player)
	elseif selectionID == 52 or selectionID == 53 or selectionID == 54 or selectionID == 56 then
		dialog:AddText("Chức năng chưa mở")
		dialog:Show(npc, player)
	end 
	if  selectionID == 57  then
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
function QuanLienDauCaoCap:OnItemSelected(scene, npc, player, itemID, otherParams)

	-- ************************** --

	-- ************************** --

end