-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi    Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '200034' bên dưới thành ID tương ứng
local PhieuTayDiem = Scripts[200054]

-- ****************************************************** --
--	Hàm này được gọi khi người chơi ấn sử dụng vật phẩm, kiểm tra điều kiện có được dùng hay không
--		scene: Scene - Bản đồ hiện tại
--		item: Item - Vật phẩm tương ứng
--		player: Player - NPC tương ứng
--	Return: True nếu thỏa mãn điều kiện có thể sử dụng, False nếu không thỏa mãn
-- ****************************************************** --
function PhieuTayDiem:OnPreCheckCondition(scene, item, player, otherParams)

	-- ************************** --
	--player:AddNotification("Enter -> PhieuTayDiem:OnPreCheckCondition")--
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
function PhieuTayDiem:OnUse(scene, item, player, otherParams)

	-- ************************** --
	-- ************************** --
	local dialog = GUI.CreateItemDialog()
	dialog:AddText("Ngươi có muốn tẩy điểm tiềm năng với kỹ năng hay không ?")
	dialog:AddSelection(1,"Có")
	dialog:AddSelection(2,"Không")
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
function PhieuTayDiem:OnSelection(scene, item, player, selectionID, otherParams)

	-- ************************** --
	--player:AddNotification("Enter -> PhieuTayDiem:OnSelection, selectionID = " .. selectionID)--
	local dialog = GUI.CreateItemDialog()
	if selectionID == 1 then
		player:UnAssignRemainPotentialPoints()
		player:ResetAllSkillsLevel()
		dialog:AddText("Tẩy điểm kỹ năng ,Kỹ Năng thành công!")
		dialog:AddSelection(2,"Thoat")
		dialog:Show(item, player)
		item:FinishUsing(player)
	end
	if selectionID == 2 then
		GUI:CloseDialog(player)
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
function PhieuTayDiem:OnItemSelected(scene, item, player, itemID)

	-- ************************** --

	-- ************************** --

end