-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000137' bên dưới thành ID tương ứng
local HoangPhi = Scripts[000137]

--****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function HoangPhi:OnOpen(scene, npc, player, otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText("Nghe đông trong Tiêu Dao Cốc này có nhiều bảo vật quý giá, ta muốn vào cốc xem một phên nhưng không ai cùng đi, Triệu lão đâu nhất quyết không cho ta vào cốc. Xem nhà ngươi thân thủ bất phàm, nếu có thể giúp ta vào cốc tìm những báu vật đó thì những gia bảo tùy thân của ta đây cho ngươi tùy chọn! thế nào?")
	dialog:AddSelection(1, "Nhìn coi đây có phải bảo vật bạn cần.")
	dialog:AddSelection(2, "Xem nhà người có gì hay")
	dialog:AddSelection(3, "<color=#FFFF66>Tìm hiểu tiêu dao lục</color>")
	dialog:AddSelection(4, "Nhận thuốc miễn phí hôm nay")
	dialog:AddSelection(5, "Xem kỷ lục vượt ải tiêu dao cốc")
	dialog:AddSelection(6, "kết thúc đối thoại.")
		
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
function HoangPhi:OnSelection(scene, npc, player, selectionID,otherParams)

	-- ************************** --
	if selectionID ==6 then
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
function HoangPhi:OnItemSelected(scene, npc, player, itemID,otherParams)

	-- ************************** --

	-- ************************** --

end