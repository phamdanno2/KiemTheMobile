-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000148' bên dưới thành ID tương ứng
local LongNguThaiGia = Scripts[000148]

--****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function LongNguThaiGia:OnOpen(scene, npc, player,otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText("Ta có thể giúp gì cho ngươi?")
	dialog:AddSelection(1, "Tạp Hóa Nguyệt Ảnh Thạch")
	dialog:AddSelection(2, "Tạp Hóa Du Long")
	dialog:AddSelection(3, "Vào Mật Thất Du Long")
	dialog:AddSelection(100, "Ta chỉ tình cờ đi qua")
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
function LongNguThaiGia:OnSelection(scene, npc, player, selectionID, otherParams)

	-- ************************** --
	if selectionID == 1 then
		Player.OpenShop(npc, player, 166)
		GUI.CloseDialog(player)
	elseif selectionID == 2 then
		Player.OpenShop(npc, player, 167)
		GUI.CloseDialog(player)
	elseif selectionID == 3 then
		-- Kiểm tra điều kiện
		local ret = EventManager.YouLong_CheckCondition(player)
		if ret ~= "OK" then
			local dialog = GUI.CreateNPCDialog()
			dialog:AddText(ret)
			dialog:Show(npc, player)
			return
		end
		
		-- Vào phụ bản
		EventManager.YouLong_Begin(player)
	elseif selectionID == 100 then
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
function LongNguThaiGia:OnItemSelected(scene, npc, player, itemID, otherParams)

	-- ************************** --

	-- ************************** --

end