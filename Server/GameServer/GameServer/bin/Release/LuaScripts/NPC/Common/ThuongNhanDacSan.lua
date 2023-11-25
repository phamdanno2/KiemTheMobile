-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000031' bên dưới thành ID tương ứng
local ThuongNhanDacSan = Scripts[000031]

--****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function ThuongNhanDacSan:OnOpen(scene, npc, player,otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText("Những thứ ta bán đều là đặc sản của vùng này, ngươi có muốn xem thử không?")
	dialog:AddSelection(1, "Đặc sản")
	dialog:AddSelection(2, "Không mua nữa")
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
function ThuongNhanDacSan:OnSelection(scene, npc, player, selectionID, otherParams)

	-- ************************** --
	if selectionID == 1 then
	     if scene:GetID() ==23 then
			Player.OpenShop(npc, player, 101)
		 elseif scene:GetID() == 24 then
			Player.OpenShop(npc, player, 102)
		 elseif  scene:GetID() ==25 then
			Player.OpenShop(npc, player, 103)
		 elseif  scene:GetID() ==27 then
			Player.OpenShop(npc, player, 104)
		 elseif  scene:GetID() ==28 then
			Player.OpenShop(npc, player, 105)
		 elseif  scene:GetID() ==26 then
			Player.OpenShop(npc, player, 106)
		 elseif  scene:GetID() ==29 then
			Player.OpenShop(npc, player, 107)
		 elseif  scene:GetID() ==2 then
			Player.OpenShop(npc, player, 108)
		 elseif  scene:GetID() ==1 then
			Player.OpenShop(npc, player, 109)
		 elseif  scene:GetID() ==3 then
			Player.OpenShop(npc, player, 110)
		 elseif  scene:GetID() ==4 then
			Player.OpenShop(npc, player, 111)
		 elseif  scene:GetID() ==5 then
			Player.OpenShop(npc, player, 112)
		 elseif  scene:GetID() ==8 then
			Player.OpenShop(npc, player, 113)
		 elseif  scene:GetID() ==7 then
			Player.OpenShop(npc, player, 114)
		 elseif  scene:GetID() ==6 then
			Player.OpenShop(npc, player, 115)
		 elseif  scene:GetID() ==9 then
			Player.OpenShop(npc, player, 116)
		 elseif  scene:GetID() ==22 then
			Player.OpenShop(npc, player, 117)
		 elseif  scene:GetID() ==18 then
			Player.OpenShop(npc, player, 118)
		 elseif  scene:GetID() ==20 then
			Player.OpenShop(npc, player, 119)
		 elseif  scene:GetID() ==11 then
			Player.OpenShop(npc, player, 120)
		 elseif  scene:GetID() ==16 then
			Player.OpenShop(npc, player, 121)
		 elseif  scene:GetID() ==17 then
			Player.OpenShop(npc, player, 122)
		 elseif  scene:GetID() ==19 then
			Player.OpenShop(npc, player, 123)
		 elseif  scene:GetID() ==15 then
			Player.OpenShop(npc, player, 124)
		 elseif  scene:GetID() ==10 then
			Player.OpenShop(npc, player, 125)
		 elseif  scene:GetID() ==14 then
			Player.OpenShop(npc, player, 126)
		 elseif  scene:GetID() ==12 then
			Player.OpenShop(npc, player, 103)
		 end
		GUI.CloseDialog(player)
	end
	if selectionID == 2 then
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
function ThuongNhanDacSan:OnItemSelected(scene, npc, player, itemID, otherParams)

	-- ************************** --

	-- ************************** --

end