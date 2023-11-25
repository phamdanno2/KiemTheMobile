-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000039' bên dưới thành ID tương ứng
local ThuongGiaTieuDao = Scripts[000039]

--****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function ThuongGiaTieuDao:OnOpen(scene, npc, player,otherParams )

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText("Nhắc đến Tiêu Dao Cốc, đó quả là tiên cảnh giữa cõi người! Không biết thiếu hiệp có từng đến đó chưa? Ai da, lạc đề rồi, lạc đề rồi… Thiếu hiệp có muốn xem thử trang bị cực phẩm thế gian hiếm có không?")
	dialog:AddSelection(1, "Mua trang bị Danh vọng hoạt động Thịnh Hạ")
	dialog:AddSelection(2, "Mua đạo cụ danh vọng chúc phúc")
	dialog:AddSelection(3, "Mua giày hoạt động Đoàn Viên")
	dialog:AddSelection(4, "Mua trang bị danh vọng lãnh thổ")
	dialog:AddSelection(5, "Mua trang bị danh vọng liên đấu")
	dialog:AddSelection(6, "Không mua nữa")
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
function ThuongGiaTieuDao:OnSelection(scene, npc, player, selectionID,otherParams)

	-- ************************** --
	if selectionID == 1 then
		Player.OpenShop(npc, player, 128)
        GUI.CloseDialog(player)
	elseif 	selectionID == 2 then
		Player.OpenShop(npc, player, 133)
        GUI.CloseDialog(player)
	elseif 	selectionID == 3 then
		Player.OpenShop(npc, player, 161)
        GUI.CloseDialog(player)	
	elseif 	selectionID == 4 then
		Player.OpenShop(npc, player, 147)
        GUI.CloseDialog(player)
	elseif 	selectionID == 5 then
		Player.OpenShop(npc, player, 134)
        GUI.CloseDialog(player)	
	elseif 	selectionID == 6 then
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
function ThuongGiaTieuDao:OnItemSelected(scene, npc, player,otherParams )

	-- ************************** --

	-- ************************** --

end