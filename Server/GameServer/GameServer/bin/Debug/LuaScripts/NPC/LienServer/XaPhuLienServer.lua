-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000013' bên dưới thành ID tương ứng
local PhuXa = Scripts[203]

--****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function PhuXa:OnOpen(scene, npc, player,otherParams)

	-- ************************** --
	
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText("Về Tân Thủ Thôn ")
	dialog:AddSelection(100012,"Về Thôn Ngay")
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
function PhuXa:OnSelection(scene, npc, player, selectionID, otherParams)
	
	
	if selectionID == 100012 then
		player:ChangeScene(5, 6141, 2227)
		GUI.CloseDialog(player)
		return
	end
	if   selectionID == 100 then
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