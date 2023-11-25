-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000064' bên dưới thành ID tương ứng
local ChuyenTongMonPhai = Scripts[000064]

--****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function ChuyenTongMonPhai:OnOpen(scene, npc, player, otherParams)

	-- ************************** --
	
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText("Ngươi muốn đi đâu ?")
	dialog:AddSelection(101,"Thành Thị")
	dialog:AddSelection(102,"Tân Thủ Thôn")
	dialog:AddSelection(100,"Không cần")
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
function ChuyenTongMonPhai:OnSelection(scene, npc, player, selectionID,otherParams)

	
	if selectionID == 101 then
		local dialog = GUI.CreateNPCDialog()
		dialog:AddText("Ngươi muốn đi đâu ?")
		for key ,value in pairs (Global_NameMapThanhID) do
			if key ~= scene:GetID() then
				dialog:AddSelection(key,value.Name)
			end
		end
		dialog:Show(npc,player)
	  
	-- ************************** --
	
	elseif   selectionID == 102 then
		local dialog = GUI.CreateNPCDialog()
		dialog:AddText("Ngươi muốn đi đâu ?")
		for key ,value in pairs (Global_NameMapThonID) do
			if value.ID ~= scene:GetID() then
				dialog:AddSelection(key,value.Name)
			end
		end
		dialog:Show(npc, player)
	elseif selectionID == 100  then
		GUI.CloseDialog(Player)
	end
	-- ************************** --

	-- ************************** --
	if Global_NameMapThanhID[selectionID] ~= nil then
		player:ChangeScene(Global_NameMapThanhID[selectionID].ID,Global_NameMapThanhID[selectionID].PosX,Global_NameMapThanhID[selectionID].PosY)
	
	-- ************************** --
	elseif Global_NameMapThonID[selectionID] ~= nil then
		player:ChangeScene(Global_NameMapThonID[selectionID].ID,Global_NameMapThonID[selectionID].PosX,Global_NameMapThonID[selectionID].PosY)
	end
	-- ************************** --
	--elseif Global_NameMapPhaiID[selectionID] ~= nil then
	--	player:ChangeScene(Global_NameMapPhaiID[selectionID].ID,Global_NameMapPhaiID[selectionID].PosX,Global_NameMapPhaiID[selectionID].PosY)
	--return
	
end
	
	



-- ****************************************************** --
--	Hàm này được gọi khi có sự kiện người chơi chọn một trong các vật phẩm, và ấn nút Xác nhận cung cấp bởi NPC thông qua NPC Dialog
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
--		itemID: number - ID vật phẩm được chọn
-- ****************************************************** --
function ChuyenTongMonPhai:OnItemSelected(scene, npc, player, itemID,otherParams)

	-- ************************** --
	
	-- ************************** --

end
function ChuyenTongMonPhai:ChangeScene(player,ID,PosX, PosY)

	-- ************************** --
		ChuyenTongMonPhai:ChangeScene(player,ID,Posx,PosY)
	
	-- ************************** --

end
function ChuyenTongMonPhai:ShowDialog(npc, player, text)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText(text)
	dialog:Show(npc, player)
	-- ************************** --
	
end