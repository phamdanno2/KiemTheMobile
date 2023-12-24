    -- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '200077' bên dưới thành ID tương ứng

local DuLongTuLuyenPhu = Scripts[200096]

-- ****************************************************** --
--	Hàm này được gọi khi người chơi ấn sử dụng vật phẩm, kiểm tra điều kiện có được dùng hay không
--		scene: Scene - Bản đồ hiện tại
--		item: Item - Vật phẩm tương ứng
--		player: Player - NPC tương ứng
--	Return: True nếu thỏa mãn điều kiện có thể sử dụng, False nếu không thỏa mãn
-- ****************************************************** --
function DuLongTuLuyenPhu:OnPreCheckCondition(scene, item, player, otherParams)

	-- ************************** --
	--player:AddNotification("Enter -> DuLongTuLuyenPhu:OnPreCheckCondition")--
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
function DuLongTuLuyenPhu:OnUse(scene, item, player, otherParams)

	-- ************************** --
	-- ************************** --
	local nLevel = item:GetItemLevel()
	
	if nLevel ~=1 then
		player:AddNotification("Lỗi dùng vật phẩm");
	end
	if player:GetFactionID()==0 then
		player:AddNotification("Chưa nhập môn phái!")
		
		return;
	end
	-- số ngày n ăn được bao nhiêu 
	local GetValue = Player.GetValueOfDailyRecore(player,item:GetItemID())
	
	if(GetValue==-1) then
		GetValue = 0;
	end
		local EXP = item:GetItemValue()
		-- local EXP = Player.GetBaseExp(player,player:GetLevel())*60;
		Player.AddRoleExp(player, EXP)
		Player.RemoveItem(player,item:GetID())
		Player.SetValueOfDailyRecore(player,item:GetItemID(),GetValue+1)
		player:AddNotification("Hôm nay đã dùng "..(GetValue+1).."==> "..item:GetName().." + "..EXP.." Điểm kinh nghiệm");
	-- ************************** --

end

-- ****************************************************** --
--	Hàm này được gọi khi có sự kiện người chơi ấn vào một trong số các chức năng cung cấp bởi vật phẩm thông qua Item Dialog
--		scene: Scene - Bản đồ hiện tại
--		item: Item - Vật phẩm tương ứng
--		player: Player - NPC tương ứng
--		selectionID: number - ID chức năng
-- ****************************************************** --
function DuLongTuLuyenPhu:OnSelection(scene, item, player, selectionID, otherParams)

	-- ************************** --
	--player:AddNotification("Enter -> DuLongTuLuyenPhu:OnSelection, selectionID = " .. selectionID)--
	
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
function DuLongTuLuyenPhu:OnItemSelected(scene, item, player, itemID)

	-- ************************** --

	-- ************************** --

end