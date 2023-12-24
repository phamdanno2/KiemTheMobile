-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '200005' bên dưới thành ID tương ứng
local TruyenThongPhu = Scripts[200005]

-- ****************************************************** --
--	Hàm này được gọi khi người chơi ấn sử dụng vật phẩm, kiểm tra điều kiện có được dùng hay không
--		scene: Scene - Bản đồ hiện tại
--		item: Item - Vật phẩm tương ứng
--		player: Player - NPC tương ứng
--	Return: True nếu thỏa mãn điều kiện có thể sử dụng, False nếu không thỏa mãn
-- ****************************************************** --
function TruyenThongPhu:OnPreCheckCondition(scene, item, player, otherParams)

	-- ************************** --
	return true
	-- ************************** --

end

-- ****************************************************** --
--	Hàm này được gọi để thực thi Logic khi người sử dụng vật phẩm, sau khi đã thỏa mãn hàm kiểm tra điều kiện
--		scene: Scene - Bản đồ hiện tại
--		item: Item - Vật phẩm tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function TruyenThongPhu:OnUse(scene, item, player, otherParams)

	-- ************************** --
	local dialog = GUI.CreateItemDialog()
	dialog:AddText("Ngươi muốn đi đâu?")
	dialog:AddSelection(1901, "Thành thị")
	dialog:AddSelection(1902, "Tân thủ thôn")
	dialog:AddSelection(1903, "Môn phái")
	-- dialog:AddSelection(1904, "Báo Danh Tống - Kim")
	dialog:AddSelection(1905, "Khu Vực Luyện Công")
	-- dialog:AddSelection(2999, "Tần Lăng")
	-- dialog:AddSelection(3000, "Phục Ngưu Sơn Quân Doanh")
	-- dialog:AddSelection(10011, "Dịch chuyển đến Hoàng Thành liên Server")
	dialog:AddSelection(1900, "Ta chưa cần...")
	dialog:Show(item, player)
	-- ************************** --

end

