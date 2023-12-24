-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '200005' bên dưới thành ID tương ứng
local ADD_SHENGWANG = {10, 20, 50}

local VoLamCaoThuLenh = Scripts[200050]

-- ****************************************************** --
--	Hàm này được gọi khi người chơi ấn sử dụng vật phẩm, kiểm tra điều kiện có được dùng hay không
--		scene: Scene - Bản đồ hiện tại
--		item: Item - Vật phẩm tương ứng
--		player: Player - NPC tương ứng
--	Return: True nếu thỏa mãn điều kiện có thể sử dụng, False nếu không thỏa mãn
-- ****************************************************** --
function VoLamCaoThuLenh:OnPreCheckCondition(scene, item, player, otherParams)

	-- ************************** --
	--player:AddNotification("Enter -> VoLamCaoThuLenh:OnPreCheckCondition")--
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
function VoLamCaoThuLenh:OnUse(scene, item, player, otherParams)

	-- ************************** --
	-- ************************** --
	local nLevel = item:GetItemLevel()
	local items = item:GetItemID()
	if (nLevel < 1 or nLevel > 3) then
		player:AddNotification("Vật phẩm không hợp lệ!")
		return;

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

	Player.SetValueOfDailyRecore(player,item:GetItemID(),GetValue+1)

	local POINT = ADD_SHENGWANG[nLevel];
	
	if items == 539 or items == 540 or items == 541 then
		Player.AddRetupeValue(player,601,POINT)
		Player.RemoveItem(player,item:GetID())
	elseif items == 542 or items == 543 or items == 544 then
		Player.AddRetupeValue(player,602,POINT)
		Player.RemoveItem(player,item:GetID())
	elseif items == 545 or items == 546 or items == 547 then
		Player.AddRetupeValue(player,603,POINT)
		Player.RemoveItem(player,item:GetID())
	elseif items == 548 or items == 549 or items == 550 then
		Player.AddRetupeValue(player,604,POINT)
		Player.RemoveItem(player,item:GetID())
	elseif items == 551 or items == 552 or items == 553 then
		Player.AddRetupeValue(player,605,POINT)
		Player.RemoveItem(player,item:GetID())
	end
	
	player:AddNotification("Hôm nay đã dùng "..(GetValue+1).." =>"..item:GetName().." + "..POINT.." Khiêu chiến Võ Lâm Cao Thủ");
	-- ************************** --

end

-- ****************************************************** --
--	Hàm này được gọi khi có sự kiện người chơi ấn vào một trong số các chức năng cung cấp bởi vật phẩm thông qua Item Dialog
--		scene: Scene - Bản đồ hiện tại
--		item: Item - Vật phẩm tương ứng
--		player: Player - NPC tương ứng
--		selectionID: number - ID chức năng
-- ****************************************************** --
function VoLamCaoThuLenh:OnSelection(scene, item, player, selectionID, otherParams)

	-- ************************** --
	--player:AddNotification("Enter -> VoLamCaoThuLenh:OnSelection, selectionID = " .. selectionID)--
	
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
function VoLamCaoThuLenh:OnItemSelected(scene, item, player, itemID)

	-- ************************** --

	-- ************************** --

end