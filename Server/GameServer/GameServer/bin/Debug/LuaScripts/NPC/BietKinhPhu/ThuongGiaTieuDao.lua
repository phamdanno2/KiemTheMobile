-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000039' bên dưới thành ID tương ứng
local ThuongGiaTieuDao = Scripts[000039]
local ItemIDSub = 819			-- Vật phẩm Hòa Thị Bích
local ItemCount =5				-- Số lượng bị trừ Hòa thị Bích
local ItemIDAdd = 828			-- Vật phẩm Vũ khí Thanh Đồng-Luyện Hóa Đồ

--****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function ThuongGiaTieuDao:OnOpen(scene, npc, player,otherParams )

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText("Nhắc đến Tiêu Dao Cốc, đó quả là tiên cảnh giữa cõi người! Không biết thiếu hiệp có từng đến đó chưa? Ai da, lạc đề rồi, lạc đề rồi… Thiếu hiệp có muốn xem thử trang bị cực phẩm thế gian hiếm có không?")
	dialog:AddSelection(1, "Mua Yêu Đái Danh Vọng Thịnh Hạ 2008")
	dialog:AddSelection(2, "Mua Hộ Phù Danh Vọng Chúc Phúc")
	dialog:AddSelection(3, "Mua giày hoạt động Đoàn Viên")
	dialog:AddSelection(4, "Mua trang bị danh vọng lãnh thổ")
	dialog:AddSelection(5, "Mua trang bị danh vọng liên đấu")
	dialog:AddSelection(7, "Mua trang bị danh vọng Di Tích Hàn Vũ")
	dialog:AddSelection(8, "Mua Ngọc Bội Danh Vọng Liên Sever")
	dialog:AddSelection(9, "Mua Nhẫn Danh Vọng Đại Hội Võ Lâm")
	dialog:AddSelection(10, "Mua Liên Danh Vọng Thịnh Hạ 2010")
	dialog:AddSelection(11, "Liên hệ Shop Tần Lăng Phát Khấu Môn")
	dialog:AddSelection(6, "Không mua nữa")
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
function ThuongGiaTieuDao:OnSelection(scene, npc, player, selectionID,otherParams)

	-- ************************** --
	if selectionID == 1 then
		Player.OpenShop(npc, player, 128)
       GUI.CloseDialog(player)
	elseif 	selectionID == 2 then
		Player.OpenShop(npc, player, 133)
        GUI.CloseDialog(player)
	elseif 	selectionID == 3 then
		Player.OpenShop(npc, player, 161)
       GUI.CloseDialog(player)	
	elseif 	selectionID == 4 then
		Player.OpenShop(npc, player, 147)
        GUI.CloseDialog(player)
	elseif 	selectionID == 5 then
		Player.OpenShop(npc, player, 134)
        GUI.CloseDialog(player)
	elseif 	selectionID == 7 then
		Player.OpenShop(npc, player, 185)
        GUI.CloseDialog(player)
	elseif 	selectionID == 8 then
		Player.OpenShop(npc, player, 169)
        GUI.CloseDialog(player)
	elseif 	selectionID == 9 then
		Player.OpenShop(npc, player, 163)
        GUI.CloseDialog(player)
	elseif 	selectionID == 10 then
		Player.OpenShop(npc, player, 164)
        GUI.CloseDialog(player)
	elseif 	selectionID == 11 then
		local dialog = GUI.CreateNPCDialog()
		dialog:AddText("Miêu Miêu Miêu : Bọn ta nhất định sẽ thực hiện được hoài bão của trại chủ!")
		dialog:AddSelection(30,"Mua Vũ Khí cấp 120-130(Kim)")
		dialog:AddSelection(31,"Mua Vũ Khí cấp 120-130(Mộc)")
		dialog:AddSelection(32,"Mua Vũ Khí cấp 120-130(Thủy)")
		dialog:AddSelection(33,"Mua Vũ Khí cấp 120-130(Hỏa)")
		dialog:AddSelection(34,"Mua Vũ Khí cấp 120-130(Thổ)")
		dialog:AddSelection(35,"Mua Luyện Hóa Đồ Trang Bị Tần Lăng")
		dialog:AddSelection(20, "Dùng 5 Hòa Thị Bích để đổi lấy 1 Vũ khí Thanh Đồng-Luyện Hóa Đồ")
		dialog:AddSelection(6, "Không mua nữa")
		dialog:Show(npc, player)
	-- ************************** --
	elseif selectionID ==30 then
			Player.OpenShop(npc, player, 156)
			GUI.CloseDialog(player)
		
	elseif selectionID ==31 then
			Player.OpenShop(npc, player, 157)
			GUI.CloseDialog(player)
		
	elseif selectionID ==32 then
			Player.OpenShop(npc, player, 158)
			GUI.CloseDialog(player)
		
	elseif selectionID ==33 then
			Player.OpenShop(npc, player, 159)
			GUI.CloseDialog(player)
		
	elseif selectionID ==34 then
			Player.OpenShop(npc, player, 160)
			GUI.CloseDialog(player)
		
	elseif selectionID ==35 then
			Player.OpenShop(npc, player, 155)
			GUI.CloseDialog(player)		
			
	elseif selectionID == 20 then 
		-- Nếu Hòa Thị Bích
		if Player.CountItemInBag(player, ItemIDSub) < ItemCount then
			dialog:AddText( "Ta cần 5 Hòa Thị Bích, có đủ rồi hãy đưa ta. Ta đang rất bận!")
			dialog:Show(npc, player)
			return
		end 
		if Player.HasFreeBagSpaces(player, 1) == false then
			dialog:AddText( "Túi của ngươi đã đầy, hãy sắp xếp <color=green>1 ô trống</color> trong túi để nhận Vật phẩm Vũ khí Thanh Đồng-Luyện Hóa Đồ!")
			dialog:Show(npc, player)
			return
		end
		-- Xóa Hòa Thị Bích
		
		Player.RemoveItem(player, ItemIDSub, ItemCount)
		
		
		-- Tạo Vật phẩm Vũ khí Thanh Đồng-Luyện Hóa Đồ
		Player.AddItemLua(player, ItemIDAdd, 1, -1, 1)
		
		
		
		dialog:AddText( "Cảm ơn ngươi về những Viên Hòa Thị Bích này, đây là phần thưởng của ngươi, hãy nhận nó!")
		dialog:Show(npc, player)
		return
	
	elseif 	selectionID == 6 then
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
function ThuongGiaTieuDao:OnItemSelected(scene, npc, player,otherParams )

	-- ************************** --

	-- ************************** --

end