-- ****************************************************** --
--	Hàm này được gọi khi có sự kiện người chơi ấn vào một trong số các chức năng cung cấp bởi vật phẩm thông qua Item Dialog
--		scene: Scene - Bản đồ hiện tại
--		item: Item - Vật phẩm tương ứng
--		player: Player - NPC tương ứng
--		selectionID: number - ID chức năng
-- ****************************************************** --
function TruyenThongPhu:OnSelection(scene, item, player, selectionID, otherParams)

	-- ************************** --
	if selectionID == 1901 then
		local dialog = GUI.CreateItemDialog()
		dialog:AddText("Ngươi muốn đi đâu?")
		for key , value in pairs (Global_NameMapItemThanhID) do
			if value.ID ~= scene:GetID() then
				dialog:AddSelection(key, value.Name)
			end
		end
		dialog:AddSelection(1900, "Ta chưa cần...")
		dialog:Show(item, player)
		return
	end
	if selectionID == 10011 then
		player:ChangeScene(1616, 7545, 4454)
		GUI.CloseDialog(player)
		return
	end
	-- ************************** --
	if selectionID == 1902 then
		local dialog = GUI.CreateItemDialog()
		dialog:AddText("Ngươi muốn đi đâu?")
		for key , value in pairs (Global_NameMapItemThonID) do
			if value.ID ~= scene:GetID() then
				dialog:AddSelection(key, value.Name)
			end
		end
		dialog:AddSelection(1900, "Ta chưa cần...")
		dialog:Show(item, player)
		return
	end
	-- ************************** --
	if selectionID == 1903 then
		local dialog = GUI.CreateItemDialog()
		dialog:AddText("Ngươi muốn đi đâu?")
		for key , value in pairs (Global_NameMapItemPhaiID) do
			if value.ID ~= scene:GetID() then
				dialog:AddSelection(key, value.Name)
			end
		end
		dialog:AddSelection(1900, "Ta chưa cần...")
		dialog:Show(item, player)
	end
	if selectionID == 1904 then
		local dialog = GUI.CreateItemDialog()
		dialog:AddText("Hãy chọn Phe Tống hoặc Kim mà các hạ muốn Báo danh Chiến Đấu !!!")
		dialog:AddSelection(2900, "Báo Danh Phe Tống")
		dialog:AddSelection(2800, "Báo Danh Phe Kim")
		dialog:AddSelection(1900, "Ta chưa cần...")
		dialog:Show(item, player)
	-- ************************** --
	end
	-- ************************** --
	if selectionID == 2900 then
	-- ************************** --
    local dialog = GUI.CreateItemDialog()
    if player:GetFactionID()== 0 then
        dialog:AddText("Chiêu Mộ Sứ Tống :Bạn chưa gia nhập môn phái,gia nhập môn phái rồi quay lại!")
        dialog:AddSelection(1900, "Kết thúc đối thoại")
        dialog:Show(item, player)
    else
        dialog:AddText("Chiêu Mộ Sứ Tống: Gần đây đại Kim Quốc ý Bắc phạt, thu phục Trung Nguyên, nay hai quân đang đối đầu tại biên ải, Mộ Binh Lệnh đã truyền khắp nơi, ta đang định chiêu mộ nhân sĩ võ lâm, trợ giúp quân Tống khôi phục sơn hà\nThời gian báo danh :      Thời gian khai chiến :<color=green>\n10h50 - 10h59               11h00\n16h50 - 16h59               17h00\n20h50 - 20h59             21h00</color>\nĐến giờ khai chiến sẽ không thể báo danh được nữa !!!\nQuý nhân sĩ hãy chú ý thời gian báo danh để không bị lỡ sự kiện")
        dialog:AddSelection(2901, "Báo danh <color=#FFFF00> Chiến trường Dương Châu (Tống)</color>")
        dialog:AddSelection(2902, "Báo danh <color=#FFFF00> Chiến trường Phượng Tường (Tống)</color>")
        dialog:AddSelection(2903, "Báo danh <color=#FFFF00> Chiến trường Cao Phượng Tường (Tống)</color>")
        dialog:AddSelection(1900, "Ta sức hèn tài mọn , đợi khi tinh thông võ nghệ sẽ đến giúp ngươi.")
        dialog:Show(item, player)
    end
	-- ************************** --
	end
	-- *************Thuc Thi Bao Danh Tông************* --
	if selectionID == 2901 then
        Player.JoinSongJinBattleTTP( player, "TONG", 1)
    end
	if selectionID == 2902 then
        Player.JoinSongJinBattleTTP( player, "TONG", 2)
    end
	if selectionID == 2903 then
        Player.JoinSongJinBattleTTP( player, "TONG", 3)
	end
	-- *************Thuc Thi Bao Danh Tông************* --
	if selectionID == 2800 then
	-- ************************** --
	local dialog = GUI.CreateItemDialog()
	if player:GetFactionID() == 0 then
		dialog:AddText("Chiêu Mộ Sứ Kim: Bạn chưa gia nhập môn phái ,gia nhập môn phái rồi hãy quay lại!")
		dialog:AddSelection(1900, "Kết thúc đối thoại.")
		dialog:Show(item, player)
	else 
		dialog:AddText("Chiêu Mộ Sứ Kim: Gần đây nước Tống lắm kẻ tiểu nhân, không tự lượng sức ý đồ chống đối Tây hạ ta, người học võ như các ngươi ở lại nước tống cũng vô ích. Chi bằng đầu quân cho tây hạ để được thỏa chí tung hoàn\nThời gian báo danh :      Thời gian khai chiến :<color=green>\n10h50 - 10h59               11h00\n16h50 - 16h59               17h00\n20h50 - 20h59             21h00</color>\nĐến giờ khai chiến sẽ không thể báo danh được nữa !!!\nQuý nhân sĩ hãy chú ý thời gian báo danh để không bị lỡ sự kiện")
		dialog:AddSelection(2801, "Báo danh <color=#FFFF00> Chiến trường Dương Châu (Kim)</color>")
        dialog:AddSelection(2802, "Báo danh <color=#FFFF00> Chiến trường Phượng Tường (Kim)</color>")
        dialog:AddSelection(2803, "Báo danh <color=#FFFF00> Chiến trường Cao Phượng Tường (Kim) </color>")
		dialog:AddSelection(1900, "Ta sức hèn tài mọn , đợi khi tinh thông võ nghệ sẽ đến giúp ngươi.")
		dialog:Show(item, player)
	end
	-- ************************** --
	end
	-- *************Thuc Thi Bao Danh Kim************* --
	if selectionID == 2801 then
        Player.JoinSongJinBattleTTP(player, "KIM", 1)
    end
	if selectionID == 2802 then
        Player.JoinSongJinBattleTTP( player, "KIM", 2)
    end
	if selectionID == 2803 then
        Player.JoinSongJinBattleTTP( player, "KIM", 3)
	end
	-- *************Thuc Thi Bao Danh Kim************* --
	if selectionID == 1905 then
		local dialog = GUI.CreateItemDialog()
		dialog:AddText("Hãy chọn bản đồ luyện công phù hợp cấp độ của bạn")
		dialog:AddSelection(1910, "Bản đồ luyện cấp 5")
		dialog:AddSelection(1915, "Bản đồ luyện cấp 15")
		dialog:AddSelection(1925, "Bản đồ luyện cấp 25")
		dialog:AddSelection(1935, "Bản đồ luyện cấp 35")
		dialog:AddSelection(1945, "Bản đồ luyện cấp 45")
		dialog:AddSelection(1955, "Bản đồ luyện cấp 55")
		dialog:AddSelection(1965, "Bản đồ luyện cấp 65")
		dialog:AddSelection(1975, "Bản đồ luyện cấp 75")
		dialog:AddSelection(1985, "Bản đồ luyện cấp 85")
		dialog:AddSelection(1995, "Bản đồ luyện cấp 95")
		dialog:AddSelection(2905, "Bản đồ luyện cấp 105")
		-- dialog:AddSelection(2915, "Bản đồ luyện cấp 115")
		dialog:AddSelection(1900, "Ta chưa cần...")
		dialog:Show(item, player)
	-- ************************** --
	end	
	if selectionID == 1910 then
		local dialog = GUI.CreateItemDialog()
		dialog:AddText("Ngươi muốn đi đâu?")
		for key , value in pairs (Global_NameMapLuyenCongCap5ID) do
			if value.ID ~= scene:GetID() then
				dialog:AddSelection(key, value.Name)
			end
		end
		dialog:AddSelection(1900, "Ta chưa cần...")
		dialog:Show(item, player)
	end	
	if selectionID == 1915 then
		local dialog = GUI.CreateItemDialog()
		dialog:AddText("Ngươi muốn đi đâu?")
		for key , value in pairs (Global_NameMapLuyenCongCap15ID) do
			if value.ID ~= scene:GetID() then
				dialog:AddSelection(key, value.Name)
			end
		end
		dialog:AddSelection(1900, "Ta chưa cần...")
		dialog:Show(item, player)
	end	
	if selectionID == 1925 then
		local dialog = GUI.CreateItemDialog()
		dialog:AddText("Ngươi muốn đi đâu?")
		for key , value in pairs (Global_NameMapLuyenCongCap25ID) do
			if value.ID ~= scene:GetID() then
				dialog:AddSelection(key, value.Name)
			end
		end
		dialog:AddSelection(1900, "Ta chưa cần...")
		dialog:Show(item, player)
	end	
	if selectionID == 1935 then
		local dialog = GUI.CreateItemDialog()
		dialog:AddText("Ngươi muốn đi đâu?")
		for key , value in pairs (Global_NameMapLuyenCongCap35ID) do
			if value.ID ~= scene:GetID() then
				dialog:AddSelection(key, value.Name)
			end
		end
		dialog:AddSelection(1900, "Ta chưa cần...")
		dialog:Show(item, player)
	end	
	if selectionID == 1945 then
		local dialog = GUI.CreateItemDialog()
		dialog:AddText("Ngươi muốn đi đâu?")
		for key , value in pairs (Global_NameMapLuyenCongCap45ID) do
			if value.ID ~= scene:GetID() then
				dialog:AddSelection(key, value.Name)
			end
		end
		dialog:AddSelection(1900, "Ta chưa cần...")
		dialog:Show(item, player)
	end	
	if selectionID == 1955 then
		local dialog = GUI.CreateItemDialog()
		dialog:AddText("Ngươi muốn đi đâu?")
		for key , value in pairs (Global_NameMapLuyenCongCap55ID) do
			if value.ID ~= scene:GetID() then
				dialog:AddSelection(key, value.Name)
			end
		end
		dialog:AddSelection(1900, "Ta chưa cần...")
		dialog:Show(item, player)
	end
	if selectionID == 1965 then
		local dialog = GUI.CreateItemDialog()
		dialog:AddText("Ngươi muốn đi đâu?")
		for key , value in pairs (Global_NameMapLuyenCongCap65ID) do
			if value.ID ~= scene:GetID() then
				dialog:AddSelection(key, value.Name)
			end
		end
		dialog:AddSelection(1900, "Ta chưa cần...")
		dialog:Show(item, player)
	end
	if selectionID == 1975 then
		local dialog = GUI.CreateItemDialog()
		dialog:AddText("Ngươi muốn đi đâu?")
		for key , value in pairs (Global_NameMapLuyenCongCap75ID) do
			if value.ID ~= scene:GetID() then
				dialog:AddSelection(key, value.Name)
			end
		end
		dialog:AddSelection(1900, "Ta chưa cần...")
		dialog:Show(item, player)
	end
	if selectionID == 1985 then
		local dialog = GUI.CreateItemDialog()
		dialog:AddText("Ngươi muốn đi đâu?")
		for key , value in pairs (Global_NameMapLuyenCongCap85ID) do
			if value.ID ~= scene:GetID() then
				dialog:AddSelection(key, value.Name)
			end
		end
		dialog:AddSelection(1900, "Ta chưa cần...")
		dialog:Show(item, player)
	end
	if selectionID == 1995 then
		local dialog = GUI.CreateItemDialog()
		dialog:AddText("Ngươi muốn đi đâu?")
		for key , value in pairs (Global_NameMapLuyenCongCap95ID) do
			if value.ID ~= scene:GetID() then
				dialog:AddSelection(key, value.Name)
			end
		end
		dialog:AddSelection(1900, "Ta chưa cần...")
		dialog:Show(item, player)
	end
	if selectionID == 2905 then
		local dialog = GUI.CreateItemDialog()
		dialog:AddText("Ngươi muốn đi đâu?")
		for key , value in pairs (Global_NameMapLuyenCongCap105ID) do
			if value.ID ~= scene:GetID() then
				dialog:AddSelection(key, value.Name)
			end
		end
		dialog:AddSelection(1900, "Ta chưa cần...")
		dialog:Show(item, player)
	end	
	if selectionID == 2915 then
		local dialog = GUI.CreateItemDialog()
		dialog:AddText("Ngươi muốn đi đâu?")
		for key , value in pairs (Global_NameMapLuyenCongCap115ID) do
			if value.ID ~= scene:GetID() then
				dialog:AddSelection(key, value.Name)
			end
		end
		dialog:AddSelection(1900, "Ta chưa cần...")
		dialog:Show(item, player)
	end	
