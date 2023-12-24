-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000013' bên dưới thành ID tương ứng
local TruyenTongBHD = Scripts[000131]

--****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function TruyenTongBHD:OnOpen(scene, npc, player,otherParams)

	-- ************************** --
	
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText("Dạo này trong Bạch Hổ Đường xuất hiện đầy bọn trộm cắp, bọn ta không đủ sức đối phó với chúng. Ngươi có thể giúp một tay không? Ta có thể đưa ngươi đến Đại Điện Bạch Hổ Đường, sự việc cụ thể ngươi có thể tìm hiểu ở chỗ Môn Đồ Bạch Hổ Đường.")
	dialog:AddSelection(1,"<color=yellow>Ta muốn vào Bạch Hổ Đường (Liên Server) </color>")
	dialog:AddSelection(2,"Ta muốn vào Bạch Hổ Đường (Sơ 1)")
	dialog:AddSelection(3,"Ta muốn vào Bạch Hổ Đường (Sơ 2)")
	dialog:AddSelection(4,"<color=#FF00FF>BXH vinh dự Bạch Hổ Đường </color>")
	dialog:AddSelection(5,"[Quy tắc hoạt động]")
	dialog:AddSelection(6,"Mua trang bị Bạch Hổ Đường ")
	dialog:AddSelection(7,"Nhận phần thưởng gia tộc Bạch Hổ Đường")
	dialog:AddSelection(8,"Phần thưởng Bạch Hổ Đường Liên Server")
	dialog:AddSelection(9,"Kết Thúc đối thoại")
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
function TruyenTongBHD:OnSelection(scene, npc, player, selectionID,otherParams)
	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	if selectionID == 6 then
		if player:GetLevel() >= 10 then
			if player:GetFactionID() == 1 or player:GetFactionID() == 2 then
				Player.OpenShop(npc, player, 65)
			elseif player:GetFactionID() == 3 or player:GetFactionID() == 4 or player:GetFactionID() == 11 then
				Player.OpenShop(npc, player, 66)
			elseif player:GetFactionID() == 5 or player:GetFactionID() == 6 or player:GetFactionID() == 12 then
				Player.OpenShop(npc, player, 67)
			elseif player:GetFactionID() == 7 or player:GetFactionID() == 8 then
				Player.OpenShop(npc, player, 68)
			elseif player:GetFactionID() == 9 or player:GetFactionID() == 10 then
				Player.OpenShop(npc, player, 69)
			end
		else
			dialog:AddText("Chưa đủ cấp !!.")
			dialog:Show(npc, player)
		end
	return
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
function TruyenTongBHD:OnItemSelected(scene, npc, player, itemID, otherParams)

	-- ************************** --
	
	-- ************************** --

end
