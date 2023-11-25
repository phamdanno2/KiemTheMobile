-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '200003' bên dưới thành ID tương ứng
local TranPhap = Scripts[200085]

local ZhenFaList = {
	[3888] = {
		ItemID = 3888,
		Name = "Trận pháp-Trung: Bát Quái Trận - Li",
		Number = 1,
	},
	[3889] = {
		ItemID = 3889,
		Name = "Trận pháp-Trung:  Bát Quái Trận - Đoài",
		Number = 1,
	},
	[3890]=  {
		ItemID=3890,
		Name="Trận pháp-Trung: Bát Quái Trận - Cấn",
		Number = 1,
	},
	[3891]=  {
		ItemID = 3891,
		Name = "Trận pháp-Trung:  Bát Quái Trận - Khảm",
		Number = 1,
	},
	[3892]=  {
		ItemID = 3892,
		Name = "Trận pháp-Trung: Bát Quái Trận - Tốn",
		Number = 1,
	},
	[3893]=  {
		ItemID = 3893,
		Name = "Trận pháp-Trung: Bát Quái Trận - Càn",
		Number = 1,
	},
	[3894]=  {
		ItemID = 3894,
		Name = "Trận pháp-Trung: Thanh Long Trận",
		Number = 1,
	},
	[3895]=  {
		ItemID = 3895,
		Name = "Trận pháp-Trung: Huyễn Vũ Trận",
		Number = 1,
	},
	[3896]=  {
		ItemID = 3896,
		Name = "Trận pháp-Trung: Bạch Hổ Trận",
		Number = 1,
	},
	[3897]=  {
		ItemID = 3897,
		Name = "Trận pháp-Trung: Chu Tước Trận",
		Number = 1,
	},
	[3898]=  {
		ItemID = 3898,
		Name = "Trận pháp-Trung: Ngũ Hành Trận",
		Number = 1,
	},
}
-- ****************************************************** --
--	Hàm này được gọi khi người chơi ấn sử dụng vật phẩm, kiểm tra điều kiện có được dùng hay không
--		scene: Scene - Bản đồ hiện tại
--		item: Item - Vật phẩm tương ứng
--		player: Player - NPC tương ứng
--	Return: True nếu thỏa mãn điều kiện có thể sử dụng, False nếu không thỏa mãn
-- ****************************************************** --
function TranPhap:OnPreCheckCondition(scene, item, player, otherParams)

	-- ************************** --
	return true
	-- ************************** --

end

-- ****************************************************** --
--	Hàm này được gọi để thực thi Logic khi người sử dụng vật phẩm, sau khi đã thỏa mãn hàm kiểm tra điều kiện
--		scene: Scene - Bản đồ hiện tại
--		item: Item - Vật phẩm tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function TranPhap:OnUse(scene, item, player, otherParams)

	-- ************************** --
	local dialog = GUI.CreateItemDialog()
	if item:GetItemID()== 739 or item:GetItemID()== 740 or item:GetItemID()== 741 then
		dialog:AddText("Ngươi muốn lấy trận pháp nào?")
		for key, value in pairs(ZhenFaList) do
			dialog:AddSelection(key, value.Name)
		end
	end
	dialog:AddSelection(100, "Để sau...")
	dialog:Show(item, player)
	-- ************************** --

end

-- ****************************************************** --
--	Hàm này được gọi khi có sự kiện người chơi ấn vào một trong số các chức năng cung cấp bởi vật phẩm thông qua Item Dialog
--		scene: Scene - Bản đồ hiện tại
--		item: Item - Vật phẩm tương ứng
--		player: Player - NPC tương ứng
--		selectionID: number - ID chức năng
-- ****************************************************** --
function TranPhap:OnSelection(scene, item, player, selectionID, otherParams)

	-- ************************** --
	if selectionID == 100 then
		GUI.CloseDialog(player)
	else
		-- Toác
		if ZhenFaList[selectionID] == nil then
			self:ShowDialog(item, player, "Vật phẩm bị lỗi, hãy thử lại sau!")
			return
		end
		
		-- Túi không đủ chỗ trống
		if Player.HasFreeBagSpaces(player, 1) == false then
			self:ShowDialog(item, player, string.format("Túi đồ cần để trống tối thiểu <color=green>1 ô trống</color> trong túi đồ để lấy trận pháp!"))
			return
		end
		
		-- Thêm vật phẩm vào
		Player.AddItemLua(player, ZhenFaList[selectionID].ItemID, ZhenFaList[selectionID].Number, -1, 1, 0, 21600)
		Player.RemoveItem(player, item:GetID())	-- Xóa vật ph
		GUI.CloseDialog(player)
		-- Thông báo nhận thành công
		player:AddNotification("Lấy trận pháp đồ thành công!")
	end
	-- ************************** --
	item:FinishUsing(player)

end

-- ****************************************************** --
--	Hàm này được gọi khi có sự kiện người chơi chọn một trong các vật phẩm, và ấn nút Xác nhận cung cấp bởi vật phẩm thông qua Item Dialog
--		scene: Scene - Bản đồ hiện tại
--		item: Item - Vật phẩm tương ứng
--		player: Player - NPC tương ứng
--		itemID: number - ID vật phẩm được chọn
-- ****************************************************** --
function TranPhap:OnItemSelected(scene, item, player, itemID)

	-- ************************** --

	-- ************************** --

end

function TranPhap:ShowDialog(item, player, text)

	-- ************************** --
	local dialog = GUI.CreateItemDialog()
	dialog:AddText(text)
	dialog:Show(item, player)
	-- ************************** --
	
end