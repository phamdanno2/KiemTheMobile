-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '200003' bên dưới thành ID tương ứng
local TruyenThongPhu_ThanhThi = Scripts[200003]

-- ****************************************************** --
--	Hàm này được gọi khi người chơi ấn sử dụng vật phẩm, kiểm tra điều kiện có được dùng hay không
--		scene: Scene - Bản đồ hiện tại
--		item: Item - Vật phẩm tương ứng
--		player: Player - NPC tương ứng
--	Return: True nếu thỏa mãn điều kiện có thể sử dụng, False nếu không thỏa mãn
-- ****************************************************** --
function TruyenThongPhu_ThanhThi:OnPreCheckCondition(scene, item, player, otherParams)

	-- ************************** --
	--player:AddNotification("Enter -> TruyenThongPhu_ThanhThi:OnPreCheckCondition")--
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
function TruyenThongPhu_ThanhThi:OnUse(scene, item, player, otherParams)

	-- ************************** --
	--player:AddNotification("Enter -> TruyenThongPhu_ThanhThi:OnUse")--
	-- ************************** --
	local dialog = GUI.CreateItemDialog()
		dialog:AddText("Ngươi muốn đi đâu ?")
		for key ,value in pairs (Global_NameMapItemThanhID) do
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
function TruyenThongPhu_ThanhThi:OnSelection(scene, item, player, selectionID, otherParams)

	-- ************************** --
	local dialog = GUI.CreateItemDialog()
	--player:AddNotification("Enter -> TruyenThongPhu_ThanhThi:OnSelection, selectionID = " .. selectionID)--
	dialog:AddSelection(100,"Không cần")
	dialog:Show(item, player)
	-- ************************** --
	if Global_NameMapItemThanhID[selectionID] ~= nil then
		player:ChangeScene(Global_NameMapItemThanhID[selectionID].ID,Global_NameMapItemThanhID[selectionID].PosX,Global_NameMapItemThanhID[selectionID].PosY)
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
function TruyenThongPhu_ThanhThi:OnItemSelected(scene, item, player, itemID)

	-- ************************** --

	-- ************************** --

end