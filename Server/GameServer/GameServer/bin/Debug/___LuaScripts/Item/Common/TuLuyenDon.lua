-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '200003' bên dưới thành ID tương ứng
local TuLuyenDon = Scripts[200084]

-- ************************** --
local Use_Max = 2
local Use_OneItem = 20
-- ************************** --

-- ****************************************************** --
--	Hàm này được gọi khi người chơi ấn sử dụng vật phẩm, kiểm tra điều kiện có được dùng hay không
--		scene: Scene - Bản đồ hiện tại
--		item: Item - Vật phẩm tương ứng
--		player: Player - NPC tương ứng
--	Return: True nếu thỏa mãn điều kiện có thể sử dụng, False nếu không thỏa mãn
-- ****************************************************** --
function TuLuyenDon:OnPreCheckCondition(scene, item, player, otherParams)

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
function TuLuyenDon:OnUse(scene, item, player, otherParams)

	-- ************************** --
	local dialog = GUI.CreateItemDialog()
	dialog:AddText("Có thể tăng 2 giờ Tu Luyện Châu, mỗi ngày uống tối đa " .. Use_Max .. " lọ. Tổng thời gian tu luyện không quá 14 giờ.")
	dialog:AddSelection(1, "Sử dụng Tu Luyện Đơn")
	dialog:AddSelection(1000, "Kết thúc đối thoại")
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
function TuLuyenDon:OnSelection(scene, item, player, selectionID, otherParams)

	-- ************************** --
	if selectionID == 1000 then
		GUI.CloseDialog(player)
		return
	end
	-- ************************** --
	if selectionID == 1 then
		local timeLeft = player:GetXiuLianZhu_TimeLeft()
		local totalTime = (timeLeft + Use_OneItem)
		
		-- Số lượng đã dùng trong ngày
		local totalUsedToday = Player.GetValueOfDailyRecore(player, item:GetItemID())
		-- Toác
		if totalUsedToday == -1 then
			totalUsedToday = 0
		end
		
		if totalUsedToday >= Use_Max then
			self:ShowDialog(item, player, "Mỗi ngày tối đa chỉ nên dùng " .. Use_Max .. " bình.")
			return
		end
		
		-- Nếu thời gian tu luyện quá nhiều
		if timeLeft >= 140 then
			self:ShowDialog(item, player, "Thời gian tu luyện của bạn còn hơn 14 giờ, không thể sử dụng Tu Luyện Đơn.")
			return
		end
		
		-- Tăng số lượng đã dùng trong ngày
		totalUsedToday = totalUsedToday + 1
		
		-- Thiết lập số lượng đã dùng trong ngày
		Player.SetValueOfDailyRecore(player, item:GetItemID(), totalUsedToday)
		
		-- Xóa vật phẩm
		Player.RemoveItem(player, item:GetID())
		-- Thiết lập thời gian
		player:SetXiuLianZhu_TimeLeft(totalTime)
		
		self:ShowDialog(item, player, "Sử dụng Tu Luyện đơn thành công!<br>Tổng số giờ tu luyện châu của bạn là: " .. (totalTime / 10) .. " giờ. Hôm nay đã dùng tổng cộng <color=yellow>" .. totalUsedToday .. "</color> bình Tu Luyện Đơn.")
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
function TuLuyenDon:OnItemSelected(scene, item, player, itemID)

	-- ************************** --

	-- ************************** --

end


function TuLuyenDon:ShowDialog(item, player, text)

	-- ************************** --
	local dialog = GUI.CreateItemDialog()
	dialog:AddText(text)
	dialog:Show(item, player)
	-- ************************** --
	
end