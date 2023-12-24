-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000038' bên dưới thành ID tương ứng
local TrieuKhan = Scripts[000038]

--****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function TrieuKhan:OnOpen(scene, npc, player,otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText("Dạo này có rất nhiều người muốn đến Tiêu Dao cốc, ngươi cũng vậy sao?")
	dialog:AddSelection(1, "Đưa ta đến Cổng Tiêu Dao Cốc 1")
	dialog:AddSelection(2, "Đưa ta đến Cổng Tiêu Dao Cốc 2")
	dialog:AddSelection(3, "Ta chỉ ghé qua thôi")
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
function TrieuKhan:OnSelection(scene, npc, player,otherParamsselectionID,otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	if selectionID == 3 then
		GUI.CloseDialog(player)
	elseif  selectionID == 1 or selectionID == 2 then
		dialog:AddText("TODO")
		dialog:Show(npc, player)
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
function TrieuKhan:OnItemSelected(scene, npc, player,otherParamsitemID,otherParams)

	-- ************************** --

	-- ************************** --

end