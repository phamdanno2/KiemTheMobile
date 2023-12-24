-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000151' bên dưới thành ID tương ứng
local CoPhongHa = Scripts[000151]

-- ****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function CoPhongHa:OnOpen(scene, npc, player, otherParams)

	local dialog = GUI.CreateNPCDialog()
	dialog:AddText("Nhân chi sơ, Tính bản thiện . Ta có thể giúp gì cho nhà ngươi ")
	dialog:AddSelection(1, "<color=#fbe66f>[Hệ thống]</color> <color=#06f455>Nhận đồng trên web<color>.")
	dialog:AddSelection(2, "<color=#51e1fb>Nhận thưởng hoạt động <color>.")
	dialog:Show(npc, player)					

end

-- ****************************************************** --
--	Hàm này được gọi khi có sự kiện người chơi ấn vào một trong số các chức năng cung cấp bởi NPC thông qua NPC Dialog
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
--		selectionID: number - ID chức năng
-- ****************************************************** --
function CoPhongHa:OnSelection(scene, npc, player, selectionID, otherParams)
	local dialog = GUI.CreateNPCDialog()
	if selectionID == 1 then
		dialog:AddText("Xin chào"..player:GetName()..",Hình như bạn không có nạp trên hệ thống nên không có đồng cho bạn rút!!!")
		dialog:AddSelection(100, "<color=#51e1fb>Kết thúc đối thoại <color>.")
		dialog:Show(npc, player)
	end	
	if selectionID ==2 then
		dialog:AddText("Sau đây là danh sách các hoạt động mà bạn có thể nhận được thưởng")
		--todo hoạt động --
		dialog:AddSelection(200, "<color=#51e1fb>Kết thúc đối thoại <color>.")
		
		dialog:Show(npc, player)
	end
	
end

-- ****************************************************** --
--	Hàm này được gọi khi có sự kiện người chơi chọn một trong các vật phẩm, và ấn nút Xác nhận cung cấp bởi NPC thông qua NPC Dialog
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
--		itemID: number - ID vật phẩm được chọn
-- ****************************************************** --
function CoPhongHa:OnItemSelected(scene, npc, player, itemID, otherParams)

	-- ************************** --
	
	-- ************************** --

end