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
	dialog:AddText("Điểm danh vọng nghĩa quân khi đạt đến trình độ nhất định, được mua trang bị nghĩa quân chỗ ta.")
	dialog:AddSelection(1, "Mua trang bị nhiệm vụ nghĩa quân")
	dialog:AddSelection(2, "Vào Bí Cảnh")
	dialog:AddSelection(50, "Kết thúc đối thoại")
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
	if selectionID == 1 then 
		if player:GetFactionID() == 0 then
			local dialog = GUI.CreateNPCDialog()
			dialog:AddText("Ngươi hãy ra nhập môn phái rồi quanh lại gặp ta")
			dialog:AddSelection(50, "Để ta suy nghĩ.")
			dialog:Show(npc, player)
		else
			if player:GetFactionID() == 1 then
				Player.OpenShop(npc, player, 37)
				GUI.CloseDialog(player)
			elseif player:GetFactionID() == 2 then
				Player.OpenShop(npc, player, 38)
				GUI.CloseDialog(player)
			elseif player:GetFactionID() == 3 then
				Player.OpenShop(npc, player, 39)
				GUI.CloseDialog(player)
			elseif player:GetFactionID() == 4 then
				Player.OpenShop(npc, player, 41)
				GUI.CloseDialog(player)			
			elseif player:GetFactionID() == 11 then
				Player.OpenShop(npc, player, 40)
				GUI.CloseDialog(player)
			elseif player:GetFactionID() == 5 then
				Player.OpenShop(npc, player, 43)
				GUI.CloseDialog(player)
			elseif player:GetFactionID() == 6 then
				Player.OpenShop(npc, player, 44)
				GUI.CloseDialog(player)
			elseif player:GetFactionID() == 12 then
				Player.OpenShop(npc, player, 42)
				GUI.CloseDialog(player)
			elseif player:GetFactionID() == 7 then
				Player.OpenShop(npc, player, 46)
				GUI.CloseDialog(player)
			elseif player:GetFactionID() == 8 then
				Player.OpenShop(npc, player, 45)
				GUI.CloseDialog(player)
			elseif player:GetFactionID() == 9 then
				Player.OpenShop(npc, player, 47)
				GUI.CloseDialog(player)
			elseif player:GetFactionID() == 10 then
				Player.OpenShop(npc, player, 48)
				GUI.CloseDialog(player)
			end
		end
	elseif selectionID == 2 then
		local dialog = GUI.CreateNPCDialog()
		dialog:AddText("Vào <color=yellow>Bí Cảnh</color> tu luyện được <color=green>2 giờ</color>, trước khi vào, đội trưởng cần giao nộp cho ta 1 tấm <color=yellow>[Bản đồ bí cảnh]</color>. Đội trưởng sẽ được <color=green>x2 kinh nghiệm</color> trong bí cảnh. Trong suốt thời gian tồn tại <color=yellow>Bí Cảnh</color>, người chơi nếu bị <color=green>mất kết nối</color> hoặc <color=green>thoát ra ngoài</color> sẽ <color=orange>không thể</color> vào lại.")
		dialog:AddSelection(3, "Ta muốn vào Bí Cảnh")
		dialog:AddSelection(50, "Ta chỉ ghé qua...")
		dialog:Show(npc, player)
	elseif selectionID == 3 then
		local ret = EventManager.MiJing_CheckCondition(player)
		if ret ~= "OK" then
			local dialog = GUI.CreateNPCDialog()
			dialog:AddText(ret)
			dialog:Show(npc, player)
			return
		end
		EventManager.MiJing_Begin(player)
	elseif selectionID == 50 then
		GUI.CloseDialog(player)
	end
	-- ************************** --
	
end
	-- ************************** --


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