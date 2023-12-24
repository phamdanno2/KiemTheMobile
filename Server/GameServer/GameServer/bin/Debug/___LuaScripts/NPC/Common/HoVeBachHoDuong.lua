-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000032' bên dưới thành ID tương ứng
local HoVeBachHoDuong = Scripts[000032]

--****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function HoVeBachHoDuong:OnOpen(scene, npc, player, otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText('Dạo này trong Bạch Hổ Đường xuất hiện đầy bọn trộm cắp, bọn ta không đủ sức đối phó với chúng. Ngươi có thể giúp một tay không? Ta có thể đưa ngươi đến "Đại Điện Bạch Hổ Đường", sự việc cụ thể ngươi có thể tìm hiểu ở chỗ "Môn Đồ Bạch Hổ Đường"')
	dialog:AddSelection(1, "Bạn muốn vào Bạch Hổ(Hoàng Kim)")
	dialog:AddSelection(2, "[Quy tắc hoạt động]")
	dialog:AddSelection(3, "Mua trang bị danh vọng Bạch Hổ Đường")
	dialog:AddSelection(4, "Kết thúc đối thoại")
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
function HoVeBachHoDuong:OnSelection(scene, npc, player, selectionID,otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	if selectionID == 1 then
		player:ChangeScene(225, 4729, 2601)	
	elseif selectionID == 2 then
		dialog:AddText("Thời gian báo danh 30 phút, thời gian hoạt động 30 phút. Sau khi hoạt động bắt đầu, trong Bạch Hổ Đường sẽ xuất hiện rất nhiều <color=red>Sấm Đường Tặc</color>, đánh bại chúng sẽ nhặt được vật phẩm và kinh nghệm. Sau một thời gian nhất định sẽ xuất hiện <color=red>Thủ Lĩnh Sấm Dường Tặc</color>, đánh bại <color=red>Thủ Linh Sấm Đường Tặc</color> sẽ xuất hiện lối vào tâng 2. Bạch Hổ Đường có 3 tâng, nếu bạn đánh bại thủ lĩnh ở cả 3 tầng thì sẽ mở được lối ra.<br><color=yellow><u>Lưu ý:</u></color> Khi vào Bạch Hổ Đường sẽ tự động bật chế độ chiến đấu bang hội, nên tốt nhất hãy tham gia hoạt động này cùng với hảo hữu hoặc bang hội (Mỗi ngày chỉ được tham gia tối đa <color=orange>%s lần</color>).")	
		dialog:AddSelection(4, "Kết thúc đối thoại")
		dialog:Show(npc, player)
	elseif selectionID == 3 then 
		if player:GetLevel() >= 10 then
			if player:GetFactionID() == 1 then
				Player.OpenShop(npc, player,221)
				GUI.CloseDialog(player)
			elseif player:GetFactionID() ==2 then
				Player.OpenShop(npc, player, 221)
				GUI.CloseDialog(player)
			elseif player:GetFactionID() ==3 then
				Player.OpenShop(npc, player, 222)
				GUI.CloseDialog(player)
			elseif player:GetFactionID() ==4 then
				Player.OpenShop(npc, player, 222)
				GUI.CloseDialog(player)
			elseif player:GetFactionID() ==5 then
				Player.OpenShop(npc, player, 223)
				GUI.CloseDialog(player)
			elseif player:GetFactionID() ==6 then
				Player.OpenShop(npc, player, 223)
				GUI.CloseDialog(player)
			elseif player:GetFactionID() ==7 then
				Player.OpenShop(npc, player, 224)
				GUI.CloseDialog(player)
			elseif player:GetFactionID() ==8 then
				Player.OpenShop(npc, player, 224)
				GUI.CloseDialog(player)	
			elseif player:GetFactionID() ==9 then
				Player.OpenShop(npc, player, 225)
				GUI.CloseDialog(player)
			elseif player:GetFactionID() ==10 then
				Player.OpenShop(npc, player, 225)
				GUI.CloseDialog(player)	
			elseif player:GetFactionID() ==11 then
				Player.OpenShop(npc, player, 222)
				GUI.CloseDialog(player)				
			elseif player:GetFactionID() == 12 then
				Player.OpenShop(npc, player, 223)
				GUI.CloseDialog(player)
			end
		else
			dialog:AddText("Chưa đủ cấp !!.")
			dialog:Show(npc, player)
		end
	end
	if selectionID == 4  then
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
function HoVeBachHoDuong:OnItemSelected(scene, npc, player, itemID,otherParams)

	-- ************************** --

	-- ************************** --

end