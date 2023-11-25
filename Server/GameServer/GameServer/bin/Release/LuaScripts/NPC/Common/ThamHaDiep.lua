-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000025' bên dưới thành ID tương ứng
local ThamHaDiep = Scripts[000025]

--****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function ThamHaDiep:OnOpen(scene, npc, player,otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText("Mời ghé vào xem, trang bị của ta tuy không bằng phi phong của Tạ Hiền, nhưng cũng là tâm huyết do một tay ta làm ra!")
		dialog:AddSelection(18, "Y phục")
		dialog:AddSelection(19, "Giày, thắt lưng")
		dialog:AddSelection(20, "Nón")
		dialog:AddSelection(21, "Hộ uyển, Thủ Trạc")
		dialog:AddSelection(22, "Khoáng thạch")
		dialog:AddSelection(23, "Da")
		dialog:AddSelection(24, "Vải")
		dialog:AddSelection(25, "Kêt thúc đối thoại")
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
function ThamHaDiep:OnSelection(scene, npc, player, selectionID, otherParams)
	-- ************************** --
	-- ************************** --
	if selectionID == 18 then 
		Player.OpenShop(npc, player, 10)
		GUI.CloseDialog(player)
	elseif selectionID == 19 then 
		Player.OpenShop(npc, player, 11)
		GUI.CloseDialog(player)
	elseif selectionID == 20 then 
		Player.OpenShop(npc, player, 12)
		GUI.CloseDialog(player)
	elseif selectionID == 21 then
		Player.OpenShop(npc, player, 13)
		GUI.CloseDialog(player)
	elseif selectionID == 22 then
		Player.OpenShop(npc, player, 16)
		GUI.CloseDialog(player)
	elseif selectionID == 23 then
		Player.OpenShop(npc, player, 20)
		GUI.CloseDialog(player)
	elseif selectionID == 24 then
		Player.OpenShop(npc, player, 17)
		GUI.CloseDialog(player)
	elseif  selectionID == 25 then
		GUI.CloseDialog(player)
	end	

	
end

-- ****************************************************** --
--	Hàm này được gọi khi có sự kiện người chơi chọn một trong các vật phẩm, và ấn nút Xác nhận cung cấp bởi NPC thông qua NPC Dialog
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
--		itemID: number - ID vật phẩm được chọn
-- ****************************************************** --
function ThamHaDiep:OnItemSelected(scene, npc, player, itemID, otherParams)

	-- ************************** --

	-- ************************** --

end