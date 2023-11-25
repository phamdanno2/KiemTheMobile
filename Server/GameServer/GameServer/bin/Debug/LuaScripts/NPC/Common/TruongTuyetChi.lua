-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '150' bên dưới thành ID tương ứng
local TruongTuyetChi = Scripts[000150]

--****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function TruongTuyetChi:OnOpen(scene, npc, player,otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText(""..npc:GetName()..": Gần đây đại Mông Cổ ý Bắc phạt, thu phục Trung Nguyên, nay hai quân đang đối đầu tại biên ải, Mộ Binh Lệnh đã truyền khắp nơi, ta đang định chiêu mộ nhân sĩ võ lâm, trợ giúp quân Tống khôi phục sơn hà. <br> Hiện <color=green> tính năng Dương Châu1 </color>chưa bắt đầu")
	dialog:AddSelection(1, "Sự việc không thể chậm trễ , ta đang định đến <color=green> báo danh tinh năng_</color>")
	dialog:AddSelection(2, "Ta muốn mua trang bị Mông Cổ Tây Hạ")
	dialog:AddSelection(3, "Ta sức hèn tài mọn , đợi khi tinh thông võ nghệ sẽ đến giúp ngươi.")
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
function TruongTuyetChi:OnSelection(scene, npc, player, selectionID, otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	if selectionID == 2 then
		if player:GetLevel() >= 10 then
			if player:GetFactionID()== 1 or player:GetFactionID()== 2 then
				Player.OpenShop(npc, player,95)
				GUI.CloseDialog(player)
			elseif player:GetFactionID()== 3 or player:GetFactionID()== 4 or player:GetFactionID()== 11  then
				Player.OpenShop(npc, player,96)
				GUI.CloseDialog(player)
			elseif player:GetFactionID()== 5 or player:GetFactionID()== 6 or player:GetFactionID()== 12 then
				Player.OpenShop(npc, player,97)
				GUI.CloseDialog(player)
			elseif player:GetFactionID()== 7 or player:GetFactionID()== 8 then
				Player.OpenShop(npc, player,98)
				GUI.CloseDialog(player)
			elseif player:GetFactionID()== 9 or player:GetFactionID()== 10 then
				Player.OpenShop(npc, player,99)
				GUI.CloseDialog(player)
			end
		else
			dialog:AddText("chưa đủ cấp !!.")
			dialog:Show(npc, player)
		end
	elseif selectionID == 3 then
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
function TruongTuyetChi:OnItemSelected(scene, npc, player, itemID, otherParams)

	-- ************************** --

	-- ************************** --

end