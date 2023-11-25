-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '200003' bên dưới thành ID tương ứng
local TuLuyenChau = Scripts[200009]

-- ****************************************************** --
--	Hàm này được gọi khi người chơi ấn sử dụng vật phẩm, kiểm tra điều kiện có được dùng hay không
--		scene: Scene - Bản đồ hiện tại
--		item: Item - Vật phẩm tương ứng
--		player: Player - NPC tương ứng
--	Return: True nếu thỏa mãn điều kiện có thể sử dụng, False nếu không thỏa mãn
-- ****************************************************** --
function TuLuyenChau:OnPreCheckCondition(scene, item, player, otherParams)

	-- ************************** --
	--player:AddNotification("Enter -> TuLuyenChau:OnPreCheckCondition")--

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
function TuLuyenChau:OnUse(scene, item, player, otherParams)

	-- ************************** --
	--player:AddNotification("Enter -> TuLuyenChau:OnUse")--
	-- ************************** --
	local dialog = GUI.CreateItemDialog()
		--[[ dialog:AddText("Đặt tay lên cảm thấy khí huyết cuộn dâng. " .."<color=yellow>Bạn có thể mở trạng thái tu luyện để nhận kinh nghiệm đánh quái x2 và tăng 10 điểm may mắn,</color> <color=red>một khi đã mở tu luyện sẽ không thể đóng lại trước khi hết giờ.</color>" .."\n Thời gian tu luyện tích lũy còn: <color=green>i</color> <color=yellow>giờ</color> <color=green>i<color> <color=yellow>phút</color>. Bạn muốn mở bao lâu?")
		dialog:AddSelection(1,"Mua Tinh Hoàn Phúc lợi")
		dialog:AddSelection(2,"Nhận Thời gian tu luyện thêm tháng này")
		dialog:AddSelection(3,"Thay đổi đến Thiếu Lâm")
		dialog:AddSelection(4,"Ta muốn mở tu luyện")
		dialog:AddSelection(5,"Kết thúc đối thoại")
		dialog:Show(item, player) ]]
		dialog:AddText("Chức năng chưa mở ")
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
function TuLuyenChau:OnSelection(scene, item, player, selectionID, otherParams)

	-- ************************** --
	--player:AddNotification("Enter -> TuLuyenChau:OnSelection, selectionID = " .. selectionID)--
	
	-- ************************** --
	
	
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
function TuLuyenChau:OnItemSelected(scene, item, player, itemID)

	-- ************************** --

	-- ************************** --

end