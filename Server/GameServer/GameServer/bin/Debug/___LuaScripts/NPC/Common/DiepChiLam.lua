-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000036' bên dưới thành ID tương ứng
local DiepChiLam = Scripts[000036]

--****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function DiepChiLam:OnOpen(scene, npc, player, otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText("Ta có thể giúp ngươi giải quyết vấn đề về các mối quan hệ, ngươi cần giúp gì?")
	dialog:AddSelection(1, "Quan hệ sư đồ")
	dialog:AddSelection(2, "Quan hệ người giới thiệu")
	dialog:AddSelection(3, "Chỉ định mật hữu")
	dialog:AddSelection(4, "Nhận thưởng mật hữu")
	dialog:AddSelection(5, "ta chỉ tiện đường đến xem")
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
function DiepChiLam:OnSelection(scene, npc, player, selectionID,otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	if selectionID == 5 then
		GUI.CloseDialog(player)
	elseif selectionID == 1 then
		dialog:AddText("TODO")
		dialog:Show(npc, player)
	elseif selectionID == 2 then
		dialog:AddText("TODO")
		dialog:Show(npc, player)
	elseif selectionID == 3 then
		dialog:AddText("TODO")
		dialog:Show(npc, player)
	elseif selectionID == 4 then
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
function DiepChiLam:OnItemSelected(scene, npc, player, itemID,otherParams)

	-- ************************** --

	-- ************************** --

end