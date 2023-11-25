-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000000' bên dưới thành ID tương ứng
local VuKhiDacChe = Scripts[000044]

--****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function VuKhiDacChe:OnOpen(scene, npc, player, otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText("Binh khí tốt chỗ ta không thiếu, nhưng chúng đều tự chọn chủ cho mình, nếu bản lĩnh khách quan không bao nhiêu thì có mua cũng vô ích. Nhưng chỉ cần vũ khí của ta, mặc thêm phi phong của Tạ Hiền thì khách quan có thể xưng hùng võ lâm.")
	dialog:AddSelection(1, "Ta cần mua vũ khí hệ Kim")
	dialog:AddSelection(2, "Ta cần mua vũ khí hệ Mộc")
	dialog:AddSelection(3, "Ta cần mua vũ khí hệ Thủy")
	dialog:AddSelection(4, "Ta cần mua vũ khí hệ Hỏa")
	dialog:AddSelection(5, "Ta cần mua vũ khí hệ Thổ")
	dialog:AddSelection(6, "Đống sắt vụn này ta không thèm")
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
function VuKhiDacChe:OnSelection(scene, npc, player, selectionID, otherParams)

	-- ************************** --
local dialog = GUI.CreateNPCDialog()
	
	if selectionID == 1 then
		if player:GetLevel() >= 10 then
			Player.OpenShop(npc, player, 135)
			GUI.CloseDialog(player)
		else
			dialog:AddText("chưa đủ cấp !!.")
			dialog:Show(npc, player)
			
		end
	elseif selectionID == 2 then
		if player:GetLevel() >= 10 then
			Player.OpenShop(npc, player, 136)
			GUI.CloseDialog(player)
		else
			dialog:AddText("Chưa đủ cấp !!.")
			dialog:Show(npc, player)
		end
	elseif selectionID == 3 then
		if player:GetLevel() >= 10 then
			Player.OpenShop(npc, player, 137)
			GUI.CloseDialog(player)
		else
			dialog:AddText("Chưa đủ cấp !!.")
			dialog:Show(npc, player)
		end
	elseif selectionID == 4 then
		if player:GetLevel() >= 10 then
			Player.OpenShop(npc, player, 138)
			GUI.CloseDialog(player)
		else
			dialog:AddText("Chưa đủ cấp !!.")
			dialog:Show(npc, player)
		end
	elseif selectionID == 5 then
		if player:GetLevel() >= 10 then
			Player.OpenShop(npc, player, 139)
			GUI.CloseDialog(player)
		else
			dialog:AddText("Chưa đủ cấp !!.")
			dialog:Show(npc, player)
		end
	elseif selectionID == 6 then
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
function VuKhiDacChe:OnItemSelected(scene, npc, player, itemID, otherParams)

	-- ************************** --

	-- ************************** --

end
