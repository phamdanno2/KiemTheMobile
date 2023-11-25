-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000028' bên dưới thành ID tương ứng
local BatDongTienSinh = Scripts[000028]

--****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function BatDongTienSinh:OnOpen(scene, npc, player, otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText("Tạp hóa Nam Bắc gì cũng có! Khách quan muốn mua gì!")
	dialog:AddSelection(1, "<color=yellow>[Bạc khóa]</color>Tạp hóa")
	dialog:AddSelection(2, "Tạp hóa")
	dialog:AddSelection(3, "Khoáng thạch")
	dialog:AddSelection(4, "Vải")
	dialog:AddSelection(5, "Thảo dược")
	dialog:AddSelection(6, "Gỗ")
	dialog:AddSelection(7, "Da")
	dialog:AddSelection(8, "Ta không mua nữa ")
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
function BatDongTienSinh:OnSelection(scene, npc, player, selectionID, otherParams)
	
	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	if selectionID == 1 then
		Player.OpenShop(npc, player, 200)
		GUI.CloseDialog(player)
	elseif selectionID == 2 then
		Player.OpenShop(npc, player, 15)
		GUI.CloseDialog(player)
	elseif selectionID == 3 then
		Player.OpenShop(npc, player, 16)
		GUI.CloseDialog(player)
	elseif selectionID == 4 then
		Player.OpenShop(npc, player, 17)
		GUI.CloseDialog(player)
	elseif selectionID == 5 then
		Player.OpenShop(npc, player, 18)
		GUI.CloseDialog(player)
	elseif selectionID == 6 then
		Player.OpenShop(npc, player, 19)
		GUI.CloseDialog(player)
	elseif selectionID == 7 then
		Player.OpenShop(npc, player, 20)
		GUI.CloseDialog(player)
	elseif selectionID == 8 then
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
function BatDongTienSinh:OnItemSelected(scene, npc, player, itemID)

	-- ************************** --
	
	-- ************************** --

end