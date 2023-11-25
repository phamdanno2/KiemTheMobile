-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000012' bên dưới thành ID tương ứng
local ThuKho = Scripts[000012]

-- *************************************************s***** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function ThuKho:OnOpen(scene, npc, player,otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText("Ta hứa sẽ luôn ở đây bảo quản đồ đạc thận cẩn thận")
	if scene:GetID()== 255 or scene:GetID()==1635 or scene:GetID()== 341 then
		dialog:AddSelection(1, "Mở thương khố")
		dialog:AddSelection(3, "Kết thúc đối thoại")
		dialog:Show(npc, player)
	else
		dialog:AddSelection(1, "Mở thương khố")
		dialog:AddSelection(2, "Lưu điểm về thành")
		dialog:AddSelection(3, "Kết thúc đối thoại")
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
function ThuKho:OnSelection(scene, npc, player, selectionID, otherParams)

	-- ************************** --
	if selectionID == 1 then
		Player.PortableGoods(npc,player)
	elseif selectionID == 2 then
		local npcPos = npc:GetPos()
		player:SetDefaultReliveInfo(scene:GetID(), npcPos:GetX(), npcPos:GetY())
		GUI.ShowNotification(player, "Lưu điểm về thành thành công!")
		GUI.CloseDialog(player)
	elseif selectionID == 3 then
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
function ThuKho:OnItemSelected(scene, npc, player, itemID, otherParams)

	-- ************************** --
	
	-- ************************** --

end