-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000013' bên dưới thành ID tương ứng
local PhuXa = Scripts[000013]

--****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function PhuXa:OnOpen(scene, npc, player,otherParams)

	-- ************************** --
	
	local dialog = GUI.CreateNPCDialog()
	if scene:GetID()==1616 then
		dialog:AddText("Về Tân Thủ Thôn ")
		dialog:AddSelection(100012,"Về Thôn Ngay")
		dialog:AddSelection(100,"Không cần")
		dialog:Show(npc, player)
	
	else
		dialog:AddText("Ngươi muốn đi đâu ?")
		dialog:AddSelection(101,"Thành Thị")
		dialog:AddSelection(102,"Tân Thủ Thôn")
		dialog:AddSelection(103,"Môn Phái")
		dialog:AddSelection(10011, "Dịch chuyển đến Hoàng Thành liên Server")
		dialog:AddSelection(100,"Không cần")
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
function PhuXa:OnSelection(scene, npc, player, selectionID, otherParams)
	
	if   selectionID == 100 then
		GUI.CloseDialog(player)
	elseif selectionID == 101 then
		local dialog = GUI.CreateNPCDialog()
		dialog:AddText("Ngươi muốn đi đâu ?")
		for key ,value in pairs (Global_NameMapThanhID) do
			if key ~= scene:GetID() then
				dialog:AddSelection(key,value.Name)
			end
		end
		dialog:Show(npc,player)
	elseif selectionID == 102 then
		local dialog = GUI.CreateNPCDialog()
		dialog:AddText("Ngươi muốn đi đâu ?")
		for key ,value in pairs (Global_NameMapThonID) do
			if value.ID ~= scene:GetID() then
				dialog:AddSelection(key,value.Name)
			end
		end
		dialog:Show(npc, player)
	elseif selectionID == 103 then
		local dialog = GUI.CreateNPCDialog()
		dialog:AddText("Ngươi muốn đi đâu ?")
  		  for key ,value in pairs (Global_NameMapPhaiID) do
			if key ~= scene:GetID() then
				dialog:AddSelection(key,value.Name)
			end
		end
		dialog:Show(npc, player)
	end
	-- ************************** --
	if Global_NameMapThanhID[selectionID] ~= nil then
		player:ChangeScene(Global_NameMapThanhID[selectionID].ID,Global_NameMapThanhID[selectionID].PosX,Global_NameMapThanhID[selectionID].PosY)
		GUI.CloseDialog(player)
		-- ************************** --
	elseif Global_NameMapThonID[selectionID] ~= nil then
		player:ChangeScene(Global_NameMapThonID[selectionID].ID,Global_NameMapThonID[selectionID].PosX,Global_NameMapThonID[selectionID].PosY)
		GUI.CloseDialog(player)
		-- ************************** --
	elseif Global_NameMapPhaiID[selectionID] ~= nil then
		player:ChangeScene(Global_NameMapPhaiID[selectionID].ID,Global_NameMapPhaiID[selectionID].PosX,Global_NameMapPhaiID[selectionID].PosY)
		GUI.CloseDialog(player)
	end
	if selectionID == 10011 then
		player:ChangeScene(1616, 7545, 4454)
		GUI.CloseDialog(player)
		return
	end
	if selectionID == 100012 then
		player:ChangeScene(5, 6141, 2227)
		GUI.CloseDialog(player)
		return
	end
end

-- ****************************************************** --
--	Hàm này được gọi khi có sự kiện người chơi chọn một trong các vật phẩm, và ấn nút Xác nhận cung cấp bởi NPC thông qua NPC Dialog
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
--		itemID: number - ID vật phẩm được chọn
-- ****************************************************** --
function PhuXa:OnItemSelected(scene, npc, player, itemID, otherParams)

	-- ************************** --
	
	-- ************************** --

end
function PhuXa:ChangeScene(player,ID,PosX, PosY)

	-- ************************** --
		PhuXa:ChangeScene(player,ID,Posx,PosY)
	
	-- ************************** --

end
function PhuXa:ShowDialog(npc, player, text)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText(text)
	dialog:Show(npc, player)
	-- ************************** --
	
end