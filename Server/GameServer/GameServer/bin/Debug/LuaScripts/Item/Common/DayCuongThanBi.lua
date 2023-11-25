-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '200005' bên dưới thành ID tương ứng
local DayCuongThanBi = Scripts[200046]

-- ****************************************************** --
--	Hàm này được gọi khi người chơi ấn sử dụng vật phẩm, kiểm tra điều kiện có được dùng hay không
--		scene: Scene - Bản đồ hiện tại
--		item: Item - Vật phẩm tương ứng
--		player: Player - NPC tương ứng
--	Return: True nếu thỏa mãn điều kiện có thể sử dụng, False nếu không thỏa mãn
-- ****************************************************** --
function DayCuongThanBi:OnPreCheckCondition(scene, item, player, otherParams)

	-- ************************** --
	--player:AddNotification("Enter -> DayCuongThanBi:OnPreCheckCondition")--
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
function DayCuongThanBi:OnUse(scene, item, player, otherParams)

	-- ************************** --
	-- ************************** --
	local dialog = GUI.CreateItemDialog()
	dialog:AddText("Dây cương thần bí,dùng được trang bị cực phẩm.Hãy chọn trang bị mình cần.<br><color=#53f9e8>(Nhận trang bị thú cưới sẽ tự động khóa)</color>")
	dialog:AddSelection(1,"Ô Vân Đạp Tuyết ")
    dialog:AddSelection(2,"Tuyệt Ảnh")
    dialog:AddSelection(3,"Chiếu Dạ Ngọc Sư Tử")
	dialog:AddSelection(4,"Hãn Huyết Bảo Ngọc ")
	dialog:AddSelection(5,"Để ta suy nghĩ đã")
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
function DayCuongThanBi:OnSelection(scene, item, player, selectionID, otherParams)

	-- ************************** --
	--player:AddNotification("Enter -> DayCuongThanBi:OnSelection, selectionID = " .. selectionID)--
	
	local dialog = GUI.CreateItemDialog()
	
	if selectionID == 1  then
		Player.AddItemLua(player,3463,1,0,1,0,43200)	
		Player.RemoveItem(player,item:GetID())  
		dialog:AddText("Nhận ngựa <color=red>thành công</color>")
		dialog:Show(item, player)
	elseif selectionID == 2 then
		Player.AddItemLua(player,3465,1,0,1,0,43200)	
		Player.RemoveItem(player,item:GetID())  
		dialog:AddText("Nhận ngựa <color=red>thành công</color>")
		dialog:Show(item, player)
	elseif selectionID == 3 then
		Player.AddItemLua(player,3466,1,0,1,0,43200)	
		Player.RemoveItem(player,item:GetID()) 
		dialog:AddText("Nhận ngựa <color=red>thành công</color>")
		dialog:Show(item, player)
	elseif selectionID == 4 then
		Player.AddItemLua(player,3467,1,0,1,0,43200)	
		Player.RemoveItem(player,item:GetID()) 
		dialog:AddText("Nhận ngựa <color=red>thành công</color>")
		dialog:Show(item, player)
	end

	if selectionID == 5 then
		GUI.CloseDialog(player)
	end
	-- ************************** --
	item:FinishUsing(player)
	-- ************************** --
	
end

-- ****************************************************** --
--	Hàm này được gọi khi có sự kiện người chơi chọn một trong các vật phẩm, và ấn nút Xác nhận cung cấp bởi vật phẩm thông qua Item Dialog
--		scene: Scene - Bản đồ hiện tại
--		item: Item - Vật phẩm tương ứng
--		player: Player - NPC tương ứng
--		itemID: number - ID vật phẩm được chọn
-- ****************************************************** --
function DayCuongThanBi:OnItemSelected(scene, item, player, itemID)

	-- ************************** --

	-- ************************** --

end