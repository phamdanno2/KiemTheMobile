-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000029' bên dưới thành ID tương ứng
local DiemTuuThuc = Scripts[000029]

--****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function DiemTuuThuc:OnOpen(scene, npc, player, otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText("Hi hi! Có vũ khí của ta trong tay, muốn trở thành đệ nhất võ lâm cao thủ dễ như trở bàn tay. Khách quan hãy mua 1 cái đi, sau đó đến chỗ Tạ Hiền mua chiếc phi phong, chắc chắn sẽ rất oai!")
	dialog:AddSelection(1, "Triền Thủ ")
	dialog:AddSelection(2, "Kiếm")
	dialog:AddSelection(3, "Đao")
	dialog:AddSelection(4, "Côn")
	dialog:AddSelection(5, "Thương")
	dialog:AddSelection(6, "Chùy")
	dialog:AddSelection(7, "Phi đao")
	dialog:AddSelection(8, "Tụ tiễn")

	dialog:AddSelection(10, "Khoáng thạch")
	dialog:AddSelection(11, "Gỗ")
	dialog:AddSelection(12, "Vải")
	dialog:AddSelection(13, "Kết thúc đối thoại")
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
function DiemTuuThuc:OnSelection(scene, npc, player, selectionID, otherParams)
	-- ************************** --
    local dialog = GUI.CreateNPCDialog()

	if selectionID == 1 then 
		Player.OpenShop(npc, player, 1)
		GUI.CloseDialog(player)
	elseif  selectionID == 2 then 
		Player.OpenShop(npc, player, 2)
		GUI.CloseDialog(player)
	elseif  selectionID == 3 then 
		Player.OpenShop(npc, player,3)
		GUI.CloseDialog(player)
	elseif  selectionID == 4 then 
		Player.OpenShop(npc, player, 4)
		GUI.CloseDialog(player)
	elseif selectionID == 5 then 
		Player.OpenShop(npc, player, 5)
		GUI.CloseDialog(player)
	elseif selectionID == 6 then
		Player.OpenShop(npc, player, 6)
		GUI.CloseDialog(player)
	elseif selectionID == 7 then
		Player.OpenShop(npc, player, 8)
		GUI.CloseDialog(player)
	elseif selectionID == 8 then
		Player.OpenShop(npc, player, 9)
		GUI.CloseDialog(player)
		
	elseif selectionID == 10 then
		Player.OpenShop(npc, player, 16)
		GUI.CloseDialog(player)
	elseif selectionID == 11 then
		Player.OpenShop(npc, player, 19)
		GUI.CloseDialog(player)
	elseif selectionID == 12 then
		Player.OpenShop(npc, player, 17)
		GUI.CloseDialog(player)
	elseif selectionID ==13 then
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
function DiemTuuThuc:OnItemSelected(scene, npc, player, itemID, otherParams)

	-- ************************** --

	-- ************************** --

end