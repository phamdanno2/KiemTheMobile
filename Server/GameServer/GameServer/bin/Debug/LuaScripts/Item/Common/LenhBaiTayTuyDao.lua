-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '200005' bên dưới thành ID tương ứng
local LenhBaiTayTuyDao = Scripts[200017]

-- ****************************************************** --
--	Hàm này được gọi khi người chơi ấn sử dụng vật phẩm, kiểm tra điều kiện có được dùng hay không
--		scene: Scene - Bản đồ hiện tại
--		item: Item - Vật phẩm tương ứng
--		player: Player - NPC tương ứng
--	Return: True nếu thỏa mãn điều kiện có thể sử dụng, False nếu không thỏa mãn
-- ****************************************************** --
function LenhBaiTayTuyDao:OnPreCheckCondition(scene, item, player, otherParams)

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
function LenhBaiTayTuyDao:OnUse(scene, item, player, otherParams)

	-- ************************** --
	local dialog = GUI.CreateItemDialog()
	dialog:AddText("Ngươi có muốn vào tẩy tủy đảo? Sử dụng sẽ tiêu hao Lệnh bài này.")
	dialog:AddSelection (1, "Xác nhận")
	dialog:AddSelection (2, "Hủy bỏ ")
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
function LenhBaiTayTuyDao:OnSelection(scene, item, player, selectionID, otherParams)

	-- ************************** --
	if selectionID == 1 then
		-- Nếu đang ở trong đảo rồi
		if scene:GetID() == 255 then
			local dialog = GUI.CreateItemDialog()
			dialog:AddText("Ngươi đang trong tẩy tủy đảo rồi!")
			dialog:Show(item, player)
			return
		end
		
		-- Xóa lệnh bài
		Player.RemoveItem(player, item:GetID())
		player:ChangeScene(255, 1750, 1930)
		GUI:CloseDialog(player)
		return
	end
	-- ************************** --
	if selectionID == 2 then
		GUI:CloseDialog(player)
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
function LenhBaiTayTuyDao:OnItemSelected(scene, item, player, itemID)

	-- ************************** --

	-- ************************** --

end