--------------------------------------	
	if selectionID == 3000 then
		player:ChangeScene(556, 3350, 3790)
		GUI.CloseDialog(player)
		return
	end

--------------------------------------	
	if selectionID == 2999 then
		-- player:ChangeScene(1536, 3991, 6362)
		-- GUI.CloseDialog(player)
		-- return
		-- Kiểm tra điều kiện
		local ret = EventManager.EmperorTomb_EnterMap_CheckCondition(player)
		-- Nếu không thỏa mãn
		if ret ~= "OK" then
			TruyenThongPhu:ShowNotify(item, player, ret)
			return
		end
		
		-- Vào Tần Lăng
		EventManager.EmperorTomb_MoveToMap(player)
		
		return
	end
	if selectionID == 1900 then
		GUI.CloseDialog(player)
		return
	end
	-- ************************** --
	if Global_NameMapItemThanhID[selectionID] ~= nil then
		player:ChangeScene(Global_NameMapItemThanhID[selectionID].ID, Global_NameMapItemThanhID[selectionID].PosX, Global_NameMapItemThanhID[selectionID].PosY)
		GUI.CloseDialog(player)
		return
	elseif Global_NameMapItemThonID[selectionID] ~= nil then
		player:ChangeScene(Global_NameMapItemThonID[selectionID].ID, Global_NameMapItemThonID[selectionID].PosX, Global_NameMapItemThonID[selectionID].PosY)
		GUI.CloseDialog(player)
		return
	elseif Global_NameMapLuyenCongCap5ID[selectionID] ~= nil then
		player:ChangeScene(Global_NameMapLuyenCongCap5ID[selectionID].ID, Global_NameMapLuyenCongCap5ID[selectionID].PosX, Global_NameMapLuyenCongCap5ID[selectionID].PosY)
		GUI.CloseDialog(player)
		return
	elseif Global_NameMapLuyenCongCap15ID[selectionID] ~= nil then
		player:ChangeScene(Global_NameMapLuyenCongCap15ID[selectionID].ID, Global_NameMapLuyenCongCap15ID[selectionID].PosX, Global_NameMapLuyenCongCap15ID[selectionID].PosY)
		GUI.CloseDialog(player)
		return
	elseif Global_NameMapLuyenCongCap25ID[selectionID] ~= nil then
		player:ChangeScene(Global_NameMapLuyenCongCap25ID[selectionID].ID, Global_NameMapLuyenCongCap25ID[selectionID].PosX, Global_NameMapLuyenCongCap25ID[selectionID].PosY)
		GUI.CloseDialog(player)
		return
	elseif Global_NameMapLuyenCongCap35ID[selectionID] ~= nil then
		player:ChangeScene(Global_NameMapLuyenCongCap35ID[selectionID].ID, Global_NameMapLuyenCongCap35ID[selectionID].PosX, Global_NameMapLuyenCongCap35ID[selectionID].PosY)
		GUI.CloseDialog(player)
		return
	elseif Global_NameMapLuyenCongCap45ID[selectionID] ~= nil then
		player:ChangeScene(Global_NameMapLuyenCongCap45ID[selectionID].ID, Global_NameMapLuyenCongCap45ID[selectionID].PosX, Global_NameMapLuyenCongCap45ID[selectionID].PosY)
		GUI.CloseDialog(player)
		return
	elseif Global_NameMapLuyenCongCap55ID[selectionID] ~= nil then
		player:ChangeScene(Global_NameMapLuyenCongCap55ID[selectionID].ID, Global_NameMapLuyenCongCap55ID[selectionID].PosX, Global_NameMapLuyenCongCap55ID[selectionID].PosY)
		GUI.CloseDialog(player)
		return
	elseif Global_NameMapLuyenCongCap65ID[selectionID] ~= nil then
		player:ChangeScene(Global_NameMapLuyenCongCap65ID[selectionID].ID, Global_NameMapLuyenCongCap65ID[selectionID].PosX, Global_NameMapLuyenCongCap65ID[selectionID].PosY)
		GUI.CloseDialog(player)
		return
	elseif Global_NameMapLuyenCongCap75ID[selectionID] ~= nil then
		player:ChangeScene(Global_NameMapLuyenCongCap75ID[selectionID].ID, Global_NameMapLuyenCongCap75ID[selectionID].PosX, Global_NameMapLuyenCongCap75ID[selectionID].PosY)
		GUI.CloseDialog(player)
		return
	elseif Global_NameMapLuyenCongCap85ID[selectionID] ~= nil then
		player:ChangeScene(Global_NameMapLuyenCongCap85ID[selectionID].ID, Global_NameMapLuyenCongCap85ID[selectionID].PosX, Global_NameMapLuyenCongCap85ID[selectionID].PosY)
		GUI.CloseDialog(player)
		return
	elseif Global_NameMapLuyenCongCap95ID[selectionID] ~= nil then
		player:ChangeScene(Global_NameMapLuyenCongCap95ID[selectionID].ID, Global_NameMapLuyenCongCap95ID[selectionID].PosX, Global_NameMapLuyenCongCap95ID[selectionID].PosY)
		GUI.CloseDialog(player)
		return
	elseif Global_NameMapLuyenCongCap105ID[selectionID] ~= nil then
		player:ChangeScene(Global_NameMapLuyenCongCap105ID[selectionID].ID, Global_NameMapLuyenCongCap105ID[selectionID].PosX, Global_NameMapLuyenCongCap105ID[selectionID].PosY)
		GUI.CloseDialog(player)
		return
	elseif Global_NameMapLuyenCongCap115ID[selectionID] ~= nil then
		player:ChangeScene(Global_NameMapLuyenCongCap115ID[selectionID].ID, Global_NameMapLuyenCongCap115ID[selectionID].PosX, Global_NameMapLuyenCongCap115ID[selectionID].PosY)
		GUI.CloseDialog(player)
		return
	elseif Global_NameMapItemPhaiID[selectionID] ~= nil then
		player:ChangeScene(Global_NameMapItemPhaiID[selectionID].ID, Global_NameMapItemPhaiID[selectionID].PosX, Global_NameMapItemPhaiID[selectionID].PosY)
		GUI.CloseDialog(player)
		return
	end
	-- ************************** --

end
function TruyenThongPhu:ShowNotify(item, player, text)

	-- ************************** --
	local dialog = GUI.CreateItemDialog()
	dialog:AddText(text)
	dialog:AddSelection(1900, "Kết thúc đối thoại")
	dialog:Show(item, player)
	-- ************************** --

end
-- ****************************************************** --
--	Hàm này được gọi khi có sự kiện người chơi chọn một trong các vật phẩm, và ấn nút Xác nhận cung cấp bởi vật phẩm thông qua Item Dialog
--		scene: Scene - Bản đồ hiện tại
--		item: Item - Vật phẩm tương ứng
--		player: Player - NPC tương ứng
--		itemID: number - ID vật phẩm được chọn
-- ****************************************************** --
function TruyenThongPhu:OnItemSelected(scene, item, player, itemID)

	-- ************************** --

	-- ************************** --

end