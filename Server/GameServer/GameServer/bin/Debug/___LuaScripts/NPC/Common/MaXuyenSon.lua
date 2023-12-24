-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '200' bên dưới thành ID tương ứng
local MaXuyenSon = Scripts[200]

-- ************************** --
local ItemIDSub = 371			-- Vật phẩm Đồng Tiền Cổ
local ItemIDAdd = 626			-- Vật phẩm Rương Gia Tộc
-- ************************** --

--****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function MaXuyenSon:OnOpen(scene, npc, player,otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText("Gần đây ta phát hiện được một bảo khố thần bí, nghe nói các nho sinh ở đây đều theo lý học, võ công bất phàm, chẵng lẽ trong bảo khố có bí mật gì ư?")
	dialog:AddSelection(1, "Thuyết minh hoạt động Vượt ải gia tộc")
	dialog:AddSelection(2, "Mua Trang bị gia tộc")
	dialog:AddSelection(3, "Dùng Đồng Tiền Cổ đổi phần thưởng")
	-- Nếu có tộc và là tộc trưởng
	if player:GetFamilyID() > 0 and player:GetFamilyRank() == Global_FamilyRank.Master then
		dialog:AddSelection(4, "Bắt đầu Vượt ải gia tộc")
		dialog:AddSelection(5, "Danh sách thành viên có thể tham gia")
	end
	dialog:AddSelection(1000, "Kết thúc đối thoại")
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
function MaXuyenSon:OnSelection(scene, npc, player, selectionID, otherParams)
	
	-- ************************** --
	if selectionID == 1000 then
		GUI.CloseDialog(player)
		return
	end
	-- ************************** --
	if selectionID == 1 then
		self:ShowDialog(npc, player, "Hoạt động <color=yellow>Vượt ải gia tộc</color> do Tộc trưởng mở, trước khi mở yêu cầu các thành viên tập trung xung quanh vị trí của Tộc trưởng.<br>Độ khó được điều chỉnh theo số người, càng nhiều người càng khó.<br><color=green>Chú ý: Mỗi tuần chỉ mở 2 lần, thời gian vượt ải tối đa 2 giờ, bất luận thế nào sau 2 giờ tất cả người chơi sẽ được đưa về thành.</color>")
		return
	end
	-- ************************** --
	if selectionID == 2 then
		if player:GetFactionID() == 0 then
			self:ShowDialog(npc, player, "Ngươi chưa gia nhập môn phái!")
			return
		elseif player:GetLevel() < 10 then
			self:ShowDialog(npc, player, "Ngươi cấp độ không đủ 10, không thể mở cửa hàng!")
			return
		end
		
		-- Mở Shop theo phái tương ứng
		if player:GetFactionID() == 1 then
			Player.OpenShop(npc, player, 77)
		elseif player:GetFactionID() == 2 then
			Player.OpenShop(npc, player, 78)
		elseif player:GetFactionID() == 3 then
			Player.OpenShop(npc, player, 79)
		elseif player:GetFactionID() == 4 then
			Player.OpenShop(npc, player, 81)
		elseif player:GetFactionID() == 11 then
			Player.OpenShop(npc, player, 80)
		elseif player:GetFactionID() == 5 then
			Player.OpenShop(npc, player, 83)
		elseif player:GetFactionID() == 6 then
			Player.OpenShop(npc, player, 84)
		elseif player:GetFactionID() == 12 then
			Player.OpenShop(npc, player, 82)
		elseif player:GetFactionID() == 7 then
			Player.OpenShop(npc, player, 86)
		elseif player:GetFactionID() == 8 then
			Player.OpenShop(npc, player, 85)
		elseif player:GetFactionID() == 9 then
			Player.OpenShop(npc, player, 87)
		elseif player:GetFactionID() == 10 then
			Player.OpenShop(npc, player, 88)
		end
		
		GUI.CloseDialog(player)
		return
	end
	-- ************************** --
	if selectionID == 3 then
		local dialog = GUI.CreateNPCDialog()
		dialog:AddText("Ta đã không nhìn lầm người, ngươi thực sự rất mạnh mẽ! Ngươi thu thập được rất nhiều Đồng Tiền Cổ, phải không? Ta sẽ đổi kho báu của ta để lấy nó!")
		dialog:AddSelection(30, "Dùng 100 Đồng Tiền Cổ đổi phần thưởng")
		dialog:AddSelection(1000, "Ta suy nghĩ đã")
		dialog:Show(npc, player)
		return
	end
	-- ************************** --
	if selectionID == 30 then 
		-- Nếu không đủ Đồng Tiền Cổ
		if Player.CountItemInBag(player, ItemIDSub) < 100 then
			self:ShowDialog(npc, player, "Ta cần 100 Đồng Tiền Cổ, có đủ rồi hãy đưa ta. Ta đang rất bận!")
			return
		end 
		
		-- Xóa Đồng Tiền Cổ
		Player.RemoveItem(player, ItemIDSub, 100)
		-- Lượng kinh nghiệm cơ bản
		local nExpAdd = Player.GetBaseExp(player, player:GetLevel())* 30 * 2
		
		-- Tạo Rương Gia Tộc
		Player.AddItemLua(player, ItemIDAdd, 1, -1, 1)
		-- Thêm kinh nghiệm tương ứng
		Player.AddRoleExp(player, nExpAdd)
		
		self:ShowDialog(npc, player, "Cảm ơn ngươi về những Đồng Tiền Cổ này, đây là phần thưởng của ngươi, hãy nhận nó!")
		return
	end
	-- ************************** --
	if selectionID == 4 then
		-- Kiểm tra điều kiện
		local ret = EventManager.FamilyFuBen_CheckCondition(player)
		-- Nếu không thỏa mãn
		if ret ~= "OK" then
			self:ShowDialog(npc, player, ret)
			return
		end
		
		-- Bắt đầu Vượt ải gia tộc
		EventManager.FamilyFuBen_Begin(player)
		GUI.CloseDialog(player)
		return
	end
	-- ************************** --
	if selectionID == 5 then
		-- Nếu không có tộc
		if player:GetFamilyID() <= 0 then
			self:ShowDialog(npc, player, "Ngươi không có gia tộc!")
			return
		-- Nếu không phải tộc trưởng
		elseif player:GetFamilyRank() ~= Global_FamilyRank.Master then
			self:ShowDialog(npc, player, "Ngươi không phải Tộc trưởng!")
			return
		end
		
		local szMsg = EventManager.FamilyFuBen_GetNearByFamilyMembersToJoinEventDescription(player)
		self:ShowDialog(npc, player, szMsg)
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
function MaXuyenSon:OnItemSelected(scene, npc, player, itemID, otherParams)

	-- ************************** --
	
	-- ************************** --

end

-- ============================================================== --
-- ============================================================== --
function MaXuyenSon:ShowDialog(npc, player, text)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText(text)
	dialog:AddSelection(1000, "Kết thúc đối thoại")
	dialog:Show(npc, player)
	-- ************************** --

end