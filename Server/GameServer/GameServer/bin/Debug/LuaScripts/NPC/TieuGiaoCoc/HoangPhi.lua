-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000021' bên dưới thành ID tương ứng
local HoangPhi = Scripts[000160]

--****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function HoangPhi:OnOpen(scene, npc, player, otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText("Nghe nói Tiêu Dao cốc có một kho báu, ta muốn tới đó dể khám phá nhưng ta muốn tới đó để khám phá nhưng chưa tìm được đồng hành ,ta không được phép đi. Xem ra ngươi có khả năng phi thường, nếu có thể vào thung lũng để giúp ta có một vào món đồ tốt m ngươi có thể lấy! ngươi muốn không ?<br> Vâng đúng! Ta nghe nói trong thung lũng có một bậc thầy chế tác đồ , nhưng ta không biết đó là ai ... Ngươi hãy đi từ từ để tì thấy ông ấy...")
	dialog:AddSelection(1, "Ta có món này quý này ")
	dialog:AddSelection(2, "Cho ta xem ngươi có mốn gì tốt")
	dialog:AddSelection(3, "Nhận thuốc Tiêu Dao Cốc")
	dialog:AddSelection(4, "Nhận thuốc Tiêu Dao cốc")
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
function HoangPhi:OnSelection(scene, npc, player, selectionID, otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
    
	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	if selectionID == 1 then
		Player.OpenShop(npc, player, 132)
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
function HoangPhi:OnItemSelected(scene, npc, player, itemID, otherParams)

	-- ************************** --

	-- ************************** --

end