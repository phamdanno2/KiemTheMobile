-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000014' bên dưới thành ID tương ứng
local QuanQuanNhu = Scripts[000017]

--****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: player - NPC tương ứng
-- ****************************************************** --
function QuanQuanNhu:OnOpen(scene, npc, player, otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText("Điểm danh vọng nghĩa quân khi đạt đến trình độ nhất định , được mua trang bị nghĩa quân chỗ ta .")
	dialog:AddSelection(1, "Mua trang bị nhiệm vụ nghĩa quân")
	dialog:AddSelection(2, "[Bí cảnh ] Vào bí Cảnh")
	dialog:AddSelection(3, "[Bí cảnh ] bí Cảnh ")
	dialog:AddSelection(4, "Muốn đổi ngân phiếu ") 
	dialog:AddSelection(5, "Kết thúc đối thoại")
	dialog:Show(npc, player)
	-- ************************** --

end

-- ****************************************************** --
--	Hàm này được gọi khi có sự kiện người chơi ấn vào một trong số các chức năng cung cấp bởi NPC thông qua NPC Dialog
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: player - NPC tương ứng
--		selectionID: number - ID chức năng
-- ****************************************************** --
function QuanQuanNhu:OnSelection(scene, npc, player, selectionID, otherParams)
	-- ************************** --
		local dialog = GUI.CreateNPCDialog()
	if selectionID == 1 then 
		if player:GetLevel() >= 10 then
			if player:GetFactionID() == 1 or player:GetFactionID() == 2 then
				Player.OpenShop(npc, player, 216)
				GUI.CloseDialog(player)
			elseif player:GetFactionID() == 3 or player:GetFactionID() == 4 or player:GetFactionID() == 11 then
				Player.OpenShop(npc, player, 217)
				GUI.CloseDialog(player)
			elseif player:GetFactionID() == 5 or player:GetFactionID() == 6 or player:GetFactionID() == 12 then
				Player.OpenShop(npc, player, 218)
				GUI.CloseDialog(player)
			elseif player:GetFactionID() == 7 or player:GetFactionID() == 8 then
				Player.OpenShop(npc, player, 219)
				GUI.CloseDialog(player)
			elseif player:GetFactionID() == 9 or player:GetFactionID() == 10 then
				Player.OpenShop(npc, player, 220)
				GUI.CloseDialog(player)
			end
		else
			dialog:AddText("Hãy tu luyện tới cấp 20 và gia nhập một môn phái rồi tới gặp ta")
			dialog:Show(npc, player)
		end
	elseif  selectionID == 2 or  selectionID == 3 or  selectionID == 4  then
			dialog:AddText("Chức năng chưa mở")
			dialog:Show(npc, player)
	elseif  selectionID == 5 then
		GUI.CloseDialog(player)
	end
	-- ************************** --
end

-- ****************************************************** --
--	Hàm này được gọi khi có sự kiện người chơi chọn một trong các vật phẩm, và ấn nút Xác nhận cung cấp bởi NPC thông qua NPC Dialog
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: player - NPC tương ứng
--		itemID: number - ID vật phẩm được chọn
-- ****************************************************** --
function QuanQuanNhu:OnItemSelected(scene, npc, player, itemID, otherParams)

	-- ************************** --
	
	-- ************************** --

end