-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '200005' bên dưới thành ID tương ứng
local TinhKhiTan = Scripts[200022]

-- ************************** --
local ADD_SHENGWANG = {500, 1000, 1500,5000}
local USED_LIMIT = 15
-- ************************** --

-- ****************************************************** --
--	Hàm này được gọi khi người chơi ấn sử dụng vật phẩm, kiểm tra điều kiện có được dùng hay không
--		scene: Scene - Bản đồ hiện tại
--		item: Item - Vật phẩm tương ứng
--		player: Player - NPC tương ứng
--	Return: True nếu thỏa mãn điều kiện có thể sử dụng, False nếu không thỏa mãn
-- ****************************************************** --
function TinhKhiTan:OnPreCheckCondition(scene, item, player, otherParams)

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
function TinhKhiTan:OnUse(scene, item, player, otherParams)

	-- ************************** --
	-- Cấp độ
	local nLevel = item:GetItemLevel()
	-- Toác
	if nLevel < 1 or nLevel > 4 then
		player:AddNotification("Vật phẩm không hợp lệ!")
		return
	end
	
	-- Số lượng đã ăn trong ngày
	local totalUsedToday = Player.GetValueOfDailyRecore(player, item:GetItemID())
	-- Chỉnh lại giá trị nếu sang ngày mới
	if totalUsedToday == -1 then
		totalUsedToday = 0
	end
	-- Nếu vượt quá giới hạn ăn trong ngày
	if totalUsedToday >= USED_LIMIT then
		player:AddNotification("Mỗi ngày chỉ có thể sử dụng tối đa " .. USED_LIMIT .. " bình!")
		return
	end
	
	-- Tăng số lượng đã ăn trong ngày
	totalUsedToday = totalUsedToday + 1
	Player.SetValueOfDailyRecore(player, item:GetItemID(), totalUsedToday)
	
	-- Xóa vật phẩm
	Player.RemoveItem(player, item:GetID())

	-- Lượng Tinh lực có
	local addPoint = ADD_SHENGWANG[nLevel]
	-- Thêm tinh lực tương ứng
	Player.AddCurGatherPoint(player, addPoint)

	player:AddNotification("Sử dụng thành công, ngày hôm nay đã dùng tổng cộng " .. totalUsedToday .. "/" .. USED_LIMIT .. " " ..item:GetName() .. ".");
	-- ************************** --

end

-- ****************************************************** --
--	Hàm này được gọi khi có sự kiện người chơi ấn vào một trong số các chức năng cung cấp bởi vật phẩm thông qua Item Dialog
--		scene: Scene - Bản đồ hiện tại
--		item: Item - Vật phẩm tương ứng
--		player: Player - NPC tương ứng
--		selectionID: number - ID chức năng
-- ****************************************************** --
function TinhKhiTan:OnSelection(scene, item, player, selectionID, otherParams)

	-- ************************** --
	
	-- ************************** --

end

-- ****************************************************** --
--	Hàm này được gọi khi có sự kiện người chơi chọn một trong các vật phẩm, và ấn nút Xác nhận cung cấp bởi vật phẩm thông qua Item Dialog
--		scene: Scene - Bản đồ hiện tại
--		item: Item - Vật phẩm tương ứng
--		player: Player - NPC tương ứng
--		itemID: number - ID vật phẩm được chọn
-- ****************************************************** --
function TinhKhiTan:OnItemSelected(scene, item, player, itemID)

	-- ************************** --

	-- ************************** --

end