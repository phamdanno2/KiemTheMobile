-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000002' bên dưới thành ID tương ứng
local NPC_Test = Scripts[000002]

-- ****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - Người chơi tương ứng
-- ****************************************************** --
function NPC_Test:OnOpen(scene, npc, player)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()			-- Hiển thị 1 bảng NPC Dialog
	-- ************************** --
	dialog:AddText("ABCXYZ~~~~")
	dialog:AddSelection(10, "Selection ID 10")		-- Selection ID ở đầu, tiếp theo là chữ hiển thị
	dialog:AddSelection(2, "Selection ID 2")
	dialog:Show(npc, player)						-- Hiện Dialog cho người chơi tương ứng
	-- ************************** --

end

-- ****************************************************** --
--	Hàm này được gọi khi có sự kiện người chơi ấn vào một trong số các chức năng cung cấp bởi NPC thông qua NPC Dialog
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - Người chơi tương ứng
--		selectionID: number - ID chức năng
-- ****************************************************** --
function NPC_Test:OnSelection(scene, npc, player, selectionID)

	-- ************************** --
	System.WriteToConsole("SelectionID = " .. selectionID)	-- In ra Debug Console
	-- ************************** --
	if selectionID == 10 then
		local dialog = GUI.CreateNPCDialog()
		dialog:AddText("Dialog con~~~~")
		dialog:AddSelection(3, "Option 3")
		dialog:Show(npc, player)
		return
	end
	-- ************************** --
	if selectionID == 3 then
		GUI.ShowNotification(player, "Selection 3")			-- Hiển thị Text cho người dùng nhìn ở Game
		return
	end
	-- ************************** --

end

-- ****************************************************** --
--	Hàm này được gọi khi có sự kiện người chơi chọn một trong các vật phẩm, và ấn nút Xác nhận cung cấp bởi NPC thông qua NPC Dialog
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - Người chơi tương ứng
--		itemID: number - ID vật phẩm được chọn
-- ****************************************************** --
function NPC_Test:OnItemSelected(scene, npc, player, itemID)

	-- ************************** --
	
	-- ************************** --

end