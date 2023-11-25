-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '100001' bên dưới thành ID tương ứng
local Item_Test = Scripts[100001]

-- ****************************************************** --
--	Hàm này được gọi khi người chơi ấn sử dụng vật phẩm, kiểm tra điều kiện có được dùng hay không
--		scene: Scene - Bản đồ hiện tại
--		item: Item - Vật phẩm tương ứng
--		player: Player - NPC tương ứng
--	Return: True nếu thỏa mãn điều kiện có thể sử dụng, False nếu không thỏa mãn
-- ****************************************************** --
function Item_Test:OnPreCheckCondition(scene, item, player,otherParams)

	-- ************************** --
	player:AddNotification("Enter -> Item_Test:OnPreCheckCondition")
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
function Item_Test:OnUse(scene, item, player,otherParams)

	-- ************************** --
	player:AddNotification("Enter -> Item_Test:OnUse")
	-- ************************** --
	local dialog = GUI.CreateItemDialog()
	dialog:AddText("Chọn địa điểm muốn dịch chuyển.")
	dialog:AddSelection(1, "Selection 1")
	dialog:AddSelection(2, "Selection 2")
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
function Item_Test:OnSelection(scene, item, player, selectionID,otherParams)

	-- ************************** --
	player:AddNotification("Enter -> Item_Test:OnSelection, selectionID = " .. selectionID)
	-- ************************** --
	if selectionID == 2 then
		local dialog = GUI.CreateItemDialog()
		dialog:AddText("Chọn địa điểm muốn dịch chuyển.")
		dialog:AddSelection(3, "Selection 3")
		dialog:AddSelection(4, "Selection 4")
		dialog:Show(item, player)
		
		return
	end
	-- ************************** --
	if selectionID == 3 then
		local dialog = GUI.CreateItemDialog()
		dialog:AddText("Selection 3 selected.")
		dialog:Show(item, player)
		
		return
	end
	-- ************************** --
	if selectionID == 4 then
		local dialog = GUI.CreateItemDialog()
		dialog:AddText("Selection 4 selected => Delete ITEM.")
		dialog:Show(item, player)
		
		-- Thực hiện gọi hàm sử dụng vật phẩm thành công
		item:FinishUsing(player)
		
		return
	end
	-- ************************** --

end

-- ****************************************************** --
--	Hàm này được gọi khi có sự kiện người chơi chọn một trong các vật phẩm, và ấn nút Xác nhận cung cấp bởi vật phẩm thông qua Item Dialog
--		scene: Scene - Bản đồ hiện tại
--		item: Item - Vật phẩm tương ứng
--		player: Player - NPC tương ứng
--		itemID: number - ID vật phẩm được chọn
-- ****************************************************** --
function Item_Test:OnItemSelected(scene, item, player, itemID)

	-- ************************** --
	
	-- ************************** --

end