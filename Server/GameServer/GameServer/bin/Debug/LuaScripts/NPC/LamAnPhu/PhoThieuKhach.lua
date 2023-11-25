-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000220' bên dưới thành ID tương ứng
local PhoThieuKhanh = Scripts[48]

--****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function PhoThieuKhanh:OnOpen(scene, npc, player,otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText("Thủ lĩnh và thành viên  cổ đông các bang có thành tích nổi bật trong Lãnh Thổ Chiến, có thể đến chỗ ta xử lý các công tác liên quan đến Quan hàm và ,mua Quan Ấn. <br> Quan hàm của các thủ lĩnh sẽ tự động nhận , tương ứng với cấp Quan hàm của bang.<br>Quan Hàm của thành viên cổ đông do thủ lĩnh ban cho thông qua việc đặt chức quan.")
	dialog:AddSelection(2, "Danh vọng lãnh thổ tạp hóa")
	dialog:AddSelection(5, "Kết thúc đối thoại")
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
function PhoThieuKhanh:OnSelection(scene, npc, player, selectionID, otherParams)
	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	if selectionID == 2 then
		if player:GetFactionID() == 0 then
			dialog:AddText("Hãy gia nhập một môn phái rồi hãy tới gặp ta.")
			dialog:Show(npc, player)
			return
		end
		if player:GetLevel() >= 10 then
			if player:GetFactionID()== 1 or player:GetFactionID()== 2 then
				Player.OpenShop(npc, player,149)
				GUI.CloseDialog(player)
			elseif player:GetFactionID()== 3 or player:GetFactionID()== 4 or player:GetFactionID()== 11  then
				Player.OpenShop(npc, player,150)
				GUI.CloseDialog(player)
			elseif player:GetFactionID()== 5 or player:GetFactionID()== 6 or player:GetFactionID()== 12 then
				Player.OpenShop(npc, player,151)
				GUI.CloseDialog(player)
			elseif player:GetFactionID()== 7 or player:GetFactionID()== 8 then
				Player.OpenShop(npc, player,152)
				GUI.CloseDialog(player)
			elseif player:GetFactionID()== 9 or player:GetFactionID()== 10 then
				Player.OpenShop(npc, player,153)
				GUI.CloseDialog(player)
			end
			GUI.CloseDialog(player)
		else
			dialog:AddText("Chưa đủ cấp !")
			dialog:Show(npc, player)
		end
	end
	if selectionID == 5 then
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
function PhoThieuKhanh:OnItemSelected(scene, npc, player, itemID, otherParams)

	-- ************************** --
	
	-- ************************** --

end