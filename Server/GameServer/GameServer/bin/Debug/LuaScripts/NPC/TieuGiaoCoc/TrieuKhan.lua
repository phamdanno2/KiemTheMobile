-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000021' bên dưới thành ID tương ứng
local TrieuKhan = Scripts[000159]

--****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function TrieuKhan:OnOpen(scene, npc, player, otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText(" Trong cốc rất nguy hiểm, để cho an toàn,<color=#00CC00>mỗi người mỗi ngày chỉ được vào cốc tối đa 3 lần, phải tạo thành nhóm ít nhất 1 người</color>, lão phu mới cho các ngươi vào. Lập đội xong, đội trưởng hãy đến chỗ ta báo danh. Mỗi ngày từ </color=#efd561>06:00 đến 23:00</color>, lão phu sẽ dẫn các vị vào cốc.")
	dialog:AddSelection(1, "Tiêu Dao cốc 1 (Đã có 0 đội vào côc) ")
	dialog:AddSelection(2, "Tiêu Dao cốc 2(Đã có 0 đội vào côc)")
	dialog:AddSelection(3, "Tiêu Dao cốc 3(Đã có 0 đội vào côc)")
	dialog:AddSelection(4, "Ta vẫn chưa chuẩn bị xong ta quay lại sau")
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
function TrieuKhan:OnSelection(scene, npc, player, selectionID, otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
    
	-- ************************** --
	
	-- ************************** --

end

-- ****************************************************** --
--	Hàm này được gọi khi có sự kiện người chơi chọn một trong các vật phẩm, và ấn nút Xác nhận cung cấp bởi NPC thông qua NPC Dialog
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
--		itemID: number - ID vật phẩm được chọn
-- ****************************************************** --
function TrieuKhan:OnItemSelected(scene, npc, player, itemID, otherParams)

	-- ************************** --

	-- ************************** --

end