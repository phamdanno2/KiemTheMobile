-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '0000152' bên dưới thành ID tương ứng
local ChuongMon = Scripts[0000152]

--****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function ChuongMon:OnOpen(scene, npc, player,otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	if player:GetFactionID() == 0 then 
		dialog:AddText("Chưởng Môn: Ngươi có cốt cách dị thường tư chất hơn người, mai đây ắt sẽ làm nên nghiệp lớn. <br><color=#00ff44>Nếu muốn quay về Tân Thủ Thôn, hãy đến gặp người Truyền Tống Môn Phái.</color>")
		dialog:AddSelection(1, "Sư Phụ, xin nhận của đệ tử một lạy.")
		dialog:AddSelection(2, "Để ta suy nghĩ sau.")
		dialog:Show(npc, player)
	elseif player:GetFactionID()~=nil then
		for key ,value in pairs (Global_NameMapItemPhaiID) do
			if key == scene:GetID() then
				if value.FactionID == player:GetFactionID() then
					dialog:AddText("Chưởng Môn: Ngươi đã là để tử "..player:GetFactionName()..",nếu muốn đúng bản đồ môn phái về tân Thủ Thôn ,hãy đối thoại với Truyền Tống Môn Phái.")
					dialog:AddSelection(3, "Nhận ngũ hành ấn")
					dialog:AddSelection(4, "Nhận Mật tịch Môn Phái")
					dialog:AddSelection(5, "Ta muốn tiến hành tu luyện")
					dialog:AddSelection(6, "Ta muốn mua trang bị thi đấu mốn phái")
					dialog:AddSelection(7, "Hoạt động thi đấu môn phái")
					dialog:AddSelection(8, "Ta muốn vào Tẩy Tủy Đảo tẩy điểm")
					dialog:AddSelection(10, "Được phong Đại sư huynh(Đại sư tỷ)")
					dialog:AddSelection(2, "Để ta suy nghĩ sau.")
					dialog:Show(npc, player)
				else
					dialog:AddText("Chưởng Môn: Ngươi đã là để tử phái "..player:GetFactionName().." Không thể gia nhập bổn phái.")
					dialog:AddSelection(2, "Kết thúc đối thoại.")
					dialog:Show(npc, player)
					
				end
			end
		end
	
	end
end

	-- ************************** --


-- ****************************************************** --
--	Hàm này được gọi khi có sự kiện người chơi ấn vào một trong số các chức năng cung cấp bởi NPC thông qua NPC Dialog
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
--		selectionID: number - ID chức năng
-- ****************************************************** --
function ChuongMon:OnSelection(scene, npc, player, selectionID,otherParams)
	
	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	if selectionID == 1 then
        for key ,value in pairs (Global_NameMapItemPhaiID) do
			if key == scene:GetID() then
				ChuongMon:JoinFaction(scene, npc, player, value.FactionID)
             end
	    end
    elseif selectionID == 6 then
		if player:GetLevel() >= 10 then
			if player:GetFactionID() == 1 then
				Player.OpenShop(npc, player,25)
			elseif player:GetFactionID() == 2 then
				Player.OpenShop(npc, player, 26)
			elseif player:GetFactionID() == 3 then
				Player.OpenShop(npc, player, 27)
			elseif player:GetFactionID() == 4 then
				Player.OpenShop(npc, player, 29)
			elseif player:GetFactionID() == 5 then
				Player.OpenShop(npc, player, 31)
			elseif player:GetFactionID() == 6 then
				Player.OpenShop(npc, player, 32)
			elseif player:GetFactionID() == 7 then
				Player.OpenShop(npc, player, 34)
			elseif player:GetFactionID() == 8 then
				Player.OpenShop(npc, player, 33)
			elseif player:GetFactionID() == 9 then
				Player.OpenShop(npc, player, 35)
			elseif player:GetFactionID() == 10 then
				Player.OpenShop(npc, player, 36)
			elseif player:GetFactionID() == 11 then
				Player.OpenShop(npc, player, 28)
			elseif player:GetFactionID() == 12 then
				Player.OpenShop(npc, player, 30)
			end
			GUI.CloseDialog(player)
		else
			dialog:AddText("Chưa đủ cấp !!.")
			dialog:Show(npc, player)
		end
	end
	if selectionID == 8 then
		dialog:AddText("Ngươi đã vào 1 lần, cần 1 lệnh bài tẩy tủy đảo , có thể mua ở khu Kỳ Trân Các.")
		dialog:AddSelection(100,"Ta Muốn đi bây giờ")
		dialog:AddSelection(2,"Kết thúc đối thoại")
		dialog:Show(npc, player)
		
	end
	if 	selectionID == 100 then
		--if player:CountItemInBag() == 336 then
			player:ChangeScene(255,1750,1930)
			player:RemoveItem(player,336)
		--else 	
		--	dialog:AddText("Không đủ số lượng Lệnh Bài Tẩy Tủy Đảo.")
		----	dialog:AddSelection(2,"Kết thúc đối thoại")
			dialog:Show(npc, player)
		--end
	end
	if selectionID == 3 then
		dialog:AddText("Ngươi có muốn nhận Ngũ Hành Ấn ")
		dialog:AddSelection(101, "Đồng ý")
		dialog:AddSelection(2,"Kết thúc đối thoại")
		dialog:Show(npc, player)
		--Player.AddItemLua()
	end
	if selectionID == 4 then
		dialog:AddText("Ngươi có muốn Mật tịch Môn Phái ")
		dialog:AddSelection(102, "Đồng ý")
		dialog:AddSelection(2,"Kết thúc đối thoại")
		dialog:Show(npc, player)
		--Player.AddItemLua()
	end
	if selectionID == 101 then
		for key ,value in pairs (Global_NameMapItemPhaiID) do
			if key == scene:GetID() then
				if value.FactionID == player:GetFactionID() then
					Player.AddItemLua(player,value.press,1,Player.GetSeries(player),1)
					dialog:AddText("Bạn đã nhận được Ngũ Hành Ấn ")
					dialog:AddSelection(2,"Kết thúc đối thoại")
					dialog:Show(npc, player)
				end
             end
	    end
	end
	if selectionID==102 then
		for key ,value in pairs (Global_NameMapItemPhaiID) do
			if key == scene:GetID() then
				if value.FactionID == player:GetFactionID() then
					Player:AddItemLua(player,value.secret1,1,Player.GetSeries(player),1)
					Player:AddItemLua(player,value.secret2,1,Player.GetSeries(player),1)
					dialog:AddText("Bạn đã nhận được Mật tịch Môn Phái ")
					dialog:AddSelection(2,"Kết thúc đối thoại")
					dialog:Show(npc, player)
				end
             end
	    end
	end
	if  selectionID == 5  or selectionID == 7 or  selectionID == 10 then
		dialog:AddText("Tính năng đang hoàn thiện")
		dialog:Show(npc, player)
	end
	if selectionID ==2 then
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
function ChuongMon:OnItemSelected(scene, npc, player, itemID,otherParams)

	-- ************************** --
	
	-- ************************** --

end
function ChuongMon:JoinFaction(scene, npc, player, factionID)
	
	-- ************************** --
	local ret = player:JoinFaction(factionID)
	-- ************************** --
	if ret == -1 then
		ChuongMon:ShowDialog(npc, player, "Người chơi không tồn tại!")
		return
	elseif ret == -2 then
		ChuongMon:ShowDialog(npc, player, "Môn phái không tồn tại!")
		return
	elseif ret == 0 then
		ChuongMon:ShowDialog(npc, player, "Giới tính của bạn không phù hợp với môn phái này!")
		return
	elseif ret == 1 then
		ChuongMon:ShowDialog(npc, player, "Gia nhập phái <color=blue>"..player:GetFactionName().."</color> thành công!")
		return
	else
		ChuongMon:ShowDialog(npc, player, "Chuyển phái thất bại, lỗi chưa rõ!")
		return
	end
	-- ************************** --

end
function ChuongMon:ShowDialog(npc, player, text)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText(text)
	dialog:Show(npc, player)
	-- ************************** --
	
end