-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '200034' bên dưới thành ID tương ứng
local IDBuff =386 
local NguHanhThach = Scripts[200035]

-- ****************************************************** --
--	Hàm này được gọi khi người chơi ấn sử dụng vật phẩm, kiểm tra điều kiện có được dùng hay không
--		scene: Scene - Bản đồ hiện tại
--		item: Item - Vật phẩm tương ứng
--		player: Player - NPC tương ứng
--	Return: True nếu thỏa mãn điều kiện có thể sử dụng, False nếu không thỏa mãn
-- ****************************************************** --
function NguHanhThach:OnPreCheckCondition(scene, item, player, otherParams)

	-- ************************** --
	--player:AddNotification("Enter -> NguHanhThach:OnPreCheckCondition")--
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
function NguHanhThach:OnUse(scene, item, player, otherParams)

	-- ************************** --
	local ItemID = item:GetItemID()

	local Itemlevel = item:GetItemLevel()
	

	local NameItem = item:GetName()


	-- ************************** --
	local dialog = GUI.CreateItemDialog()
	
	local MSG = "Ngươi có muốn dùng "..NameItem.." "
	local dialog = GUI.CreateItemDialog()
	dialog:AddText(MSG)

	dialog:AddSelection(1, "Ta muốn sử dụng bây giờ")
    dialog:AddSelection(2, "Ta suy nghĩ đã")
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
function NguHanhThach:OnSelection(scene, item, player, selectionID, otherParams)

	-- ************************** --
	--player:AddNotification("Enter -> NguHanhThach:OnSelection, selectionID = " .. selectionID)--
	local ItemID = item:GetItemID()

	local Itemlevel = item:GetItemLevel()
	if Itemlevel < 1 or Itemlevel > 10 then
		player:AddNotification("Lỗi dùng vật phẩm")
		return;
	end
	-- ************************** --
	if selectionID == 1 then
		if(player:HasBuff(IDBuff)) then
			player:RemoveBuff(IDBuff)
		end
		player:AddBuff(IDBuff, Itemlevel)
		Player.RemoveItem(player,item:GetID())  
		GUI.CloseDialog(player)
	else 
		if selectionID == 2 then
            GUI.CloseDialog(player)
        end
	end
	item:FinishUsing(player)
	

end

-- ****************************************************** --
--	Hàm này được gọi khi có sự kiện người chơi chọn một trong các vật phẩm, và ấn nút Xác nhận cung cấp bởi vật phẩm thông qua Item Dialog
--		scene: Scene - Bản đồ hiện tại
--		item: Item - Vật phẩm tương ứng
--		player: Player - NPC tương ứng
--		itemID: number - ID vật phẩm được chọn
-- ****************************************************** --
function NguHanhThach:OnItemSelected(scene, item, player, itemID)

	-- ************************** --

	-- ************************** --

end