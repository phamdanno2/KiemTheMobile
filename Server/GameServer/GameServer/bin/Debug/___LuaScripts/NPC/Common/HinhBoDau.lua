-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000060' bên dưới thành ID tương ứng
local HinhBoDau = Scripts[202]
local RecordKey = 1652123 -- RecordKey
-- ****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function HinhBoDau:OnOpen(scene, npc, player, otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	-- ************************** --
	local record = Player.GetValueForeverRecore(player, RecordKey)
	if player:GetPKValue(player) == 0 then
		dialog:AddText("Không ngờ gặp ngươi ở nơi này. Muốn đi khỏi đây, trước tiên phải trả lời câu hỏi của ta trước. Muốn đi đâu?")
		dialog:AddSelection(3, "Thành thị")
		dialog:AddSelection(4, "Tân thủ thôn")
		dialog:AddSelection(5, "Môn phái")
		dialog:AddSelection(2, "Kết thúc đối thoại")
	else 
		dialog:AddText("Ngươi sát khí quá nặng,đã được ta tha bổng cho "..record.." lần  muốn đi khỏi đây thì phải đưa ta <color=yellow>"..((record+1)*1000).." knb</color>, <color=yellow>"..((record+1)*(100000/10000)).." vạn bạc</color> để xóa sát khí. Còn không thì chịu khó ngồi đây xám hối khi nào hết sát khí ta sẽ tha!")
		dialog:AddSelection(1, "Ta muốn tẩy sát khí")
		dialog:AddSelection(2, "Kết thúc đối thoại")	
	end
	-- ************************** --
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
function HinhBoDau:OnSelection(scene, npc, player, selectionID, otherParams)
	local record = Player.GetValueForeverRecore(player, RecordKey)
	-- ************************** --
	if selectionID == 2 then
		GUI.CloseDialog(player)
		return
	end
	-- ************************** --
	if selectionID == 1 then
		local dialog = GUI.CreateNPCDialog()
		dialog:AddText("Ngươi đồng ý dùng <color=yellow>"..((record+1)*1000).." knb</color>, <color=yellow>"..((record+1)*100000).." vạn bạc</color> để tẩy toàn bộ sát khí?")
		dialog:AddSelection(11, "Xác nhận")
		dialog:AddSelection(2, "Hủy bỏ")
		dialog:Show(npc, player)
		return
	end
	-- ************************** --
	if selectionID == 11 then
		-- Nếu không đủ đồng
		if Player.CheckMoney(player, 2) < ((record+1)*1000) then
			HinhBoDau:ShowNotify(npc, player, "Ngươi không có đủ <color=yellow>"..((record+1)*1000).." đồng</color>, không thể tẩy sát khí!")
			return
		-- Nếu không đủ bạc
		elseif Player.CheckMoney(player, 1) < ((record+1)*100000) then
			HinhBoDau:ShowNotify(npc, player, "Ngươi không có đủ <color=yellow>"..((record+1)*(100000/10000)).." vạn bạc</color>, không thể tẩy sát khí!")
			return
		end
		
		-- Thực hiện xóa bạc và đồng
		Player.MinusMoney(player, 2, ((record+1)*1000))
		Player.MinusMoney(player, 1, ((record+1)*100000))
		Player.SetPKValue(player, 0)
		
		HinhBoDau:ShowNotify(npc, player, "Tẩy sát khí thành công. Ngươi có thể rời khỏi đây rồi. Từ nay nhớ hướng thiện, đừng để phải quay lại đây lần nữa!")
		return
	end
	-- ************************** --
	if selectionID == 3 then
		local dialog = GUI.CreateNPCDialog()
		dialog:AddText("Ngươi muốn đi đâu?")
		for key, value in pairs (Global_NameMapThanhID) do
			if key ~= scene:GetID() then
				dialog:AddSelection(key, value.Name)
			end
		end
		dialog:Show(npc,player)
		
		return
	end
	-- ************************** --
	if selectionID == 4 then
		local dialog = GUI.CreateNPCDialog()
		dialog:AddText("Ngươi muốn đi đâu ?")
		for key, value in pairs (Global_NameMapThonID) do
			if value.ID ~= scene:GetID() then
				dialog:AddSelection(key, value.Name)
			end
		end
		dialog:Show(npc, player)
		
		return
	end
	-- ************************** --
	if selectionID == 5 then
		local dialog = GUI.CreateNPCDialog()
		dialog:AddText("Ngươi muốn đi đâu ?")
		for key, value in pairs (Global_NameMapPhaiID) do
			if value.ID ~= scene:GetID() then
				dialog:AddSelection(key, value.Name)
			end
		end
		dialog:Show(npc, player)
		
		return
	end
	-- ************************** --
	-- Nếu chọn dịch đến thành thị
	if Global_NameMapThanhID[selectionID] ~= nil then
		-- Bật Captcha lên
		local ret = Player.OpenCaptcha(player, function(isCorrect)
			-- Nếu trả lời đúng
			if isCorrect == true then
				player:ChangeScene(Global_NameMapThanhID[selectionID].ID,Global_NameMapThanhID[selectionID].PosX,Global_NameMapThanhID[selectionID].PosY)
			end
		end)
		-- Nếu toác
		if ret == false then
			HinhBoDau:ShowNotify(npc, player, "Ngươi vừa trả lời sai, hãy kiên nhẫn đợi giây lát ta sẽ thẩm vấn lại!")
			return
		end
		
		-- Đóng khung
		GUI.CloseDialog(player)
		
		return
	-- Nếu chọn dịch đến thôn
	elseif Global_NameMapThonID[selectionID] ~= nil then
		-- Bật Captcha lên
		local ret = Player.OpenCaptcha(player, function(isCorrect)
			-- Nếu trả lời đúng
			if isCorrect == true then
				player:ChangeScene(Global_NameMapThonID[selectionID].ID,Global_NameMapThonID[selectionID].PosX,Global_NameMapThonID[selectionID].PosY)
			end
		end)
		-- Nếu toác
		if ret == false then
			HinhBoDau:ShowNotify(npc, player, "Ngươi vừa trả lời sai, hãy kiên nhẫn đợi giây lát ta sẽ thẩm vấn lại!")
			return
		end
		
		-- Đóng khung
		GUI.CloseDialog(player)
		
		return
	-- Nếu chọn dịch đến môn phái
	elseif Global_NameMapPhaiID[selectionID] ~= nil then
		-- Bật Captcha lên
		local ret = Player.OpenCaptcha(player, function(isCorrect)
			-- Nếu trả lời đúng
			if isCorrect == true then
				player:ChangeScene(Global_NameMapPhaiID[selectionID].ID,Global_NameMapPhaiID[selectionID].PosX,Global_NameMapPhaiID[selectionID].PosY)
			end
		end)
		-- Nếu toác
		if ret == false then
			HinhBoDau:ShowNotify(npc, player, "Ngươi vừa trả lời sai, hãy kiên nhẫn đợi giây lát ta sẽ thẩm vấn lại!")
			return
		end
		
		-- Đóng khung
		GUI.CloseDialog(player)
		
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
function HinhBoDau:OnItemSelected(scene, npc, player, itemID, otherParams)

	-- ************************** --

	-- ************************** --

end


-- ==================================================== --
-- ==================================================== --
function HinhBoDau:ShowNotify(npc, player, text)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText(text)
	dialog:AddSelection(2, "Kết thúc đối thoại")
	dialog:Show(npc, player)
	-- ************************** --
	
end