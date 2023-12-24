-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '200004' bên dưới thành ID tương ứng
local TruyenThongPhu_MonPhai = Scripts[200004]

-- ****************************************************** --
--	Hàm này được gọi khi người chơi ấn sử dụng vật phẩm, kiểm tra điều kiện có được dùng hay không
--		scene: Scene - Bản đồ hiện tại
--		item: Item - Vật phẩm tương ứng
--		player: Player - NPC tương ứng
--	Return: True nếu thỏa mãn điều kiện có thể sử dụng, False nếu không thỏa mãn
-- ****************************************************** --
function TruyenThongPhu_MonPhai:OnPreCheckCondition(scene, item, player, otherParams)

	-- ************************** --
	--player:AddNotification("Enter -> TruyenThongPhu_MonPhai:OnPreCheckCondition")--
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
function TruyenThongPhu_MonPhai:OnUse(scene, item, player, otherParams)

	-- ************************** --
	--player:AddNotification("Enter -> TruyenThongPhu_MonPhai:OnUse")--
	-- ************************** --
	local dialog = GUI.CreateItemDialog()
		dialog:AddText("Ngươi muốn đi đâu ?")
		for key ,value in pairs (Global_NameMapPhaiID) do
			if value.ID ~= scene:GetID() then
				dialog:AddSelection(key,value.Name)
			end
		end
		dialog:AddSelection(100,"Không cần")
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
function TruyenThongPhu_MonPhai:OnSelection(scene, item, player, selectionID, otherParams)

	-- ************************** --
	--player:AddNotification("Enter -> TruyenThongPhu_MonPhai:OnSelection, selectionID = " .. selectionID)--
	local dialog = GUI.CreateItemDialog()
	if selectionID == 100 then
		GUI.CloseDialog(player)
	end
	-- ************************** --
	if Global_NameMapPhaiID[selectionID] ~= nil then
		player:ChangeScene(Global_NameMapPhaiID[selectionID].ID,Global_NameMapPhaiID[selectionID].PosX,Global_NameMapPhaiID[selectionID].PosY)
		GUI.CloseDialog(player)
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
function TruyenThongPhu_MonPhai:OnItemSelected(scene, item, player, itemID)

	-- ************************** --

	-- ************************** --

end