-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000027' bên dưới thành ID tương ứng
local TruongTramKinh = Scripts[000027]

--****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function TruongTramKinh:OnOpen(scene, npc, player,otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText("Những năm gần đây cục thế biến loạn nhưng tiệm thuốc của ta lại làm ăn ngày càng phát đạt, ta nên vui hay nên buồn đây…Thật mong sao phi phong của Tạ Hiền có thể thay đổi thời cuộc này.")
	dialog:AddSelection(1, "<color=#fff94d>[Bạc khóa]</color>Thuốc")	
	dialog:AddSelection(2, "Thuốc")
	dialog:AddSelection(3, "Thảo dược")
	if scene:GetID()== 255 then
		dialog:AddSelection(4, "Vũ Khí")
	end
	dialog:AddSelection(5, "Ta Không mua nữa")
	
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
function TruongTramKinh:OnSelection(scene, npc, player, selectionID, otherParams)
	
	-- ************************** --
	if selectionID == 1 then
		Player.OpenShop(npc, player, 1444)
		GUI.CloseDialog(player)
	elseif selectionID == 2 then
		Player.OpenShop(npc, player, 14)
		GUI.CloseDialog(player)
	elseif selectionID == 3 then
		Player.OpenShop(npc, player, 18)
		GUI.CloseDialog(player)
	elseif selectionID == 4 then
		Player.OpenShop(npc, player, 146)
		GUI.CloseDialog(player)
	elseif selectionID == 5 then
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
function TruongTramKinh:OnItemSelected(scene, npc, player, itemID, otherParams)

	-- ************************** --
	
	-- ************************** --

end