-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000019' bên dưới thành ID tương ứng
local HoanNhanLongHuc = Scripts[000219]

--****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function HoanNhanLongHuc:OnOpen(scene, npc, player)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	if player:GetFactionID() == 0 then
		dialog:AddText("Bạn chưa gia nhập môn phái ,gia nhập môn phái rồi hãy quay lại!")
		dialog:AddSelection(6, "Kết thúc đối thoại.")
		dialog:Show(npc, player)
	else 
		dialog:AddText(""..npc:GetName()..": Gần đây nước Tống lắm kẻ tiểu nhân, không tự lượng sức ý đồ chống đối Tây hạ ta, người học võ như các ngươi ở lại nước tống cũng vô ích. Chi bằng đầu quân cho tây hạ để được thỏa chí tung hoàn")
        dialog:AddSelection(3, "Báo danh <color=#FFFF00> Chiến trường Liên Máy Chủ (Kim) </color>")
        dialog:AddSelection(4, "Mua trang bị chiến trường Dương Châu")
		dialog:AddSelection(5, "Mua trang bị chiến trường Phượng Tường")
		dialog:AddSelection(6, "Ta không mua mữa")
		dialog:Show(npc, player)
	end
	-- ************************** --
end

-- ****************************************************** --
--	Hàm này được gọi khi có sự kiện người chơi ấn vào một trong số các chức năng cung cấp bởi NPC thông qua NPC Dialog
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
--		selectionID: number - ID chức năng
-- ****************************************************** --
function HoanNhanLongHuc:OnSelection(scene, npc, player, selectionID)

	if selectionID == 1 then
        Player.JoinSongJinBattle(npc, player, "KIM", 1)
    elseif selectionID == 2 then
        Player.JoinSongJinBattle(npc, player, "KIM", 2)
    elseif selectionID == 3 then
        Player.JoinSongJinBattle(npc, player, "KIM", 3)
	elseif selectionID == 4 then 
		if player:GetLevel() >= 10 then
			if player:GetFactionID() == 1 then
				Player.OpenShop(npc, player,49)
			elseif player:GetFactionID() ==2 then
				Player.OpenShop(npc, player, 50)
			elseif player:GetFactionID() ==3 then
				Player.OpenShop(npc, player, 51)
			elseif player:GetFactionID() ==4 then
				Player.OpenShop(npc, player, 53)
			elseif player:GetFactionID() ==5 then
				Player.OpenShop(npc, player, 55)
			elseif player:GetFactionID() ==6 then
				Player.OpenShop(npc, player, 56)
			elseif player:GetFactionID() ==7 then
				Player.OpenShop(npc, player, 58)
			elseif player:GetFactionID() ==8 then
				Player.OpenShop(npc, player, 57)	
			elseif player:GetFactionID() ==9 then
				Player.OpenShop(npc, player, 59)
			elseif player:GetFactionID() ==10 then
				Player.OpenShop(npc, player, 60)	
			elseif player:GetFactionID() ==11 then
				Player.OpenShop(npc, player, 52)				
			elseif player:GetFactionID() == 12 then
				Player.OpenShop(npc, player, 54)
			end
			GUI.CloseDialog(player)
		else
			dialog:AddText("Chưa đủ cấp !!.")
			dialog:Show(npc, player)
		end
	elseif selectionID == 5 then 
		if player:GetLevel() >= 10 then
			if player:GetFactionID() == 1 then
				Player.OpenShop(npc, player,61)
			elseif player:GetFactionID() ==2 then
				Player.OpenShop(npc, player, 62)
			elseif player:GetFactionID() ==3 then
				Player.OpenShop(npc, player, 63)
			elseif player:GetFactionID() ==4 then
				Player.OpenShop(npc, player, 65)
			elseif player:GetFactionID() == 5 then
				Player.OpenShop(npc, player, 67)
			elseif player:GetFactionID() ==6 then
				Player.OpenShop(npc, player, 68)
			elseif player:GetFactionID() ==7 then
				Player.OpenShop(npc, player, 70)
			elseif player:GetFactionID() ==8 then
				Player.OpenShop(npc, player, 69)	
			elseif player:GetFactionID() ==9 then
				Player.OpenShop(npc, player, 71)
			elseif player:GetFactionID() ==10 then
				Player.OpenShop(npc, player, 72)	
			elseif player:GetFactionID() ==11 then
				Player.OpenShop(npc, player, 64)				
			elseif player:GetFactionID() == 12 then
				Player.OpenShop(npc, player, 66)
			end
			GUI.CloseDialog(player)
		else
			dialog:AddText("Chưa đủ cấp !!.")
			dialog:Show(npc, player)
		end
	end
	if selectionID == 6 then
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
function HoanNhanLongHuc:OnItemSelected(scene, npc, player, itemID)

	-- ************************** --

	-- ************************** --

end