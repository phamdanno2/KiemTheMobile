-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000041' bên dưới thành ID tương ứng
local HoangThuong = Scripts[000041]

--****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng	Player.OpenShop(npc, player, 133)
--		player: Player - NPC tương ứng
-- ****************************************************** --
function HoangThuong:OnOpen(scene, npc, player, otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText("Chỉ cần độ thân mật đật đến 100, đẳng cấp phải đạt đến 50 thì đến chỗ ta lập lập bang hội.")
	if player:GetFamilyID() <= 0 then
		dialog:AddSelection(1,"Lập gia tộc")
	end
		
	if player:GetFamilyID() > 0 and player:GetFamilyRank() == Global_FamilyRank.Master then
		dialog:AddSelection(2,"Giải tán gia tộc")
	end
	if player:GetGuildID() <= 0 and player:GetFamilyID() > 0 and player:GetFamilyRank() == Global_FamilyRank.Master then
		dialog:AddSelection(3,"Lập bang hội")
	end
	
	if player:GetGuildID() > 0 then
		dialog:AddSelection(10,"Cửa hàng bang hội")
	end
	
	if player:GetGuildID() > 0 and player:GetGuildRank() == Global_GuildRank.Master then
		dialog:AddSelection(4,"Giải tán bang hội")
	end
		dialog:AddSelection(5,"Danh sách gia tộc chiêu mộ")
	dialog:AddSelection(100, "Kết thúc đối thoại")
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
function HoangThuong:OnSelection(scene, npc, player, selectionID, otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	if selectionID == 100 then
		GUI.CloseDialog(player)
		return
	end
	-- ************************** --
	if selectionID == 1 then
		if player:GetFamilyID() > 0 then
			self:ShowMessageBox(npc, player, "Bạn đã có gia tộc, không thể tạo thêm!")
			return
		end
		
		GUI.OpenUI(player, "UICreateFamily")
		GUI.CloseDialog(player)
		return
	end
	-- ************************** --
	if selectionID == 2 then
		if player:GetFamilyID() <= 0 then
			self:ShowMessageBox(npc, player, "Bạn không có gia tộc, không thể giải tán!")
			return
		elseif player:GetFamilyRank() ~= Global_FamilyRank.Master then
			self:ShowMessageBox(npc, player, "Chỉ có tộc trưởng mới có thể giải tán gia tộc!")
			return
		elseif player:GetGuildID() > 0 then
			self:ShowMessageBox(npc, player, "Bạn phải thoát khỏi bang hội trước, sau đó mới có thể giải tán gia tộc!")
			return
		elseif  player:GetFactionID() == 0 then
			self:ShowMessageBox(npc, player, "Bạn phải gia nhập môn phái mới lập được gia tộc.")
		end
		
		Player.RetainFamily(player)
		GUI.CloseDialog(player)
		return
	end
	-- ************************** --
	if selectionID == 3 then
		if player:GetGuildID() > 0 then
			self:ShowMessageBox(npc, player, "Bạn đã có bang hội, không thể tạo thêm!")
			return
		elseif player:GetFamilyID() <= 0 then
			self:ShowMessageBox(npc, player, "Bạn không có gia tộc, không thể tạo bang hội!")
			return
		elseif player:GetFamilyRank() ~= Global_FamilyRank.Master then
			self:ShowMessageBox(npc, player, "Chỉ có tộc trưởng mới có thể tạo bang!")
			return
		elseif  player:GetFactionID() == 0 then
			self:ShowMessageBox(npc, player, "Bạn phải gia nhập môn phái mới lập được bang hội.")
		end
		GUI.OpenUI(player, "UICreateGuild")
		GUI.CloseDialog(player)
		return
	end
	
	if selectionID == 10 then
		if player:GetGuildID() <= 0 then
			self:ShowMessageBox(npc, player, "Bạn không có bang hội, không thể giải tán!")
			return
		
		end
		
		Player.OpenShop(npc, player, 226)
		GUI.CloseDialog(player)
		return
	end
	-- ************************** --
	if selectionID == 4 then
		if player:GetGuildID() <= 0 then
			self:ShowMessageBox(npc, player, "Bạn không có bang hội, không thể giải tán!")
			return
		elseif player:GetGuildRank() ~= Global_GuildRank.Master then
			self:ShowMessageBox(npc, player, "Chỉ có bang chủ mới có thể giải tán bang hội!")
			return
		end
		
		Player.RetainGuild(player)
		GUI.CloseDialog(player)
		return
	end
	if selectionID==5 then
		GUI.OpenUI(player, "UIFamilyList")
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
function HoangThuong:OnItemSelected(scene, npc, player, itemID,otherParams)

	-- ************************** --

	-- ************************** --

end

-- ****************************************************** --
-- ****************************************************** --
function HoangThuong:ShowMessageBox(npc, player, msg)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText(msg)
	dialog:Show(npc, player)
	-- ************************** --

end