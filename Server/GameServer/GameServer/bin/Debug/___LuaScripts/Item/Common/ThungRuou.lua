-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '200005' bên dưới thành ID tương ứng
local ThungRuou = Scripts[200037]

-- ************************** --
local Jiu = {5, 50}
-- ************************** --
local tbJiu = {
	[247] = {ItemID = 247, NameJiu = "Rượu Tây Bắc Vọng"}, 
	[248] = {ItemID = 248, NameJiu = "Rượu Đạo Hoa Hương"}, 
	[249] = {ItemID = 249, NameJiu = "Rượu Nữ Nhi Hồng"}, 
	[250] = {ItemID = 250, NameJiu = "Rượu Hạnh Hoa Thôn"}, 
	[251] = {ItemID = 251, NameJiu = "Rượu Thiêu Đao Tử"}, 
}
-- ************************** --


-- ****************************************************** --
--	Hàm này được gọi khi người chơi ấn sử dụng vật phẩm, kiểm tra điều kiện có được dùng hay không
--		scene: Scene - Bản đồ hiện tại
--		item: Item - Vật phẩm tương ứng
--		player: Player - NPC tương ứng
--	Return: True nếu thỏa mãn điều kiện có thể sử dụng, False nếu không thỏa mãn
-- ****************************************************** --
function ThungRuou:OnPreCheckCondition(scene, item, player, otherParams)

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
function ThungRuou:OnUse(scene, item, player, otherParams)

	-- ************************** --
	local nLevel = item:GetItemLevel()
	local dialog = GUI.CreateItemDialog()
	dialog:AddText("Uống rượu giúp tăng 2 lần kinh nghiệm lửa trại, ngoài ra có thể sử dụng chung với Tu Luyện Châu để tăng thêm 4 lần kinh nghiệm lửa trại. Ngươi muốn lấy rượu nào?")
	for key, value in pairs(tbJiu)do
		dialog:AddSelection(value.ItemID, value.NameJiu)	
	end
	dialog:AddSelection(1000, "Để ta suy nghĩ đã");
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
function ThungRuou:OnSelection(scene, item, player, selectionID, otherParams)

	-- ************************** --
	if selectionID == 247 or selectionID == 248 or selectionID == 249 or selectionID == 250 or selectionID == 251 or selectionID == 252 then
		local itemLevel = item:GetItemLevel()
		if itemLevel < 1 or itemLevel > 2 then
			self:ShowDialog(item, player, "Vật phẩm không hợp lệ!")
			return
		end
		
		-- Túi không đủ chỗ trống
		if Player.HasFreeBagSpaces(player, 1) == false then
			self:ShowDialog(item, player, string.format("Túi đồ cần để trống tối thiểu <color=green>1 ô trống</color> trong túi đồ để lấy rượu!"))
			return
		end
		
		-- Xóa vật phẩm trước
		Player.RemoveItem(player, item:GetID())
		
		-- ID rượu tương ứng
		local itemID = tbJiu[selectionID].ItemID
		-- Số lượng
		local itemNumber = Jiu[itemLevel]
		
		-- Thêm vật phẩm
		Player.AddItemLua(player, itemID, itemNumber, -1, 0)
		
		self:ShowDialog(item, player, "Sử dụng thành công!")
		GUI.CloseDialog(player)
		return
	end
	-- ************************** --
	if selectionID == 1000 then
		GUI.CloseDialog(player)
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
function ThungRuou:OnItemSelected(scene, item, player, itemID)

	-- ************************** --

	-- ************************** --

end

function ThungRuou:ShowDialog(item, player, text)

	-- ************************** --
	local dialog = GUI.CreateItemDialog()
	dialog:AddText(text)
	dialog:Show(item, player)
	-- ************************** --
	
end