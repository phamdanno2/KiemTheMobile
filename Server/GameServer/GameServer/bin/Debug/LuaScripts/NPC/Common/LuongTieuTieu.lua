-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000013' bên dưới thành ID tương ứng
local LuongTieuTieu = Scripts[900001]

--****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function LuongTieuTieu:OnOpen(scene, npc, player,otherParams)

	-- ************************** --	
	local dialog = GUI.CreateNPCDialog()
		dialog:AddText("Tần lăng là nơi nguy hiểm trùng trùng điệp điệp ! Ngươi có sẵn sàng đi tới đó ?")
		dialog:AddSelection(101,"Đưa ta đi")
		dialog:AddSelection(100,"Để ta suy nghĩ lại")
		dialog:Show(npc, player)
	-- ************************** --

end

-- ****************************************************** --
-- ****************************************************** --
function LuongTieuTieu:OnSelection(scene, npc, player, selectionID, otherParams)
	
	if   selectionID == 100 then
		GUI.CloseDialog(player)
	elseif selectionID == 101 then
		player:ChangeScene(1536, 7545, 4454)
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
function LuongTieuTieu:OnItemSelected(scene, npc, player, itemID, otherParams)

	-- ************************** --
	
	-- ************************** --

end
