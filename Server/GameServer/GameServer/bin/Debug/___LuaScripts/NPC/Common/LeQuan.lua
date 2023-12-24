-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000148' bên dưới thành ID tương ứng
local LeQuan = Scripts[000149]

--****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function LeQuan:OnOpen(scene, npc, player,otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText("Bạn hữu, làm thế nào ta có thể giúp bạn ?.")
	dialog:AddSelection(1,"Đổi đồng")
	dialog:AddSelection(3,"Mua tinh hoạt ưu đãi")
--	dialog:AddSelection(100030, "GiftCode")
	dialog:AddSelection(2, "Kết thúc đối thoại")
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
function LeQuan:OnSelection(scene, npc, player, selectionID, otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	if selectionID == 1 then
	
		Player.TokenTransClick(npc,player)
		
	end 
	if selectionID == 3 then
	
		Player.BuyDiscount(npc,player)
		
	end 
	if selectionID == 2 then
		GUI.CloseDialog(player)
	end
	if selectionID == 100030 then
		GUI.OpenUI(player, "UIGiftCode")
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
function LeQuan:OnItemSelected(scene, npc, player, itemID, otherParams)

	-- ************************** --

	-- ************************** --

end