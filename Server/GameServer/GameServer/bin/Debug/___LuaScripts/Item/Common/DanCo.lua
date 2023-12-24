-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi    Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '200034' bên dưới thành ID tương ứng
local DanCo = Scripts[200053]

-- ****************************************************** --
--	Hàm này được gọi khi người chơi ấn sử dụng vật phẩm, kiểm tra điều kiện có được dùng hay không
--		scene: Scene - Bản đồ hiện tại
--		item: Item - Vật phẩm tương ứng
--		player: Player - NPC tương ứng
--	Return: True nếu thỏa mãn điều kiện có thể sử dụng, False nếu không thỏa mãn
-- ****************************************************** --
function DanCo:OnPreCheckCondition(scene, item, player, otherParams)

	-- ************************** --
	--player:AddNotification("Enter -> DanCo:OnPreCheckCondition")--
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
function DanCo:OnUse(scene, item, player, otherParams)

	-- ************************** --
	-- ************************** --
	


	
	-- ************************** --

end

-- ****************************************************** --
--	Hàm này được gọi khi có sự kiện người chơi ấn vào một trong số các chức năng cung cấp bởi vật phẩm thông qua Item Dialog
--		scene: Scene - Bản đồ hiện tại
--		item: Item - Vật phẩm tương ứng
--		player: Player - NPC tương ứng
--		selectionID: number - ID chức năng
-- ****************************************************** --
function DanCo:OnSelection(scene, item, player, selectionID, otherParams)

	-- ************************** --
	--player:AddNotification("Enter -> DanCo:OnSelection, selectionID = " .. selectionID)--
	
	item:FinishUsing(player)
	

end

-- ****************************************************** --
--	Hàm này được gọi khi có sự kiện người chơi chọn một trong các vật phẩm, và ấn nút Xác nhận cung cấp bởi vật phẩm thông qua Item Dialog
--		scene: Scene - Bản đồ hiện tại
--		item: Item - Vật phẩm tương ứng
--		player: Player - NPC tương ứng
--		itemID: number - ID vật phẩm được chọn
-- ****************************************************** --
function DanCo:OnItemSelected(scene, item, player, itemID)

	-- ************************** --

	-- ************************** --

end
function DanCo:JoinFaction(scene, item, player, factionID)
	
	-- ************************** --
	local ret = player:JoinFaction(factionID)
	-- ************************** --
	if ret == -1 then
		DanCo:ShowDialog(item, player, "Người chơi không tồn tại!")
		return
	elseif ret == -2 then
		DanCo:ShowDialog(item, player, "Môn phái không tồn tại!")
		return
	elseif ret == 0 then
		DanCo:ShowDialog(item, player, "Giới tính của bạn không phù hợp với môn phái này!")
		return
	elseif ret == 1 then
		DanCo:ShowDialog(item, player, "Gia nhập phái <color=blue>" .. player:GetFactionName() .. "</color> thành công!")
		return
	else
		DanCo:ShowDialog(item, player, "Chuyển phái thất bại, lỗi chưa rõ!")
		return
	end
	-- ************************** --

end
function DanCo:ShowDialog(item, player, text)

	-- ************************** --
	 local dialog = GUI.CreateItemDialog()
	dialog:AddText(text)
	dialog:Show(item, player)
	-- ************************** --
	
end