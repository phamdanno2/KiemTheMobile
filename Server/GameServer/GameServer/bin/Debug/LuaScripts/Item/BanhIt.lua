-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '200005' bên dưới thành ID tương ứng
local BanhIt = Scripts[190101]

-- ****************************************************** --
--	Hàm này được gọi khi người chơi ấn sử dụng vật phẩm, kiểm tra điều kiện có được dùng hay không
--		scene: Scene - Bản đồ hiện tại
--		item: Item - Vật phẩm tương ứng
--		player: Player - NPC tương ứng
--	Return: True nếu thỏa mãn điều kiện có thể sử dụng, False nếu không thỏa mãn
-- ****************************************************** --
function BanhIt:OnPreCheckCondition(scene, item, player, otherParams)

	-- ************************** --
	--player:AddNotification("Enter -> LuaTrai:OnPreCheckCondition")--
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
local Record1 = 102119
local Record2 = 102120
local Record3 = 102121
function BanhIt:OnUse(scene, item, player, otherParams)

	local id = item:GetItemID();
	local name = item:GetName();
	if id == 747 then --okie
		local record1 = Player.GetValueForeverRecore(player, Record1)
		if record1 < 0 then
			record1 = 0
		end
		if record1 < 100 then
			Player.SetValueOfForeverRecore(player, Record1, record1 + 1)
			Player.AddRoleExp(player,10000)
			player:AddNotification("Dùng "..name.." thành công nhận được <color=red>10000 điểm kinh nghiệm</color> !")
			Player.RemoveItem(player, item:GetID())
		else
			player:AddNotification("Dã dùng "..name.." tới giới hạn !")
		end
	elseif id == 748 then --okie
		local record2 = Player.GetValueForeverRecore(player, Record2)
		if record2 < 0 then
			record2 = 0
		end
		if record2 < 2 then
			Player.SetValueOfForeverRecore(player, Record2, record2 + 1)
			player:AddBonusRemainPotentialPoint(10)
			player:AddNotification("Dùng "..name.." thành công nhận được <color=red>10 điểm tiềm năng</color> !")
			Player.RemoveItem(player, item:GetID())
		else
			player:AddNotification("Dã dùng "..name.." tới giới hạn !")
		end
	elseif id == 749 then --okie
		local record3 = Player.GetValueForeverRecore(player, Record3)
		if record3 < 0 then
			record3 = 0
		end
		if record3 < 2 then
			Player.SetValueOfForeverRecore(player, Record3, record3 + 1)
			Player.AddBonusSkillPoint(player,1)
			player:AddNotification("Dùng "..name.." thành công nhận được <color=red>1 điểm kỹ năng</color> !")
			Player.RemoveItem(player, item:GetID())
		else
			player:AddNotification("Dã dùng "..name.." tới giới hạn !")
		end
	else
		player:AddNotification("Hãy tặng cho <color=red>trại chủ Tần Oa</color> để thêm số lần thi đấu !")
	end
end

-- ****************************************************** --
--	Hàm này được gọi khi có sự kiện người chơi ấn vào một trong số các chức năng cung cấp bởi vật phẩm thông qua Item Dialog
--		scene: Scene - Bản đồ hiện tại
--		item: Item - Vật phẩm tương ứng
--		player: Player - NPC tương ứng
--		selectionID: number - ID chức năng
-- ****************************************************** --
function BanhIt:OnSelection(scene, item, player, selectionID, otherParams)

	-- ************************** --
	--player:AddNotification("Enter -> LuaTrai:OnSelection, selectionID = " .. selectionID)--
	
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
function BanhIt:OnItemSelected(scene, item, player, itemID)

	-- ************************** --

	-- ************************** --

end