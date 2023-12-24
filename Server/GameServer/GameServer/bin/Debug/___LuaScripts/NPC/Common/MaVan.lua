-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '200' bên dưới thành ID tương ứng
local MaVan = Scripts[201]

--****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function MaVan:OnOpen(scene, npc, player,otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText("Gần đây ta phát hiện được 1 thư viện thần bí, nghe nói các nho sinh ở đây đều theo lý học, võ công bất phàm, chẵng lẽ trong thư viện có bí mật gì ư?<br> Nhưng điều quan quan trong là <color=#03fc3d> cần có đủ 8 người và phải đi vào từ Lãnh địa gia tộc,nếu không đủ người thì không thể vào Thư Viện thần bí này.</color>")
	dialog:AddSelection(1, "Được! Đưa người của ta qua đi!")
	dialog:AddSelection(2, "Mua Trang bị gia tộc")
	dialog:AddSelection(3, "Muốn đùng Đồng tiền cổ đổi phần thưởng")
	dialog:AddSelection(4, "<color=#f1ff29>Ta muốn dùng Tiền đông cổ đổi phần thưởng")
	dialog:AddSelection(5, "Nói về túi tiền đi ")
	dialog:AddSelection(6, "Hãy nói về túi tiền đông cổ")
	dialog:AddSelection(7, "Thuyết minh hoạt động")
    dialog:AddSelection(8, "Kết thúc đối thoại")
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
function MaVan:OnSelection(scene, npc, player, selectionID, otherParams)
	
	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	if selectionID == 8 then
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
function MaVan:OnItemSelected(scene, npc, player, itemID, otherParams)

	-- ************************** --
	
	-- ************************** --

end